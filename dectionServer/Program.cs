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
using Alturos.Yolo;
using System.Diagnostics;

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
            //Program.detector.Run();
            //return;


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
                            catch (Exception e)
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

    internal class Program
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
        //private static Alturos.Yolo.YoloWrapper _yoloWrapper = new YoloWrapper("yolo_meteor.cfg", "yolo_meteor_last.weights", "labels.txt");
        #region Nested classes to support running as service
        public const string ServiceName = "DetectionServer";
        public static DetectionServer ws = new DetectionServer(SendResponse, "http://192.168.1.199:7071/api/detection/");
        //public static TensorFlowNET.Examples.ObjectDetection detector = new TensorFlowNET.Examples.ObjectDetection();
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

        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            try
            {


                long originalPosition = 0;
                MemoryStream ms = new MemoryStream();
                if (stream.CanSeek)
                {
                    originalPosition = stream.Position;
                    stream.Position = 0;
                }

                byte[] buffer = new byte[16383];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                }

                return ms.ToArray();
            }
            catch
            {
                return null;
            }


            //try
            //{
            //    byte[] readBuffer = new byte[4096];

            //    int totalBytesRead = 0;
            //    int bytesRead;

            //    while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            //    {
            //        totalBytesRead += bytesRead;

            //        if (totalBytesRead == readBuffer.Length)
            //        {
            //            int nextByte = stream.ReadByte();
            //            if (nextByte != -1)
            //            {
            //                byte[] temp = new byte[readBuffer.Length * 2];
            //                Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
            //                Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
            //                readBuffer = temp;
            //                totalBytesRead++;
            //            }
            //        }
            //    }

            //    byte[] buffer = readBuffer;
            //    if (readBuffer.Length != totalBytesRead)
            //    {
            //        buffer = new byte[totalBytesRead];
            //        Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            //    }
            //    return buffer;
            //}
            //finally
            //{
            //    if (stream.CanSeek)
            //    {
            //        stream.Position = originalPosition;
            //    }
            //}
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
                if (myParams == null)
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
                    dateTaken = DateTime.ParseExact(myParams[0], "MM/dd/yyyy hh:mm:ss tt", null);
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

                byte[] buffer = ReadToEnd(ss);
                //byte[] buffer = new byte[16000];
                //int totalCount = 0;

                //while (true)
                //{
                //    int currentCount = ss.Read(buffer, totalCount, buffer.Length - totalCount);
                //    if (currentCount == 0)
                //        break;

                //    totalCount += currentCount;
                //    if (totalCount == buffer.Length)
                //        Array.Resize(ref buffer, buffer.Length * 2);
                //}

                //Array.Resize(ref buffer, totalCount);
                qe.img = buffer;

                //note: buffer should be jpeg format


                var imgms = new MemoryStream(buffer);

                Image img = new Bitmap(imgms);
                var bitmapMS = new MemoryStream();
                bool found = false;
                float[,,] boxes = null;
                float[,] scores = null;
                float[,] classes = null;
                float[] num = null;
                bool pushToCloud = true;
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

                //img.Save(bitmapMS,ImageFormat.Bmp);
                //buffer = bitmapMS.ToArray();

                DateTime start = DateTime.Now;

                lock (syncLock) {
                md.examine(qe.img, qe.filename, ref boxes, ref scores, ref classes, ref num, ref found);
                }
                Console.WriteLine("elapsed time: {0}", DateTime.Now - start);
                //totalImagesProcessed++;

                qe.filename = qe.filename.Replace("bmp", "jpg");
                qe.filename = qe.filename.Replace("png", "jpg");
                qe.filename = Path.GetFileName(qe.filename);
                //create skyimage object
                Bitmap b;

                b = new Bitmap(img);
                SkyImage si = new SkyImage();
                si.width = b.Width;
                si.height = b.Height;
                si.camera = qe.cameraID;
                si.date = qe.dateTaken;
                si.filename = qe.filename;
                //si.skyImageId = DateTime.Now.Second;
                int skyObjectId = 0;
                int bbId = 0;
                si.detectedObjects = new List<SkyObjectDetection>();

                if (found)
                {
                    //upload original image to file storage



                    //put score at front of filename
                    float highscore = scores[0, 0];
                    for (int f = 0; f < scores.Length - 1; f++)
                    {
                        if (classes[0, f] == 1)
                        {
                            //found a meteor
                            highscore = scores[0, f];
                            break;
                        }
                    }


                    string hs = highscore.ToString(".00");


                    qe.filename = hs.Substring(1) + qe.filename;
                    //Console.WriteLine("about to save bmp");
                    Bitmap c = new Bitmap(b);
                    try
                    {
                        c.Save("\\found\\" + qe.filename, jgpEncoder, myEncoderParameters);
                    }
                    catch { }

                    string width, height;
                    width = Convert.ToString(c.Width);
                    height = Convert.ToString(c.Height);

                    XElement boxxml = md.GetBoxesXML(boxes, scores, classes, qe.filename, width, height);



                    boxxml.FirstNode.AddAfterSelf(new XElement("camera", qe.cameraID));
                    boxxml.FirstNode.AddAfterSelf(new XElement("dateTaken", qe.dateTaken));
                    boxxml.Save("\\found\\" + qe.filename.Replace("jpg", "xml"));



                    foreach (XElement xe in boxxml.Elements())
                    {
                        if (xe.Name == "object")
                        {
                            var newObject = new SkyObjectDetection();
                            skyObjectId = skyObjectId + 1;
                           

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
                                        newBBox.skyObjectID = skyObjectId;
                                        newObject.bbox = newBBox;
                                        break;
                                    default:
                                        break;


                                }



                            }
                            si.detectedObjects.Add(newObject);


                        }

                    }
                    //check with Yolo model
                }
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                bool yoloFound = false;
                //lock (syncLock)
                //{
                //    var items = _yoloWrapper.Detect(buffer);

                //    stopWatch.Stop();
                //    // Get the elapsed time as a TimeSpan value.
                //    TimeSpan ts = stopWatch.Elapsed;

                //    // Format and display the TimeSpan value.
                //    string elapsedTime = String.Format("{0:00}:{1:00}",
                //        ts.Seconds,
                //        ts.Milliseconds / 10);
                //    Console.WriteLine(qe.filename + " " + elapsedTime);
                //    //

                //    foreach (Alturos.Yolo.Model.YoloItem detection in items)
                //    {

                //        //Rectangle rect = new Rectangle(detection.X, detection.Y, detection.Width, detection.Height);
                //        // Graphics gr = Graphics.FromImage(b);
                //        // Pen p = new Pen(Brushes.LightGreen);
                //        // gr.DrawRectangle(p, rect);
                //        // gr.DrawString(detection.Type + " " + detection.Confidence.ToString(), new Font(FontFamily.GenericSansSerif, 18), Brushes.Red, new PointF(detection.X, detection.Y - 15));
                //        var newObject = new SkyObjectDetection();
                //        skyObjectId = skyObjectId + 1;
                //        var newBBox = new BoundingBox();
                //        newObject.score = Convert.ToDecimal(detection.Confidence);
                //        newObject.skyObjectClass = "yolo:" + detection.Type;
                //        newBBox.xmin = detection.X;
                //        newBBox.ymin = detection.Y;
                //        newBBox.xmax = detection.X + detection.Width;
                //        newBBox.ymax = detection.Y + detection.Height;
                //        newObject.bbox = newBBox;
                //        if (detection.Type == "meteor")
                //            yoloFound = true;
                //        si.detectedObjects.Add(newObject);
                //        //p.Dispose();
                //        //gr.Dispose();
                //    }
                //}
                ////
                if (yoloFound)
                {

                    // b.Save("\\found\\" + "yolo-" + qe.filename, jgpEncoder, myEncoderParameters);
                }
                b.Dispose();
                //}
                //else

                //if (!found)
                //{
                //    Console.WriteLine("nothing found");
                   
                //    b = new Bitmap(img);
                //    Console.WriteLine("about to save bmp");
                //    Bitmap c = new Bitmap(b);
                //    qe.filename = Path.GetFileName(qe.filename);
                //    c.Save("d:\\notfound\\" + qe.filename, ImageFormat.Jpeg);
                //}

                found = found | yoloFound;
                if (pushToCloud & found)
                {
                    //push to cloud for further analysis
                    ServicePointManager.Expect100Continue = true;

                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3333/api/SkyImages");

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    var ims = new MemoryStream();
                    string base64String = Convert.ToBase64String(qe.img);
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




                    b.Dispose();
                }
                //send to cloud

                ss.Close();
                //


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
            //detector.Run();

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