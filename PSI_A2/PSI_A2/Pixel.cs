using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_A2
{
    struct Pixel
    {
        private byte r;
        private byte g;
        private byte b;
        
        /// <summary>
        /// Constructeur d'un pixel avec des valeurs RGB en entrée
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Pixel(byte _r,byte _g,byte _b)
        {

            this.r = _r;
            this.g = _g;
            this.b = _b;
        }

        public Pixel(bool isWhite)
        {
            if (isWhite)
            {
                this.r = 255;
                this.g = 255;
                this.b = 255;
            }else
            {
                this.r = 0;
                this.g = 0;
                this.b = 0;
            }
        }

        /// <summary>
        /// Constructeur d'un pixel avec des un string en entrée, soit 'w' pour white ou 'b' pour black
        /// </summary>
        /// <param name="color"></param>
        public Pixel(string color)
        {
            color = color.ToLower();
            switch (color)
            {
                case "b":
                    this.r = 0;
                    this.g = 0;
                    this.b = 0;
                    break;
                case "w":
                    this.r = 255;
                    this.g = 255;
                    this.b = 255;
                    break;
                default:
                    this.r = 0;
                    this.g = 0;
                    this.b = 0;
                    break;

            } 
        }


        /// <summary>
        /// Accesseur de la valeur du pixel rouge
        /// </summary>
        /// <value></value>
        public byte R
        {
            get { return r; }
            set { r = value; }
        }

        /// <summary>
        /// Accesseur de la valeur du pixel vert
        /// </summary>
        /// <value></value>
        public byte G
        {
            get { return g; }
            set { g = value; }
        }

        /// <summary>
        /// Accesseur de la valeur du pixel bleu
        /// </summary>
        /// <value></value>
        public byte B
        {
            get { return b; }
            set { b = value; }
        }

        public bool IsWhite
        {
            get { return (r == 255 && g == 255 && b == 255); }
        }


    }
}
