using System;
using System.Collections.Generic;
using System.IO;


namespace PSI_A2
{
    class MyImage
    {
        private string type;
        private int taille;
        private int offset;
        private int width;
        private int height;
        private int bits_by_Color;
        private byte[,] image;
        private byte[] header;



        public MyImage(string type, int taille, int offset, int width, int height, int bits_by_Color, byte[,] image)
        {
            this.type = type;
            this.taille = taille;
            this.offset = offset;
            this.width = width;
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
                this.width = Convertir_Endian_To_Int(TableauByte(image_temp, 18, 4)) * 3;
                this.height = Convertir_Endian_To_Int(TableauByte(image_temp, 22, 4));
                this.bits_by_Color = Convertir_Endian_To_Int(TableauByte(image_temp, 28, 2));
                this.image = new byte[this.height, this.width];
                this.header = new byte[this.offset-1];

                for (int i = 0; i < this.header.Length; i++)
                {
                    this.header[i] = image_temp[i];
                }
                for (int i = 0; i < this.image.GetLength(0); i++)
                {
                    for (int j = 0; j < this.image.GetLength(1); j++)
                    {
                        this.image[i, j] = image_temp[this.offset + j + i * this.image.GetLength(1)-1];
                    }
                }





            }
        }

        public byte[,] Image
        {
            get { return this.image; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public int Width
        {
            get { return this.width; }
        }

        public void FromImageToFile(string file)
        {
            if (this.type == "BM")
            {
                byte[] image_to_write = new byte[this.offset + (this.height * this.width)];
                int counter = 0;
                for (int i = 0; i < this.offset-1; i++)
                {
                    image_to_write[i] = this.header[i];
                }

                //Recalculating size, width and height
                for (int i = 0; i < 4; i++)
                {
                    image_to_write[i + 14] = Convertir_Int_To_Endian(this.header.Length + (this.height * this.width), 4,new byte[4])[i];
                    image_to_write[i + 18] = Convertir_Int_To_Endian(this.height, 4, new byte[4])[i];
                    image_to_write[i + 22] = Convertir_Int_To_Endian(this.width, 4, new byte[4])[i];
                }




                for (int i = 0; i < this.image.GetLength(0); i++)
                {
                    for (int j = 0; j < this.image.GetLength(1); j++)
                    {
                        image_to_write[this.offset + counter] = this.image[i, j];
                        //Console.WriteLine(image_to_write[this.offset + counter]);
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

        public byte[] Convertir_Int_To_Endian(int entier, int target_byte, byte[] res)
        {


            if (entier / Math.Pow(256, target_byte) > 1)
            {
                return res;
            }
            else if (target_byte <= 0) return res;
            else if (entier / Math.Pow(256, target_byte - 1) >= 1)
            {
                res[target_byte-1] = Convert.ToByte(entier / Math.Pow(256, target_byte - 1)-1);
                return Convertir_Int_To_Endian(entier - (Convert.ToInt32(entier / Math.Pow(256, target_byte - 1))-1) * Convert.ToInt32(Math.Pow(256, target_byte - 1)), target_byte - 1, res);
            }
            else if (entier / Math.Pow(256, target_byte - 1) < 1)
            {
                res[target_byte-1] = 0;
                return Convertir_Int_To_Endian(entier, target_byte-1, res);
            }
            
            else return res;
        }

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


        public byte[] TableauByte(byte[] tab, int pos, int length)
        {
            byte[] retour = new byte[length];
            for (int i = pos; i < pos + length; i++)
            {
                retour[i - pos] = tab[i];
            }
            return retour;
        }

        public void Affiche()
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
                    Console.WriteLine(image[i, j] + " ");
                }
                Console.WriteLine();
            }






        }
    }
}
