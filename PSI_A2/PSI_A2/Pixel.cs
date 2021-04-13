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
        
        public Pixel(byte _r,byte _g,byte _b)
        {

            this.r = _r;
            this.g = _g;
            this.b = _b;
        }

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

        public byte R
        {
            get { return r; }
            set { r = value; }
        }

        public byte G
        {
            get { return g; }
            set { g = value; }
        }
        public byte B
        {
            get { return b; }
            set { b = value; }
        }


    }
}
