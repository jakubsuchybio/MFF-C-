using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Huffman;

namespace MFF_Huffman_Tests {
    [TestClass]
    public class Huffman3Tests {
        public void Assert_Files_Are_Equal(string tempFile, string expectedFile) {
            BinaryReader expected = new BinaryReader(File.OpenRead(expectedFile));
            BinaryReader actual = new BinaryReader(File.OpenRead(tempFile));

            Assert.AreEqual(expected.BaseStream.Length, actual.BaseStream.Length);
            while(expected.BaseStream.Length == expected.BaseStream.Position || actual.BaseStream.Length == actual.BaseStream.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();
        }

        [TestMethod]
        public void H3_Run_BadArgument() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert.AreEqual("Argument Error" + Environment.NewLine, stdOut.ToString());

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple.in.huff";
            string expectedFile = Program.SLNPath + @"CodEx\data\simple.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesBinary() {
            string inFile = Program.SLNPath + @"CodEx\data3\binary.in.huff";
            string expectedFile = Program.SLNPath + @"CodEx\data3\binary.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple2() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple2.in.huff";
            string expectedFile = Program.SLNPath + @"CodEx\data3\simple2.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple3() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple3.in.huff";
            string expectedFile = Program.SLNPath + @"CodEx\data3\simple3.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();
            
            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple4() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple4.in.huff";
            string expectedFile = Program.SLNPath + @"CodEx\data3\simple4.in";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple4_Bad() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple4_bad.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert.AreEqual("File Error"+Environment.NewLine, stdOut.ToString());

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple4_Bad2() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple4_bad2.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert.AreEqual("File Error" + Environment.NewLine, stdOut.ToString());

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H3_Run_FilesSimple4_Bad3() {
            string inFile = Program.SLNPath + @"CodEx\data3\simple4_bad3.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert.AreEqual("File Error" + Environment.NewLine, stdOut.ToString());

            stdOut.Close();
            File.Delete(tempFileName);
        }
        
        [TestMethod]
        public void H3_Run_FilesBinary_Bad() {
            string inFile = Program.SLNPath + @"CodEx\data3\binary_bad.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileDecodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert.AreEqual("File Error" + Environment.NewLine, stdOut.ToString());

            stdOut.Close();
            File.Delete(tempFileName);
        }
    }
}
