using System;
using System.IO;


namespace PSI_A2
{
    class MyImage
    {
        private string type;
        private int taille;
        private int offset;
        private int width_pixel;
        private int width_byte;
        private int height;
       
        private byte[] header;
        private bool rigth_file = true;

        private Pixel[,] pixel_image;



        public MyImage(string type, int offset, byte[] header, Pixel[,] image)
        {
            this.type = type;
            
            this.offset = offset;
            this.header = header;
            
            this.pixel_image = image;
        }

        public MyImage(string myfile)
        {

            try
            {
                byte[] image_temp = File.ReadAllBytes(myfile);


                this.type = (image_temp[0] == 66 && image_temp[1] == 77) ? "BM" : "-1";

                if (this.type == "BM")
                {


                    this.taille = Convertir_Endian_To_Int(TableauByte(image_temp, 2, 4));
                    this.offset = Convertir_Endian_To_Int(TableauByte(image_temp, 10, 4));
                    this.width_byte = Convertir_Endian_To_Int(TableauByte(image_temp, 18, 4)) * 3;
                    this.width_pixel = Convertir_Endian_To_Int(TableauByte(image_temp, 18, 4));

                    this.height = Convertir_Endian_To_Int(TableauByte(image_temp, 22, 4));
                    //this.bits_by_Color = Convertir_Endian_To_Int(TableauByte(image_temp, 28, 2));
                    //this.image = new byte[this.height, this.width_byte];
                    this.header = new byte[this.offset];
                    this.pixel_image = new Pixel[this.height, this.width_pixel];

                    for (int i = 0; i < this.header.Length; i++)
                    {
                        this.header[i] = image_temp[i];
                    }


                    //J'instaure le nouveau système


                    int width_to_save = (((pixel_image.GetLength(1) * 3) + 3) / 4) * 4;
                    for (int i = 0; i < pixel_image.GetLength(0); i++)
                    {
                        for (int j = 0; j < width_to_save; j++)
                        {
                            if (j < pixel_image.GetLength(1))
                            {
                                this.pixel_image[i, j].B = image_temp[this.offset + i * width_to_save + j * 3];
                                this.pixel_image[i, j].G = image_temp[this.offset + i * width_to_save + j * 3 + 1];
                                this.pixel_image[i, j].R = image_temp[this.offset + i * width_to_save + j * 3 + 2];
                            }


                        }

                    }



                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Votre nom de fichier n'est pas bon");
                Console.ReadKey();
                this.rigth_file = false;

            }
        }
        public int Width_Pixel
        {
            get { return this.pixel_image.GetLength(1); }
        }

        public int Height_Pixel
        {
            get { return this.pixel_image.GetLength(0); }
        }
        public bool Right_File
        {
            get { return rigth_file; }
        }

        public Pixel[,] Pixel_image
        {
            get { return this.pixel_image; }
            set { this.pixel_image = value; }
        }



        public void FromImageToFile(string file)
        {
            if (this.type == "BM")
            {
                int width_to_save = (((pixel_image.GetLength(1) * 3) + 3) / 4) * 4;
                this.taille = this.offset + width_to_save * pixel_image.GetLength(0);
                
                byte[] image_to_write = new byte[this.taille];
                
                Console.WriteLine("Calcul : "+(pixel_image.GetLength(1) * pixel_image.GetLength(0) * 24) / 8);

                //Console.WriteLine(image_to_write.Length);

                for (int i = 0; i < this.offset; i++)
                {
                    image_to_write[i] = this.header[i];
                }
                image_to_write[0] = 66;
                image_to_write[1] = 77;
                //Recalculating size, width and height
                for (int i = 0; i < 4; i++)
                {
                    image_to_write[i + 2] = Convertir_Int_To_Endian(((pixel_image.GetLength(1)*pixel_image.GetLength(0)*24)/8)+this.offset)[i];
                    image_to_write[i + 18] = Convertir_Int_To_Endian(pixel_image.GetLength(1))[i];
                    image_to_write[i + 22] = Convertir_Int_To_Endian(pixel_image.GetLength(0))[i];
                   



                }

                //Im testing to understand better

               /* for(int i = 0; i < this.offset; i++)
                {
                    if (i < 14)
                    {
                        Console.Write(image_to_write[i] + " ");
                    }
                    else if (i == 14) Console.Write("\n" + image_to_write[i] + " ");
                    else Console.Write(image_to_write[i] + " ");
                    
                }*/



                


                for (int i = 0; i < pixel_image.GetLength(0); i++)
                {
                    for (int j = 0; j < width_to_save; j++)
                    {

                        if (j < pixel_image.GetLength(1))
                        {
                            image_to_write[this.offset + i * width_to_save + j * 3] = this.pixel_image[i, j].B;
                            image_to_write[this.offset + i * width_to_save + j * 3 + 1] = this.pixel_image[i, j].G;
                            image_to_write[this.offset + i * width_to_save + j * 3 + 2] = this.pixel_image[i, j].R;
                        }

                        
                    }

                }


                File.WriteAllBytes(file, image_to_write);
                
                Console.WriteLine("Save and Done");
            }

        }


        public void Fractale()
        {
            int imagex = 3000;
            int imagey = 3000;
            int iteration_max = 50;
            double c_r = 0;
            double c_i = 0;
            double z_r = 0;
            double z_i = 0;
            int i = 0;
            double tmp = 0;

            Pixel[,] fractale = new Pixel[imagex, imagey];

            for (int x = 0; x < imagex; x++)
            {
                for (int y = 0; y < imagey; y++)
                {
                    c_r = (double)(x - (imagex) / 2) / (double)(imagex / 3);
                    c_i = (double)(y - (imagey) / 2) / (double)(imagey / 3);
                    z_r = 0;
                    z_i = 0;
                    i = 0;

                    while (i < iteration_max && z_r * z_r + z_i * z_i < 4)
                    {
                        tmp = z_r;
                        z_r = z_r * z_r - z_i * z_i + c_r;
                        z_i = 2 * z_i * tmp + c_i;
                        i = i + 1;
                    }
                    if (i == iteration_max)
                    {

                        fractale[x, y] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        fractale[x, y] = new Pixel((byte)(i * 255 / iteration_max), 0, (byte)(i * 255 / iteration_max));

                    }

                }
            }
            this.pixel_image = fractale;
        }


        public int Convertir_Endian_To_Int(byte[] tab)
        {

            int s = 0;
            for (int n = 0; n < tab.Length; n++)
            {
                s += tab[n] * Convert.ToInt32(Math.Pow(256, n));
            }
            return s;
        }

        static public byte[] Convertir_Int_To_Endian(long entier)
        {
            int p = 0;
            while ((entier / Convert.ToInt64(Math.Pow(256, p)) >= 1))
            {
                p++;
            }
            p--;
            byte[] retour = new byte[p + 4 - p % 4];
            for (int i = 0; i < retour.Length; i++)
            {
                retour[i] = 0;
            }
            for (int i = p; i >= 0; i--)
            {
                retour[i] = Convert.ToByte(entier / Convert.ToInt64(Math.Pow(256, p)));
                entier -= retour[i] * Convert.ToInt64(Math.Pow(256, p));
                p--;
            }
            return retour;
        }


        public void Nuance_de_Gris()
        {

            for (int i = 0; i < this.pixel_image.GetLength(0); i++)
            {
                for (int j = 0; j < this.pixel_image.GetLength(1); j++)
                {
                    byte moyenne = Convert.ToByte((this.pixel_image[i, j].R + this.pixel_image[i, j].G + this.pixel_image[i, j].B) / 3);


                    this.pixel_image[i, j].R = moyenne;
                    this.pixel_image[i, j].G = moyenne;
                    this.pixel_image[i, j].B = moyenne;



                }
            }

        }

        public void Noir_et_Blanc()
        {
            for (int i = 0; i < this.pixel_image.GetLength(0); i++)
            {
                for (int j = 0; j < this.pixel_image.GetLength(1); j++)
                {
                    byte moyenne = Convert.ToByte((this.pixel_image[i, j].R + this.pixel_image[i, j].G + this.pixel_image[i, j].B) / 3);

                    if (moyenne < 255 / 2)
                    {
                        this.pixel_image[i, j].R = 0;
                        this.pixel_image[i, j].G = 0;
                        this.pixel_image[i, j].B = 0;
                    }
                    else
                    {
                        this.pixel_image[i, j].R = 255;
                        this.pixel_image[i, j].G = 255;
                        this.pixel_image[i, j].B = 255;
                    }


                }
            }



        }




        public void Resize(int new_height, int new_width)
        {

            Pixel[,] new_matrix = new Pixel[new_height, new_width];
            for (int i = 0; i < new_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < new_matrix.GetLength(1); j++)
                {
                    int new_x = (int)(((double)i * (double)this.pixel_image.GetLength(0)) / (double)new_matrix.GetLength(0));
                    int new_y = (int)(((double)j * (double)this.pixel_image.GetLength(1)) / (double)new_matrix.GetLength(1));
                    new_matrix[i, j] = new Pixel(this.pixel_image[new_x, new_y].R, this.pixel_image[new_x, new_y].G, this.pixel_image[new_x, new_y].B);

                }
            }

            this.pixel_image = new_matrix;


        }

        public void Rotation(double angle_rad)
        {
            angle_rad = (angle_rad / 180.0) * Math.PI;
            int[] center = { this.pixel_image.GetLength(0) / 2, this.pixel_image.GetLength(1) / 2 };
            int new_height = this.pixel_image.GetLength(0) / 2;
            int new_width = this.pixel_image.GetLength(1) / 2;
            int x_max = 0;
            int x_min = 0;
            int y_max = 0;
            int y_min = 0;
            for (int i = 0; i < this.pixel_image.GetLength(0); i += pixel_image.GetLength(0) - 1)
            {
                for (int j = 0; j < this.pixel_image.GetLength(1); j += pixel_image.GetLength(1) - 1)
                {


                    double r = Math.Sqrt(Math.Pow(i - center[0], 2) + Math.Pow(j - center[1], 2));
                    double theta = Math.Atan2(i - center[0], j - center[1]) + angle_rad;
                    x_max = (x_max < r * Math.Cos(theta)) ? (int)(r * Math.Cos(theta)) : x_max;
                    x_min = (x_min > r * Math.Cos(theta)) ? (int)(r * Math.Cos(theta)) : x_min;
                    y_max = (y_max < r * Math.Sin(theta)) ? (int)(r * Math.Sin(theta)) : y_max;
                    y_min = (y_min > r * Math.Sin(theta)) ? (int)(r * Math.Sin(theta)) : y_min;


                }
            }
            new_height = y_max - y_min + 1;
            new_width = x_max - x_min + 1;

            Pixel[,] new_matrix = new Pixel[new_height, new_width];
            int[] new_center = { new_matrix.GetLength(0) / 2, new_matrix.GetLength(1) / 2 };
            for (int i = 0; i < new_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < new_matrix.GetLength(1); j++)
                {
                    double rayon = Math.Sqrt(Math.Pow(i - new_center[0], 2) + Math.Pow(j - new_center[1], 2));
                    double theta = Math.Atan2(j - new_center[1], i - new_center[0]) - angle_rad;
                    int new_x = (int)(center[0] + rayon * Math.Cos(theta));
                    int new_y = (int)(center[1] + rayon * Math.Sin(theta));
                    if (new_x >= 0 && new_x < this.pixel_image.GetLength(0) && new_y >= 0 && new_y < this.pixel_image.GetLength(1))
                    {
                        new_matrix[i, j] = this.pixel_image[new_x, new_y];
                    }

                }
            }
            this.pixel_image = new_matrix;



        }

        public void Miror(bool axe_horizontal)
        {
            Pixel[,] image_temp = new Pixel[this.pixel_image.GetLength(0), this.pixel_image.GetLength(1)];
            if (!axe_horizontal)
            {

                for (int i = 0; i < image_temp.GetLength(0); i++)
                {
                    for (int j = 0; j < image_temp.GetLength(1); j++)
                    {

                        image_temp[i, j] = this.pixel_image[i, this.pixel_image.GetLength(1) - 1 - j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < image_temp.GetLength(0); i++)
                {
                    for (int j = 0; j < image_temp.GetLength(1); j++)
                    {
                        image_temp[i, j] = this.pixel_image[this.pixel_image.GetLength(0) - 1 - i, j];

                    }
                }
            }
            this.pixel_image = image_temp;
        }

        public void Blur()
        {
            int[,] mat_blur = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            this.pixel_image = Convolution(mat_blur);


        }

        public void Edges_detection()
        {
            int[,] mat = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            this.pixel_image = Convolution(mat);
        }

        public void Repoussage()
        {
            int[,] mat = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            this.pixel_image = Convolution(mat);
        }

        public void EdgesReinforcement()
        {
            int[,] mat = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            this.pixel_image = Convolution(mat);
        }


        public byte[] TableauByte(byte[] tab, int pos, int length)
        {

            byte[] retour = new byte[length];
            for (int i = pos; i < pos + length; i++)
            {
                retour[i - pos] = tab[i];
            }
            return retour;
        }


        public Pixel[,] Convolution(int[,] mat_conv)
        {
            Pixel[,] new_matrix = new Pixel[pixel_image.GetLength(0), pixel_image.GetLength(1)];
            int sommer = 0;
            int sommeg = 0;
            int sommeb = 0;
            int a = 0;
            int b = 0;
            int somme_noyau = 0;
            for (int i = 0; i < mat_conv.GetLength(0); i++)
            {
                for (int n = 0; n < mat_conv.GetLength(1); n++)
                {
                    somme_noyau = somme_noyau + mat_conv[i, n];
                }
            }
            if (somme_noyau == 0)
            {
                somme_noyau = 0;
                for (int i = 0; i < mat_conv.GetLength(0); i++)
                {
                    for (int n = 0; n < mat_conv.GetLength(1); n++)
                    {
                        if (mat_conv[i, n] < 0)
                        {
                            somme_noyau = somme_noyau + mat_conv[i, n] * -1;
                        }
                        else
                        {
                            somme_noyau = somme_noyau + mat_conv[i, n];
                        }
                    }
                }
            }
            for (int i = 1; i < new_matrix.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < new_matrix.GetLength(1) - 1; j++)
                {

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {

                            sommer = sommer + mat_conv[a, b] * this.pixel_image[k, l].R;
                            sommeg = sommeg + mat_conv[a, b] * this.pixel_image[k, l].G;
                            sommeb = sommeb + mat_conv[a, b] * this.pixel_image[k, l].B;
                            b++;
                        }
                        a++;
                        b = 0;
                    }
                    a = 0;
                    new_matrix[i, j].R = (byte)(sommer / somme_noyau);
                    new_matrix[i, j].G = (byte)(sommeg / somme_noyau);
                    new_matrix[i, j].B = (byte)(sommeb / somme_noyau);
                    sommer = 0;
                    sommeg = 0;
                    sommeb = 0;
                }
            }
            return new_matrix;
        }



        public void Affiche(bool only_header)
        {


            if (!only_header)
            {
                




                Console.WriteLine("\n IMAGE \n");

                for (int i = 0; i < this.pixel_image.GetLength(0); i++)
                {
                    for (int j = 0; j < this.pixel_image.GetLength(1); j++)
                    {
                        Console.Write("(" + this.pixel_image[i, j].R + ", " + this.pixel_image[i, j].G + ", " + this.pixel_image[i, j].B + ')');
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\n HEADER \n");
                for (int i = 0; i < 14; i++)
                {
                    Console.Write(this.header[i] + " ");
                }
                Console.WriteLine("\n HEADER INFO \n");

                for (int i = 14; i < 54; i++)
                {
                    Console.Write(this.header[i] + " ");

                }

            }
        }
        public void Histogramme()
        {


            Pixel rouge = new Pixel(255, 0, 0);
            Pixel vert = new Pixel(0, 255, 0);
            Pixel bleu = new Pixel(0, 0, 255);
            Pixel gris = new Pixel(172, 172, 172);

            int[] tab_red = new int[256];
            int[] tab_green = new int[256];
            int[] tab_blue = new int[256];
            int[] tab_grey = new int[256];

            for (int i = 0; i < tab_red.Length; i++)
            {
                tab_red[i] = pixel_count(pixel_image, (byte)(i), "r") / 10;
            }
            for (int i = 0; i < tab_green.Length; i++)
            {
                tab_green[i] = pixel_count(pixel_image, (byte)(i), "g") / 10;
            }
            for (int i = 0; i < tab_blue.Length; i++)
            {
                tab_blue[i] = pixel_count(pixel_image, (byte)(i), "b") / 10;
            }
            for (int i = 0; i < tab_grey.Length; i++)
            {
                tab_grey[i] = pixel_count(pixel_image, (byte)(i), "grey") / 10;
            }



            int max = 0;

            max = (Maximum(tab_red) > Maximum(tab_green)) ? Maximum(tab_red) : Maximum(tab_green);
            max = (max > Maximum(tab_blue)) ? max : Maximum(tab_blue);
            max = (max > Maximum(tab_grey)) ? max : Maximum(tab_grey);

            Pixel[,] histo = new Pixel[max, 4 * 256 + 30];

            for (int j = 0; j < 256; j++)
            {
                for (int n = 0; n < tab_red[j]; n++)
                {
                    histo[n, j] = rouge;
                }
            }
            for (int j = 256 + 10; j < 256 + 10 + 256; j++)
            {
                for (int n = 0; n < tab_green[j - 256 - 10]; n++)
                {
                    histo[n, j] = vert;
                }
            }
            for (int j = 256 + 10 + 256 + 10; j < 256 + 10 + 256 + 10 + 256; j++)
            {
                for (int n = 0; n < tab_blue[j - 256 - 10 - 256 - 10]; n++)
                {
                    histo[n, j] = bleu;
                }
            }

            for (int j = 256 + 10 + 256 + 10 + 256 + 10; j < 256 + 10 + 256 + 10 + 256 + 10 + 256; j++)
            {
                for (int n = 0; n < tab_grey[j - 256 - 10 - 256 - 10 - 256 - 10]; n++)
                {
                    histo[n, j] = gris;
                }
            }


            this.pixel_image = histo;
        }
        public int pixel_count(Pixel[,] image, byte temp, string c)
        {
            c.ToLower();
            int retour = 0;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (c == "r")
                    {
                        if (image[i, j].R == temp)
                        {
                            retour++;
                        }
                    }
                    else if (c == "g")
                    {
                        if (image[i, j].G == temp)
                        {
                            retour++;
                        }
                    }
                    else if (c == "b")
                    {
                        if (image[i, j].B == temp)
                        {
                            retour++;
                        }
                    }
                    else if (c == "grey")
                    {
                        if ((image[i, j].B + image[i, j].G + image[i, j].R) / 3 == temp)
                        {
                            retour++;
                        }
                    }
                }

            }
            return retour;
        }
        public int Maximum(int[] tab)
        {
            int retour = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] > retour)
                {
                    retour = tab[i];
                }
            }
            return retour;
        }
        public void HiddenPic(Pixel[,] image2)
        {
            Pixel[,] image_retour = new Pixel[this.pixel_image.GetLength(0), this.pixel_image.GetLength(1)];
            for (int i = 0; i < image_retour.GetLength(0); i++)
            {
                for (int n = 0; n < image_retour.GetLength(1); n++)
                {
                    image_retour[i, n].B = (byte)Convertir_Binairy_To_Int(Concaténer_tableaux(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].B), Convertir_Int_To_Binairy((int)image2[i, n].B)));
                    image_retour[i, n].G = (byte)Convertir_Binairy_To_Int(Concaténer_tableaux(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].G), Convertir_Int_To_Binairy((int)image2[i, n].G)));
                    image_retour[i, n].R = (byte)Convertir_Binairy_To_Int(Concaténer_tableaux(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].R), Convertir_Int_To_Binairy((int)image2[i, n].R)));
                }
            }
            this.pixel_image = image_retour;
        }
        public void Retrouver_image()
        {
            Pixel[,] image_retour = new Pixel[this.pixel_image.GetLength(0), this.pixel_image.GetLength(1)];
            for (int i = 0; i < image_retour.GetLength(0); i++)
            {
                for (int n = 0; n < image_retour.GetLength(1); n++)
                {
                    image_retour[i, n].B = (byte)Convertir_Binairy_To_Int(Recuperer_tableau(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].B)));
                    image_retour[i, n].G = (byte)Convertir_Binairy_To_Int(Recuperer_tableau(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].G)));
                    image_retour[i, n].R = (byte)Convertir_Binairy_To_Int(Recuperer_tableau(Convertir_Int_To_Binairy((int)this.pixel_image[i, n].R)));
                }
            }
            this.pixel_image = image_retour;
        }
        public int Convertir_Binairy_To_Int(int[] tab)
        {
            int s = 0;
            for (int n = tab.Length - 1; n >= 0; n--)
            {
                s += tab[tab.Length - 1 - n] * Convert.ToInt32(Math.Pow(2, n));
            }
            return s;
        }

        public int[] Convertir_Int_To_Binairy(int entier)
        {
            int p = 0;
            while ((entier / Convert.ToInt64(Math.Pow(2, p)) >= 1))
            {
                p++;
            }
            p--;
            int[] retour = new int[8];
            int[] temp = new int[8];
            for (int i = 0; i < retour.Length; i++)
            {
                temp[i] = 0;
            }
            for (int i = p; i >= 0; i--)
            {
                temp[i] = Convert.ToByte(entier / Convert.ToInt64(Math.Pow(2, p)));
                entier -= temp[i] * Convert.ToInt32(Math.Pow(2, p));
                p--;
            }
            for (int i = 0; i < retour.Length; i++)
            {
                retour[i] = temp[retour.Length - i - 1];
            }
            return retour;
        }
        public int[] Concaténer_tableaux(int[] tab1, int[] tab2)
        {
            int[] retour = new int[8];
            for (int i = 0; i < 4; i++)
            {
                retour[i] = tab1[i];
            }
            for (int i = 4; i < 8; i++)
            {
                retour[i] = tab2[i - 4];
            }
            return retour;
        }
        public int[] Recuperer_tableau(int[] tab)
        {
            int[] retour = new int[8];
            for (int i = 0; i < retour.Length; i++)
            {
                retour[i] = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                retour[i] = tab[4 + i];
            }
            return retour;
        }
    }




}

