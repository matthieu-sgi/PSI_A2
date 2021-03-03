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

            string path = @"..\..\..\Images\";
            string writing_path = @"..\..\..\Images\";







            string exit = "";
            Console.WriteLine("Veuillez entrer le nom du fichier de résultat en spécifiant l'extension (.bmp) : ");
            string nom_image = Console.ReadLine();
            writing_path += nom_image;

            Console.WriteLine("Veuillez entrer le nom de votre image placée dans le dossier \"Images\" du projet, veuillez spécifier l'extension (.bmp)" +
                "\nSi vous ne remplissez rien, l'image léna sera chargée");
            nom_image = Console.ReadLine();
            if (nom_image.Length == 0)
            {
                path += "lena.bmp";

            }
            else path += nom_image;
            MyImage image = new MyImage(path);
            

            while (exit != "exit" && image.Right_File)
            {
                Console.WriteLine("Que voulez vous faire ? (veuillez entrer un nombre)\n1- Afficher les bytes de l'image"
                                    + "\n2- Effet miroir avec axe de symétrie horizontal" +
                                    "\n3- Effet miroir avec axe de symétrie vertical" +
                                    "\n4- Image en nuances de gris" +
                                    "\n5- Image en noir et blanc" +
                                    "\n6- Redimmensionner l'image" +
                                    "\n7- Image effet flou" +
                                    "\n8- Rotation de l'image" +
                                    "\n9- Détection des contours" +
                                    "\n10- Repoussage" +
                                    "\n11- Renforcement des bords" +
                                    "\nEntrer \"exit\" pour quitter");
                exit = Console.ReadLine();
                switch (exit)
                {
                    case "1":
                        image.Affiche(false);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        image.Miror(true);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        image.Miror(false);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        image.Nuance_de_Gris();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        image.Noir_et_Blanc();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        int new_width;
                        int new_height;
                        
                        Console.Write("Veuillez entrer une longueur entière : ");
                        bool right_width = int.TryParse(Console.ReadLine(), out new_width);
                        Console.Write("Veuillez entrer une largeur entière : ");
                        bool rigth_height = int.TryParse(Console.ReadLine(), out new_height);
                        while (!right_width)
                        {
                            
                            Console.Write("Veuillez entrer une longueur entière svp : ");
                            right_width = int.TryParse(Console.ReadLine(), out new_width);

                        }
                        while (!rigth_height)
                        {

                            Console.Write("Veuillez entrer une largeur entière svp : ");
                            rigth_height = int.TryParse(Console.ReadLine(), out new_height);

                        }
                        image.Resize(new_height, new_width);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "7":
                        image.Blur();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "8":
                        double angle;
                        
                        Console.Write("Veuillez entrer une valeur d'angle en degré : ");
                        bool temp = double.TryParse(Console.ReadLine(), out angle);
                        while (!temp)
                        {
                            Console.Write("Veuillez entrer un double svp : ");
                            temp = double.TryParse(Console.ReadLine(), out angle);
                            
                        } 
                        image.Rotation(angle);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "9":
                        Console.Write("L'outil détection des contours ne fonctionne pas vraiment, il le sera lors de la prochaine mise à jour :)");
                        image.Edges_detection();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "10":
                        Console.Write("L'outil repoussage ne fonctionne pas vraiment, il le sera lors de la prochaine mise à jour :)");
                        image.Repoussage();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "11":
                        Console.Write("L'outil renforcement des contours ne fonctionne pas vraiment, il le sera lors de la prochaine mise à jour :)");
                        image.EdgesReinforcement();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Save and Done");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "exit":
                        Console.WriteLine("Au revoir");
                        Console.ReadKey();
                        break;
                    default:
                        Console.Write("Votre entrée n'est pas valide, veuillez entrer un nombre");
                        Console.ReadKey();
                        Console.Clear();
                        break;


                }
            }
            
            //image.Rotation(Math.PI/4);
            //image.Affiche(false, true);
            //image.Miror(false);
            //image.Miror(false);
            //image.Affiche(true);
            //Console.WriteLine(traie.Height_Pixel);
            //image.Nuance_de_Gris();
            //image.Resize(400, 400);
            //image.edges_detection();
            //image.blur();

            //image.FromImageToFile(writing_path,true);
            //image.FromImageToFile(writing_path, true);
            




            /*Console.WriteLine("Save and Done");
            Console.ReadKey(); */
        }
    }
}
