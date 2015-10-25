using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Evaluator;

namespace MFF_Evaluator_Tests {
    [TestClass]
    public class RunIntTests {
        //Codex tests
        [TestMethod]
        public void Codex1() {
            string expected = "2" + Environment.NewLine;
            StringReader reader = new StringReader("+ ~ 1 3");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Codex2() {
            string expected = "-7" + Environment.NewLine;
            StringReader reader = new StringReader("/ + - 5 2 * 2 + 3 3 ~ 2");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Codex3() {
            string expected = "Overflow Error" + Environment.NewLine;
            StringReader reader = new StringReader("- - 2000000000 2100000000 2100000000");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Codex4() {
            string expected = "Divide Error" + Environment.NewLine;
            StringReader reader = new StringReader("/ 100 - + 10 10 20");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Codex5() {
            string expected = "Format Error" + Environment.NewLine;
            StringReader reader = new StringReader("+ 1 2 3");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Codex6() {
            string expected = "Format Error" + Environment.NewLine;
            StringReader reader = new StringReader("- 2000000000 4000000000");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        //Operation tests
        [TestMethod]
        public void Value() {
            string expected = "1" + Environment.NewLine;
            StringReader reader = new StringReader("1");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Negate() {
            string expected = "-3" + Environment.NewLine;
            StringReader reader = new StringReader("~ 3");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Add() {
            string expected = "3" + Environment.NewLine;
            StringReader reader = new StringReader("+ 1 2");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Subtract() {
            string expected = "-3" + Environment.NewLine;
            StringReader reader = new StringReader("- 4 7");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Multiply() {
            string expected = "12" + Environment.NewLine;
            StringReader reader = new StringReader("* 3 4");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Divide() {
            string expected = "12" + Environment.NewLine;
            StringReader reader = new StringReader("/ 24 2");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        //Error tests
        [TestMethod]
        public void Error_ValueOverflow() {
            string expected = "Format Error" + Environment.NewLine;
            StringReader reader = new StringReader("4000000000");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Error_WrongParam() {
            string expected = "Format Error" + Environment.NewLine;
            StringReader reader = new StringReader("auto");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Error_WrongExpression() {
            string expected = "Format Error" + Environment.NewLine;
            StringReader reader = new StringReader("+ 1 2 3");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void Error_DivideByZero() {
            string expected = "Divide Error" + Environment.NewLine;
            StringReader reader = new StringReader("/ 1 0");
            StringWriter writer = new StringWriter();

            Program.RunInt(reader, writer);

            Assert.AreEqual(expected, writer.ToString());
        }
    }
}
