using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace batchFileReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            // start here
            // get all png files and replace with specific image file while maintaining files names

            var directory = new DirectoryInfo("C:\\Users\\UserPC\\AppData\\Roaming\\.minecraft\\resourcepacks\\turf\\assets\\minecraft\\textures\\blocks");
            var files = directory.GetFiles("*.png"); //.Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
            Bitmap b;
            b = new Bitmap("C:\\Users\\UserPC\\AppData\\Roaming\\.minecraft\\resourcepacks\\turf\\assets\\minecraft\\textures\\beacon.png");

            foreach (FileInfo afile in files)
            {
                if (afile != null)
                {
                    
                    string file = afile.FullName;
                    b.Save(file);
                    Console.WriteLine("replaced {0}", file);
                    
                }

            }



        }
    }
}
