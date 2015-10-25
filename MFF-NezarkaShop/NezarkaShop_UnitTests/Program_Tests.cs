using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_NezarkaShop;

namespace MFF_NezarkaShop_UnitTests {
    [TestClass]
    public class Program_Tests {
        [TestMethod]
        public void NezarkaTestFiles() {
            StreamReader reader = new StreamReader("NezarkaTest.in");
            StreamWriter writer = new StreamWriter("output1.txt");
            Program.Run(new string[0] {}, reader, writer);
            reader.Close();
            writer.Close();

            FileStream expected = File.OpenRead("NezarkaTest.out");
            FileStream actual = File.OpenRead("output1.txt");
            Assert.AreEqual(expected.Length, actual.Length);
            while(expected.Length == expected.Position || actual.Length == actual.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();
        }

        [TestMethod]
        public void Program_CorrectNoData() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("", output.ToString());
        }
        
        [TestMethod]
        public void Program_CorrectSomeData() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "BOOK;1;Lord of the Rings;J. R. R. Tolkien;59\n" +
                "BOOK;2;Hobbit: There and Back Again;J. R. R. Tolkien;49\n" +
                "BOOK;3;Going Postal;Terry Pratchett;28\n" +
                "BOOK;4;The Colour of Magic;Terry Pratchett;159\n" +
                "BOOK;5;I Shall Wear Midnight;Terry Pratchett;31\n" +
                "CUSTOMER;1;Pavel;Jezek\n" +
                "CUSTOMER;2;Jan;Kofron\n" +
                "CUSTOMER;3;Petr;Hnetynka\n" +
                "CUSTOMER;4;Tomas;Bures\n" +
                "CART-ITEM;2;1;3\n" +
                "CART-ITEM;2;5;1\n" +
                "DATA-END\n");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataBegining(){
            StringReader input = new StringReader(
                "DATA-BEGIn\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataEnding() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "DATA-ENd");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataTag() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "BoOK;1;Lord of the Rings;J. R. R. Tolkien;59\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataNegativeBookId() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "BOOK;-1;Lord of the Rings;J. R. R. Tolkien;59\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataNegativeBookPrice() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "BOOK;0;Lord of the Rings;J. R. R. Tolkien;-1\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataNegativeCustId() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "CUSTOMER;-1;Pavel;Jezek\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataNegativeCartBookId() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "CUSTOMER;2;Pavel;Jezek\n" +
                "CART-ITEM;2;-1;3\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }

        [TestMethod]
        public void Program_IncorrectDataNegativeCartBookCount() {
            StringReader input = new StringReader(
                "DATA-BEGIN\n" +
                "CUSTOMER;2;Pavel;Jezek\n" +
                "BOOK;3;I Shall Wear Midnight;Terry Pratchett;31\n" +
                "CART-ITEM;2;3;-3\n" +
                "DATA-END");
            StringWriter output = new StringWriter();
            Program.Run(new string[0] { }, input, output);
            Assert.AreEqual("Data error.", output.ToString());
        }
    }
}