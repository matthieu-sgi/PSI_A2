using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace PSI_A2
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap damier = new Bitmap(@"..\..\..\Images\damier.bmp");

            

            Console.WriteLine(damier);
            damier.Save(@"..\..\..\Images\Tests_damier.bmp");
            Console.ReadKey();


        }
    }
}
