using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_WordJustify;


namespace MFF_WordJustify_Tests {

    [TestClass]
    public class WordReaderByWordTests {

        [TestMethod]
        public void ReadWordByWord_EmptyInput() {
            var reader = new WordReader(new StringReader(""));

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_SingleLineWithoutTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain in Spain falls mainly on the plain."));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_SingleLineIncludingTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain in Spain falls mainly on the plain.\n"));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_SingleLineWithManySpacesIncludingTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("   The rain    in \tSpain\tfalls\t\t\tmainly on the plain.\n"));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_MutipleLinesWithoutTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain in\nSpain falls mainly\non the plain."));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_MutipleLinesIncludingTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain in\nSpain falls mainly\non the plain.\n"));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_MutipleLinesWithManySpacesIncludingTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain      in   \n   Spain\tfalls\t\t\tmainly\non the plain.    \n"));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_MutipleIncludingEmptyLinesWithManySpacesIncludingTerminatingNewLine() {
            var words = new[] { "The", "rain", "in", "", "Spain", "falls", "mainly", "on", "the", "plain." };
            var reader = new WordReader(new StringReader("The rain      in   \n     \n   \n   \t\n  Spain\tfalls\t\t\tmainly\non the plain.    \n   \n    \n\n"));

            foreach(var word in words) {
                Assert.AreEqual(word, reader.ReadWord());
            }

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        public void ReadWordByWord_OnlyWhitecharacters() {
            var reader = new WordReader(new StringReader("   \t\n  \n    \n\n\n    \n \t\t\t  \n"));

            Assert.IsNull(reader.ReadWord());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadWordByWord_Constructor_NullTextReader() {
            var reader = new WordReader(null);
        }
    }
}
