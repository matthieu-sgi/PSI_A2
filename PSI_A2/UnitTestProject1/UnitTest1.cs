using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PSI_A2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_Convertir_Endian_To_Int()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            byte[] tab = new byte[4];
            tab[0] = 3;
            tab[1] = 3;
            tab[2] = 3;
            tab[3] = 3;
            int result = image.Convertir_Endian_To_Int(tab);
            Assert.AreEqual(result, 50529027);
        }
        [TestMethod]
        public void TestMethod_Convertir_Int_To_Endian()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            byte[] tab = new byte[4];
            tab[0] = 232;
            tab[1] = 3;
            tab[2] = 0;
            tab[3] = 0;
            byte[] result = MyImage.Convertir_Int_To_Endian(1000);

            bool test = true;

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] != result[i])
                {
                    test = false;
                }
            }
            Assert.AreEqual(true, test);
        }
        [TestMethod]
        public void TestMethod_TableauByte()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            byte[] tab = new byte[8];
            tab[0] = 232;
            tab[1] = 3;
            tab[2] = 145;
            tab[3] = 45;
            tab[4] = 14;
            tab[5] = 230;
            tab[6] = 47;
            tab[7] = 184;

            byte[] tabverif = new byte[4];
            tabverif[0] = 145;
            tabverif[1] = 45;
            tabverif[2] = 14;
            tabverif[3] = 230;

            byte[] result = image.TableauByte(tab, 2, 4);

            bool test = true;

            for (int i = 0; i < tabverif.Length; i++)
            {
                if (tabverif[i] != result[i])
                {
                    test = false;
                }
            }
            Assert.AreEqual(true, test);
        }
        [TestMethod]
        public void TestMethod_Maximum()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            int[] tab = new int[4];
            tab[0] = 4;
            tab[1] = 145;
            tab[2] = 123;
            tab[3] = 478;
            int result = image.Maximum(tab);
            Assert.AreEqual(result, 478);
        }
        [TestMethod]
        public void TestMethod_Convertir_Binairy_To_Int()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            int[] tab = new int[8];
            tab[0] = 1;
            tab[1] = 0;
            tab[2] = 1;
            tab[3] = 1;
            tab[4] = 0;
            tab[5] = 0;
            tab[6] = 0;
            tab[7] = 1;
            int result = image.Convertir_Binairy_To_Int(tab);
            Assert.AreEqual(177, result);
        }
        [TestMethod]
        public void TestMethod_Convertir_Int_To_Binairy()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            int[] tab = new int[8];
            tab[0] = 1;
            tab[1] = 0;
            tab[2] = 1;
            tab[3] = 1;
            tab[4] = 0;
            tab[5] = 0;
            tab[6] = 0;
            tab[7] = 1;

            int[] result = image.Convertir_Int_To_Binairy(177);

            bool test = true;

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] != result[i])
                {
                    test = false;
                }
            }
            Assert.AreEqual(true, test);
        }
        [TestMethod]
        public void TestMethod_Concaténer_tableaux()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            int[] tab1 = new int[4];
            tab1[0] = 232;
            tab1[1] = 3;
            tab1[2] = 145;
            tab1[3] = 45;

            int[] tab2 = new int[4];
            tab2[0] = 145;
            tab2[1] = 45;
            tab2[2] = 14;
            tab2[3] = 230;

            int[] tabverif = new int[8];
            tabverif[0] = 232;
            tabverif[1] = 3;
            tabverif[2] = 145;
            tabverif[3] = 45;
            tabverif[4] = 145;
            tabverif[5] = 45;
            tabverif[6] = 14;
            tabverif[7] = 230;

            int[] result = image.Concaténer_tableaux(tab1, tab2);

            bool test = true;

            for (int i = 0; i < tabverif.Length; i++)
            {
                if (tabverif[i] != result[i])
                {
                    test = false;
                }
            }
            Assert.AreEqual(true, test);
        }
        [TestMethod]
        public void TestMethod_Recuperer_tableau()
        {
            MyImage image = new MyImage(@"..\..\..\Images\coco.bmp");
            int[] tab = new int[8];
            tab[0] = 1;
            tab[1] = 0;
            tab[2] = 1;
            tab[3] = 1;
            tab[4] = 0;
            tab[5] = 0;
            tab[6] = 0;
            tab[7] = 1;
            int[] tabverif = new int[8];
            tabverif[0] = 0;
            tabverif[1] = 0;
            tabverif[2] = 0;
            tabverif[3] = 1;

            int[] result = image.Recuperer_tableau(tab);

            bool test = true;

            for (int i = 0; i < tabverif.Length; i++)
            {
                if (tabverif[i] != result[i])
                {
                    test = false;
                }
            }
            Assert.AreEqual(true, test);
        }
    }
}



