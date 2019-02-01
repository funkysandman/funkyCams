using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

using ExampleCommon;
using Mono.Options;
using System.Reflection;
using System.Net;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;






namespace ExampleObjectDetection
{


    class Program
    {
        private static IEnumerable<CatalogItem> _catalog;
        private static string _currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string _input = Path.Combine(_currentDir, "test_images/input.jpg");
        private static string _output = Path.Combine(_currentDir, "test_images/output.jpg");
        private static string _catalogPath;
        private static string _modelPath;

        private static double MIN_SCORE_FOR_OBJECT_HIGHLIGHTING = 0.3;

        static OptionSet options = new OptionSet()
        {
            { "input_image=",  "Specifies the path to an image ", v => _input = v },
            { "output_image=",  "Specifies the path to the output image with detected objects", v => _output = v },
            { "catalog=", "Specifies the path to the .pbtxt objects catalog", v=> _catalogPath = v},
            { "model=", "Specifies the path to the trained model", v=> _modelPath = v},
            { "h|help", v => Help () }
        };

        /// <summary>
        /// Run the ExampleObjectDetection util from command line. Following options are available:
        /// input_image - optional, the path to the image for processing (the default is 'test_images/input.jpg')
        /// output_image - optional, the path where the image with detected objects will be saved (the default is 'test_images/output.jpg')
        /// catalog - optional, the path to the '*.pbtxt' file (by default, 'mscoco_label_map.pbtxt' been loaded)
        /// model - optional, the path to the '*.pb' file (by default, 'frozen_inference_graph.pb' model been used, but you can download any other from here 
        /// https://github.com/tensorflow/models/blob/master/object_detection/g3doc/detection_model_zoo.md or train your own)
        /// 
        /// for instance, 
        /// ExampleObjectDetection --input_image="/demo/input.jpg" --output_image="/demo/output.jpg" --catalog="/demo/mscoco_label_map.pbtxt" --model="/demo/frozen_inference_graph.pb"
        /// </summary>
        /// <param name="args"></param>
        /// 
        //static void  Main (string [] args)
        //{
        //	options.Parse (args);

        //	if (_catalogPath == null) {
        //		_catalogPath = DownloadDefaultTexts (_currentDir);
        //	}

        //	if (_modelPath == null) {
        //		_modelPath = DownloadDefaultModel (_currentDir);
        //	}

        //	_catalog = CatalogUtil.ReadCatalogItems (_catalogPath);
        //	var fileTuples = new List<(string input, string output)> () { (_input, _output) };

        //          //loop through all files in dir


        //          //var x1 = 255;
        //          //Console.WriteLine(sigmoid(x1));




        //          string modelFile = _modelPath;
        //          string outfile = "";
        //          TFTensor tensor;
        //          TFSession.Runner runner;
        //          DateTime from_date = DateTime.Now.AddHours(-1600000);
        //          DateTime to_date = DateTime.Now;
        //          float detectedClass = 0;
        //          int c = 0;
        //          using (var graph = new TFGraph ()) {
        //		var model = File.ReadAllBytes (modelFile);
        //		graph.Import (new TFBuffer (model));

        //              using (var session = new TFSession (graph)) {
        //                  var directory = new DirectoryInfo("c:\\test");
        //                  var files = directory.GetFiles("*.bmp").Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
        //                  foreach ( FileInfo afile in files)
        //                  {
        //                      string file = afile.FullName;

        //                      Bitmap b = new Bitmap(file);
        //                      //float[,] trans = { { 1f, 1f,1f }, { 1f, 1f ,1f}, { 1f, 1f ,1f}, { 1f, 1f ,1f} };
        //                      //float[,] lut = { { .1f, .2f,.3f },{ .3f, .5f,2.5f },{ .1f, 1.2f,1.1f } };
        //                      //PolynomialTransform(b, trans, lut);


        //                      //b.Save(Path.GetDirectoryName(file) + "\\transform\\" + Path.GetFileName(file));
        //                      //split into 4 tiles..check each one
        //                      var t = 1;
        //                      for (int tilex = 0; tilex < t; tilex++)
        //                      {
        //                          for (int tiley = 0; tiley < t; tiley++)
        //                          {

        //                              Rectangle cloneRect = new Rectangle(b.Width * tilex / t, b.Height * tiley / t, b.Width / t, b.Height / t);
        //                              System.Drawing.Imaging.PixelFormat format =
        //                                  b.PixelFormat;
        //                              Bitmap img = b.Clone(cloneRect, format);

        //                              // img = AdjustCurves(img);ci
        //                              // img = AdjustGamma(img, 1.1f);
        //                              // create filter
        //                              //AForge.Imaging.Filters.GaussianSharpen filter2 = new AForge.Imaging.Filters.GaussianSharpen();
        //                              //// process image
        //                              //filter2.ApplyInPlace(img);


        //                              //get median


        //                              //AForge.Imaging.ImageStatistics stat = new AForge.Imaging.ImageStatistics(img);
        //                              // get red channel's histogram


        //                              int mean;

        //                              //mean = (stat.Blue.Median + stat.Green.Median + stat.Red.Median) / 3;



        //                              //effects effects = new effects(); //contains all the basic effects 
        //                              //layers layers = new layers(); //photoshop like layer implementation

        //                              //BitmapW a = new BitmapW(file); //load an image
        //                              //BitmapW res;


        //                              //res = effects.curves(a, new Point(0, 0), new Point(mean-5, mean - 10), new Point(mean + 5, mean + 10), new Point(255, 255));


        //                              // create filter
        //                              //AForge.Imaging.Filters.GammaCorrection filter = new AForge.Imaging.Filters.GammaCorrection(1.2);
        //                              // apply the filter
        //                              //filter.ApplyInPlace(img);

        //                              //AForge.Imaging.Filters.ContrastStretch filter = new AForge.Imaging.Filters.ContrastStretch();
        //                              //// process image
        //                              //filter.ApplyInPlace(img);
        //                              //AForge.Imaging.Filters.HistogramEqualization filter2 = new AForge.Imaging.Filters.HistogramEqualization();
        //                              //// process image
        //                              //filter2.ApplyInPlace(img);
        //                              MemoryStream ms = new MemoryStream();

        //                              img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //                              // img.Save(Path.GetDirectoryName(file) + "\\transform\\" + Path.GetFileName(file), System.Drawing.Imaging.ImageFormat.Jpeg);
        //                              var contents = ms.ToArray();

        //                              //Azure function test

        //                              //   HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(apiURL));

        //                              //var body = "foo";

        //                              callFunction(contents, file);


        //                              // ByteArrayContent byteContent = new ByteArrayContent(ms.GetBuffer());
        //                              // request.Content = byteContent;

        //                              //HttpWebResponse response = client.PostAsync

        //                              //request.Content = new HttpStringContent(content, Utf8, "application/json");

        //                              //client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

        //                              //HttpResponseMessage response = await client.SendRequestAsync(request);

        //                              //var responseString = response.Content.ReadAsStringAsync().GetResults();

        //                              //
        //                          }

        //                      }
        //                              //tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
        //                              //runner = session.GetRunner();

        //                              //runner
        //                              //    .AddInput(graph["image_tensor"][0], tensor)
        //                              //    .Fetch(
        //                              //    graph["detection_boxes"][0],
        //                              //    graph["detection_scores"][0],
        //                              //    graph["detection_classes"][0],
        //                              //    graph["num_detections"][0]);
        //                              //var output = runner.Run();

        //                              //var boxes = (float[,,])output[0].GetValue(jagged: false);
        //                              //var scores = (float[,])output[1].GetValue(jagged: false);
        //                              //var classes = (float[,])output[2].GetValue(jagged: false);
        //                              //var num = (float[])output[3].GetValue(jagged: false);
        //                              //bool found = false;

        //                              //float maxscore = 0;
        //                              //c = 0;
        //                              //foreach (float score in scores)
        //                              //{

        //                              //    if (score > maxscore)
        //                              //    {
        //                              //        maxscore = score;
        //                              //        detectedClass = classes[0, c];
        //                              //    }
        //                              //    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING && detectedClass==1)
        //                              //    {
        //                              //        found = true;
        //                              //        // maxClass = classes[i];
        //                              //    }
        //                              //    c++;
        //                              //}
        //                              //Console.WriteLine(file + " " + maxscore);
        //                              //outfile = Path.GetDirectoryName(file) + "\\found\\" + Path.GetFileName(file);
        //                              //System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(file), "found"));
        //                              //if (found)
        //                              //{
        //                              //    DrawBoxes(boxes, scores, classes, img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
        //                              //    Console.WriteLine("found meteor in " + file);
        //                              //}
        //                              //tensor.Dispose();
        //                              //if (!found)
        //                              //{
        //                              //    ////try flipping image horizontally
        //                              //    //ms.Close();

        //                              //    //img.RotateFlip(RotateFlipType.RotateNoneFlipX);
        //                              //    //ms = new MemoryStream();
        //                              //    //img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                              //    //contents = ms.ToArray();
        //                              //    ////

        //                              //    //tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
        //                              //    //runner = session.GetRunner();

        //                              //    //runner
        //                              //    //    .AddInput(graph["image_tensor"][0], tensor)
        //                              //    //    .Fetch(
        //                              //    //    graph["detection_boxes"][0],
        //                              //    //    graph["detection_scores"][0],
        //                              //    //    graph["detection_classes"][0],
        //                              //    //    graph["num_detections"][0]);
        //                              //    //output = runner.Run();
        //                              //    //img.RotateFlip(RotateFlipType.RotateNoneFlipX);//flip back 
        //                              //    //boxes = (float[,,])output[0].GetValue(jagged: false);
        //                              //    //scores = (float[,])output[1].GetValue(jagged: false);
        //                              //    //classes = (float[,])output[2].GetValue(jagged: false);
        //                              //    //num = (float[])output[3].GetValue(jagged: false);
        //                              //    //found = false;
        //                              //    //maxscore = 0;

        //                              //    //c = 0;
        //                              //    //foreach (float score in scores)
        //                              //    //{
        //                              //    //    if (score > maxscore)
        //                              //    //    {
        //                              //    //        maxscore = score;
        //                              //    //        detectedClass = classes[0, c];
        //                              //    //    }
        //                              //    //    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING && detectedClass == 1)
        //                              //    //        found = true;
        //                              //    //    c++;
        //                              //    //}
        //                              //    //Console.WriteLine(file + " " + maxscore);

        //                              //    //outfile = Path.GetDirectoryName(file) + "\\found\\x" + Path.GetFileName(file);
        //                              //    ////var  i=0;
        //                              //    //if (found)
        //                              //    //{
        //                              //    //    //reverse boxes horizontally
        //                              //    //    //foreach (float box in boxes)
        //                              //    //    //{
        //                              //    //    //    Console.WriteLine(box);
        //                              //    //    //    i++;
        //                              //    //    //}
        //                              //    //    DrawBoxes(boxes, scores, classes, img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, true);
        //                              //    //    Console.WriteLine("found meteor in " + file);
        //                              //    //}
        //                              //    //else
        //                              //    //{
        //                              //   // img.Save(Path.GetDirectoryName(file) + "\\not_found\\" + Path.GetFileName(file), System.Drawing.Imaging.ImageFormat.Jpeg);

        //                              //    //}
        //                              //    //tensor.Dispose();
        //                              //    //img.Dispose();

        //                              //}

        //                       //   }
        //                     // }
        //			}
        //		}
        //	}
        //          Console.ReadLine();
        //      }

        public static async void callFunction(byte[] contents, string file)
        {
            var apiURL = "http://localhost:7071/api/detection?file=" + file;
            HttpClient client = new HttpClient();

            ByteArrayContent byteContent = new ByteArrayContent(contents);
            HttpResponseMessage response = await client.PostAsync(apiURL, byteContent);



        }
        //public static Bitmap AdjustContrast(Image Image, float Value)
        //{
        //    Value = (100.0f + Value) / 100.0f;
        //    Value *= Value;
        //    Bitmap NewBitmap = (Bitmap)Image.Clone();
        //    BitmapData data = NewBitmap.LockBits(
        //        new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
        //        ImageLockMode.ReadWrite,
        //        NewBitmap.PixelFormat);
        //    int Height = NewBitmap.Height;
        //    int Width = NewBitmap.Width;

        //    unsafe
        //    {
        //        for (int y = 0; y < Height; ++y)
        //        {
        //            byte* row = (byte*)data.Scan0 + (y * data.Stride);
        //            int columnOffset = 0;
        //            for (int x = 0; x < Width; ++x)
        //            {
        //                byte B = row[columnOffset];
        //                byte G = row[columnOffset + 1];
        //                byte R = row[columnOffset + 2];

        //                float Red = R / 255.0f;
        //                float Green = G / 255.0f;
        //                float Blue = B / 255.0f;
        //                Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
        //                Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
        //                Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

        //                int iR = (int)Red;
        //                iR = iR > 255 ? 255 : iR;
        //                iR = iR < 0 ? 0 : iR;
        //                int iG = (int)Green;
        //                iG = iG > 255 ? 255 : iG;
        //                iG = iG < 0 ? 0 : iG;
        //                int iB = (int)Blue;
        //                iB = iB > 255 ? 255 : iB;
        //                iB = iB < 0 ? 0 : iB;

        //                row[columnOffset] = (byte)iB;
        //                row[columnOffset + 1] = (byte)iG;
        //                row[columnOffset + 2] = (byte)iR;

        //                columnOffset += 3;
        //            }
        //        }
        //    }

        //    NewBitmap.UnlockBits(data);

        //    return NewBitmap;
        //}

        //public static Bitmap AdjustCurves(Image Image)
        //{
        //    //Value = (100.0f + Value) / 100.0f;
        //    //Value *= Value;

        //    //get mean value for red,green,blue



        //    Bitmap NewBitmap = (Bitmap)Image.Clone();
        //    BitmapData data = NewBitmap.LockBits(
        //        new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
        //        ImageLockMode.ReadWrite,
        //        NewBitmap.PixelFormat);
        //    int Height = NewBitmap.Height;
        //    int Width = NewBitmap.Width;
        //    int total=0;
        //    int meanIntensity = 0;
        //    int adjustment = 0;
        //    unsafe
        //    {

        //        for (int y = 0; y < Height; ++y)
        //        {
        //            byte* row = (byte*)data.Scan0 + (y * data.Stride);
        //            int columnOffset = 0;
        //            for (int x = 0; x < Width; ++x)
        //            {
        //                byte B = row[columnOffset];
        //                byte G = row[columnOffset + 1];
        //                byte R = row[columnOffset + 2];
        //                total = total + B + G + R;
        //                                     columnOffset += 3;
        //            }
        //        }

        //        meanIntensity = (int)( total / (Height * Width*3));
        //        adjustment = 30 - meanIntensity;
        //        //adjust the mean
        //        for (int y = 0; y < Height; ++y)
        //        {
        //            byte* row = (byte*)data.Scan0 + (y * data.Stride);
        //            int columnOffset = 0;
        //            for (int x = 0; x < Width; ++x)
        //            {
        //                byte B = row[columnOffset];
        //                byte G = row[columnOffset + 1];
        //                byte R = row[columnOffset + 2];



        //                int iB = B + ( adjustment);
        //                int iG = G + ( adjustment); 
        //                int iR = R + ( adjustment);

        //                if (iB < 0) iB = 0;
        //                if (iG < 0) iG = 0;
        //                if (iR < 0) iR = 0;

        //                if (iB > 255) iB = 255;
        //                if (iG > 255) iG = 255;
        //                if (iR > 255) iR = 255;

        //                row[columnOffset] = (byte)iB;
        //                row[columnOffset + 1] = (byte)iG;
        //                row[columnOffset + 2] = (byte)iR;

        //                columnOffset += 3;
        //            }
        //        }


        //        for (int y = 0; y < Height; ++y)
        //        {
        //            byte* row = (byte*)data.Scan0 + (y * data.Stride);
        //            int columnOffset = 0;
        //            for (int x = 0; x < Width; ++x)
        //            {
        //                byte B = row[columnOffset];
        //                byte G = row[columnOffset + 1];
        //                byte R = row[columnOffset + 2];



        //                int iB = (int)(sigmoid((double)(B/5-(meanIntensity*.2+4))) * 255);
        //                int iG = (int)(sigmoid((double)(G/5-(meanIntensity * .2 + 4))) * 255);
        //                int iR = (int)(sigmoid((double)(R/5-(meanIntensity * .2 + 4))) * 255);
        //                row[columnOffset] = (byte)iB;
        //                row[columnOffset + 1] = (byte)iG;
        //                row[columnOffset + 2] = (byte)iR;

        //                columnOffset += 3;
        //            }
        //        }
        //    }

        //    NewBitmap.UnlockBits(data);

        //    return NewBitmap;
        //}

        private static string DownloadDefaultModel(string dir)
        {
            string defaultModelUrl = ConfigurationManager.AppSettings["DefaultModelUrl"] ?? throw new ConfigurationErrorsException("'DefaultModelUrl' setting is missing in the configuration file");

            var modelFile = Path.Combine(dir, "ssd_mobilenet_v1_coco_2017_11_17/frozen_inference_graph.pb");
            var zipfile = Path.Combine(dir, "ssd_mobilenet_v1_coco_2017_11_17.tar.gz");

            // var modelFile = Path.Combine(dir, "c:\tmp\frozen_inference_graph.pb");
            // var zipfile = Path.Combine(dir, "ssd_mobilenet_v1_coco_2017_11_17.tar.gz");

            if (File.Exists(modelFile))
                return modelFile;

            if (!File.Exists(zipfile))
            {
                var wc = new WebClient();
                wc.DownloadFile(defaultModelUrl, zipfile);
            }

            ExtractToDirectory(zipfile, dir);
            File.Delete(zipfile);

            return modelFile;
        }


        //public static unsafe void PolynomialTransform(Bitmap objBitmap,  float[,] Transform, float[,] LUT)
        //{
        //    //Do LUT and polynomial transform on whole image ... 
        //    //Use unmanaged pointers to go FAST. In Memory RGB values 
        //    //are stored in the order B-G-R!!!!!! 

        //    int x, y;
        //    IntPtr iPtrPixel0;


        //    Rectangle rect = new Rectangle(0, 0, objBitmap.Width, objBitmap.Height);
        //    System.Drawing.Imaging.BitmapData bitmapData = objBitmap.LockBits(rect,
        //    System.Drawing.Imaging.ImageLockMode.ReadWrite,
        //    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        //    iPtrPixel0 = bitmapData.Scan0;


        //    //Byte pointer and corresponding managed array type 
        //    byte* byPixel;
        //    byte[] bySafePixel = new byte[3];

        //    for (y = 0; y < bitmapData.Height; y++)
        //    {
        //        byPixel = (byte*)(iPtrPixel0 + y * bitmapData.Stride);
        //        for (x = 0; x < bitmapData.Width; x++)
        //        {
        //            bySafePixel[2] = *byPixel;
        //            bySafePixel[1] = *(byPixel + 1);
        //            bySafePixel[0] = *(byPixel + 2);

        //            ColorLib.colorlib.GammaRGBToGammasRGB(bySafePixel, Transform, LUT);
        //            *byPixel = bySafePixel[2];
        //            byPixel++;
        //            *byPixel = bySafePixel[1];
        //            byPixel++;
        //            *byPixel = bySafePixel[0];
        //            byPixel++;
        //        }
        //    }
        //    objBitmap.UnlockBits(bitmapData);
        //}

        private static void ExtractToDirectory(string file, string targetDir)
        {
            using (Stream inStream = File.OpenRead(file))
            using (Stream gzipStream = new GZipInputStream(inStream))
            {
                TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
                tarArchive.ExtractContents(targetDir);
            }
        }

        private static string DownloadDefaultTexts(string dir)
        {
            dir = "\\tmp";
            string defaultTextsUrl = ConfigurationManager.AppSettings["DefaultTextsUrl"] ?? throw new ConfigurationErrorsException("'DefaultTextsUrl' setting is missing in the configuration file");
            var textsFile = Path.Combine(dir, "mscoco_label_map.pbtxt");
            var wc = new WebClient();
            wc.DownloadFile(defaultTextsUrl, textsFile);

            return textsFile;
        }

        private static void DrawBoxes(float[,,] boxes, float[,] scores, float[,] classes, Bitmap inputFile, string outputFile, double minScore, Boolean flip)
        {
            var x = boxes.GetLength(0);
            var y = boxes.GetLength(1);
            var z = boxes.GetLength(2);

            float ymin = 0, xmin = 0, ymax = 0, xmax = 0;

            using (var editor = new ImageEditor(ref inputFile, outputFile))
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (scores[i, j] < minScore) continue;

                        for (int k = 0; k < z; k++)
                        {
                            var box = boxes[i, j, k];
                            switch (k)
                            {
                                case 0:
                                    ymin = box;
                                    break;
                                case 1:
                                    xmin = box;
                                    break;
                                case 2:
                                    ymax = box;
                                    break;
                                case 3:
                                    xmax = box;
                                    break;
                            }

                        }

                        int value = Convert.ToInt32(classes[i, j]);
                        CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == value);
                        if (catalogItem.DisplayName == "meteor")
                        {
                            //if (flip)
                            //{ editor.AddBoxFlip(xmin, xmax, ymin, ymax, $"{catalogItem.DisplayName}: {(scores[i, j] * 100).ToString("0")}%"); }
                            //else
                            //{
                                editor.AddBox(xmin, xmax, ymin, ymax, $"{catalogItem.DisplayName} : {(scores[i, j] * 100).ToString("0")}%");
                            //}
                        }
                    }
                }
            }
        }

        private static void Help()
        {
            options.WriteOptionDescriptions(Console.Out);
        }
        private static Bitmap AdjustGamma(Image image, float gamma)
        {
            // Set the ImageAttributes object's gamma value.
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetGamma(gamma);

            // Draw the image onto the new bitmap
            // while applying the new gamma value.
            Point[] points =
            {
        new Point(0, 0),
        new Point(image.Width, 0),
        new Point(0, image.Height),
        };
            Rectangle rect =
                new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }
        private static Bitmap AdjustCurves(Image image, float gamma)
        {
            // Set the ImageAttributes object's gamma value.
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetGamma(gamma);

            // Draw the image onto the new bitmap
            // while applying the new gamma value.
            Point[] points =
            {
        new Point(0, 0),
        new Point(image.Width, 0),
        new Point(0, image.Height),
        };
            Rectangle rect =
                new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect,
                    GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }
        public static double sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}
