﻿using System;
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
            Console.WriteLine(9.ToString("X"));

            /* MyImage traie = new MyImage(path);
             traie.FromImageToFile(writing_path);
             MyImage recup = new MyImage(writing_path);
             recup.Affiche();

             Console.WriteLine(recup.Height);
             Console.WriteLine(recup.Width);

             byte[] test = recup.Convertir_Int_To_Endian(200, 4);
             for(int i = 0; i < test.Length; i++)
             {
                 Console.Write(test[i]);
             }*/



            Console.ReadLine(); 
        }
    }
}
