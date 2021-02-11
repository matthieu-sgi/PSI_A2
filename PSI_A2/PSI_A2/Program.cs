using System;
using System.IO;



namespace PSI_A2
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            /*//Bitmap damier = new Bitmap(@"..\..\..\Images\damier.bmp");

            byte[] file = File.ReadAllBytes(@"..\..\..\Images\damier.bmp");
            Console.WriteLine("\n HEADER \n");
            for (int i = 0; i < 14; i++)
            {
                Console.Write(file[i] + " ");
            }
            Console.WriteLine("\n HEADER INFO \n");

            for (int i = 14; i < 54; i++)
            {
                Console.Write(file[i] + " ");

            }





            Console.WriteLine("\n IMAGE \n");

            for (int i = 54; i < file.Length; i += 60)
            {
                for (int j = i; j < i + 60; j++)
                {
                    Console.WriteLine(file[j] + " ");
                }
                Console.WriteLine();
            }



            //damier.Save(@"..\..\..\Images\Tests_damier.bmp");
            //Equivalent de la save de bitmap
            //File.WriteAllBytes(@"..\..\..\Images\Tests_damier.bmp",file);
            Console.ReadKey();*/
            #endregion

            string path = @"..\..\..\Images\Test001.bmp";
            string writing_path = @"..\..\..\Images\Lalala.bmp";

            MyImage TEST = new MyImage("BM", 2902, 1312, 123, 13, 13, new byte[4, 4]);
            byte[] tab = { 19,12,21,123,153,136, 19, 0, 0 };
            //Console.WriteLine(TEST.Convertir_Endian_To_Int(tab));
            /*foreach (byte i in TEST.TableauByte(tab, 3, 4))
            {
                Console.WriteLine(i);
            }*/
            MyImage traie = new MyImage(path);
            
            traie.Affiche();
            traie.FromImageToFile(writing_path);

            /*MyImage recup = new MyImage(writing_path);
            recup.Affiche();*/

            //Console.WriteLine(recup.Height);
            //Console.WriteLine(recup.Width);




            Console.WriteLine("Done");
            Console.ReadLine(); 
        }
    }
}
