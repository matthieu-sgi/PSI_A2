using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            if(this.type == "BM")
            {
                
                
                this.taille = Convertir_Endian_To_Int(TableauByte(image_temp,2,4));
                this.offset = Convertir_Endian_To_Int(TableauByte(image_temp, 10, 4));
                this.width = Convertir_Endian_To_Int(TableauByte(image_temp, 18, 4))*3;
                this.height = Convertir_Endian_To_Int(TableauByte(image_temp, 22, 4));
                this.bits_by_Color = Convertir_Endian_To_Int(TableauByte(image_temp, 28, 2));
                this.image = new byte[this.height, this.width];
                this.header = new byte[this.offset];

                for(int i = 0; i < this.header; i++)
                {
                    this.header[i] = image_temp[i]; 
                }
                for (int i = this.offset; i < this.image.Length; i+=this.height)
                {
                    for(int j = i; j < this.width; j++)
                    {
                        this.image[i, j] = image_temp[j];
                    }
                }




            }
        }

        public void FromImageToFile(string file)
        {
            if(this.type == "BM")
            {
                byte[] image_to_write = new byte[this.offset + (this.height * this.width)];
                int counter = 0;
                for(int i = 0; i < this.offset; i++)
                {
                    image_to_write[i] = this.header[i];
                }

                //Recalculating size, width and height
                for (int i = 0; i < 4; i++)
                {
                    image_to_write[i + 14] = Convertir_Int_To_Endian(this.header.Length + (this.height * this.width))[i];
                    image_to_write[i + 18] = Convertir_Int_To_Endian(this.height)[i];
                    image_to_write[i + 22] = Convertir_Int_To_Endian(this.width)[i];
                }

                


                for(int i = 0; i < this.image.GetLength(0); i++)
                {
                    for(int j= 0;j< this.image.GetLength(1); j++)
                    {
                        image_to_write[this.offset + counter] = this.image[i, j];
                        counter++;
                    }
                }

            }
                
        }

        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int pui = 0;
            int s = 0;
            for (int n = 0; n < tab.Length; n++)
            {
                s = s + tab[n] * (pui = Convert.ToInt32(Math.Pow(256, n)));
            }
            return s;
        }

        public byte[] Convertir_Int_To_Endian(int entier)
        {
            int pui = 0;
            int p = 0;

            while (entier / (pui = Convert.ToInt32(Math.Pow(256, p))) >= 1)
            {
                p = p + 1;
            }

            p = p - 1;
            byte[] tab = new byte[p + 1];
            for (int i = tab.Length - 1; i >= 0; i--)
            {
                tab[i] = entier / (pui = Convert.ToInt32(Math.Pow(256, p)));
                entier = entier - tab[i] * (pui = Convert.ToInt32(Math.Pow(256, p)));
                p = p - 1;
            }
            return tab;
        }

        static int ByteToInt(byte[] tab,int pos, int length)
        {
            string retour = "";
            for(int i = pos; i <= length+pos; i++)
            {
                retour += tab[i];
            }
            return Convert.ToInt32(retour);
        }
        public byte[] TableauByte(byte[] tab,int pos, int length)
        {
            byte[] retour = new byte[length];
            for(int i = pos;i<length;i++)
            {
                retour[pos - i] = tab[i];
            }
            return retour;
        }
    }
}
