using System;
using System.IO;
using System.Drawing;



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

            string path = @"..\..\..\Images\lena.bmp";
            string writing_path = @"..\..\..\Images\Lalala.bmp";

            
            
            
            MyImage traie = new MyImage(path);
            //
           traie.blur();
            //traie.Rotation(Math.PI/4);
            //traie.Affiche(false, true);
            //traie.Miror(false);
            //traie.Miror(false);
            //traie.Affiche(true);
            //Console.WriteLine(traie.Height_Pixel);
            //traie.Nuance_de_Gris();
            //traie.Resize(400, 400);
            //traie.edges_detection();

            traie.FromImageToFile(writing_path,true);
            //traie.FromImageToFile(writing_path, true);
            




            Console.WriteLine("Save and Done");
            Console.ReadKey(); 
        }
    }
}
