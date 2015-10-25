using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Evaluator;

namespace MFF_Evaluator_Tests {
    [TestClass]
    public class RunTests {
        public void Assert_Stream_File_Are_Equals(TextWriter writer, string expectedFile) {
            string tempFileName = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(tempFileName, Encoding.UTF8.GetBytes(writer.ToString()));

            BinaryReader expected = new BinaryReader(File.OpenRead(expectedFile));
            BinaryReader actual = new BinaryReader(File.OpenRead(tempFileName));

            //Assert.AreEqual(expected.BaseStream.Length, actual.BaseStream.Length);
            while(expected.BaseStream.Length == expected.BaseStream.Position || actual.BaseStream.Length == actual.BaseStream.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();

            File.Delete(tempFileName);
        }
        
        [TestMethod]
        public void Codex() {
            string input = "i" + Environment.NewLine +
                "$#!%" + Environment.NewLine +
                "= + 2 3" + Environment.NewLine +
                "i" + Environment.NewLine +
                "d" + Environment.NewLine +
                "= / 5 2" + Environment.NewLine +
                "i" + Environment.NewLine +
                "d" + Environment.NewLine +
                "= +" + Environment.NewLine +
                "i" + Environment.NewLine +
                "end" + Environment.NewLine;
            string expected = "Expression Missing" + Environment.NewLine +
                "Format Error" + Environment.NewLine +
                "5" + Environment.NewLine +
                "5,00000" + Environment.NewLine +
                "2" + Environment.NewLine +
                "2,50000" + Environment.NewLine +
                "Format Error" + Environment.NewLine +
                "Expression Missing" + Environment.NewLine;
            StringReader reader = new StringReader(input);
            StringWriter writer = new StringWriter();

            Program.Run(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void SomeErrorFormats() {
            string input = "i" + Environment.NewLine +
                "= 5" + Environment.NewLine +
                "i " + Environment.NewLine +
                "= -5" + Environment.NewLine +
                "i" + Environment.NewLine +
                "=                     + 1 1" + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "end" + Environment.NewLine +
                "i" + Environment.NewLine;
            string expected = "Expression Missing" + Environment.NewLine +
                "Format Error" + Environment.NewLine +
                "Format Error" + Environment.NewLine +
                "Expression Missing" + Environment.NewLine;

            StringReader reader = new StringReader(input);
            StringWriter writer = new StringWriter();

            Program.Run(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void SomeErrors2() {
            string input = 
                "= 5" + Environment.NewLine +
                "i" + Environment.NewLine +
                "i " + Environment.NewLine +
                "i" + Environment.NewLine;
            string expected =
                "5" + Environment.NewLine +
                "Format Error" + Environment.NewLine +
                "5" + Environment.NewLine;

            StringReader reader = new StringReader(input);
            StringWriter writer = new StringWriter();

            Program.Run(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }
        
        [TestMethod]
        public void CodexFeedback() {
            string input =
                "= + 2000000000 2000000000" + Environment.NewLine +
                "i" + Environment.NewLine +
                "d" + Environment.NewLine +
                "= / ~ 2000000000 0" + Environment.NewLine +
                "i" + Environment.NewLine +
                "d" + Environment.NewLine;

            string expected =
                "Overflow Error" + Environment.NewLine +
                "4000000000,00000" + Environment.NewLine +
                "Divide Error" + Environment.NewLine +
                "-∞" + Environment.NewLine;

            StringReader reader = new StringReader(input);
            StringWriter writer = new StringWriter();

            Program.Run(reader, writer);

            Assert.AreEqual(expected, writer.ToString());

        }

        [TestMethod]
        public void EmptyInput() {
            string input = "";
            string expected = "";

            StringReader reader = new StringReader(input);
            StringWriter writer = new StringWriter();

            Program.Run(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Data1() {
            string inFile = Program.SLNPath + @"codex\data\01.in";
            string expectedFile = Program.SLNPath + @"codex\data\01.out";

            TextWriter output = new StringWriter();

            Program.Run(new StreamReader(inFile), output);
            
            Assert_Stream_File_Are_Equals(output, expectedFile);
        }

        [TestMethod]
        public void Data2() {
            string inFile = Program.SLNPath + @"codex\data\02.in";
            string expectedFile = Program.SLNPath + @"codex\data\02.out";

            TextWriter output = new StringWriter();

            Program.Run(new StreamReader(inFile), output);

            Assert_Stream_File_Are_Equals(output, expectedFile);
        }

        [TestMethod]
        public void Data3() {
            string inFile = Program.SLNPath + @"codex\data\03.in";
            string expectedFile = Program.SLNPath + @"codex\data\03.out";

            TextWriter output = new StringWriter();

            Program.Run(new StreamReader(inFile), output);

            Assert_Stream_File_Are_Equals(output, expectedFile);
        }

        [TestMethod]
        public void Data4() {
            string inFile = Program.SLNPath + @"codex\data\04.in";
            string expectedFile = Program.SLNPath + @"codex\data\04.out";

            TextWriter output = new StringWriter();

            Program.Run(new StreamReader(inFile), output);

            Assert_Stream_File_Are_Equals(output, expectedFile);
        }

        [TestMethod]
        public void Data5() {
            string inFile = Program.SLNPath + @"codex\data\05.in";
            string expectedFile = Program.SLNPath + @"codex\data\05.out";

            TextWriter output = new StringWriter();

            Program.Run(new StreamReader(inFile), output);

            Assert_Stream_File_Are_Equals(output, expectedFile);
        }
    }
}
