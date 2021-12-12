using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tensorflow.Binding;

namespace TensorFlowNET.OD
{
    class Program
    {
        static void Main(string[] args)
        {

            DetectInMobilenet d = new DetectInMobilenet();
            d.Run();



        }

      
    }
}
