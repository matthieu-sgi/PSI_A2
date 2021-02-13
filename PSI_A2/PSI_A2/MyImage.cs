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

                for (int i = 0; i < this.header.Length; i++)
                {
                    this.header[i] = image_temp[i];
                }
                for (int i = 0; i < this.height; i++)
                {
                    for (int j = 0; j < this.width_byte; j++)
                    {
                        this.image[i, j] = image_temp[this.offset + j + i * (this.width_byte + this.width_byte % 4)];
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




        public void FromImageToFile(string file)
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




        public void Resize(double factor)
        {
            
            if( factor > 0)
            {
                byte[,] image_temp = new byte[Convert.ToInt32(this.image.GetLength(0) * factor), Convert.ToInt32(this.image.GetLength(1) * factor)];
                for(int i = 0; i < image_temp.GetLength(0); i++)
                {
                    for(int j= 0; j < image_temp.GetLength(1); j++)
                    {
                        
                    }
                }

            }


        }

        public void Rotation()
        {

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
    }
}
