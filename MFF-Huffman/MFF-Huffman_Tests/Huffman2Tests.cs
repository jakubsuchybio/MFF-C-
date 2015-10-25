using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Huffman;

namespace MFF_Huffman_Tests {
    [TestClass]
    public class Huffman2Tests {
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
        public void H2_Run_FilesBinary() {
            string inFile = Program.SLNPath + @"CodEx\data\binary.in";
            string expectedFile = Program.SLNPath + @"CodEx\data\binary.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileEncodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H2_Run_FilesSimple() {
            string inFile = Program.SLNPath + @"CodEx\data\simple.in";
            string expectedFile = Program.SLNPath + @"CodEx\data\simple.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileEncodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H2_Run_FilesSimple2() {
            string inFile = Program.SLNPath + @"CodEx\data\simple2.in";
            string expectedFile = Program.SLNPath + @"CodEx\data\simple2.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileEncodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H2_Run_FilesSimple3() {
            string inFile = Program.SLNPath + @"CodEx\data\simple3.in";
            string expectedFile = Program.SLNPath + @"CodEx\data\simple3.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileEncodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H2_Run_FilesSimple4() {
            string inFile = Program.SLNPath + @"CodEx\data\simple4.in";
            string expectedFile = Program.SLNPath + @"CodEx\data\simple4.in.huff";

            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter actualStream = new BinaryWriter(File.OpenWrite(tempFileName));

            TextWriter stdOut = new StringWriter();

            Program.Run_FileEncodeWithTree(new string[] { inFile }, stdOut, actualStream);
            actualStream.Close();

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

    }
}
