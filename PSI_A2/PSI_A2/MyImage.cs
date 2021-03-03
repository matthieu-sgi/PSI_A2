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
        private int bits_by_Color;
        private byte[,] image;
        private byte[] header;

        private Pixel[,] pixel_image;



        public MyImage(string type, int taille, int offset, int width, int height, int bits_by_Color, byte[,] image)
        {
            this.type = type;
            this.taille = taille;
            this.offset = offset;
            this.width_pixel = width;
            this.height = height;
            this.bits_by_Color = bits_by_Color;
            this.image = image;
        }

        public MyImage(string myfile)
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
                this.bits_by_Color = Convertir_Endian_To_Int(TableauByte(image_temp, 28, 2));
                this.image = new byte[this.height, this.width_byte];
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
                            this.pixel_image[i, j].R = image_temp[this.offset + i * width_to_save + j * 3];
                            this.pixel_image[i, j].G = image_temp[this.offset + i * width_to_save + j * 3 + 1];
                            this.pixel_image[i, j].B = image_temp[this.offset + i * width_to_save + j * 3 + 2];
                        }


                    }

                }



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




        public void FromImageToFile(string file, bool using_pixel_class)
        {
            if (this.type == "BM")
            {
                int width_to_save = (((pixel_image.GetLength(1) * 3) + 3) / 4) * 4;
                this.taille = this.offset + width_to_save * pixel_image.GetLength(0);

                byte[] image_to_write = new byte[this.taille];


                //Console.WriteLine(image_to_write.Length);

                for (int i = 0; i < this.offset; i++)
                {
                    image_to_write[i] = this.header[i];
                }

                //Recalculating size, width and height
                for (int i = 0; i < 4; i++)
                {
                    image_to_write[i + 2] = Convertir_Int_To_Endian(this.taille)[i];
                    image_to_write[i + 18] = Convertir_Int_To_Endian(pixel_image.GetLength(1))[i];
                    image_to_write[i + 22] = Convertir_Int_To_Endian(pixel_image.GetLength(0))[i];



                }






                for (int i = 0; i < pixel_image.GetLength(0); i++)
                {
                    for (int j = 0; j < width_to_save; j++)
                    {
                        if (j < pixel_image.GetLength(1))
                        {
                            image_to_write[this.offset + i * width_to_save + j * 3] = this.pixel_image[i, j].R;
                            image_to_write[this.offset + i * width_to_save + j * 3 + 1] = this.pixel_image[i, j].G;
                            image_to_write[this.offset + i * width_to_save + j * 3 + 2] = this.pixel_image[i, j].B;
                        }


                    }

                }


                File.WriteAllBytes(file, image_to_write);

            }

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

        public byte[] Convertir_Int_To_Endian(long entier)
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

        #region Test de Fonctions "Convertir_Int_To_Endian"
        /*public byte[] Convertir_Int_To_Endian(int entier, int target_byte, byte[] res)
        {


            if (entier / Math.Pow(256, target_byte) > 1)
            {
                return res;
            }
            else if (target_byte <= 0) return res;
            else if (entier / Math.Pow(256, target_byte - 1) >= 1)
            {
                res[target_byte - 1] = Convert.ToByte(Math.Floor(entier / Math.Pow(256, target_byte - 1)));
                return Convertir_Int_To_Endian(entier - (Convert.ToInt32(entier / Math.Pow(256, target_byte - 1)) - 1) * Convert.ToInt32(Math.Pow(256, target_byte - 1)), target_byte - 1, res);
            }
            else if (entier / Math.Pow(256, target_byte - 1) < 1)
            {
                res[target_byte - 1] = 0;
                return Convertir_Int_To_Endian(entier, target_byte - 1, res);
            }

            else return res;
        }*/

        /*public byte[] Convertir_Int_To_Endian(int entier, int target_byte)
        {

            int p = 0;

            while ((entier / Convert.ToInt32(Math.Pow(256, p)) >= 1))
            {
                p++;
            }

            p--;

            List<byte> tab = new List<byte>(p + 1);
            for (int i = tab.Capacity - 1; i >= 0; i--)
            {
                tab[i] = Convert.ToByte(entier / Convert.ToInt32(Math.Pow(256, p)));
                entier -= tab[i] * Convert.ToInt32(Math.Pow(256, p));
                p--;
            }

            while (tab.Count < target_byte)
            {
                tab.Add(0);


            }

            return tab.ToArray();
        }*/
        #endregion


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
            Console.WriteLine(new_height + " " + new_width);
            Console.ReadKey();
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

        public void blur()
        {
            //int[,] mat_blur = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int[,] mat_blur = { { 1, 4, 6, 4, 1 }, { 4, 16, 24, 16, 4 }, { 6, 24, 36, 24, 6 }, { 4, 16, 24, 16, 4 } };
            this.pixel_image = Convolution(mat_blur);


        }

        public void edges_detection()
        {
            int[,] mat = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
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
            new_matrix = this.pixel_image;
            int somme_matrix_convo = 0;
            int decalage = 0;
            for (int i = 0; i < mat_conv.GetLength(0); i++)
            {
                for (int j = 0; j < mat_conv.GetLength(1); j++)
                {
                    somme_matrix_convo += mat_conv[i, j];

                }
            }

            if (somme_matrix_convo == 0)
            {
                somme_matrix_convo = 1;
                decalage = 128;
            }
            decalage = (somme_matrix_convo< 0) ? 255 : decalage;



            for (int i = mat_conv.GetLength(0) / 2; i < new_matrix.GetLength(0) - mat_conv.GetLength(0) / 2; i++)
            {
                for (int j = mat_conv.GetLength(1) / 2; j < new_matrix.GetLength(1) - mat_conv.GetLength(1) / 2; j++)
                {

                    
                    int ope_red = 0;
                    int ope_green = 0;
                    int ope_blue = 0;
                    

                    for (int k = -mat_conv.GetLength(0) / 2; k < mat_conv.GetLength(0) / 2; k++)
                    {
                        for (int l = -mat_conv.GetLength(1) / 2; l < mat_conv.GetLength(1) / 2; l++)
                        {
                            ope_red += (this.pixel_image[i + k, j + l].R * mat_conv[k + mat_conv.GetLength(0) / 2, l + mat_conv.GetLength(1) / 2])/somme_matrix_convo;
                            ope_green += (this.pixel_image[i + k, j + l].G * mat_conv[k + mat_conv.GetLength(0) / 2, l + mat_conv.GetLength(1) / 2])/somme_matrix_convo;
                            ope_blue += (this.pixel_image[i + k, j + l].R * mat_conv[k + mat_conv.GetLength(0) / 2, l + mat_conv.GetLength(1) / 2])/somme_matrix_convo;

                        }
                    }
                    
                    new_matrix[i, j] = new Pixel((byte)(ope_red+decalage), (byte)(ope_blue+decalage), (byte)(ope_green+decalage));
                }
            }
            return new_matrix;
        }

        public void Affiche(bool only_header)
        {


            if (!only_header)
            {
                Console.WriteLine("\n HEADER \n");
                for (int i = 0; i < 14; i++)
                {
                    Console.Write(header[i] + " ");
                }
                Console.WriteLine("\n HEADER INFO \n");

                for (int i = 14; i < 54; i++)
                {
                    Console.Write(header[i] + " ");

                }





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
                    Console.Write(header[i] + " ");
                }
                Console.WriteLine("\n HEADER INFO \n");

                for (int i = 14; i < 54; i++)
                {
                    Console.Write(header[i] + " ");

                }

            }







        }
    }
}
