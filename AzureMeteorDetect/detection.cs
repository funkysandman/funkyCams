using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureMeteorDetect
{
    public class queueEntry
    {
        public byte[] img;
        public string filename;
      
    }


    public  class detection
    {
        private static Queue qt = new Queue();
        private static bool queueRunning = false;
        private static Thread t = new Thread(processImages);
        private static object syncLock = new object();
        private static object syncLock2 = new object();
        private static ObjectDetection.TFDetector md = new ObjectDetection.TFDetector();
        private static ImageCodecInfo jgpEncoder;
        private static EncoderParameters myEncoderParameters;
        private static EncoderParameter myEncoderParameter;
        private static System.Drawing.Imaging.Encoder myEncoder;
        private static Microsoft.Azure.WebJobs.ExecutionContext ec;
        private static int queued = 0;
        private static int totalImagesProcessed = 0;
        [FunctionName("detection")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, Microsoft.Azure.WebJobs.ExecutionContext executionContext)
        {

            ec = executionContext;

            
            // parse query parameter
            string filename = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
                .Value;
            log.Info("file:" + filename);
            log.Info("images processed count:" + totalImagesProcessed);
            if (filename != null)
            {
                // Get request body


                
                AzureMeteorDetect.queueEntry qe = new AzureMeteorDetect.queueEntry();
                qe.filename = filename;
                qe.img = await req.Content.ReadAsByteArrayAsync();

               // dynamic data = await req.Content.ReadAsAsync<object>();
                //string photoBase64String = data.photoBase64;

                //Image img = Image.FromFile(name);
                //byte[] arr;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //    arr = ms.GetBuffer();
                //}
                    log.Info("file size:" + qe.img.Length);
               //qe.img = data;

                lock (syncLock)
                {
                    popQueue(ref qe,false);
                    if ( t.ThreadState!=ThreadState.Running)
                    {



                        try
                        {

                            
                           

                            t.Start();
                            log.Info("starting queue thread");
                            
                        }
                        catch (Exception e)
                        {
                            log.Info(e.Message);
                        }



                    }

                }
            }

            else
            {
                log.Info("no file ");
            }
            return filename == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + filename);
        }

        public static void processImages()
        {
            queueRunning = true;
            //CloudBlobClient client;
            //CloudBlobContainer container;
            while (true)
            {
                if (qt.Count > 0)
                {
                    //queueEntry anEntry = (queueEntry)qt.Dequeue();
                    //AzureMeteorDetect.queueEntry qe = (AzureMeteorDetect.queueEntry)anEntry;
                    queueEntry qe = new queueEntry();
                    popQueue(ref qe, true);
                    bool found = false;
                    float[,,] boxes = null;
                    float[,] scores = null;
                    float[,] classes = null;
                    float[] num = null;

                    Console.WriteLine("about to examine " + qe.filename);

                    //
                    if (!md.isLoaded())
                    {
                        md.LoadModel(ec.FunctionAppDirectory + "\\frozen_inference_graph.pb", ec.FunctionAppDirectory + "\\object-detection.pbtxt");

                        myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                        myEncoderParameters = new EncoderParameters(1);

                        myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                    }
                    //md.examine(qe.img, qe.filename, ref boxes, ref scores, ref classes, ref num, ref found);
                    //
                    totalImagesProcessed++;

                    if (found)
                    {
                        //upload original image to file storage



                        Console.WriteLine("object detected");


                        //Microsoft.WindowsAzure.Storage
                        //    .CloudStorageAccount storageAccount = CloudStorageAccount
                        //    .Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);

                        //Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=meteorshots;AccountKey=M+rGNU1Ija+Zrs09fVL8FiVj+HVWkx1ji4MvRcSC0Yaa/G+A+MOdN3rAWWCMu8pLBBrFfxM8K4d68FBbsTOmYw==;EndpointSuffix=core.windows.net");
                        //client = storageAccount.CreateCloudBlobClient();
                        //container = client.GetContainerReference("found");





                        //container.CreateIfNotExistsAsync(
                        //  BlobContainerPublicAccessType.Blob,
                        //  new BlobRequestOptions(),
                        //  new OperationContext());
                        qe.filename = qe.filename.Replace("bmp", "png");
                        qe.filename = qe.filename.Replace("jpg", "png");
                        qe.filename = Path.GetFileName(qe.filename);
                        // CloudBlockBlob blob = container.GetBlockBlobReference(Path.GetFileName(qe.filename));
                        //blob.Properties.ContentType = "image/png";
                        Bitmap b;
                        using (var imagems = new MemoryStream(qe.img))
                        {
                            b = new Bitmap(imagems);
                        }
                        Console.WriteLine("about to save png");

                        b.Save("e:\\found\\" + qe.filename);

                        qe.filename = qe.filename.Replace("png", "jpg");
                        md.DrawBoxes(boxes, scores, classes, ref b, qe.filename, .35, false);

                        Console.WriteLine("about to save jpg");
                        b.Save("e:\\found\\" + qe.filename, jgpEncoder, myEncoderParameters);


                        b.Dispose();

                    }
                    else { Console.WriteLine("nothing found"); }

                    //
                    qe = null;

                }
                Thread.Sleep(50);
            }
            queueRunning = false;
        
        }
        //public class Startup : IExtensionConfigProvider
        //{
        //    public void Initialize(ExtensionConfigContext context)
        //    {
        //        // Put your intialization code here.
        //    }
        //}
        public static queueEntry popQueue(ref queueEntry qe, bool popoff)

        {
            lock (syncLock2)
            {
                if (popoff)
                {
                    queueEntry anEntry = (queueEntry)qt.Dequeue();
                    qe = (AzureMeteorDetect.queueEntry)anEntry;
                }
                else
                {

                    qt.Enqueue(qe);
                }
            }

            return qe;
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
    }
}
