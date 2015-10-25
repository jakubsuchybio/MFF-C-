using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_WordJustify;

namespace MFF_WordJustify_Tests {
    class WordReaderMockup : IWordReader {
        private string[] words;
        private int nextWord = 0;

        public WordReaderMockup(string[] words) {
            this.words = words;
        }

        public string ReadWord() {
            if(nextWord >= words.Length) {
                return null;
            }

            return words[nextWord++];
        }
    }

    class WordProcessorMockup : IWordProcessor {
        public List<string> Words = new List<string>();

        public void ProcessWord(string word) {
            Words.Add(word);
        }

        public void Finish() {
            // Nothing to do here.
        }
    }

    [TestClass]
    public class ProgramTests {
        public void ProcessWords_TestOnSetOfWords(string[] words) {
            var reader = new WordReaderMockup(words);
            var processor = new WordProcessorMockup();

            Program.ProcessWords(reader, processor);

            CollectionAssert.AreEqual(words, processor.Words);
        }

        [TestMethod]
        public void ProcessWords_NonEmptyListOfWords() {
            ProcessWords_TestOnSetOfWords(new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." });
        }

        [TestMethod]
        public void ProcessWords_EmptyListOfWords() {
            ProcessWords_TestOnSetOfWords(new string[0]);
        }

        //
        //
        //

        public string Call_Run(string[] args) {
            var writer = new StringWriter();
            var fileWriter = new StringWriter();
            Program.Run(args, writer, fileWriter);
            return writer.ToString() + fileWriter.ToString();
        }

        [TestMethod]
        public void Run_NoArguments() {
            var result = Call_Run(new string[0]);

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_EmptyArgument() {
            var result = Call_Run(new[] { "", "output.txt", "1" });

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_EmptyArgument2() {
            var result = Call_Run(new[] { "input.txt", "", "1" });

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_ZeroArgument3() {
            var result = Call_Run(new[] { "input.txt", "output.txt", "0" });

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_NegativeArgument3() {
            var result = Call_Run(new[] { "input.txt", "output.txt", "-1" });

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_NonExistentFile() {
            File.Delete("xxyyzz.txt");

            var result = Call_Run(new[] { "xxyyzz.txt", "output.txt", "1" });

            Assert.AreEqual("File Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_CodexExample1() {
            var result = Call_Run(new[] { "plain.txt", "format.txt", "abc" });
            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_ValidEmptyFile() {
            File.Create("validtestinputfile.txt").Dispose();

            var result = Call_Run(new[] { "validtestinputfile.txt", "output.txt", "1" });

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void Run_OpensValidEmptyFileAndClosesIt() {
            File.Create("validtestinputfile.txt").Dispose();

            var result = Call_Run(new[] { "validtestinputfile.txt", "output.txt", "1" });
            Assert.AreEqual("", result);

            // Try to open the input file to verify it was closed by Program.Run method.
            File.OpenRead("validtestinputfile.txt").Dispose();
        }

        [TestMethod]
        public void Run_ValidFileAndAdditionalDummyArguments() {
            File.Create("validtestinputfile.txt").Dispose();

            var result = Call_Run(new[] { "validtestinputfile.txt", "output.txt", "1", "dummy1", "dummy1" });

            Assert.AreEqual("Argument Error" + Environment.NewLine, result);
        }

        [TestMethod]
        public void Run_LoremIpsumTestLen40() {
            Program.Run(new[] { "LoremIpsum.txt", "LoremIpsumOutput.txt", "40" }, Console.Out);
            FileStream expected = File.OpenRead("LoremIpsum_Aligned.txt");
            FileStream actual = File.OpenRead("LoremIpsumOutput.txt");
            Assert.AreEqual(expected.Length, actual.Length);
            while(expected.Length == expected.Position || actual.Length == actual.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();
        }

        [TestMethod]
        public void Run_LoremIpsumTestLen1() {
            Program.Run(new[] { "LoremIpsum.txt", "LoremIpsumOutput2.txt", "1" }, Console.Out);
            FileStream expected = File.OpenRead("LoremIpsum_Aligned2.txt");
            FileStream actual = File.OpenRead("LoremIpsumOutput2.txt");
            Assert.AreEqual(expected.Length, actual.Length);
            while(expected.Length == expected.Position || actual.Length == actual.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
            expected.Close();
            actual.Close();
        }
    }
}
