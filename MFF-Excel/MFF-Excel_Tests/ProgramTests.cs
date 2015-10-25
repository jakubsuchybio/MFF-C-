using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Excel;

namespace MFF_Excel_Tests {
    [TestClass]
    public class ProgramTests {
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

        //[TestMethod]
        //public void NumberToColumn_SomeValues() {
        //    for(int i = 1; i <= 26; i++) {
        //        string actual = Helper.NumberToColumn(i);
        //        string expected = Convert.ToChar(64+i).ToString();

        //        Assert.AreEqual(expected, actual);
        //    }

        //    for(int i = 27; i <= 26+26; i++) {
        //        string actual = Helper.NumberToColumn(i);
        //        string expected = "A" + Convert.ToChar(64 + i - 26).ToString();

        //        Assert.AreEqual(expected, actual);
        //    }
        //}

        [TestMethod]
        public void ParseWrite_Codex() {
            string codexString =
                "[] 3 =B1*A2" + Environment.NewLine +
                "19 =C1+C2 42" + Environment.NewLine +
                "auto" + Environment.NewLine +
                "=B2/A1 =A1-B4 =C2+A4" + Environment.NewLine +
                "=chyba =A1+autobus" + Environment.NewLine;

            var input = new StringReader(codexString);
            var output = new StringWriter();

            Sheet main = new Sheet();
            main.ParseStream(input);
            main.WriteDocument(output);

            Assert.AreEqual(codexString, output.ToString());

            input.Close();
            output.Close();
        }

        [TestMethod]
        public void ParseWrite_Mail1() {
            var input = new StreamReader(Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE.in");
            var output = new StringWriter();

            Sheet main = new Sheet();
            main.ParseStream(input);
            main.WriteDocument(output);

            Assert_Stream_File_Are_Equals(output, Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE.in");

            input.Close();
            output.Close();
        }

        [TestMethod]
        public void ParseWrite_Mail2() {
            var input = new StreamReader(Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE_prohozene.in");
            var output = new StringWriter();

            Sheet main = new Sheet();
            main.ParseStream(input);
            main.WriteDocument(output);

            Assert_Stream_File_Are_Equals(output, Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE_prohozene.in");

            input.Close();
            output.Close();
        }

        [TestMethod]
        public void Run_Nothing() {
            string str = "";
            string expectedOutput = "";

            var input = new StringReader(str);
            var output = new StringWriter();
            var stdOut = new StringWriter();

            string tempFileName = System.IO.Path.GetTempFileName();

            Program.RunBasic(new string[] { "test", tempFileName }, stdOut, input, output);

            Assert.AreEqual(expectedOutput, output.ToString());

            input.Close();
            output.Close();
            stdOut.Close();
        }

        [TestMethod]
        public void Run_NoArguments() {
            string str = "=+";
            string expectedOutput = "#FORMULA" + Environment.NewLine;

            var input = new StringReader(str);
            var output = new StringWriter();
            var stdOut = new StringWriter();

            Program.RunBasic(new string[] { "test", "test" }, stdOut, input, output);

            Assert.AreEqual(expectedOutput, output.ToString());

            input.Close();
            output.Close();
            stdOut.Close();
        }

        [TestMethod]
        public void Run_SomeValues() {
            string str = "[] 3 =+ -3 =A1+B1 =A1*B1 = A1-B1 =A1-B1 =A1/B1 =B1/A1 =B1/F1 =D1+A1 =C1+A1" + Environment.NewLine +
                "=A1+A2" + Environment.NewLine +
                "=B3+A1 =A3+A1" + Environment.NewLine +
                "=B4+A1 =C4+A1 =A4+A1" + Environment.NewLine +
                "=B5+A1 =C5+A1 =D5+A1 =A5+A1" + Environment.NewLine +
                "=A1+ =B1+ZZZZ3    " + Environment.NewLine;
            string expectedOutput = "[] 3 #FORMULA #INVVAL 3 0 #MISSOP #INVVAL -3 0 #DIV0 #DIV0 #FORMULA #ERROR" + Environment.NewLine +
                "#CYCLE" + Environment.NewLine +
                "#CYCLE #CYCLE" + Environment.NewLine +
                "#CYCLE #CYCLE #CYCLE" + Environment.NewLine +
                "#CYCLE #CYCLE #CYCLE #CYCLE" + Environment.NewLine +
                "#FORMULA 3" + Environment.NewLine;

            var input = new StringReader(str);
            var output = new StringWriter();
            var stdOut = new StringWriter();

            Program.RunBasic(new string[] { "test", "test" }, stdOut, input, output);

            Assert.AreEqual(expectedOutput, output.ToString());
        }

        [TestMethod]
        public void Run_Codex() {
            string str =
                "[] 3 =B1*A2" + Environment.NewLine +
                "19 =C1+C2 42" + Environment.NewLine +
                "auto" + Environment.NewLine +
                "=B2/A1 =A1-B4 =C2+A4" + Environment.NewLine +
                "=chyba =A1+autobus" + Environment.NewLine;

            string expectedOutput =
                "[] 3 57" + Environment.NewLine +
                "19 99 42" + Environment.NewLine +
                "#INVVAL" + Environment.NewLine +
                "#DIV0 #CYCLE #ERROR" + Environment.NewLine +
                "#MISSOP #FORMULA" + Environment.NewLine;

            var input = new StringReader(str);
            var output = new StringWriter();
            var stdOut = new StringWriter();

            string tempFileName = System.IO.Path.GetTempFileName();

            Program.RunBasic(new string[] { "test", tempFileName }, stdOut, input, output);

            Assert.AreEqual(expectedOutput, output.ToString());

            input.Close();
            output.Close();
            stdOut.Close();
        }

        [TestMethod]
        public void Run_Mail1() {
            string inFile = Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE.in";
            string expectedFile = Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE.out";

            string tempFileName = System.IO.Path.GetTempFileName();

            TextWriter stdOut = new StringWriter();

            Program.RunBasic(new string[] { inFile, tempFileName }, stdOut);

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void Run_Mail2() {
            string inFile = Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE_prohozene.in";
            string expectedFile = Program.SLNPath + @"codex\PrikladPrednostERRORorCYCLE_prohozene.out";

            string tempFileName = System.IO.Path.GetTempFileName();

            TextWriter stdOut = new StringWriter();

            Program.RunBasic(new string[] { inFile, tempFileName }, stdOut);

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void Run_LongCycle5000() {
            string inFile = Program.SLNPath + @"codex\longCycle.in";
            string expectedFile = Program.SLNPath + @"codex\longCycle.out";

            string tempFileName = System.IO.Path.GetTempFileName();

            TextWriter stdOut = new StringWriter();

            Program.RunBasic(new string[] { inFile, tempFileName }, stdOut);

            Assert_Files_Are_Equal(tempFileName, expectedFile);

            stdOut.Close();
            File.Delete(tempFileName);
        }
    }
}