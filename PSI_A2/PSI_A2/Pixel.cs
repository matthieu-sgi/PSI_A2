using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_A2
{
    class Pixel
    {
        private byte[] rgb;
        
        public Pixel(byte r,byte g,byte b)
        {
            this.rgb = new byte[3];
            this.rgb[0] = r;
            this.rgb[1] = g;
            this.rgb[2] = b;
        }

        public byte R
        {
            get { return rgb[0]; }
            set { rgb[0] = value; }
        }

        public byte G
        {
            get { return rgb[1]; }
            set { rgb[1] = value; }
        }
        public byte B
        {
            get { return rgb[2]; }
            set { rgb[2] = value; }
        }


    }
}
