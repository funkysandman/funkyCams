using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.ServiceProcess;
using System.Xml.Linq;
using System.Collections.Specialized;
using MeteorIngestAPI.Models;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace DetectionServer
{
    public class DetectionServer
    {
       
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public DetectionServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }

            // URI prefixes are required eg: "http://localhost:8080/test/"
            if (prefixes == null || prefixes.Count == 0)
            {
                throw new ArgumentException("URI prefixes are required");
            }

            if (method == null)
            {
                throw new ArgumentException("responder method required");
            }

            foreach (var s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;
            _listener.Start();
        }

        public DetectionServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
           : this(prefixes, method)
        {
        }


        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
               // Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var rstr = _responderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch( Exception e)
                            {
                                // ignored
                                //Console.WriteLine(e.Message);
                            }
                            finally
                            {
                                // always close the stream
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                   // Console.WriteLine(ex.Message);
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }

    internal  class Program
    {

        private static Queue qt = new Queue();
        //private bool queueRunning = false;
        //private static Thread t = new Thread(processImages);
        private static object syncLock = new object();
        private static object syncLock2 = new object();
        public static Boolean running = false;
        private static ImageCodecInfo jgpEncoder;
        private static EncoderParameters myEncoderParameters;
        private static EncoderParameter myEncoderParameter;
        private static System.Drawing.Imaging.Encoder myEncoder;
        private static double MIN_SCORE_FOR_OBJECT_HIGHLIGHTING = 0.1;
        #region Nested classes to support running as service
        public const string ServiceName = "DetectionServer";
        public static DetectionServer ws = new DetectionServer(SendResponse, "http://192.168.1.192:7071/api/detection/");
        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }
        #endregion
        public class queueEntry
        {
            public byte[] img;
            public string filename;
            public string cameraID;
            public DateTime dateTaken;

        }

          private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static string SendResponse(HttpListenerRequest request)

            {
            //Console.WriteLine(request.ContentLength64);


            try
            {
             
                //    // Get request body
                string[] myParams = request.QueryString.GetValues("file");

                string filename = myParams[0];
                myParams = request.QueryString.GetValues("cameraID");

                string cameraID;
                if (myParams==null)
                {
                    cameraID = "unknown";
                }
                else
                {
                    cameraID = myParams[0];
                }
                myParams = request.QueryString.GetValues("dateTaken");
                DateTime dateTaken;
                if (myParams == null)
                {
                    dateTaken = DateTime.Now;
                }
                else
                {
                    dateTaken = DateTime.ParseExact(myParams[0],"MM/dd/yyyy hh:mm tt",null);
                }
                //
                ObjectDetection.TFDetector md = new ObjectDetection.TFDetector();

                queueEntry qe = new queueEntry();
                qe.filename = filename;
                qe.dateTaken = dateTaken;
                qe.cameraID = cameraID;

                if (qe.dateTaken == null)
                {
                    qe.dateTaken = DateTime.Now;
                    
                }
                        
                if (qe.cameraID == null)
                {
                    qe.cameraID = "unknown";
                }
                var ss = request.InputStream;


                byte[] buffer = new byte[16000];
                int totalCount = 0;

                while (true)
                {
                    int currentCount = ss.Read(buffer, totalCount, buffer.Length - totalCount);
                    if (currentCount == 0)
                        break;

                    totalCount += currentCount;
                    if (totalCount == buffer.Length)
                        Array.Resize(ref buffer, buffer.Length * 2);
                }

                Array.Resize(ref buffer, totalCount);
                qe.img = buffer;

                var imgms = new MemoryStream(buffer);
                
                Image img = new Bitmap(imgms);
                
                bool found = false;
                float[,,] boxes = null;
                float[,] scores = null;
                float[,] classes = null;
                float[] num = null;

                //Console.WriteLine("about to examine " + qe.filename);
                //Console.WriteLine("items in queue: {0} ", qt.Count);
                //
                if (!md.isLoaded())
                {
                    md.LoadModel(AppDomain.CurrentDomain.BaseDirectory + "frozen_inference_graph.pb", AppDomain.CurrentDomain.BaseDirectory + "object-detection.pbtxt");
                }
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                myEncoderParameters = new EncoderParameters(1);

                myEncoderParameter = new EncoderParameter(myEncoder, 99L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                
                DateTime start = DateTime.Now;

                
                md.examine(img, qe.filename, ref boxes, ref scores, ref classes, ref num, ref found);

                //Console.WriteLine("elapsed time: {0}", DateTime.Now - start);
                //totalImagesProcessed++;

                if (found)
                {
                    //upload original image to file storage



                    //Console.WriteLine("object detected");

                    //put score at front of filename
                    float highscore = scores[0, 0];
                    for (int f = 0; f < scores.Length - 1; f++)
                    {
                        if (classes[0,f]==1)
                        {
                            //found a meteor
                            highscore = scores[0, f];
                            break ;
                        }
                    }
                       
                    
                    string hs = highscore.ToString(".00");

                    qe.filename = hs.Substring(1) + qe.filename;
                    qe.filename = qe.filename.Replace("bmp", "jpg");
                    qe.filename = qe.filename.Replace("png", "jpg");
                    qe.filename = Path.GetFileName(qe.filename);
                    Bitmap b;
                   // using (var imagems = new MemoryStream(qe.img))
                   // {
                        b = new Bitmap(img);
                    //}
                    //Console.WriteLine("about to save bmp");
                    Bitmap c = new Bitmap(b);
                    c.Save("e:\\found\\" + qe.filename, jgpEncoder, myEncoderParameters);
                    string width, height;
                    width = Convert.ToString(c.Width);
                    height = Convert.ToString(c.Height);
                    b.Dispose();
                    XElement boxxml = md.GetBoxesXML(boxes, scores, classes, qe.filename,width,height);
                   
                    
                   
                    boxxml.FirstNode.AddAfterSelf(new XElement("camera", qe.cameraID));
                    boxxml.FirstNode.AddAfterSelf(new XElement("dateTaken", qe.dateTaken));
                    boxxml.Save("e:\\found\\" + qe.filename.Replace("jpg", "xml"));

                    //create skyimage object

                    SkyImage si = new SkyImage();
                    si.width = c.Width;
                    si.height = c.Height;
                    si.camera = qe.cameraID;
                    si.date = qe.dateTaken;
                    si.filename = qe.filename;
                    //si.skyImageId = DateTime.Now.Second;
                    int skyObjectId = 0;
                    int bbId = 0;
                    si.detectedObjects = new List<SkyObjectDetection>();

                    foreach (XElement xe in boxxml.Elements())
                    {
                        if (xe.Name == "object")
                        {
                            var newObject = new SkyObjectDetection();
                            skyObjectId = skyObjectId + 1;
                            //newObject.skyObjectID = skyObjectId;
                            
                            foreach (XElement xe2 in xe.Elements())
                            {
                                switch (xe2.Name.ToString())
                                {
                                    case "name":
                                        newObject.skyObjectClass = xe2.Value;
                                        break;
                                    case "score":
                                        newObject.score = Convert.ToDecimal(xe2.Value);
                                        break;
                                    case "bndbox":
                                        var newBBox = new BoundingBox();
                                        bbId++;
                                        foreach (XElement xe3 in xe2.Elements())
                                        {
                                            switch (xe3.Name.ToString())
                                            {

                                                case "xmin":
                                                    newBBox.xmin = int.Parse(xe3.Value);
                                                    break;
                                                case "xmax":
                                                    newBBox.xmax = int.Parse(xe3.Value);
                                                    break;
                                                case "ymin":
                                                    newBBox.ymin = int.Parse(xe3.Value);
                                                    break;
                                                case "ymax":
                                                    newBBox.ymax = int.Parse(xe3.Value);
                                                    break;
                                                default:

                                                    break;
                                            }
                                        }
                                        //newBBox.boundingBoxId = bbId;
                                        newObject.bbox = newBBox;
                                        break;
                                    default:
                                        break;


                                }
                               

                               
                            }
                            si.detectedObjects.Add(newObject);


                        }

                    }


                    //push to cloud for further analysis
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://imageingest.azurewebsites.net/api/SkyImages");

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    var ims = new MemoryStream();
                    string base64String = Convert.ToBase64String(buffer);
                    var imgdata = new ImageData();

                    imgdata.imageData = base64String;
                    si.imageData = imgdata;
                    var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                    var ms = new MemoryStream();
                    //string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
                    //var jsonSerializerSettings = new DataContractJsonSerializerSettings
                    //{
                    //    DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat(dateTimeFormat)
                    //};
                    var settings = new DataContractJsonSerializerSettings();



                    settings.DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ");

                    // var json = JsonConvert.SerializeObject(DateTime.Now, settings);

                    var ser = new DataContractJsonSerializer(typeof(SkyImage), settings);
                    ser.WriteObject(ms, si);
                    ms.Position = 0;

                    string json = Encoding.Default.GetString(ms.ToArray());

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                    File.WriteAllText("c:\\json.txt", json);
                    ms.Close();
                    var result = "";
                    try
                    {
                        using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                        {
                            if (httpWebRequest.HaveResponse && response != null)
                            {
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    result = reader.ReadToEnd();
                                    Console.WriteLine("sent to cloud");
                                }
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        if (e.Response != null)
                        {
                            using (var errorResponse = (HttpWebResponse)e.Response)
                            {
                                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                {
                                    string error = reader.ReadToEnd();
                                    result = error;
                                }
                            }

                        }
                    }




                    c.Dispose();

                    //send to cloud


                }
                else
                {
                    Console.WriteLine("nothing found");
                //    Bitmap b;
                //    b = new Bitmap(i);
                //    Console.WriteLine("about to save bmp");
                //    Bitmap c = new Bitmap(b);
                //    c.Save("e:\\notfound\\" + qe.filename);
                }
                ss.Close();

                //
                qe = null;
                //
            
                //return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                //get image and filename
                return "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine("sendresponse: " + e.Message);
                return "ok";
            }

        }

        private static void Start(string[] args)
        {
            // onstart code here
            running = true;
            ws.Run();
            //Console.WriteLine("A simple webserver. Press a key to quit.");
            //Console.ReadKey();
            //while (running)
            //{
            //    System.Threading.Thread.Sleep(5000);
            //}

            //ws.Stop();
        }

        private static void Stop()
        {
            // onstop code here
            //Console.WriteLine("A simple webserver. Press a key to quit.");
            //Console.ReadKey();
            running = false;
            ws.Stop();
        }



        private static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
                // running as service
                using (var service = new Service())
                { 
                    ServiceBase.Run(service);
                }
            else
            {
                
                ws.Run();
                //Console.WriteLine("A simple webserver. Press a key to quit.");
                Console.ReadKey();
                ws.Stop();
            }
        }
    }
}