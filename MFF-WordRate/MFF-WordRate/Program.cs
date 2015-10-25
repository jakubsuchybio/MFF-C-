using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MFF_WordRate {
    class Program {
        static void Main(string[] args) {
            if(args.Length != 1) {
                Console.WriteLine("Argument Error");
                return;
            }

            string fileName = args[0];

            try {
                using(StreamReader sr = new StreamReader(File.OpenRead(fileName))) {
                    var dr = new Dictionary<string, long>();
                    while(!sr.EndOfStream) {
                        string line = sr.ReadLine();
                        string[] words = line.Split(
                            new[] { ' ', '\t', '\r', '\n' },
                            StringSplitOptions.RemoveEmptyEntries);
                        foreach(var item in words)
                            if(dr.ContainsKey(item))
                                dr[item]++;
                            else
                                dr.Add(item, 1);
                    }
                    var sortedKeys = from pair in dr orderby pair.Key ascending select pair;
                    foreach(var pair in sortedKeys)
                        Console.WriteLine(pair.Key + ": " + pair.Value);
                }
            }
            catch(Exception) {
                Console.WriteLine("File Error");
                return;
            }
        }
    }
}