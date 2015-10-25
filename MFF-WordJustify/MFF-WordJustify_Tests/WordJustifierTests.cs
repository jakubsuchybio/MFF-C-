using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_WordJustify;

namespace MFF_WordJustify_Tests {

    [TestClass]
    public class WordJustifierTests {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullTextWriter() {
            var counter = new WordJustifier(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_ZeroLineLength() {
            var result = new StringWriter();
            var processor = new WordJustifier(result, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeLineLength() {
            var result = new StringWriter();
            var processor = new WordJustifier(result, -1);
        }

        [TestMethod]
        public void ProcessAndFinish_NoWords() {
            var result = new StringWriter();
            var processor = new WordJustifier(result, 1);

            processor.Finish();

            Assert.AreEqual("", result.ToString());
        }

        [TestMethod]
        public void ProcessWords_Reader_Processor_Example1() {
            var input = "If a train station is where the train stops, what is a work station?";
            var reader = new WordReader(new StringReader(input));

            var result = new StringWriter();
            var processor = new WordJustifier(result, 17);
            Program.ProcessWords(reader, processor);

            var expectedOutput = "If     a    train\nstation  is where\nthe  train stops,\nwhat  is  a  work\nstation?\n";

            Assert.AreEqual(expectedOutput, result.ToString().Replace("\r", ""));
        }

        [TestMethod]
        public void ProcessWords_Reader_Processor_Example1_Paragraph() {
            var input = "If a train station is where the train stops, what is a work station?\n\n\nParagraph";
            var reader = new WordReader(new StringReader(input));

            var result = new StringWriter();
            var processor = new WordJustifier(result, 17);
            Program.ProcessWords(reader, processor);

            var expectedOutput = "If     a    train\nstation  is where\nthe  train stops,\nwhat  is  a  work\nstation?\n\nParagraph\n";

            Assert.AreEqual(expectedOutput, result.ToString().Replace("\r", ""));
        }


    }
}
