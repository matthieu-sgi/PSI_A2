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
