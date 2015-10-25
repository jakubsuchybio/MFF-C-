using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Huffman;

namespace MFF_Huffman_Tests {
    [TestClass]
    public class Huffman1Tests {

        public void Assert_Stream_File_Are_Equals(TextWriter writer, string expectedFile) {
            string tempFileName = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(tempFileName, Encoding.UTF8.GetBytes(writer.ToString()));

            BinaryReader expected = new BinaryReader(File.OpenRead(expectedFile));
            BinaryReader actual = new BinaryReader(File.OpenRead(tempFileName));

            Assert.AreEqual(expected.BaseStream.Length, actual.BaseStream.Length);
            while(expected.BaseStream.Length == expected.BaseStream.Position || actual.BaseStream.Length == actual.BaseStream.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();

            File.Delete(tempFileName);
        }

        [TestMethod]
        public void H1_Run_CodexTest1() {
            string stringIn = "aaabbc";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "6 *97:3 3 *99:1 *98:2";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_Empty() {
            string stringIn = "";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_SingleByte() {
            string stringIn = "a";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "*97:1";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_MultipleSameBytes() {
            string stringIn = "aaaaaaaaaa";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "*97:10";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_MultipleDifferentBytes() {
            string stringIn = "aaaaaaaaaabbbbb";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "15 *98:5 *97:10";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_SingleDifferentBytes() {
            string stringIn = "abcde";
            BinaryReader fileIn = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(stringIn)));
            TextWriter stdOut = new StringWriter(); ;
            string expected = "5 2 *99:1 *100:1 3 *101:1 2 *97:1 *98:1";

            Program.Run_FileReadTreeBuild(new string[] { "test" }, stdOut, fileIn);

            Assert.AreEqual(expected, stdOut.ToString());
        }

        [TestMethod]
        public void H1_Run_FilesBinary() {
            string inFile = Program.SLNPath + @"CodEx\huffman-data\binary.in";
            string expectedFile = Program.SLNPath + @"CodEx\huffman-data\binary.out";

            TextWriter stdOut = new StringWriter();

            Program.Run_FileReadTreeBuild(new string[] { inFile }, stdOut);

            Assert_Stream_File_Are_Equals(stdOut, expectedFile);
        }

        [TestMethod]
        public void H1_Run_FilesSimple() {
            string inFile = Program.SLNPath + @"CodEx\huffman-data\simple.in";
            string expectedFile = Program.SLNPath + @"CodEx\huffman-data\simple.out";

            TextWriter stdOut = new StringWriter();

            Program.Run_FileReadTreeBuild(new string[] { inFile }, stdOut);

            Assert_Stream_File_Are_Equals(stdOut, expectedFile);
        }

        [TestMethod]
        public void H1_Run_FilesSimple2() {
            string inFile = Program.SLNPath + @"CodEx\huffman-data\simple2.in";
            string expectedFile = Program.SLNPath + @"CodEx\huffman-data\simple2.out";

            TextWriter stdOut = new StringWriter();

            Program.Run_FileReadTreeBuild(new string[] { inFile }, stdOut);

            Assert_Stream_File_Are_Equals(stdOut, expectedFile);
        }

        [TestMethod]
        public void H1_Run_FilesSimple3() {
            string inFile = Program.SLNPath + @"CodEx\huffman-data\simple3.in";
            string expectedFile = Program.SLNPath + @"CodEx\huffman-data\simple3.out";

            TextWriter stdOut = new StringWriter();

            Program.Run_FileReadTreeBuild(new string[] { inFile }, stdOut);

            Assert_Stream_File_Are_Equals(stdOut, expectedFile);
        }

        [TestMethod]
        public void H1_Run_FilesSimple4() {
            string inFile = Program.SLNPath + @"CodEx\huffman-data\simple4.in";
            string expectedFile = Program.SLNPath + @"CodEx\huffman-data\simple4.out";

            TextWriter stdOut = new StringWriter();

            Program.Run_FileReadTreeBuild(new string[] { inFile }, stdOut);

            Assert_Stream_File_Are_Equals(stdOut, expectedFile);
        }
    }
}
