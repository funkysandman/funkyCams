// An example for using the TensorFlow C# API for image recognition
// using a pre-trained inception model (http://arxiv.org/abs/1512.00567).
// 
// Sample usage: <program> -dir=/tmp/modeldir imagefile
// 
// The pre-trained model takes input in the form of a 4-dimensional
// tensor with shape [ BATCH_SIZE, IMAGE_HEIGHT, IMAGE_WIDTH, 3 ],
// where:
// - BATCH_SIZE allows for inference of multiple images in one pass through the graph
// - IMAGE_HEIGHT is the height of the images on which the model was trained
// - IMAGE_WIDTH is the width of the images on which the model was trained
// - 3 is the (R, G, B) values of the pixel colors represented as a float.
// 
// And produces as output a vector with shape [ NUM_LABELS ].
// output[i] is the probability that the input image was recognized as
// having the i-th label.
// 
// A separate file contains a list of string labels corresponding to the
// integer indices of the output.
// 
// This example:
// - Loads the serialized representation of the pre-trained model into a Graph
// - Creates a Session to execute operations on the Graph
// - Converts an image file to a Tensor to provide as input to a Session run
// - Executes the Session and prints out the label with the highest probability
// 
// To convert an image file to a Tensor suitable for input to the Inception model,
// this example:
// - Constructs another TensorFlow graph to normalize the image into a
//   form suitable for the model (for example, resizing the image)
// - Creates an executes a Session to obtain a Tensor in this normalized form.
using System;
using TensorFlow;
using Mono.Options;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Collections.Generic;
using ExampleCommon;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
namespace TensorFlowDetector
{
	public  class MeteorChecker
	{
        static string[] labels;

        const int W = 224;
        const int H = 224;
        const float Mean = 128f;
        const float Scale = 128f; // --input_std, should be 128 for mobilenet, don't use 1 or it won't work

        static void Error(string msg)
        {
            Console.WriteLine("Error: {0}", msg);
            Environment.Exit(1);
        }

        static void Help()
        {
            options.WriteOptionDescriptions(Console.Out);
        }

        static bool jagged = true;

		static OptionSet options = new OptionSet ()
		{
			{ "m|dir=",  "Specifies the directory where the model and labels are stored", v => dir = v },
			{ "h|help", v => Help () },

			{ "amulti", "Use multi-dimensional arrays instead of jagged arrays", v => jagged = false }
		};


		static string dir, modelFile, labelsFile;
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());




        }


        public  void CheckFile(string imgdir,string imageFileName)
        
        {
           string[] args;
			//var files = options.Parse (args);
			//if (dir == null) {
				dir = "/tmp";
			//	//Error ("Must specify a directory with -m to store the training data");
			//}

			//if (files == null || files.Count == 0)
			//	Error ("No files were specified");

			//if (files.Count == 0)
			//	files = new List<string> () { imageFilePath };
			
			ModelFiles (dir);

			// Construct an in-memory graph from the serialized form.
			 var graph = new TFGraph ();
			// Load the serialized GraphDef from a file.
			var model = File.ReadAllBytes (modelFile);

			graph.Import (model, "");
			using (var session = new TFSession (graph)) {
				var labels = File.ReadAllLines (labelsFile);
                //

                //
                //

                var b = Bitmap.FromFile(Path.Combine(imgdir, imageFileName));
                var contents = ImageToByteArray(b);
                var tensor = TFTensor.CreateString(contents);
                tensor = CreateTensor(tensor, TFDataType.Float);

                var runner = session.GetRunner();

                runner.AddInput(graph["input"][0], tensor).Fetch(graph["final_result"][0]);
                var output = runner.Run();
                // Run inference on the image files
                // For multiple images, session.Run() can be called in a loop (and
                // concurrently). Alternatively, images can be batched since the model
                // accepts batches of image data as input.

     //           var tensor = CreateTensorFromImageFile (Path.Combine(imgdir,imageFilePath));
                    
               
					//var runner = session.GetRunner ();
     //               runner.AddInput(graph["input"][0], tensor).Fetch (graph["final_result"][0]);
					//var output = runner.Run ();
					// output[0].Value() is a vector containing probabilities of
					// labels for each image in the "batch". The batch size was 1.
					// Find the most probably label index.



					var result = output [0];
					var rshape = result.Shape;
					if (result.NumDims != 2 || rshape [0] != 1) {
						var shape = "";
						foreach (var d in rshape) {
							shape += $"{d} ";
						}
						shape = shape.Trim ();
						Console.WriteLine ($"Error: expected to produce a [1 N] shaped tensor where N is the number of labels, instead it produced one with shape [{shape}]");
						Environment.Exit (1);
					}

					// You can get the data in two ways, as a multi-dimensional array, or arrays of arrays, 
					// code can be nicer to read with one or the other, pick it based on how you want to process
					// it
					bool jagged = true;

					var bestIdx = 0;
					float p = 0, best = 0;

					if (jagged) {
						var probabilities = ((float [] [])result.GetValue (jagged: true)) [0];
						for (int i = 0; i < probabilities.Length; i++) {
							if (probabilities [i] > best) {
								bestIdx = i;
								best = probabilities [i];
							}
						}

					} else {
						var val = (float [,])result.GetValue (jagged: false);	

						// Result is [1,N], flatten array
						for (int i = 0; i < val.GetLength (1); i++) {
							if (val [0, i] > best) {
								bestIdx = i;
								best = val [0, i];
							}
						}
					}

                    if (best>0.99 && bestIdx==0)
                {
                    //copy file to positive folder
                    File.Copy(Path.Combine(imgdir, imageFileName), Path.Combine(imgdir,"positive", ((int)(best* 100)).ToString() + "pct_"+ imageFileName));

                }
                    else
                {
                    File.Copy(Path.Combine(imgdir, imageFileName), Path.Combine(imgdir, "negative", ((int)(best* 100)).ToString() + "pct_"+ imageFileName));



                }
                //Console.WriteLine ($"{file} best match: [{bestIdx}] {best * 100.0}% {labels [bestIdx]}");

            }
		}
		
		//
		// Downloads the inception graph and labels
		//
		 void ModelFiles (string dir)
		{
			string url = "https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip";

            //modelFile = Path.Combine(dir, "tensorflow_inception_graph.pb");
            //labelsFile = Path.Combine(dir, "imagenet_comp_graph_label_strings.txt");
            //var zipfile = Path.Combine(dir, "inception5h.zip");
            modelFile = Path.Combine(dir, "output_graph.pb");
            labelsFile = Path.Combine(dir, "output_labels.txt");
            var zipfile = Path.Combine(dir, "inception5h.zip");


            if (File.Exists (modelFile) && File.Exists (labelsFile))
				return;

			Directory.CreateDirectory (dir);
			var wc = new WebClient ();
			wc.DownloadFile (url, zipfile);
			ZipFile.ExtractToDirectory (zipfile, dir);
			File.Delete (zipfile);
		}
         byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png); // Encode as a png
                return ms.ToArray();
            }
        }

         byte[] ImageToByteArray(Bitmap image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png); // Encode as a png
                return ms.ToArray();
            }
        }

        // Mini struct to return name and confidence :)
        public struct ClassificationResult
        {
            public string Name;
            public float Confidence;

            public ClassificationResult(string n, float conf)
            {
                Name = n;
                Confidence = conf;
            }
        }
        //static ClassificationResult ClassifyImage(byte[] contents)
        //{
        //    var tensor = TFTensor.CreateString(contents);
        //    tensor = CreateTensor(tensor, TFDataType.Float);

        //    var runner = session.GetRunner();

        //    runner.AddInput(graph["input"][0], tensor).Fetch(graph["final_result"][0]);
        //    var output = runner.Run();

        //    var result = output[0];
        //    bool jagged = true;
        //    var bestIdx = 0;
        //    float p = 0, best = 0;

        //    if (jagged)
        //    {
        //        var probabilities = ((float[][])result.GetValue(jagged: true))[0];
        //        for (int i = 0; i < probabilities.Length; i++)
        //        {
        //            if (probabilities[i] > best)
        //            {
        //                bestIdx = i;
        //                best = probabilities[i];
        //            }
        //        }

        //    }
        //    else
        //    {
        //        var val = (float[,])result.GetValue(jagged: false);

        //        // Result is [1,N], flatten array
        //        for (int i = 0; i < val.GetLength(1); i++)
        //        {
        //            if (val[0, i] > best)
        //            {
        //                bestIdx = i;
        //                best = val[0, i];
        //            }
        //        }
        //    }

        //    return new ClassificationResult(labels[bestIdx], best);
        //}

        //static ClassificationResult ClassifyImage(System.Drawing.Image toClassifyImage)
        //{
        //    return ClassifyImage(ImageToByteArray(toClassifyImage));
        //}

        //static ClassificationResult ClassifyImage(Bitmap toClassifyImage)
        //{
        //    return ClassifyImage(ImageToByteArray(toClassifyImage));
        //}

        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public  TFTensor CreateTensorFromImageFile(string file, TFDataType destinationDataType = TFDataType.Float)
        {
            var contents = File.ReadAllBytes(file);
            var tensor = TFTensor.CreateString(contents);

            return CreateTensor(tensor, destinationDataType);
        }

        public  TFTensor CreateTensor(TFTensor tensor, TFDataType destinationDataType = TFDataType.Float)
        {
            TFGraph graph;
            TFOutput input, output;

            // Construct a graph to normalize the image
            ConstructGraphToNormalizeImage(out graph, out input, out output, destinationDataType);

            // Execute that graph to normalize this one image
            using( var session = new TFSession(graph))
            {
                var normalized = session.Run(
                         inputs: new[] { input },
                         inputValues: new[] { tensor },
                         outputs: new[] { output });

                return normalized[0];
            }
        }

        private  void ConstructGraphToNormalizeImage(out TFGraph graph, out TFOutput input, out TFOutput output, TFDataType destinationDataType = TFDataType.Float)
        {
            graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            output = graph.Cast(graph.Div(
                x: graph.Sub(
                    x: graph.ResizeBilinear(
                        images: graph.ExpandDims(
                            input: graph.Cast(
                                graph.DecodeJpeg(contents: input, channels: 3), DstT: TFDataType.Float),
                            dim: graph.Const(0, "make_batch")),
                        size: graph.Const(new int[] { W, H }, "size")),
                    y: graph.Const(Mean, "mean")),
                y: graph.Const(Scale, "scale")), destinationDataType);
        }
    }
}
