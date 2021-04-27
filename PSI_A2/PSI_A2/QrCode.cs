using System;

namespace PSI_A2
{
    class QrCode
    {
        private string my_string;

        private string writing_path;
        private MyImage qr;


        public QrCode(string _my_string, string writing_path)
        {
            this.my_string = _my_string.ToUpper();
            this.writing_path = writing_path;

            QrCode_Generator();
        }

        private byte[] Header_Generator()
        {
            byte[] header = new byte[54];
            for (int i = 0; i < 4; i++)
            {
                byte[] tab_temp = { 0, 18, 11, 0 };
                header[10 + i] = MyImage.Convertir_Int_To_Endian(54)[i];
                header[14 + i] = MyImage.Convertir_Int_To_Endian(40)[i];
                header[38 + i] = MyImage.Convertir_Int_To_Endian(1000)[i];
                header[42 + i] = MyImage.Convertir_Int_To_Endian(1000)[i];
                //header[38 + i] = tab_temp[i];
                //header[42 + i] = tab_temp[i];

            }

            for (int i = 0; i < 2; i++)
            {
                header[26 + i] = MyImage.Convertir_Int_To_Endian(1)[i];
                header[28 + i] = MyImage.Convertir_Int_To_Endian(24)[i];
            }


            return header;
        }

        public void Qr_Save()
        {

            this.qr.FromImageToFile(this.writing_path);
        }

        private void Mutual_Part(Pixel[,] qr)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (((i == 1 || i == 5) && j > 0 && j < 6) || ((j == 1 || j == 5) && i > 1 && i < 5))
                    {
                        qr[i, j] = new Pixel("w");
                        qr[i, j + qr.GetLength(1) - 7] = new Pixel("w");
                        qr[i + qr.GetLength(0) - 7, j] = new Pixel("w");

                    }
                    else
                    {
                        qr[i, j] = new Pixel("b");
                        qr[i, j + qr.GetLength(1) - 7] = new Pixel("b");
                        qr[i + qr.GetLength(0) - 7, j] = new Pixel("b");
                    }
                    //Je fais les liseré blanc autour des séparateurs

                    qr[i, 7] = new Pixel("w");
                    qr[i, qr.GetLength(1) - 1 - 7] = new Pixel("w");

                    qr[7, qr.GetLength(1) - 1 - j] = new Pixel("w");
                    qr[7, j] = new Pixel("w");

                    qr[qr.GetLength(0) - 1 - i, 7] = new Pixel("w");
                    qr[qr.GetLength(0) - 1 - 7, j] = new Pixel("w");


                }
            }
            //Les trois pixels de coin des liserés
            qr[7, 7] = new Pixel("w");
            qr[qr.GetLength(0) - 1 - 7, 7] = new Pixel("w");
            qr[7, qr.GetLength(1) - 1 - 7] = new Pixel("w");

            //Motifs de synchronisation
            for (int j = 9; j < qr.GetLength(1) - 7 - 1; j += 2)
            {
                qr[7, j] = new Pixel("w");
            }
            for (int i = 9; i < qr.GetLength(0) - 7 - 1; i += 2)
            {
                qr[i, 7] = new Pixel("w");
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
            Mutual_Part(qr);

        }

        /*private Pixel[,] MandatoryPart_V2(Pixel[,] qr)
        {

        }*/

        public void QrCode_Generator()
        {
            string str_to_hide = "";
            if (this.my_string.Length < 26)
            {
                Pixel[,] pixel_qr = new Pixel[21, 21];
                Mutual_Part(pixel_qr);
                this.qr = new MyImage("BM", 54, Header_Generator(), pixel_qr);


            }
            else if (this.my_string.Length < 48)
            {
                Pixel[,] pixel_qr = new Pixel[25, 25];
                Mutual_Part(pixel_qr);
                this.qr = new MyImage("BM", 54, Header_Generator(), pixel_qr);


            }
            else Console.WriteLine("La chaîne de caractère entrée est trop longue");



        }

        public int[] String_To_Int(string arg)
        {

            arg = arg.ToUpper();
            int[] array_retour = new int[arg.Length / 2 + arg.Length % 2];

            for (int i = 0; i < arg.Length; i++)
            {
                if (char.IsLetterOrDigit(arg[i]))
                {

                    array_retour[i / 2] += (arg.Length % 2 == 1 && i == arg.Length - 1) ? (int)(Math.Pow(45, 0) * ((int)(arg[i]) - 55)) : (int)(Math.Pow(45, (i + 1) % 2) * ((int)(arg[i]) - 55));

                }
                else
                {
                    int value;
                    switch (arg[i])
                    {
                        case ' ':
                            value = 36;
                            break;
                        case '$':
                            value = 37;
                            break;
                        case '%':
                            value = 38;
                            break;
                        case '*':
                            value = 39;
                            break;
                        case '+':
                            value = 40;
                            break;
                        case '-':
                            value = 41;
                            break;
                        case '.':
                            value = 42;
                            break;
                        case '/':
                            value = 43;
                            break;
                        case ':':
                            value = 44;
                            break;
                        default:
                            value = 0;
                            break;

                    }
                    array_retour[i / 2] += (arg.Length % 2 == 1 && i == arg.Length - 1) ? (int)(Math.Pow(45, 0)) * (value) : (int)(Math.Pow(45, (i + 1) % 2)) * (value);
                }

            }
            Console.WriteLine(String.Join(" ", array_retour));
            return array_retour;
        }


        public string String_To_Save(int total_octet,int ec_octet)
        {
            string retour_string = "0010";
            string specific_octet_1 = "1110110000010001";

            retour_string += BinaryConverterFromInt(this.my_string.Length, 8); //Longueur de la chaine de caractères
            int[] result_int = String_To_Int(this.my_string);

            //Données encodées avec un test pour savoir s'il faut l'encoder sur 8 ou 6 bits
            for (int i = 0; i < result_int.Length; i++)
            {   
                retour_string += (this.my_string.Length % 2 == 1 && i == result_int.Length - 1) ? BinaryConverterFromInt(result_int[i], 6) : BinaryConverterFromInt(result_int[i], 8);
            }
            //Terminaison
            for (int i = 0; i < 4 && retour_string.Length < total_octet * 8; i++)
            {
                retour_string += 0;
            }

            //Complément pour faire un multiple de 8
            for(int i = 0; i < retour_string.Length % 8; i++)
            {
                retour_string += 0;
            }
            string temp = "";
            //Ajoute les octets spécifiques 
            for (int i = 0; i < (((total_octet * 8 - retour_string.Length) / 8) - 7) * 8;  i++)
            {
                temp += specific_octet_1[(i) % 16];
                
            }
            retour_string += temp;


            //Console.WriteLine(retour_string+"\n"+retour_string.Length);

            return retour_string;
        }

        public void Affiche(bool only_header)
        {
            if (only_header)
            {
                byte[] header = Header_Generator();

                for (int i = 0; i < 14; i++)
                {
                    Console.Write(header[i] + " ");
                }
                Console.WriteLine();

                for (int i = 14; i < header.Length; i++)
                {
                    Console.Write(header[i] + " ");
                }
            }
            else
            {
                for (int i = 0; i < this.qr.Pixel_image.GetLength(0); i++)
                {
                    for (int j = 0; j < this.qr.Pixel_image.GetLength(1); j++)
                    {
                        if (this.qr.Pixel_image[i, j].R == 255 && this.qr.Pixel_image[i, j].G == 255 && this.qr.Pixel_image[i, j].B == 255)
                        {
                            Console.Write(1 + " ");
                        }
                        else Console.Write(0 + " ");
                    }
                    Console.WriteLine();
                }
            }
        }


        private string Right_Length(string bits, int len)
        {
            string retour = "";
            for (int i = 0; i < len - bits.Length; i++)
            {
                retour += 0;
            }
            return retour + bits;
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

            for (int i = 0; i < taille; i++)
            {
                if (p >= 0)
                {
                    binary = num % 2 + binary;
                    num /= 2;
                    p--;

                }
                else binary = 0 + binary;


            }
            return binary;
        }
    }
}
