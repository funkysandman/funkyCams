using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using ExampleCommon;
using Mono.Options;
using System.Reflection;
using System.Net;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using TensorFlow;

namespace ObjectDetection
{
    public class TFDetector
    {
        private static IEnumerable<CatalogItem> _catalog;
        private static string _currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string _input = Path.Combine(_currentDir, "test_images/input.jpg");
        private static string _output = Path.Combine(_currentDir, "test_images/output.jpg");
        private static string _catalogPath;
        private static string _modelPath;
        private static TFGraph graph;
        private static double MIN_SCORE_FOR_OBJECT_HIGHLIGHTING = 0.3;
        private static TFSession mySession;
        private TFTensor tensor;
        private TFSession.Runner runner;
        //private static MemoryStream ms = new MemoryStream();
        private System.Drawing.Imaging.Encoder myEncoder;
        private static ImageCodecInfo jgpEncoder;
        private static EncoderParameters myEncoderParameters;
        private static EncoderParameter myEncoderParameter;
        private  bool found;
        private  float[,,] boxes;
        private  float[,] scores;
        private  float[,] classes;
        private  float[] num;
        public  Boolean examining = false;
        public  Boolean drawingBoxes = false;
        //static OptionSet options = new OptionSet()
        //{
        //    { "input_image=",  "Specifies the path to an image ", v => _input = v },
        //    { "output_image=",  "Specifies the path to the output image with detected objects", v => _output = v },
        //    { "catalog=", "Specifies the path to the .pbtxt objects catalog", v=> _catalogPath = v},
        //    { "model=", "Specifies the path to the trained model", v=> _modelPath = v},
        //    { "h|help", v => Help () }
        //};

        public Boolean isExamining()
        {
            return examining;
        }
        public bool isLoaded()
        {
            return  _modelPath != null;
        }
        public void LoadModel(string modelPath, string catPath)
        {
            _modelPath = modelPath;
            _catalogPath = catPath;
            _catalog = CatalogUtil.ReadCatalogItems(_catalogPath);
            graph = new TFGraph();

            var model = File.ReadAllBytes(_modelPath);
            graph.Import(new TFBuffer(model));

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            myEncoderParameters = new EncoderParameters(1);

            myEncoderParameter = new EncoderParameter(myEncoder,  100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            if (!(  mySession is  null))
                mySession.Dispose();
            mySession = new TFSession(graph);
        }

        //public static Bitmap AdjustContrast(Bitmap Image, float Value)
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

        //                columnOffset += 4;
        //            }
        //        }
        //    }

        //    NewBitmap.UnlockBits(data);

        //    return NewBitmap;
        //}

        public void examine(string aFile)
        {
            //options.Parse(args);

            //if (_catalogPath == null)
            //{
            //    _catalogPath = DownloadDefaultTexts(_currentDir);
            //}

            //if (_modelPath == null)
            //{
            //    _modelPath = DownloadDefaultModel(_currentDir);
            //}

            //_catalog = CatalogUtil.ReadCatalogItems(_catalogPath);
            //var fileTuples = new List<(string input, string output)>() { (_input, _output) };

            //pass in filename

            //TFTensor tensor;
            //TFSession.Runner runner;


            string outfile = "";
            float detectedClass = 0;
            int c = 0;
            if (mySession is null)
                mySession = new TFSession(graph);
            using (mySession)
            {



                //Image img = Bitmap.FromFile(aFile);
                var x = Image.FromFile(aFile);


                Bitmap img = new Bitmap(x);

                //img = AdjustContrast(img, 2f);
                //for (int i = 0; i < img.Width; i++)
                //{
                //    for (int j = 0; j < img.Height; j++)
                //    {
                //        Color co = img.GetPixel(i, j);

                //        //Apply conversion equation
                //       // byte gray = (byte)(.21 * co.R + .71 * co.G + .071 * co.B);
                //        byte gray = (byte)(.33 * co.R + .33 * co.G + .33 * co.B);
                //        //Set the color of this pixel
                //        img.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                //    }
                //}
                // img.Save("testc.jpg");
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var contents = ms.ToArray();


                tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                runner = mySession.GetRunner();

                runner
                    .AddInput(graph["image_tensor"][0], tensor)
                    .Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["detection_classes"][0],
                    graph["num_detections"][0]);
                var output = runner.Run();
                

                boxes = (float[,,])output[0].GetValue(jagged: false);
                scores = (float[,])output[1].GetValue(jagged: false);
                classes = (float[,])output[2].GetValue(jagged: false);
                num = (float[])output[3].GetValue(jagged: false);

                
                found = false;

                float maxscore = 0;
                c = 0;
                foreach (float score in scores)
                {

                    if (score > maxscore)
                    {
                        maxscore = score;
                        detectedClass = classes[0, c];
                    }
                    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                    {
                        found = true;
                        // maxClass = classes[i];
                    }
                    c++;
                }

                outfile = Path.GetDirectoryName(aFile) + "\\found\\" + Path.GetFileName(aFile);
                System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
                if (found)
                {
                    DrawBoxes(boxes, scores, classes, ref img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
                    //Console.WriteLine("found meteor in " + aFile);
                }
                tensor.Dispose();
                if (!found)
                {
                    //try flipping image horizontally
                    ms.Close();

                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    contents = ms.ToArray();
                    //

                    tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                    runner = mySession.GetRunner();

                    runner
                        .AddInput(graph["image_tensor"][0], tensor)
                        .Fetch(
                        graph["detection_boxes"][0],
                        graph["detection_scores"][0],
                        graph["detection_classes"][0],
                        graph["num_detections"][0]);
                    output = runner.Run();
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    boxes = (float[,,])output[0].GetValue(jagged: false);
                    scores = (float[,])output[1].GetValue(jagged: false);
                    classes = (float[,])output[2].GetValue(jagged: false);
                    num = (float[])output[3].GetValue(jagged: false);
                    found = false;
                    maxscore = 0;

                    c = 0;
                    foreach (float score in scores)
                    {
                        if (score > maxscore)
                        {
                            maxscore = score;
                            detectedClass = classes[0, c];
                        }
                        if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                            found = true;
                        c++;
                    }
                    Console.WriteLine(aFile + " " + maxscore);

                    outfile = Path.GetDirectoryName(aFile) + "\\found\\x" + Path.GetFileName(aFile);
                    //var  i=0;
                    if (found)
                    {
                        //reverse boxes horizontally
                        //foreach (float box in boxes)
                        //{
                        //    Console.WriteLine(box);
                        //    i++;
                        //}
                        DrawBoxes(boxes, scores, classes, ref img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, true);
                        Console.WriteLine("found meteor in " + aFile);
                    }
                    tensor.Dispose();
                }

            }
        }


        public void examine(Bitmap img, string aFile)
        {
            //options.Parse(args);

            //if (_catalogPath == null)
            //{
            //    _catalogPath = DownloadDefaultTexts(_currentDir);
            //}

            //if (_modelPath == null)
            //{
            //    _modelPath = DownloadDefaultModel(_currentDir);
            //}

            //_catalog = CatalogUtil.ReadCatalogItems(_catalogPath);
            //var fileTuples = new List<(string input, string output)>() { (_input, _output) };

            //pass in filename




            string outfile = "";
            float detectedClass = 0;
            int c = 0;
            if (mySession is null)
                mySession = new TFSession(graph);
            




                //img = AdjustContrast(img, 2f);
                //for (int i = 0; i < img.Width; i++)
                //{
                //    for (int j = 0; j < img.Height; j++)
                //    {
                //        Color co = img.GetPixel(i, j);

                //        //Apply conversion equation
                //       // byte gray = (byte)(.21 * co.R + .71 * co.G + .071 * co.B);
                //        byte gray = (byte)(.33 * co.R + .33 * co.G + .33 * co.B);
                //        //Set the color of this pixel
                //        img.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                //    }
                //}
                // img.Save("testc.jpg");
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var contents = ms.ToArray();


                tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                runner = mySession.GetRunner();

                runner
                    .AddInput(graph["image_tensor"][0], tensor)
                    .Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["detection_classes"][0],
                    graph["num_detections"][0]);
                var output = runner.Run();

                var boxes = (float[,,])output[0].GetValue(jagged: false);
                var scores = (float[,])output[1].GetValue(jagged: false);
                var classes = (float[,])output[2].GetValue(jagged: false);
                var num = (float[])output[3].GetValue(jagged: false);
                bool found = false;

                float maxscore = 0;
                c = 0;
                foreach (float score in scores)
                {

                    if (score > maxscore)
                    {
                        maxscore = score;
                        detectedClass = classes[0, c];
                    }
                    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                    {
                        found = true;
                        // maxClass = classes[i];
                    }
                    c++;
                }

                outfile = Path.GetDirectoryName(aFile) + "\\found\\" + Path.GetFileName(aFile);
                System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
                if (found)
                {

                    //save original bitmap lossless
                    outfile = outfile.Replace("jpg", "bmp");
                    img.Save(outfile, ImageFormat.Bmp);
                    outfile = outfile.Replace("bmp", "jpg");
                    DrawBoxes(boxes, scores, classes, ref img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
                    img.Save(outfile, ImageFormat.Jpeg);
            }
                tensor.Dispose();
                if (!found)
                {
                    //try flipping image horizontally
                    ms.Close();

                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    contents = ms.ToArray();
                    //

                    tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                    runner = mySession.GetRunner();

                    runner
                        .AddInput(graph["image_tensor"][0], tensor)
                        .Fetch(
                        graph["detection_boxes"][0],
                        graph["detection_scores"][0],
                        graph["detection_classes"][0],
                        graph["num_detections"][0]);
                    output = runner.Run();
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    boxes = (float[,,])output[0].GetValue(jagged: false);
                    scores = (float[,])output[1].GetValue(jagged: false);
                    classes = (float[,])output[2].GetValue(jagged: false);
                    num = (float[])output[3].GetValue(jagged: false);
                    found = false;
                    maxscore = 0;

                    c = 0;
                    foreach (float score in scores)
                    {
                        if (score > maxscore)
                        {
                            maxscore = score;
                            detectedClass = classes[0, c];
                        }
                        if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                            found = true;
                        c++;
                    }
                    Console.WriteLine(aFile + " " + maxscore);

                    outfile = Path.GetDirectoryName(aFile) + "\\found\\x" + Path.GetFileName(aFile);
                    //var  i=0;
                    if (found)
                    {
                        //reverse boxes horizontally
                        //foreach (float box in boxes)
                        //{
                        //    Console.WriteLine(box);
                        //    i++;
                        //}

                        Console.WriteLine("found meteor in " + aFile);
                        outfile = outfile.Replace("jpg", "bmp");
                        img.Save(outfile, ImageFormat.Bmp);
                        outfile = outfile.Replace("bmp", "jpg");
                        DrawBoxes(boxes, scores, classes, ref img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, true);
                        img.Save(outfile, ImageFormat.Jpeg);
                }
                    tensor.Dispose();
                }

            
        }
        public void drawBoxesOnly(ref Bitmap img)
        {


            if (found)
            {

                //save original bitmap lossless
                //outfile = outfile.Replace("jpg", "bmp");
                //img.Save(outfile, ImageFormat.Bmp);
                //outfile = outfile.Replace("bmp", "jpg");
                while (examining)
                {
                    Thread.Sleep(50);
                }
                DrawBoxes(boxes, scores, classes, ref img, "none", MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
            }

        }

        public void examine(byte[] imageData,string aFile)
        {

            // make bitmap
            Console.WriteLine("examining {0}",aFile);
            Bitmap img;
            using (var imagems = new MemoryStream(imageData))
            {
                img = new Bitmap(imagems);
            }

            if (examining) return;
            examining = true;

            string outfile = "";
            float detectedClass = 0;
            int c = 0;
            if (mySession is null)
                mySession = new TFSession(graph);
            //using (mySession)
            //{



            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.

            MemoryStream ms = new MemoryStream();


            img.Save(ms, jgpEncoder, myEncoderParameters);
            var contents = ms.ToArray();
            ms.Close();

            tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
            //if (runner is null)
            runner = mySession.GetRunner();

            runner
                .AddInput(graph["image_tensor"][0], tensor)
                .Fetch(
                graph["detection_boxes"][0],
                graph["detection_scores"][0],
                graph["detection_classes"][0],
                graph["num_detections"][0]);
            var output = runner.Run();
            while (drawingBoxes) { };//make sure boxes aren't in use
            boxes = (float[,,])output[0].GetValue(jagged: false);
            scores = (float[,])output[1].GetValue(jagged: false);
            classes = (float[,])output[2].GetValue(jagged: false);
            num = (float[])output[3].GetValue(jagged: false);
            found = false;

            float maxscore = 0;
            c = 0;
            foreach (float score in scores)
            {

                if (score > maxscore)
                {
                    maxscore = score;
                    detectedClass = classes[0, c];
                }
                if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                {
                    found = true;
                    // maxClass = classes[i];
                }
                c++;
            }
            //store in the cloud


            //return boxes, scores, classes
            outfile = "e:\\test\\found\\" + Path.GetFileName(aFile);
            System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
            if (found)

            {
                outfile = outfile.Replace("jpg", "png");
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                    {
                        img.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                //save original bitmap lossless
                //outfile = outfile.Replace("bmp", "png");
                //img.Save(outfile);
                outfile = outfile.Replace("png", "jpg");
                //outfile = "none";
                DrawBoxes(boxes, scores, classes,  img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                    {
                        img.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            tensor.Dispose();
            ms.Dispose();
            examining = false;
            // }
        }
        public void examine(Image imageData, string aFile, ref float[,,] _boxes, ref float[,] _scores, ref float[,] _classes, ref float[] _num, ref bool _found)
        {

            // make bitmap

            Console.WriteLine("examining {0}", aFile);
            Bitmap img;

            img = new Bitmap(imageData);
            

            if (examining) return;
            examining = true;
            try
            {
                string outfile = "";
                float detectedClass = 0;
                int c = 0;
                if (mySession is null)
                    mySession = new TFSession(graph);
                //using (mySession)
                //{



                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.

                MemoryStream ms = new MemoryStream();
                Bitmap cloneImg = new Bitmap(img);

                img.Dispose();

                cloneImg.Save(ms, jgpEncoder, myEncoderParameters);
                var contents = ms.ToArray();
                ms.Close();
                cloneImg.Dispose();
                tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                //if (runner is null)
                runner = mySession.GetRunner();

                runner
                    .AddInput(graph["image_tensor"][0], tensor)
                    .Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["detection_classes"][0],
                    graph["num_detections"][0]);
                var output = runner.Run();
                while (drawingBoxes) { };//make sure boxes aren't in use
                boxes = (float[,,])output[0].GetValue(jagged: false);
                scores = (float[,])output[1].GetValue(jagged: false);
                classes = (float[,])output[2].GetValue(jagged: false);
                num = (float[])output[3].GetValue(jagged: false);
                found = false;

                float maxscore = 0;
                c = 0;
                foreach (float score in scores)
                {

                    if (score > maxscore && classes[0, c] == 1)
                    {
                        maxscore = score;
                        detectedClass = classes[0, c];
                    }
                    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING && classes[0, c] == 1)
                    {
                        found = true;
                        // maxClass = classes[i];
                    }
                    c++;
                }
                _found = found;
                _boxes = boxes;
                _scores = scores;
                _classes = classes;
                _num = num;
                //store in the cloud


                //return boxes, scores, classes
                //outfile = "e:\\test\\found\\" + Path.GetFileName(aFile);
                //System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
                //if (found)

                //{
                //    outfile = outfile.Replace("jpg", "png");
                //    using (MemoryStream memory = new MemoryStream())
                //    {
                //        using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                //        {
                //            img.Save(memory, ImageFormat.Png);
                //            byte[] bytes = memory.ToArray();
                //            fs.Write(bytes, 0, bytes.Length);
                //        }
                //    }
                //    //save original bitmap lossless
                //    //outfile = outfile.Replace("bmp", "png");
                //    //img.Save(outfile);
                //    outfile = outfile.Replace("png", "jpg");
                //    //outfile = "none";
                //    DrawBoxes(boxes, scores, classes, img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
                //    using (MemoryStream memory = new MemoryStream())
                //    {
                //        using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                //        {
                //            img.Save(memory, ImageFormat.Jpeg);
                //            byte[] bytes = memory.ToArray();
                //            fs.Write(bytes, 0, bytes.Length);
                //        }
                //    }
                //}
                tensor.Dispose();

                ms.Dispose();
                examining = false;
            }

            catch (Exception e)
            {
                Console.Write("error:" + e.Message);
                examining = false;
            }
            // }
        }

        public void examine(byte[] imageData, string aFile,ref float[,,] _boxes,ref float[,] _scores,ref float[,] _classes,ref float[] _num, ref bool _found)
        {

            // make bitmap

            Console.WriteLine("examining {0}", aFile);
            Bitmap img;
            using (var imagems = new MemoryStream(imageData))
            {
                img = new Bitmap(imagems);
            }

            if (examining) return;
            examining = true;
            try
            {
                string outfile = "";
                float detectedClass = 0;
                int c = 0;
                if (mySession is null)
                    mySession = new TFSession(graph);
                //using (mySession)
                //{



                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.

                MemoryStream ms = new MemoryStream();
                Bitmap cloneImg = new Bitmap(img);

                img.Dispose();

                cloneImg.Save(ms, jgpEncoder, myEncoderParameters);
                var contents = ms.ToArray();
                ms.Close();
                cloneImg.Dispose();
                tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
                //if (runner is null)
                runner = mySession.GetRunner();

                runner
                    .AddInput(graph["image_tensor"][0], tensor)
                    .Fetch(
                    graph["detection_boxes"][0],
                    graph["detection_scores"][0],
                    graph["detection_classes"][0],
                    graph["num_detections"][0]);
                var output = runner.Run();
                while (drawingBoxes) { };//make sure boxes aren't in use
                boxes = (float[,,])output[0].GetValue(jagged: false);
                scores = (float[,])output[1].GetValue(jagged: false);
                classes = (float[,])output[2].GetValue(jagged: false);
                num = (float[])output[3].GetValue(jagged: false);
                found = false;

                float maxscore = 0;
                c = 0;
                foreach (float score in scores)
                {

                    if (score > maxscore && classes[0, c] == 1)
                    {
                        maxscore = score;
                        detectedClass = classes[0, c];
                    }
                    if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING && classes[0, c] == 1)
                    {
                        found = true;
                        // maxClass = classes[i];
                    }
                    c++;
                }
                _found = found;
                _boxes = boxes;
                _scores = scores;
                _classes = classes;
                _num = num;
                //store in the cloud


                //return boxes, scores, classes
                //outfile = "e:\\test\\found\\" + Path.GetFileName(aFile);
                //System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
                //if (found)

                //{
                //    outfile = outfile.Replace("jpg", "png");
                //    using (MemoryStream memory = new MemoryStream())
                //    {
                //        using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                //        {
                //            img.Save(memory, ImageFormat.Png);
                //            byte[] bytes = memory.ToArray();
                //            fs.Write(bytes, 0, bytes.Length);
                //        }
                //    }
                //    //save original bitmap lossless
                //    //outfile = outfile.Replace("bmp", "png");
                //    //img.Save(outfile);
                //    outfile = outfile.Replace("png", "jpg");
                //    //outfile = "none";
                //    DrawBoxes(boxes, scores, classes, img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
                //    using (MemoryStream memory = new MemoryStream())
                //    {
                //        using (FileStream fs = new FileStream(outfile, FileMode.Create, FileAccess.ReadWrite))
                //        {
                //            img.Save(memory, ImageFormat.Jpeg);
                //            byte[] bytes = memory.ToArray();
                //            fs.Write(bytes, 0, bytes.Length);
                //        }
                //    }
                //}
                tensor.Dispose();
             
                ms.Dispose();
                examining = false;
            }

            catch (Exception e)
            {
                Console.Write("error:" + e.Message);
                examining = false;
            }
            // }
        }

        public void examine(Bitmap img)
        {
            //options.Parse(args);

            //if (_catalogPath == null)
            //{
            //    _catalogPath = DownloadDefaultTexts(_currentDir);
            //}

            //if (_modelPath == null)
            //{
            //    _modelPath = DownloadDefaultModel(_currentDir);
            //}

            //_catalog = CatalogUtil.ReadCatalogItems(_catalogPath);
            //var fileTuples = new List<(string input, string output)>() { (_input, _output) };

            //pass in filename

            //TFTensor tensor;
            // TFSession.Runner runner;
            if (examining) return;
            examining = true;

            string outfile = "";
            float detectedClass = 0;
            int c = 0;
            if (mySession is null)
                mySession = new TFSession(graph);
            //using (mySession)
            //{



            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.

            MemoryStream ms = new MemoryStream();


            img.Save(ms, jgpEncoder, myEncoderParameters);
            var contents = ms.ToArray();


            tensor = ImageUtil.CreateTensorFromImageFile(contents, TFDataType.UInt8);
            //if (runner is null)
            runner = mySession.GetRunner();

            runner
                .AddInput(graph["image_tensor"][0], tensor)
                .Fetch(
                graph["detection_boxes"][0],
                graph["detection_scores"][0],
                graph["detection_classes"][0],
                graph["num_detections"][0]);
            var output = runner.Run();
            while (drawingBoxes) { };//make sure boxes aren't in use
            boxes = (float[,,])output[0].GetValue(jagged: false);
            scores = (float[,])output[1].GetValue(jagged: false);
            classes = (float[,])output[2].GetValue(jagged: false);
            num = (float[])output[3].GetValue(jagged: false);
            found = false;

            float maxscore = 0;
            c = 0;
            foreach (float score in scores)
            {

                if (score > maxscore)
                {
                    maxscore = score;
                    detectedClass = classes[0, c];
                }
                if (score > MIN_SCORE_FOR_OBJECT_HIGHLIGHTING)
                {
                    found = true;
                    // maxClass = classes[i];
                }
                c++;
            }

            //outfile = Path.GetDirectoryName(aFile) + "\\found\\" + Path.GetFileName(aFile);
            //System.IO.Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(aFile), "found"));
            //if (found)
            //{

            //    //save original bitmap lossless
            //    //outfile = outfile.Replace("jpg", "bmp");
            //    //img.Save(outfile, ImageFormat.Bmp);
            //    //outfile = outfile.Replace("bmp", "jpg");
            //    outfile = "none";
            //    DrawBoxes(boxes, scores, classes, ref img, outfile, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING, false);
            //}
            tensor.Dispose();
            ms.Dispose();
            examining = false;
            // }
        }

        private  void DrawBoxes(float[,,] boxes, float[,] scores, float[,] classes,  Bitmap inputFile, string outputFile, double minScore, Boolean flip)
        {
            drawingBoxes = true;
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
                        //if (flip)
                        //{ editor.AddBoxFlip(xmin, xmax, ymin, ymax, $"{catalogItem.DisplayName}: {(scores[i, j] * 100).ToString("0")}%"); }
                        //else
                        //{
                            editor.AddBox(xmin, xmax, ymin, ymax, $"{catalogItem.DisplayName} : {(scores[i, j] * 100).ToString("0")}%");
                        //}
                    }
                }
            }
            drawingBoxes = false;
        }

        public void DrawBoxes(float[,,] boxes, float[,] scores, float[,] classes, ref Bitmap inputFile, string outputFile, double minScore, Boolean flip)
        {
            drawingBoxes = true;
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
                        if (scores[i, j] < MIN_SCORE_FOR_OBJECT_HIGHLIGHTING) continue;

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
                        if (value == 1) { 
                        CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == value);
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
            drawingBoxes = false;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
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
        //private static void Help()
        //{
        //    options.WriteOptionDescriptions(Console.Out);
        //}
    }
}

