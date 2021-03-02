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
                //Je laisse le truc d'avant par sécurité 
                for (int i = 0; i < this.height; i++)
                {
                    for (int j = 0; j < this.width_byte; j++)
                    {
                        this.image[i, j] = image_temp[this.offset + j + i * (this.width_byte + this.width_byte % 4)];
                        
                    }
                }

                //J'instaure le nouveau système
                int counter = 0;
                for (int i = 0; i < this.height; i++)
                {
                    for (int j = 0; j < this.width_pixel; j++)
                    {
                        byte red= image_temp[this.offset +counter];
                        byte green = image_temp[this.offset +counter+1];
                        byte blue = image_temp[this.offset +counter+ 2];
                        counter += 3 + (this.width_byte % 4);
                        this.pixel_image[i, j] = new Pixel(red, green, blue);
                    }
                }






            }
        }
        public int Width_Pixel
        {
            get { return this.width_pixel; }
        }

        public int Height_Pixel
        {
            get { return this.height; }
        }




        public void FromImageToFile(string file, bool using_pixel_class)
        {
            if (this.type == "BM")
            {
                byte[] image_to_write = new byte[this.offset + (this.height * this.width_byte)];


                //Console.WriteLine(image_to_write.Length);
                int counter = 0;
                for (int i = 0; i < this.offset; i++)
                {
                    image_to_write[i] = this.header[i];
                }

                //Recalculating size, width and height
                for (int i = 0; i < 4; i++)
                {
                    image_to_write[i + 2] = Convertir_Int_To_Endian(this.taille)[i];
                    image_to_write[i + 18] = Convertir_Int_To_Endian(this.width_pixel)[i];
                    image_to_write[i + 22] = Convertir_Int_To_Endian(this.height)[i];



                }

                //Console.WriteLine(this.height);
                //Console.WriteLine(this.width/3);
                //Console.WriteLine(image_to_write.Length);
                //Console.WriteLine(this.width_pixel);

                if (!using_pixel_class)
                {
                    for (int i = 0; i < this.image.GetLength(0); i++)
                    {
                        for (int j = 0; j < this.image.GetLength(1); j++)
                        {
                            image_to_write[this.offset + counter] = this.image[i, j];
                            //Console.WriteLine(image_to_write[this.offset + counter]);
                            counter++;

                        }
                        for (int j = 0; j < this.width_byte % 4; j++)
                        {
                            image_to_write[this.offset + counter + j] = 0;
                            counter++;
                        }

                    }
                }
                else
                {
                    int width_to_save = (((pixel_image.GetLength(1) * 3) + 3) / 4) * 4;
                    for (int i = 0; i < pixel_image.GetLength(0); i++)
                    {
                        for(int j = 0; j < width_to_save; j++)
                        {
                            if (j < pixel_image.GetLength(1))
                            {
                                image_to_write[this.offset + i * width_to_save + j*3] = this.pixel_image[i, j].R;
                                image_to_write[this.offset + i * width_to_save  + j*3 + 1] = this.pixel_image[i, j].G;
                                image_to_write[this.offset + i * width_to_save + j*3 + 2] = this.pixel_image[i, j].B;
                            }
                            
                            
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

            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j += 3)
                {
                    byte moyenne = Convert.ToByte((this.image[i, j] + this.image[i, j + 1] + this.image[i, j + 2]) / 3);

                    this.image[i, j] = moyenne;
                    this.image[i, j + 1] = moyenne;
                    this.image[i, j + 2] = moyenne;



                }
            }

        }

        public void Noir_et_Blanc()
        {
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j += 3)
                {
                    byte moyenne = Convert.ToByte((this.image[i, j] + this.image[i, j + 1] + this.image[i, j + 2]) / 3);

                    if (moyenne < 255 / 2)
                    {
                        this.image[i, j] = 0;
                        this.image[i, j + 1] = 0;
                        this.image[i, j + 2] = 0;
                    }
                    else
                    {
                        this.image[i, j] = 255;
                        this.image[i, j + 1] = 255;
                        this.image[i, j + 2] = 255;
                    }


                }
            }



        }




        public void Resize(int new_height,int new_width)
        {

            Pixel[,] new_matrix = new Pixel[new_height, new_width];
            for(int i = 0; i < new_matrix.GetLength(0); i++)
            {
                for(int j = 0; j < new_matrix.GetLength(1); j++)
                {
                    int new_x = (int)(((double)i * (double)this.pixel_image.GetLength(0)) / (double)new_matrix.GetLength(0));
                    int new_y = (int)(((double)j * (double)this.pixel_image.GetLength(1)) / (double)new_matrix.GetLength(1));
                    new_matrix[i,j] = new Pixel(this.pixel_image[new_x, new_y].R, this.pixel_image[new_x, new_y].G, this.pixel_image[new_x, new_y].B);
                    
                }
            }

            this.pixel_image = new_matrix;
            this.height = new_height;
            this.width_byte = new_width*3;
            this.width_pixel = new_width;

        }

        public void Rotation(double angle_rad)
        {
            int[] center = { this.pixel_image.GetLength(0) / 2, this.pixel_image.GetLength(1) / 2 };
            int new_height = this.pixel_image.GetLength(0) / 2;
            int new_width = this.pixel_image.GetLength(1) / 2;
            
            for (int i = 0; i < this.pixel_image.GetLength(0); i += pixel_image.GetLength(0) - 1)
            {
                for (int j = 0; j < this.pixel_image.GetLength(1); j += pixel_image.GetLength(1) - 1)
                {


                    double r = Math.Sqrt(Math.Pow(i - center[0], 2) + Math.Pow(j - center[1], 2));
                    double theta = Math.Atan2(i-center[0], j-center[1]) + angle_rad;
                    if( new_width< center[0] + r * Math.Cos(theta))
                    {
                        new_width = (int)(center[0] + r * Math.Cos(theta));
                    }
                    if(new_height < center[1] + r * Math.Sin(theta))
                    {
                        new_height = (int)(center[1] + r * Math.Sin(theta));
                    }
                    
                }
            }
            Console.WriteLine(new_height + " " + new_width);
            Console.ReadKey();
            Pixel[,] new_matrix = new Pixel[new_height, new_width];
            int[] new_center = { new_matrix.GetLength(0) / 2, new_matrix.GetLength(1) / 2 };
            for(int i = 0; i < new_matrix.GetLength(0); i++)
            {
                for(int j= 0; j < new_matrix.GetLength(1); j++)
                {
                    double rayon = Math.Sqrt(Math.Pow(i - new_center[0], 2) + Math.Pow(j - new_center[1], 2));
                    double theta = Math.Atan2(i - new_center[0], j - new_center[1]) - angle_rad;
                    int new_x = (int)(center[0] + rayon * Math.Cos(theta));
                    int new_y = (int)(center[1] + rayon * Math.Sin(theta));
                    if(new_x >=0 && new_x<this.pixel_image.GetLength(0)&&new_y>=0 && new_y < this.pixel_image.GetLength(1))
                    {
                        new_matrix[i, j] = this.pixel_image[new_x, new_y];
                    }

                }
            }
            this.pixel_image = new_matrix;
            this.width_pixel = new_width;
            this.height = new_height;
            this.width_byte = (new_width) * 3;



        }

        public void Miror(bool horizontal)
        {
            byte[,] image_temp = new byte[this.image.GetLength(0), this.image.GetLength(1)];
            if (horizontal)
            {

                for (int i = 0; i < image_temp.GetLength(0); i++)
                {
                    for (int j = 0; j < image_temp.GetLength(1); j+=3)
                    {
                        image_temp[i, j] = this.image[i, this.image.GetLength(1) - 1 - j-2];
                        image_temp[i,j+1] = this.image[i, this.image.GetLength(1) - 1 - j-1];
                        image_temp[i,j+2] = this.image[i, this.image.GetLength(1) - 1 - j];
                    }
                }
            }else
            {
                for (int i = 0; i < image_temp.GetLength(0); i++)
                {
                    for (int j = 0; j < image_temp.GetLength(1); j ++)
                    {
                        image_temp[i, j] = this.image[this.image.GetLength(0)-1-i, j];
                        
                    }
                }
            }
            for (int i = 0; i < image_temp.GetLength(0); i++)
            {
                for (int j = 0; j < image_temp.GetLength(1); j++)
                {
                    this.image[i, j] = image_temp[i, j];
                }
            }
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

        public void Affiche(bool only_header,bool using_pixel_class)
        {
            if (!using_pixel_class)
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

                    for (int i = 0; i < image.GetLength(0); i++)
                    {
                        for (int j = 0; j < image.GetLength(1); j++)
                        {
                            Console.Write(image[i, j] + " ");
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
            else
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
                            Console.Write("(" + this.pixel_image[i, j].R + ", " + this.pixel_image[i,j].G+", "+this.pixel_image[i,j].B+')');
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
}
