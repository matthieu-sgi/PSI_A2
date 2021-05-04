using System;



namespace PSI_A2
{
    public class Program
    {
        static void Main(string[] args)
        {


            string path = @"..\..\..\Images\";
            string writing_path = @"..\..\..\Images\";







            string exit="";
            Console.WriteLine("Veuillez entrer le nom du fichier de sortie : ");
            string nom_image = Console.ReadLine();
            string test = "";
            for(int i = nom_image.Length -4; i< nom_image.Length && nom_image.Length>0; i++)
            {
                test += nom_image[i];
            }
            if (test != ".bmp" || nom_image.Length == 0) writing_path += nom_image + ".bmp";
            else writing_path += nom_image;
            

            Console.WriteLine("Veuillez entrer le nom de votre image placée dans le dossier \"Images\" du projet, veuillez spécifier l'extension (.bmp)" +
                "\nSi vous ne remplissez rien, l'image léna sera chargée (pour le qrcode vous pouvez simplement appuyer sur enter");
            nom_image = Console.ReadLine();
            if (nom_image.Length == 0)
            {
                path += "lena.bmp";
                nom_image = "lena.bmp";

            }
            else path += nom_image;
            MyImage image = new MyImage(path);
            Console.WriteLine("Vous avez choisi " + nom_image + " !\nAmusez-vous bien !");

            #region Menu sympathique

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
                                    "\n12- Fractale" +
                                    "\n13- Histogramme" +
                                    "\n14- Stéganographie" +
                                    "\n15- QrCode" +
                                    "\nEntrer \"exit\" pour quitter ou faites ctrl + c");
                exit = Console.ReadLine();
                switch (exit)
                {
                    case "1":
                        image.Affiche(false);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        image.Miror(true);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        image.Miror(false);
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        image.Nuance_de_Gris();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        image.Noir_et_Blanc();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
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
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    case "7":
                        image.Blur();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
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
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "9":

                        image.Edges_detection();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "10":

                        image.Repoussage();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "11":

                        image.EdgesReinforcement();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "12":
                        image.Fractale();
                        image.FromImageToFile(writing_path);
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "13":
                        Console.WriteLine("Entrez le type d'histogramme souhaité");
                        Console.WriteLine("- 1) pour l'histogramme RGB");
                        Console.WriteLine("- 2) pour l'histogramme nuance de gris");
                        int type = Convert.ToInt32(Console.ReadLine());

                        if (type == 1 || type == 2)
                        {
                            image.Histogramme(type);
                            image.FromImageToFile(writing_path);
                            Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Rentrez un nombre valide.");
                            Console.ReadKey();
                        }
                        break;
                    case "14":
                        Console.WriteLine("Voulez-vous : ");
                        Console.WriteLine("- 1) Cacher une image dans l'image sélectionner.");
                        Console.WriteLine("- 2) Retrouver l'image cachée dans l'image sélectionnée.");
                        int choix = Convert.ToInt32(Console.ReadLine());

                        if (choix == 1)
                        {
                            Console.WriteLine("Quelle image souhaitez-vous cacher (veuillez entrer le nom du fichier en spécifiant l'extension (.bmp))?");
                            string s = Console.ReadLine();
                            string path2 = @"..\..\..\Images\" + s;
                            string writing_path2 = @"..\..\..\Images\image_cachee.bmp";
                            
                            MyImage image2 = new MyImage(path2);

                            if (image2.Right_File)
                            {
                                image2.Resize(image.Pixel_image.GetLength(0), image.Pixel_image.GetLength(1));
                                Pixel[,] imagecachée = image2.Pixel_image;
                                image.HiddenPic(imagecachée);
                                image.FromImageToFile(writing_path2);
                                Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                                Console.ReadKey();
                            }
                            Console.Clear();
                        }
                        else if (choix == 2)
                        {
                            string writing_path3 = @"..\..\..\Images\image_retrouvee.bmp";
                            image.Retrouver_image();
                            image.FromImageToFile(writing_path3);
                            Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Rentrez un nombre valide.");
                            Console.ReadKey();
                        }
                        break;
                    case "15":
                        Console.WriteLine("Quel string voulez-vous cacher (alphanumérique seulement) ?");
                        QrCode qrcode = new QrCode(Console.ReadLine(), writing_path);
                        qrcode.Qr_Save();
                        Console.WriteLine("Saved and done...(presser un bouton pour revenir au menu)");
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
            #endregion




            Console.ReadKey();
        }
    }
}
