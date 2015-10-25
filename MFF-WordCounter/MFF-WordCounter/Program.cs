using System;
using System.IO;

namespace MFF_WordCounter {
    class Program {
        static void Main(string[] args) {
            if(args.Length != 1) {
                Console.WriteLine("Argument Error");
                return;
            }

            string fileName = args[0];

            try {
                using(StreamReader sr = new StreamReader(File.OpenRead(fileName))) {
                    long counter = 0;
                    while(!sr.EndOfStream)
                        counter += sr.ReadLine().Split(
                            new[] { ' ', '\t', '\r', '\n' },
                            StringSplitOptions.RemoveEmptyEntries).Length;
                    Console.WriteLine(counter);
                }
            }
            catch(Exception) {
                Console.WriteLine("File Error");
                return;
            }
        }
    }
}