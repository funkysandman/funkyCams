using NumSharp;
using System;
using System.IO;
using System.Linq;
using Tensorflow;
using System.Drawing;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

//using TensorFlowNET.Examples.Utility;
using static Tensorflow.Binding;
using System.Drawing.Drawing2D;

namespace TensorFlowNET.OD
{
    public class DetectInMobilenet //: SciSharpExample, IExample
    {
        public float MIN_SCORE = 0.5f;

        string modelDir = "d://data//export_Nov27b";
        string imageDir = "d://images";
        string pbFile = "d://data//export_Nov27b//saved_model//saved_model.pb";
        string labelFile = "d://meteor_corpus//object-detection.pbtxt";
        string picFile = "input.jpg";

        //public ExampleConfig InitConfig()
        //    => Config = new ExampleConfig
        //    {
        //        Name = "Object Detection in MobileNet (Graph)",
        //        Enabled = true,
        //        IsImportingGraph = true
        //    };
        Tensor[] return_tensors;
        public bool Run()
        {
            
            
            //PrepareData();

            Predict();

            return true;
        }

        public  Graph ImportGraph()
        {
             MLContext mlContext = new MLContext();
           // DataViewSchema inputSchema = new DataViewSchema( );
          //  TensorFlowModel tensorFlowModel = mlContext.Model.Load(pbFile,inputSchema);
            //Tensorflow.ModelLoadSetting
            //var session = tf.Session();
            //session.i
            //var graph = sess.graph;
            //var graph = new Graph().as_default();

            //graph.Import(pbFile, "");
            //return graph;
            return null;
        }

        public  void Predict()
        {
            // read in the input image
            var graph = ImportGraph();
            tf.compat.v1.disable_eager_execution();
            var imgArr = ReadTensorFromImageFile(Path.Combine(imageDir, "input.jpg"));

           

            using (var sess = tf.Session(graph))
            {
                Tensor tensorNum = graph.OperationByName("num_detections");
                Tensor tensorBoxes = graph.OperationByName("detection_boxes");
                Tensor tensorScores = graph.OperationByName("detection_scores");
                Tensor tensorClasses = graph.OperationByName("detection_classes");
                Tensor imgTensor = graph.OperationByName("image_tensor");
                Tensor[] outTensorArr = new Tensor[] { tensorNum, tensorBoxes, tensorScores, tensorClasses };

                var results = sess.run(outTensorArr, new FeedItem(imgTensor, imgArr));

                buildOutputImage(results);
            }
        }

        private NDArray ReadTensorFromImageFile(string file_name)
        {
            var graph = tf.Graph().as_default();

            var file_reader = tf.io.read_file(file_name, "file_reader");
            var decodeJpeg = tf.image.decode_jpeg(file_reader, channels: 3, name: "DecodeJpeg");
            var casted = tf.cast(decodeJpeg, TF_DataType.TF_UINT8);
            var dims_expander = tf.expand_dims(casted, 0);

            using (var sess = tf.Session(graph))
                return sess.run(dims_expander);
        }

        private void buildOutputImage(NDArray[] resultArr)
        {
            // get pbtxt items
            PbtxtItems pbTxtItems = PbtxtParser.ParsePbtxtFile(labelFile);

            // get bitmap
            Bitmap bitmap = new Bitmap(Path.Combine(imageDir, "input.jpg"));

            var scores = resultArr[2].AsIterator<float>();
            var boxes = resultArr[1].GetData<float>();
            var id = np.squeeze(resultArr[3]).GetData<float>();
            for (int i = 0; i < scores.size; i++)
            {
                float score = scores.MoveNext();
                if (score > MIN_SCORE)
                {
                    float top = boxes[i * 4] * bitmap.Height;
                    float left = boxes[i * 4 + 1] * bitmap.Width;
                    float bottom = boxes[i * 4 + 2] * bitmap.Height;
                    float right = boxes[i * 4 + 3] * bitmap.Width;

                    Rectangle rect = new Rectangle()
                    {
                        X = (int)left,
                        Y = (int)top,
                        Width = (int)(right - left),
                        Height = (int)(bottom - top)
                    };

                    string name = pbTxtItems.items.Where(w => w.id == id[i]).Select(s => s.display_name).FirstOrDefault();

                    drawObjectOnBitmap(bitmap, rect, score, name);
                }
            }

            string path = Path.Combine(imageDir, "output.jpg");
            bitmap.Save(path);
            Console.WriteLine($"Processed image is saved as {path}");
        }

        private void drawObjectOnBitmap(Bitmap bmp, Rectangle rect, float score, string name)
        {
            using (Graphics graphic = Graphics.FromImage(bmp))
            {
                graphic.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(Color.Red, 2))
                {
                    graphic.DrawRectangle(pen, rect);

                    Point p = new Point(rect.Right + 5, rect.Top + 5);
                    string text = string.Format("{0}:{1}%", name, (int)(score * 100));
                    graphic.DrawString(text, new Font("Verdana", 8), Brushes.Red, p);
                }
            }
        }
    }
}
