using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MFF-Excel_Tests")]

namespace MFF_Excel {

    /// <summary> Implementation of IWordReader with buffer. (As in CodEx's reader.cs) </summary>
    class WordReader {
        private TextReader reader;
        private char[] buffer;
        private const int BufferChunkLenght = 800000;
        private long index;
        private long len;

        public WordReader(TextReader reader) {
            if(reader == null) {
                throw new ArgumentNullException("reader");
            }
            this.reader = reader;
            buffer = new char[BufferChunkLenght];
            index = 0;
            len = 0;
        }

        /// <summary> Makes sure, that index not out of index of buffer. And checks for input EoF. </summary>
        /// <returns>false when EoF of input stream, otherwise true.</returns>
        private bool UpdateBuf() {
            if(reader == null || len == -1)
                return false;
            if(index < len)
                return true;

            index = 0;
            if((len = reader.Read(buffer, 0, BufferChunkLenght)) == 0) {
                len = -1;
                buffer = null;
                return false;
            }
            return true;
        }

        /// <summary> Helper method for check if char is space or tab. </summary>
        /// <param name="c">Character to check.</param>
        /// <returns>true when character is space or tab, otherwise false.</returns>
        private bool isSpaceChar(char c) {
            if(c == ' ' || c == '\t')
                return true;
            else
                return false;
        }

        /// <summary> Helper method for check if char newline. </summary>
        /// <param name="c">Character to check.</param>
        /// <returns>true when character is newline, otherwise false.</returns>
        private bool isNewlineChar(char c) {
            if(c == '\r' || c == '\n')
                return true;
            else
                return false;
        }

        /// <summary> Skips spaces and tabs on current line. </summary>
        /// <returns>false when EoF, otherwise true.</returns>
        private bool SkipSpacesOnLine() {
            while(UpdateBuf()) {
                while(index < len && isSpaceChar(buffer[index]))
                    index++;
                if(index < len)
                    return true;
            }
            return false;
        }

        /// <summary> Skips lines and sets wasNewLine if there was a new line.</summary>
        /// <param name="wasNewLine">Indicator if there were new line(s).</param>
        /// <returns>false when EoF, otherwise true.</returns>
        private bool SkipBlankLines(out bool wasNewLine) {
            int newlines = 0;

            while(UpdateBuf()) {
                while(index < len && isNewlineChar(buffer[index])) {
                    newlines++;
                    index++;
                    while(index < len && isSpaceChar(buffer[index]))
                        index++;
                }
                if(index < len) {
                    wasNewLine = newlines >= 1 ? true : false;
                    return true;
                }
            }
            wasNewLine = false;
            return false;
        }

        /// <summary> Helper method for getting words from buffer. With long indexers. </summary>
        /// <param name="buffer">Buffer object.</param>
        /// <param name="start">Starting index of the word.</param>
        /// <param name="len">Length of the word.</param>
        /// <returns>Word starting at 'start' with length 'len'.</returns>
        private string WordFromBuffer(char[] buffer, long start, long len) {
            StringBuilder ret = new StringBuilder();
            for(long i = start; i < start + len; i++)
                ret.Append(buffer[i]);

            return ret.ToString();
        }

        /// <summary> Reads one word from input. </summary>
        /// <returns>null when EOF, "" when blank lines found, otherwise one word</returns>
        public string ReadWord() {
            if(!SkipSpacesOnLine())
                return null;

            var wasNewLine = false;
            if(!SkipBlankLines(out wasNewLine))
                return null;
            else if(wasNewLine)
                return "";

            long start = index;

            // When word is in the buffer, then just retrive it.
            while(index < len && !System.Char.IsWhiteSpace(buffer[index]))
                index++;
            if(index < len)
                return WordFromBuffer(buffer, start, index - start);

            // When word is not completely in buffer.
            // First it saves part of the word to builder.
            // Then it calls UpdateBuf for updating buffer with next chunk of data.
            // Then it finishes search for end of the word.
            // And finally it appends it to first part of the word and returns the word.
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(WordFromBuffer(buffer, start, index - start));
            while(index == len && UpdateBuf()) {
                while(index < len && !System.Char.IsWhiteSpace(buffer[index]))
                    index++;
                sb.Append(WordFromBuffer(buffer, 0, index));
            }
            return sb.ToString();
        }

        public void Dispose() {
            reader.Dispose();
        }
    }

    class Helper {
        public static bool TryColumnToNumber(string column, out int num) {
            num = 0;
            for(int i = 0, pow = column.Length - 1; i < column.Length; ++i, --pow) {
                var cVal = (int)column[i] - 64; //col A is index 1
                if(cVal < 1 || cVal > 26)
                    return false;
                num += cVal * ((int)Math.Pow(26, pow));
            }
            return true;
        }

        public static string ToTuple(int a, int b) {
            return String.Concat(new string[] { a.ToString(), "_", b.ToString() });
        }
    }

    class Sheet {
        class Node {
            public string Content = null;
            public bool Processed = false;

            public override string ToString() {
                return Content;
            }
        }

        List<List<Node>> document;
        HashSet<string> cycleStack;

        public Sheet() {
            document = new List<List<Node>>();
            cycleStack = new HashSet<string>();
        }

        public void ParseStream(TextReader input) {
            WordReader reader = new WordReader(input);

            string word = "";
            while(word != null) {
                word = reader.ReadWord();
                var line = new List<Node>();
                while(word != "" && word != null) {
                    Node node = new Node();
                    node.Content = word;
                    line.Add(node);
                    word = reader.ReadWord();
                }
                if(line.Count != 0)
                    document.Add(line);
            }
        }

        public void WriteDocument(TextWriter output) {
            bool firstWord = true;
            for(int i = 0; i < document.Count; i++) {
                firstWord = true;
                for(int j = 0; j < document[i].Count; j++) {
                    if(firstWord)
                        firstWord = false;
                    else
                        output.Write(' ');

                    output.Write(document[i][j].Content);
                }
                output.Write(Environment.NewLine);
            }
        }

        private string EvaluateNodeBasic(string columnNum) {
            int digitIndex = columnNum.IndexOfAny("123456789".ToCharArray());
            if(digitIndex == -1)
                return "#INVVAL";

            string col = columnNum.Substring(0, digitIndex);
            string num = columnNum.Substring(digitIndex);

            int number;
            if(!int.TryParse(num, out number))
                return "#INVVAL";

            int column;
            if(!Helper.TryColumnToNumber(col, out column))
                return "#INVVAL";

            Node node;
            number--;
            column--;

            if(number >= 0 && number < document.Count && column >= 0 && column < document[number].Count)
                node = document[number][column];
            else
                node = null;

            return EvaluateNodeBasic(number, column, node);
        }

        private string EvaluateNodeBasic(int i, int j, Node node) {
            if(node == null)
                return "0";

            if(node.Processed)
                return node.Content;

            if(node.Content == "[]") {
                node.Processed = true;
                return node.Content;
            }

            uint tryint;
            if(uint.TryParse(node.Content, out tryint)) {
                node.Processed = true;
                return node.Content;
            }

            if(node.Content.Length == 0 || node.Content[0] != '=') {
                node.Processed = true;
                node.Content = "#INVVAL";
                return "#INVVAL";
            }

            //Invariant from now on evaluating =expression
            //Checked all types [],uint,=

            //MISSOP error
            int operandIndex = node.Content.IndexOfAny("+-*/".ToCharArray());
            if(operandIndex == -1) {
                node.Processed = true;
                node.Content = "#MISSOP";
                return "#MISSOP";
            }
            if(operandIndex == 1 || operandIndex + 1 >= node.Content.Length) {
                node.Processed = true;
                node.Content = "#FORMULA";
                return "#FORMULA";
            }

            var tuple = Helper.ToTuple(i, j);
            if(!cycleStack.Add(tuple)) {
                node.Processed = true;
                node.Content = "#CYCLE";
                return "#CYCLE";
            }

            string leftEvaluated = EvaluateNodeBasic(node.Content.Substring(1, operandIndex - 1));
            string rightEvaluated = EvaluateNodeBasic(node.Content.Substring(operandIndex + 1));

            if(leftEvaluated == "#CYCLE" || rightEvaluated == "#CYCLE") {
                node.Processed = true;
                node.Content = "#CYCLE";
                return "#CYCLE";
            }

            char operand = node.Content[operandIndex];

            if(leftEvaluated == "[]")
                leftEvaluated = "0";
            if(rightEvaluated == "[]")
                rightEvaluated = "0";

            if(leftEvaluated[0] != '#' && rightEvaluated == "0" && operand == '/') {
                node.Processed = true;
                node.Content = "#DIV0";
                return "#DIV0";
            }

            if(leftEvaluated == "#INVVAL" || rightEvaluated == "#INVVAL") {
                node.Processed = true;
                node.Content = "#FORMULA";
                return "#FORMULA";
            }

            int left;
            int right;
            if(leftEvaluated[0] == '#' ||
                rightEvaluated[0] == '#' ||
                !int.TryParse(leftEvaluated, out left) ||
                !int.TryParse(rightEvaluated, out right)) {
                node.Processed = true;
                node.Content = "#ERROR";
                return "#ERROR";
            }

            switch(operand) {
                case '+':
                    node.Content = (left + right).ToString();
                    break;
                case '-':
                    node.Content = (left - right).ToString();
                    break;
                case '*':
                    node.Content = (left * right).ToString();
                    break;
                case '/':
                    node.Content = (left / right).ToString();
                    break;
            }

            node.Processed = true;
            return node.Content;
        }

        public void EvaluateDocumentBasic() {
            for(int i = 0; i < document.Count; i++) {
                for(int j = 0; j < document[i].Count; j++) {
                    if(cycleStack.Count > 0)
                        cycleStack.Clear();
                    if(document[i][j].Processed)
                        continue;
                    EvaluateNodeBasic(i, j, document[i][j]);
                }
            }
        }

    }

    class Program {
        internal static string SLNPath = @"c:\Users\Jakub\Documents\Visual Studio 2013\Projects\MFF-Excel\";

        static void FileErrorWriter(TextWriter stdOut) {
            stdOut.WriteLine("File Error");
        }

        internal static void RunBasic(string[] args, TextWriter stdOut, TextReader input = null, TextWriter output = null) {
            if(args.Length != 2) {
                stdOut.WriteLine("Argument Error");
                return;
            }

            try {
                if(input == null)
                    input = new StreamReader(args[0]);

                if(output == null)
                    output = new StreamWriter(args[1]);

                Sheet main = new Sheet();
                main.ParseStream(input);
                main.EvaluateDocumentBasic();
                main.WriteDocument(output);
            }
            catch(IOException) {
                FileErrorWriter(stdOut);
                return;
            }
            catch(UnauthorizedAccessException) {
                FileErrorWriter(stdOut);
                return;
            }
            catch(ArgumentException) {
                FileErrorWriter(stdOut);
                return;
            }
            catch(System.Security.SecurityException) {
                FileErrorWriter(stdOut);
                return;
            }
            finally {
                if(input != null)
                    input.Close();
                if(output != null)
                    output.Close();
            }
        }

        static void Main(string[] args) {
            RunBasic(args, Console.Out);
        }
    }
}