﻿using System;
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

        public MyImage(string type, int taille, int offset, int width, int height, int bits_by_Color, int[,] image)
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
                
                
                this.taille = Convertir_Endian_To_Int(ByteToInt(image_temp,2,4));
                this.offset = Convertir_Endian_To_Int(ByteToInt(image_temp, 10, 4));
                this.width = Convertir_Endian_To_Int(ByteToInt(image_temp, 18, 4))*3;
                this.height = Convertir_Endian_To_Int(ByteToInt(image_temp, 22, 4))*3;
                this.bits_by_Color = Convertir_Endian_To_Int(ByteToInt(image_temp, 28, 2));
                this.image = new byte[this.height, this.width];
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

        static int ByteToInt(byte[] tab,int pos, int length)
        {
            string retour = "";
            for(int i = pos; i <= length+pos; i++)
            {
                retour += tab[i];
            }
            return Convert.ToInt32(retour);
        }

    }
}
