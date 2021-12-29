
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using Models;
using System.Xml.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace pushImagesToDB
{
    static class   Program
    {

        static void Main(string[] args)
        {

            DateTime from_date = DateTime.Now.AddHours(-189999);
            DateTime to_date = DateTime.Now;
            var directory = new DirectoryInfo("e:\\meteor_corpus");
            bool pushToCloud = true;
            bool createYOLO = false;
            byte[] buffer;

            var files = directory.GetFiles("*.jpg"); //.Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);

            foreach (FileInfo afile in files)
            {
                if (afile != null)
                {
                    string file = afile.Name;
                    Bitmap b = new Bitmap(afile.FullName);
                    
                    //
                    //encode - push to cloud
                    SkyImage si = new SkyImage();
                    si.width = b.Width;
                    si.height = b.Height;
                    b.Dispose();
                    buffer = File.ReadAllBytes(afile.FullName);
                    si.camera = "see filename";
                    //try to get date from filename
                    //83img_-06Oct2019-200721.jpg
                    if (afile.Name.Length >= 20)
                    {
                        DateTime aDate;
                        String datestr = afile.Name.Substring(afile.Name.Length - 20, 20);
                        datestr = datestr.Replace(".jpg", "");
                        try
                        {
                            aDate = DateTime.ParseExact(datestr, "ddMMMyyyy-HHmmss", null);
                            si.date = aDate;
                        }
                        catch
                        {
                            si.date = DateTime.MinValue;
                        }
                       
                    }
                    else
                    {
                        si.date = DateTime.MinValue;
                    }
                    si.filename = file;
                    //si.skyImageId = DateTime.Now.Second;
                    int skyObjectId = 0;
                    int bbId = 0;
                    si.detectedObjects = new List<SkyObjectDetection>();
                    XElement boxxml = XElement.Load(afile.FullName.Replace(".jpg", ".xml"));
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
                                        if (newBBox.xmax>si.width | newBBox.ymax>si.height | newBBox.xmin > si.width | newBBox.ymin > si.height)
                                        {
                                            Console.WriteLine(afile.FullName);
                                        }
                                        break;
                                    default:
                                        break;


                                }



                            }
                            si.detectedObjects.Add(newObject);


                        }

                    }

                    if (pushToCloud)
                    {
                        //push to cloud for further analysis
                        ServicePointManager.Expect100Continue = true;

                        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.199:3333/api/SkyImages");

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



                        //

                        b.Dispose();
                        ms.Close();

                    }
                    if (createYOLO)
                    {
                        //create file
                        string txtfileName = afile.FullName.Replace(".jpg", ".txt");
                        string annotationStr = "";
                        float x1 = 0;
                        float y1 = 0;
                        float xw1 = 0;
                        float yh1 = 0;
                        bool itemsFound = false;
                        

                        foreach (SkyObjectDetection sod in si.detectedObjects)
                        {
                            switch (sod.skyObjectClass)
                            {
                                case "meteor":
                                    annotationStr = annotationStr + "0";
                                    break;
                                case "plane":
                                    annotationStr = annotationStr + "1";
                                    break;
                                case "satellite":
                                    annotationStr = annotationStr + "2";
                                    break;
                                case "flare":
                                    annotationStr = annotationStr + "3";
                                    break;
                                case "flarecurve":
                                    annotationStr = annotationStr + "4";
                                    break;
                                case "flarespike":
                                    annotationStr = annotationStr + "5";
                                    break;
                            }

                                itemsFound = true;
                                   
                                x1 = (float)(sod.bbox.xmax - sod.bbox.xmin) / 2 + (float)sod.bbox.xmin; //center of box
                                y1 = (float)(sod.bbox.ymax - sod.bbox.ymin) / 2 + (float)sod.bbox.ymin; //center of box
                                x1 = x1 / (float)(si.width);
                                y1 = y1 / (float)si.height;
                                if (x1==0 | y1==0)
                                {
                                    x1 = (float)0.0001;
                                    y1 = (float)0.0001;
                                }
                                xw1 = (float)(sod.bbox.xmax- sod.bbox.xmin) / (float)si.width;
                                yh1 = (float)(sod.bbox.ymax- sod.bbox.ymin) / (float)si.height;
                                if (xw1 > 1 | yh1 > 1)
                                {
                                    xw1 = (float)1;
                                    yh1 = (float)1;
                                }
                                annotationStr = annotationStr + " " + x1.ToString() + " " + y1.ToString() + " " + xw1.ToString() + " " + yh1.ToString();
                                annotationStr = annotationStr + "\r\n";
                              
                           

                        }
                        if (itemsFound)
                        {
                            StreamWriter sw = File.CreateText(txtfileName);
                            sw.Write(annotationStr);
                            sw.Close();
                        }
                        


                    }
                }
               
                //split into groups
                //sw.Close();
                //List<FileInfo[]> splitted = new List<FileInfo[]>();//This list will contain all the splitted arrays.
                //int lengthToSplit = 25;

                //int arrayLength = files.Length;

                //for (int i = 0; i < arrayLength; i = i + lengthToSplit)
                //{
                //    FileInfo[] val = new FileInfo[lengthToSplit];

                //    if (arrayLength < i + lengthToSplit)
                //    {
                //        lengthToSplit = arrayLength - i;
                //    }
                //    Array.Copy(files, i, val, 0, lengthToSplit);
                //    splitted.Add(val);
                //}

                //foreach (FileInfo[] f in splitted)
                //{

                //    StartTheThread(f);
                //}




            }
            
        }
        public static Thread StartTheThread(FileInfo[] param1)
        {
            var t = new Thread(() => checkFiles(param1));
            t.Start();
            return t;
        }
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
        public static void   checkFiles(FileInfo[] f)
        {
            foreach (FileInfo afile in f)
            {
                if (afile != null) {
                string file = afile.FullName;
                Bitmap b = new Bitmap(file);
                MemoryStream ms = new MemoryStream();
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var contents = ms.ToArray();
                callFunction(contents, Path.GetFileName(file));

                b.Dispose();
                ms.Close();

            }

            }

        }
        public static  void callFunction(byte[] contents, string file)
        {
            var apiURL = "http://192.168.1.192:7071/api/detection?file=" + file;
            HttpClient client = new HttpClient();
            Debug.WriteLine(file);
            ByteArrayContent byteContent = new ByteArrayContent(contents);
            var response = client.PostAsync(apiURL, byteContent);
            var responseString = (HttpResponseMessage)response.Result;

            Debug.WriteLine(responseString);

        }

    }
}
