
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

namespace detectionTest
{
    static class   Program
    {

        static void Main(string[] args)
        {

            DateTime from_date = DateTime.Now.AddHours(-1600000);
            DateTime to_date = DateTime.Now;
            var directory = new DirectoryInfo("E:\\image\\2018-Aug-27");


            var files = directory.GetFiles("*.jpg"); //.Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);

            foreach (FileInfo afile in files)
            {
                if (afile != null)
                {
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
                //split into groups

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
