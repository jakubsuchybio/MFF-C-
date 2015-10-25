using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MFF-WordJustify_Tests")]

namespace MFF_WordJustify {

    interface IWordReader {
        string ReadWord();
    }

    interface IWordProcessor {
        void ProcessWord(string word);
        void Finish();
    }

    /// <summary> Implementation of IWordReader with buffer. (As in CodEx's reader.cs) </summary>
    class WordReader : IWordReader, IDisposable {
        private TextReader reader;
        private char[] buffer;
        private const int BufferChunkLenght = 1000000;
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
            if(c == '\n')
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
        
        /// <summary> Skips lines and sets wasBlank if there was a blank line. (two or mode newlines) </summary>
        /// <param name="wasBlank">Indicator if there were blank line(s). (For paragraph purposes)</param>
        /// <returns>false when EoF, otherwise true.</returns>
        private bool SkipBlankLines(out bool wasBlank) {
            int newlines = 0;

            while(UpdateBuf()) {
                while(index < len && isNewlineChar(buffer[index])) {
                    newlines++;
                    index++;
                    while(index < len && isSpaceChar(buffer[index]))
                        index++;
                }
                if(index < len) {
                    wasBlank = newlines > 1 ? true : false;
                    return true;
                }
            }
            wasBlank = false;
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

            var wasBlankLine = false;
            if(!SkipBlankLines(out wasBlankLine))
                return null;
            else if(wasBlankLine)
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
    
    /// <summary> Implementation of IWordProcessor for Justifying words to column. </summary>
    class WordJustifier : IWordProcessor, IDisposable {
        private TextWriter writer;
        private long maxLineLength;
        private List<string> currentLine = new List<string>();
        private long currentLineLength = 0;
        private bool firstLine = true;

        /// <summary> Constructor for WordJustifier, which also sets ouput's stream NewLine char to '\n'. </summary>
        public WordJustifier(TextWriter writer, long lineLength) {
            if(writer == null)
                throw new ArgumentNullException("writer");
            if(lineLength <= 0)
                throw new ArgumentOutOfRangeException("lineLength");

            this.maxLineLength = lineLength;
            this.writer = writer;
            this.writer.NewLine = "\n";
        }

        /// <summary> Helper method for creating long strings of spaces. </summary>
        /// <param name="count">Number of spaces.</param>
        /// <returns>String of 'count' spaces.</returns>
        private string createSpaces(long count) {
            StringBuilder builder = new StringBuilder();
            for(long i = 0; i < count; i++) {
                builder.Append(' ');
            }
            return builder.ToString();
        }

        /// <summary> Writes one line of words Justified to column </summary>
        /// <param name="EndOfParagraph">Indicator, if this line is End of paragraph.</param>
        private void FinishLine(bool EndOfParagraph) {
            if(currentLine.Count == 0)
                return;

            // Do not print '\n' before first line
            if(!firstLine)
                writer.WriteLine();
            else
                firstLine = false;

            // When end of paragraph, do not Justify. Just print remaining word(s) joined with spaces from left.
            if(EndOfParagraph)
                writer.WriteLine(String.Join(" ", currentLine.ToArray()));
            else {
                // Justification alghoritm
                // First it calculates how much space is there for spaces
                // Then it distributes more to less spaces from left to right between words
                long spacesCount = currentLine.Count - 1;
                long wordLength = 0;
                foreach(var word in currentLine)
                    wordLength += word.Length;

                long spacesLeft = maxLineLength - wordLength;

                var firstWord = true;
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < currentLine.Count; i++) {
                    if(!firstWord) {
                        long spaceLenght = spacesLeft / spacesCount + (spacesLeft % spacesCount > 0 ? 1 : 0);
                        sb.Append(createSpaces(spaceLenght));
                        spacesLeft -= spaceLenght;
                        spacesCount--;
                    }
                    else
                        firstWord = false;

                    sb.Append(currentLine[i]);
                }
                writer.Write(sb.ToString());
            }

            currentLine.Clear();
            currentLineLength = 0;
        }

        /// <summary> Adds one word for current line and finishes line, 
        /// when line is bigger then justifying column or when new paragraph. </summary>
        /// <param name="word"></param>
        public void ProcessWord(string word) {
            if(word == "") {
                FinishLine(true);
                return;
            }

            if(currentLineLength + word.Length + currentLine.Count - 1 < maxLineLength) {
                currentLine.Add(word);
                currentLineLength += word.Length;
            }
            else {
                FinishLine(false);
                currentLine.Add(word);
                currentLineLength = word.Length;
            }
        }

        /// <summary> When EoF, then writes the line and makes sure that it is flushed. </summary>
        public void Finish() {
            FinishLine(true);
            writer.Flush();
        }

        public void Dispose() {
            writer.Dispose();
        }
    }

    class Program {
        // The main word processing algorithm
        public static void ProcessWords(IWordReader reader, IWordProcessor processor) {
            string word;
            while((word = reader.ReadWord()) != null) {
                processor.ProcessWord(word);
            }

            processor.Finish();
        }

        /// <summary> Global output stream. </summary>
        static TextWriter output;

        /// <summary> Helper method for reporting File Error. </summary>
        static void ReportFileError() {
            output.WriteLine("File Error");
        }

        /// <summary> Helper method for reporting Argument Error. </summary>
        static void ReportArgumentError() {
            output.WriteLine("Argument Error");
        }

        /// <summary> Main function with Output streams to stdOut and file. </summary>
        /// <param name="args">Program arguments</param>
        /// <param name="standardWriter">stdOut stream for reporting errors.</param>
        /// <param name="fileWriter">File stream for program data output.</param>
        public static void Run(string[] args, TextWriter standardWriter, TextWriter fileWriter = null) {
            output = standardWriter;
            long lineLenght = 0;

            try {
                if(args.Length != 3 || args[0] == "" || args[1] == "")
                    throw new Exception();

                long.TryParse(args[2], out lineLenght);
                if(lineLenght <= 0)
                    throw new Exception();
            }
            catch(Exception) {
                ReportArgumentError();
                return;
            }

            WordReader reader = null;
            WordJustifier justifier = null;
            try {
                TextWriter writer = null;
                if(fileWriter == null)
                    writer = new StreamWriter(args[1]);
                else
                    writer = fileWriter;

                reader = new WordReader(new StreamReader(args[0]));
                justifier = new WordJustifier(writer, lineLenght);

                ProcessWords(reader, justifier);
            }
            catch(FileNotFoundException) {
                ReportFileError();
            }
            catch(IOException) {
                ReportFileError();
            }
            catch(UnauthorizedAccessException) {
                ReportFileError();
            }
            catch(System.Security.SecurityException) {
                ReportFileError();
            }
            catch(Exception) {
                ReportFileError();
            }
            finally {
                if(reader != null) reader.Dispose();
                if(justifier != null) justifier.Dispose();
            }
        }

        static void Main(string[] args) {
            Run(args, Console.Out);
        }
    }
}