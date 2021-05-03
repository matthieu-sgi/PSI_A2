using System;

namespace PSI_A2
{
    class QrCode
    {
        private string my_string;

        private string writing_path;
        private MyImage qr;
        private bool[,] modif;


        /// <summary>
        /// Constructeur unique de notre QRcode avec un string à cacher et un chemin d'écriture du QRcode
        /// </summary>
        /// <param name="_my_string"></param>
        /// <param name="writing_path"></param>

        public QrCode(string _my_string, string writing_path)
        {
            this.my_string = _my_string.ToUpper();
            this.writing_path = writing_path;



            QrCode_Generator();
        }

        /// <summary>
        /// Fonction générant le header de notre QRcode pour pouvoir l'enregistrer
        /// </summary>
        /// <returns></returns>

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

        /// <summary>
        /// Fonction d'enregistrement du QRcode, appelant la fonction FromImageToFile de la class MyImage
        /// </summary>

        public void Qr_Save()
        {
            Pixel[,] temp = new Pixel[qr.Pixel_image.GetLength(0), qr.Pixel_image.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = this.qr.Pixel_image[temp.GetLength(0) - 1 - i, j];
                }
            }
            this.qr.Pixel_image = temp;
            this.qr.FromImageToFile(this.writing_path);
        }


        /// <summary>
        /// Fonction générant les caractères commun à tous les QRcodes (les motifs d'alignements et de synchronisation)
        /// </summary>
        /// <param name="qr"></param>
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
                    //Je fais les liserés blancs autour des séparateurs

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
                qr[6, j] = new Pixel("w");
            }
            for (int i = 9; i < qr.GetLength(0) - 7 - 1; i += 2)
            {
                qr[i, 6] = new Pixel("w");
            }

        }


        /// <summary>
        /// Fonction encodant le string dans le QRcode. Booléen qui indique si nous sommes en version 1 ou 2
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="version_2"></param>
        private void Encoding(string bits, bool version_2)
        {

            bool monte = true;
            string mask = "111011111000100";
            int counter = 0;


            int offset = 0;
            //Boucle plaçant les bits de mon string sur le QrCode
            for (int j = 0; j < (this.qr.Pixel_image.GetLength(1) / 2); j++)
            {


                for (int y = monte ? this.qr.Pixel_image.GetLength(0) - 1 : 0; monte ? y >= 0 : y < this.qr.Pixel_image.GetLength(0); y += monte ? -1 : +1)
                {
                    for (int x = this.qr.Pixel_image.GetLength(0) - 1 - offset; x > this.qr.Pixel_image.GetLength(0) - offset - 3; x--)
                    {

                        if ((y < 9 && x < 9) || (y < 9 && x > this.qr.Pixel_image.GetLength(1) - 9) || (y > this.qr.Pixel_image.GetLength(0) - 9 && x < 8)
                            || y == 6 || x == 6 || (y > this.qr.Pixel_image.GetLength(0) - 9 && x == 8)) continue;

                        else
                        {


                            if (version_2)
                            {
                                if ((y < this.qr.Pixel_image.GetLength(0) - 9 || x < this.qr.Pixel_image.GetLength(1) - 9) || (y > this.qr.Pixel_image.GetLength(0) - 5
                                 || x > this.qr.Pixel_image.GetLength(1) - 5))
                                {

                                    Console.Write(counter + " ");
                                    if (bits.Length <= counter)
                                    {
                                        this.qr.Pixel_image[y, x] = new Pixel("b");
                                    }
                                    else if (bits[counter] == '0')
                                    {

                                        this.qr.Pixel_image[y, x] = new Pixel("w");
                                    }
                                    else this.qr.Pixel_image[y, x] = new Pixel("b");
                                    counter++;

                                }
                            }
                            else if (!version_2)
                            {

                                Console.Write(counter + " ");

                                if (bits[counter] == '0')
                                {

                                    this.qr.Pixel_image[y, x] = new Pixel("w");
                                }
                                else this.qr.Pixel_image[y, x] = new Pixel("b");
                                counter++;
                            }




                        }


                    }


                }
                monte = !monte;
                offset += 2;
                if (this.qr.Pixel_image.GetLength(0) - 1 - offset == 6)
                {
                    offset++;
                }
            }
            
            offset = 0;
            counter = 0;

            
            //Boucle appliquant le masque au QrCode
            for (int j = 0; j < (this.qr.Pixel_image.GetLength(1) / 2); j++)
            {


                for (int y = monte ? this.qr.Pixel_image.GetLength(0) - 1 : 0; monte ? y >= 0 : y < this.qr.Pixel_image.GetLength(0); y += monte ? -1 : +1)
                {
                    for (int x = this.qr.Pixel_image.GetLength(0) - 1 - offset; x > this.qr.Pixel_image.GetLength(0) - offset - 3; x--)
                    {

                        if ((y < 9 && x < 9) || (y < 9 && x > this.qr.Pixel_image.GetLength(1) - 9) || (y > this.qr.Pixel_image.GetLength(0) - 9 && x < 8)
                            || y == 6 || x == 6 || (y > this.qr.Pixel_image.GetLength(0) - 9 && x == 8)) continue;

                        else
                        {


                            if (version_2)
                            {
                                if ((y < this.qr.Pixel_image.GetLength(0) - 9 || x < this.qr.Pixel_image.GetLength(1) - 9) || (y > this.qr.Pixel_image.GetLength(0) - 5
                                 || x > this.qr.Pixel_image.GetLength(1) - 5))
                                {

                                    if (bits.Length <= counter)
                                    {
                                        this.qr.Pixel_image[y, x] = new Pixel("b");
                                    }
                                    else if ((x + y) % 2 == 0 ^ this.qr.Pixel_image[y, x].IsWhite)
                                    {

                                        this.qr.Pixel_image[y, x] = new Pixel("w");
                                    }
                                    else this.qr.Pixel_image[y, x] = new Pixel("b");
                                    counter++;

                                }
                            }
                            else if (!version_2)
                            {

                                Console.Write(counter + " ");

                                if ((x + y) % 2 == 0 ^ this.qr.Pixel_image[y, x].IsWhite)
                                {

                                    this.qr.Pixel_image[y, x] = new Pixel("w");
                                }
                                else this.qr.Pixel_image[y, x] = new Pixel("b");
                                counter++;
                            }




                        }


                    }


                }
                monte = !monte;
                offset += 2;
                if (this.qr.Pixel_image.GetLength(0) - 1 - offset == 6)
                {
                    offset++;
                }
            }




            //partie du code qui ajoute les bits de masques aux endroits nécessaires

            for (int i = 0; i < mask.Length; i++)
            {
                bool temp = (mask[i] == '0') ? true : false;
                if (i < 6)
                {
                    this.qr.Pixel_image[8, i] = new Pixel(temp);
                    this.qr.Pixel_image[this.qr.Pixel_image.GetLength(0) - 1 - i, 8] = new Pixel(temp);
                }
                else if (i > 8)
                {
                    this.qr.Pixel_image[8, this.qr.Pixel_image.GetLength(1) - 15 + i] = new Pixel(temp);
                    this.qr.Pixel_image[14 - i, 8] = new Pixel(temp);
                }
                else if (i == 6)
                {
                    this.qr.Pixel_image[this.qr.Pixel_image.GetLength(0) - 1 - i, 8] = new Pixel(temp);
                    this.qr.Pixel_image[8, 7] = new Pixel(temp);


                }
                else
                {
                    this.qr.Pixel_image[8, this.qr.Pixel_image.GetLength(1) - 15 + i] = new Pixel(temp);
                    this.qr.Pixel_image[15 - i, 8] = new Pixel(temp);
                }
            }




        }




        /// <summary>
        /// Fonction ajoutant le motif de recherche pour la version 2 du QRCode
        /// </summary>
        /// <param name="qr"></param>
        private void MandatoryPart_V2(Pixel[,] qr)
        {

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 2 && j == 2) qr[i + qr.GetLength(0) - 9, j + qr.GetLength(1) - 9] = new Pixel("b");
                    else if (i == 0 || j == 0 || j == 4 || i == 4) qr[i + qr.GetLength(0) - 9, j + qr.GetLength(1) - 9] = new Pixel("b");
                    else qr[i + qr.GetLength(0) - 9, j + qr.GetLength(1) - 9] = new Pixel("w");
                }
            }

        }

        /// <summary>
        /// Fonction regroupant les différents fonctions de notre classe et générant notre QRcode
        /// </summary>

        public void QrCode_Generator()
        {
            
            if (this.my_string.Length < 26)
            {
                Pixel[,] pixel_qr = new Pixel[21, 21];
                Mutual_Part(pixel_qr);
                this.qr = new MyImage("BM", 54, Header_Generator(), pixel_qr);
                Encoding(String_To_Save(19, 7), false);

            }
            else if (this.my_string.Length < 48 && this.my_string.Length > 25)
            {
                Pixel[,] pixel_qr = new Pixel[25, 25];
                Mutual_Part(pixel_qr);
                MandatoryPart_V2(pixel_qr);
                this.qr = new MyImage("BM", 54, Header_Generator(), pixel_qr);
                Encoding(String_To_Save(34, 10), true);


            }
            else Console.WriteLine("La chaîne de caractère entrée est trop longue");



        }

        /// <summary>
        /// Fonction retournant le tableau d'entier nécessaire au QRcode à partir du string que nous voulons encoder
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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

            return array_retour;
        }

        /// <summary>
        /// Fonction qui retourne le string de bits à encoder dans le QRcode en utilisant notamment la classe ReedSolomon
        /// </summary>
        /// <param name="total_octet"></param>
        /// <param name="ec_octet"></param>
        /// <returns></returns>
        public string String_To_Save(int total_octet, int ec_octet)
        {
            string retour_string = "0010";
            string specific_octet_1 = "11101100";
            string specific_octet_2 = "00010001";
            //retour_string += BinaryConverterFromInt(this.my_string.Length, 9); //Longueur de la chaine de caractères
            retour_string += Convert.ToString(this.my_string.Length, 2).PadLeft(9, '0');
            int[] result_int = String_To_Int(this.my_string);

            //Données encodées avec un test pour savoir s'il faut l'encoder sur 11 ou 6 bits
            for (int i = 0; i < result_int.Length; i++)
            {
                retour_string += (this.my_string.Length % 2 == 1 && i == result_int.Length - 1) ?
                    Convert.ToString(result_int[i], 2).PadLeft(6, '0') : Convert.ToString(result_int[i], 2).PadLeft(11, '0');
            }

            //Terminaison
            for (int i = 0; i < 4 && retour_string.Length < total_octet * 8; i++)
            {
                retour_string += 0;
            }


            //Complément pour faire un multiple de 8
            for (int i = 0; i < retour_string.Length % 8; i++)
            {
                retour_string += 0;
            }

            bool tipe = true;
            //Ajoute les octets spécifiques 
            int init = (total_octet * 8 - retour_string.Length) / 8;
            for (int i = 0; i < init; i++)
            {
                if (!tipe) retour_string += specific_octet_2;
                else retour_string += specific_octet_1;
                tipe = !tipe;

            }

            
            byte[] buff = String_To_ByteArray(retour_string);

            byte[] ecc = ReedSolomonAlgorithm.Encode(buff, ec_octet, ErrorCorrectionCodeType.QRCode);
            for (int i = 0; i < ecc.Length; i++)
            {
                retour_string += BinaryConverterFromInt(ecc[i], 8);
            }
            //Console.WriteLine(retour_string+"\n"+retour_string.Length);
            //Console.WriteLine(retour_string + "\n" + retour_string.Length);
           

            return retour_string;
        }


        /// <summary>
        /// Afficahge du Qrcode entier ou seulement du header
        /// </summary>
        /// <param name="only_header"></param>
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


        /// <summary>
        /// Convertis un entier en un string de bits
        /// </summary>
        /// <param name="num"></param>
        /// <param name="taille"></param>
        /// <returns></returns>
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

        
        /// <summary>
        /// Transforme un string en un tableau d'octets
        /// </summary>
        /// <param name="my_string"></param>
        /// <returns></returns>
        private byte[] String_To_ByteArray(string my_string)
        {
            byte[] retour = new byte[my_string.Length / 8];
            for (int i = 0; i < my_string.Length; i += 8)
            {
                byte temp = 0;
                for (int j = 0; j < 8; j++)
                {
                    temp = (byte)(temp << 1);
                    temp = (byte)(temp | ((int)(my_string[i + j]) - 48));

                }
                retour[i / 8] = temp;
            }

            return retour;


        }
    }
}
