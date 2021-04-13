using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_A2
{
    class QrCode
    {
        private string my_string;

        private string my_path;
        private MyImage qr;
       

        public QrCode(string _my_string, string _my_path)
        {
            this.my_string = _my_string.ToUpper();
            this.my_path = _my_path;
        }

        private void Mutual_Part(Pixel[,] qr)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (((i == 1 || i == 5) && j > 0 && j < 6) || ((j == 1 || j == 5) && i > 1 && i < 4))
                    {
                        qr[i, j] = new Pixel("w");
                        qr[i, j + qr.GetLength(1)-7] = new Pixel("w");
                        qr[i, j] = new Pixel("w");

                    }
                    else qr[i, j] = new Pixel("b");
                }
            }

        }


        private void MandatoryPart_V1(Pixel[,] qr)
        {
            /*for(int i = 0;i< qr.GetLength(0); i++)
            {
                for(int j = 0; j < qr.GetLength(1); j++)
                {
                    if( (i == 0 || i== 6 || i == qr.GetLength(0)-7 || i == qr.GetLength(0)-1)  && (j) )
                }
            }*/
            

        }

        private Pixel[,] MandatoryPart_V2(Pixel[,] qr)
        {

        } 

        public void QrCode_Generator()
        {
            string str_to_hide = "";
            if (this.my_string.Length < 26)
            {
                Pixel[,] pixel_qr = new Pixel[21,21];
                MandatoryPart_V1(pixel_qr);
                //this.qr = new MyImage("BM",)

            }
            else if (this.my_string.Length < 48)
            {
                Pixel[,] pixel_qr = new Pixel[25, 25];
                //this.qr = new MyImage("BM",)


            }
            else Console.WriteLine("La chaîne de caractère entrée est trop longue");
            


        }



        private string BinaryConverterFromInt(int num, int taille = -1)
        {
            string binary = "";
            int p = 0;
            while (num / Math.Pow(2, p) >= 1)
            {
                p++;
            }
            taille = (taille == -1) ? p : taille;
            
            for(int i = 0; i<taille; i++)
            {
                if (p >= 0)
                {
                    binary = num%2 + binary;
                    num /= 2;
                    p--;

                }
                else binary = 0+binary;


            }
            return binary;
        }
    }
}
