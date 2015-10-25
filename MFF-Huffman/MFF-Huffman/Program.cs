using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MFF-Huffman_Tests")]

namespace MFF_Huffman {

    /// <summary>
    /// My old implementation of Heap using List as normal dynamic array
    /// </summary>
    /// <typeparam name="T">Any comparable type</typeparam>
    public class Heap<T> where T : IComparable<T> {
        private List<T> array = new List<T>();
        public int Count = 0; //Actual count (array can be bigger sometimes)

        /// <summary>
        /// Inserts item into the heap and checks if consistency wasn't broken
        /// If it was, then it moves inserted value up to its correct position
        /// </summary>
        /// <param name="item">Item to insert</param>
        public void Add(T item) {
            array.Add(item);

            int i = Count;
            int father = (i - 1) / 2;
            while(i != 0) {
                if(item.CompareTo(array[father]) < 0) {
                    array[i] = array[father];
                    i = father;
                    father = (i - 1) / 2;
                }
                else
                    break;
            }
            array[i] = item;
            Count++;
        }
        
        /// <summary>
        /// Removes the root item (minimal value) from heap and corrects consistency
        /// </summary>
        /// <returns>Minimal valued item</returns>
        public T RemoveMin() {
            if(Count != 0) {
                T ret = array[0];
                Count--;
                T item = array[Count];
                if(item.CompareTo(array[0]) < 0)
                    array[0] = item;
                else {
                    int i = 0;
                    int smallestSon = 2 * i + 1; ;
                    while(i < Count) {
                        for(int j = 2; (j <= 2) && (2 * i + j < Count); j++) {
                            if(array[smallestSon].CompareTo(array[2 * i + j]) >= 0)
                                smallestSon = 2 * i + j;
                        }
                        if(array[smallestSon].CompareTo(item) < 0) {
                            array[i] = array[smallestSon];
                            i = smallestSon;
                            if(2 * i + 1 < Count)
                                smallestSon = 2 * i + 1;
                            else
                                break;
                        }
                        else
                            break;
                    }
                    array[i] = item;
                }

                return ret;
            }
            else
                return default(T);
        }

    }
    

    /// <summary>
    /// Extension class for BitArray with some additional functionality.
    /// </summary>
    public static class BitArrayExtender {
        /// <summary>
        /// Transfers BitArray to array of bytes.
        /// </summary>
        /// <param name="bits">Input BitArray.</param>
        /// <returns>Array of byte representation of input bits.</returns>
        public static byte[] ToByteArray(BitArray bits) {
            if(bits.Length == 0)
                throw new FormatException("ToByteArray: BitArray Lenght = 0");

            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        /// Transfers BitArray to array of ulongs.
        /// </summary>
        /// <param name="bits">Input BitArray.</param>
        /// <returns>Array of ulongs representation of input bits.</returns>
        public static ulong[] ToULongArray(BitArray bits) {
            if(bits.Length == 0)
                throw new FormatException("ToByteArray: BitArray Lenght = 0");

            ulong[] ret = new ulong[(bits.Length - 1) / 64 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        /// Copies part of BitArray to another BitArray.
        /// </summary>
        /// <param name="outStartIndex">Starting position for insertion.</param>
        /// <param name="inStartIndex">Starting position from where insertion starts.</param>
        /// <param name="inLenght">Lenght of parth that will be inserted.</param>
        /// <param name="inArray">Bitarray that is inserted from.</param>
        /// <param name="outArray">Bitarray that is inserted into.</param>
        public static void CopyToFromBitArray(int outStartIndex, int inStartIndex, int inLenght,
            BitArray inArray, BitArray outArray) {
            if(outStartIndex + inLenght > outArray.Length)
                throw new FormatException("Wrong use of CopyToFromBitArray");

            for(int i = inStartIndex, j = outStartIndex; i < inStartIndex + inLenght; i++, j++)
                outArray[j] = inArray[i];
        }

        /// <summary>
        /// Tries to append BitArray to another if it fits and returns the rest which doesn't fit.
        /// </summary>
        /// <param name="outArray">BitArray that is inserted into.</param>
        /// <param name="position">Starting position for insertion.</param>
        /// <param name="inArray">BitArray that is inserted from.</param>
        /// <returns>null when insertion fits and BitArray of the rest, when it doesn't fit.</returns>
        public static BitArray TryAppend(BitArray outArray, int position, BitArray inArray) {
            int i, j;

            for(i = 0, j = position; j < outArray.Length && i < inArray.Length; i++, j++)
                outArray[j] = inArray[i];

            if(i >= inArray.Length)
                return null;
            else {
                int size = inArray.Length - i;
                BitArray ret = new BitArray(size);
                for(int k = 0; k < size; k++, i++)
                    ret[k] = inArray[i];

                return ret;
            }
        }
    }

    public class HuffmansTree : IDisposable {

        /// <summary>
        /// Represents Huffman tree code.
        /// Using Order=0 for lists and Order>0 for inner nodes.
        /// </summary>
        public class Node : IComparable<Node> {
            public byte Code = 0;
            public ulong Sum = 0;

            public Node Left = null;
            public Node Right = null;

            //Using Order 0 for lists and bigger for inner nodes
            public ulong Order = 0;

            public int CompareTo(Node item) {
                //By sum
                if(this.Sum < item.Sum)
                    return -1;
                else if(this.Sum > item.Sum)
                    return 1;
                //if Sums equals, then by type (list,inner)
                else if(this.Order == 0 && item.Order > 0)
                    return -1;
                else if(this.Order > 0 && item.Order == 0)
                    return 1;
                //if types equals and are lists, then by Code
                else if(this.Order == 0 && item.Order == 0)
                    return this.Code < item.Code ? -1 : 1;
                //If types equals and are inners, then by Time of creation
                else
                    return this.Order < item.Order ? -1 : 1;
            }
        }


        private BitArray[] encodedNodes;
        public TextWriter stdOut;
        public Node root;
        private bool first;
     
        public HuffmansTree(TextWriter stdOut, Dictionary<byte, ulong> rates) {
            this.stdOut = stdOut;
            this.root = this.FromDictionaryOfCounts(rates);
        }

        public HuffmansTree(TextWriter stdOut, BinaryReader fileIn) {
            this.stdOut = stdOut;
            this.root = FromFile(fileIn);
        }

        /// <summary>
        /// If not exist byte encodes, then create and return.
        /// </summary>
        /// <returns>Returns hash table of byte codes.</returns>
        public BitArray[] GetEncodedNodes() {
            if(encodedNodes == null) {
                encodedNodes = new BitArray[256];
                var path = new List<bool>();

                EncodeNodes(this.root, path);
            }

            return encodedNodes;
        }
        
        /// <summary>
        /// Creates Huffman tree from input stream, which contains Huffman file header.
        /// </summary>
        /// <param name="fileIn">Input stream.</param>
        /// <returns>Root node of the tree or null, when input stream has errors.</returns>
        /// <exception cref="IOException">When format of file is wrong.</exception>
        internal static Node FromFile(BinaryReader fileIn) {
            fileIn.BaseStream.Position = 0;
            if(!Program.CheckHeader(fileIn))
                throw new IOException(); //Structural error, path in tree not found

            try {
                Node ret = ReCreateTree(fileIn);

                if(ret == null)
                    return null; //File contains only ending without tree
                
                if(fileIn.ReadUInt64() != (ulong)0)
                    throw new IOException(); //Bad ending

                if(ret.Left == null || ret.Right == null)
                    throw new IOException(); //Huffman tree must consist at least from 3 od zero nodes.

                return ret; //Correct
            }
            catch(Exception) {
                throw new IOException();
            }
        }
        /// <summary>
        /// Recursively recreates Huffman tree from file.
        /// </summary>
        /// <param name="fileIn">Input stream. (Starts from first byte of the tree)</param>
        /// <returns>Root node.</returns>
        internal static Node ReCreateTree(BinaryReader fileIn) {
            Node ret = null;

            ulong node = fileIn.ReadUInt64();

            ret = ReCreateNode(node);
            if(ret == null)
                return null;

            if(ret.Order == 0)
                return ret;

            ret.Left = ReCreateTree(fileIn);
            ret.Right= ReCreateTree(fileIn);
            return ret;
        }
        /// <summary>
        /// Parses 8bytes of binary representation and creates HuffmanTree Node
        /// </summary>
        /// <param name="node">Binary representation of node.</param>
        /// <returns>Parsed node.</returns>
        internal static Node ReCreateNode(ulong node) {
            if(node == 0)
                return null;

            Node ret = new Node();
            ret.Order = (node & 1) == 1 ? 0UL : 1UL;
            ret.Sum = (node & 0x00FFFFFFFFFFFFFF) >> 1;
            ret.Code = (byte)((node & 0xFF00000000000000) >> 56);
            return ret;
        }

        /// <summary>
        /// Creates a forest (with heap) and assembles Huffman's tree until there is only a root.
        /// </summary>
        /// <param name="rates">Dictionary<byte, long> of byte's frequencies.</param>
        /// <returns>Root node of the tree or null, when rates is empty.</returns>
        private Node FromDictionaryOfCounts(Dictionary<byte, ulong> rates) {
            Heap<Node> heap = new Heap<Node>();
            foreach(var item in rates) {
                heap.Add(new Node() { 
                    Code = item.Key,
                    Sum = item.Value
                });
            }

            ulong order = 1;
            while(heap.Count > 1) {
                Node first = heap.RemoveMin();
                Node second = heap.RemoveMin();
                heap.Add(new Node() {
                    Sum = first.Sum + second.Sum,
                    Order = order++,
                    Left = first,
                    Right = second
                });
            }

            return heap.RemoveMin();
        }

        /// <summary>
        /// Public interface to print prefixed tree.
        /// </summary>
        public void PrintPrefixReadable() {
            first = true;
            PrintPrefixReadable(root);
        }
        /// <summary>
        /// Recursive printing function for prefixed version of Huffmans tree.
        /// </summary>
        /// <param name="tree">Root code.</param>
        private void PrintPrefixReadable(Node tree) {
            if(tree == null)
                return;

            if(!first)
                stdOut.Write(" ");
            else
                first = false;

            if(tree.Order == 0)
                stdOut.Write("*" + tree.Code + ":" + tree.Sum);
            else {
                stdOut.Write(tree.Sum);
                PrintPrefixReadable(tree.Left);
                PrintPrefixReadable(tree.Right);
            }
        }
        
        /// <summary>
        /// Recursive traversal method that have stack of path and creates byte encodes.
        /// </summary>
        /// <param name="tree">Root node.</param>
        /// <param name="bitPath">Reference to stack.</param>
        private void EncodeNodes(Node tree, List<bool> bitPath) {
            if(tree == null)
                return;
            
            if(tree.Order != 0) {
                bitPath.Add(false);
                EncodeNodes(tree.Left, bitPath);
                bitPath.RemoveAt(bitPath.Count - 1);
                bitPath.Add(true);
                EncodeNodes(tree.Right, bitPath);
                bitPath.RemoveAt(bitPath.Count - 1);
            }
            else {
                CreateEncodeNode(tree.Code, bitPath);
            }
        }
        /// <summary>
        /// Just adds new byte encode to hash table.
        /// </summary>
        /// <param name="code">byte code.</param>
        /// <param name="bitPath">Reference to stack of path.</param>
        /// <exception cref="FormatException">Checks for duplicate bytes, which shouldn't exist.</exception>
        private void CreateEncodeNode(byte code, List<bool> bitPath) {
            BitArray pathArray = new BitArray(bitPath.ToArray());
            encodedNodes[code] = pathArray;
        }

        /// <summary>
        /// Wrapper method for writing Huffman Tree in preorder with ending zero 8Bytes.
        /// </summary>
        /// <param name="fileOut">Output stream.</param>
        public void PrintPrefixBinary(BinaryWriter fileOut) {
            PrintPrefixBinary(root, fileOut);
            fileOut.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
        }
        /// <summary>
        /// Recursive preorder traversal.
        /// </summary>
        /// <param name="tree">Current node.</param>
        /// <param name="fileOut">Output stream.</param>
        private void PrintPrefixBinary(Node tree, BinaryWriter fileOut) {
            if(tree == null)
                return;

            PrintNodeBinary(tree, fileOut);
            PrintPrefixBinary(tree.Left, fileOut);
            PrintPrefixBinary(tree.Right, fileOut);
        }
        /// <summary>
        /// Creates 64bit representation of Huffman Tree Node as:
        /// list:  0bit = 1
        ///        1-55bit = node.sum
        ///        56-64 = node.code
        /// inner: 0bit = 0
        ///        1-55bit = node.sum
        ///        56-64 = 0
        /// </summary>
        /// <param name="tree">Current node.</param>
        /// <param name="fileOut">Output stream.</param>
        private void PrintNodeBinary(Node tree, BinaryWriter fileOut) {
            BitArray sumBits = new BitArray(BitConverter.GetBytes(tree.Sum));

            BitArray node = new BitArray(64, false);
            BitArrayExtender.CopyToFromBitArray(1, 0, 55, sumBits, node);

            //List
            if(tree.Order == 0) {
                BitArray codeByte = new BitArray(new[] { tree.Code });
                node.Set(0, true);
                BitArrayExtender.CopyToFromBitArray(56, 0, 8, codeByte, node);
            }
            //Inner
            else {
                node.Set(0, false);
            }

            byte[] nodeBytes = BitArrayExtender.ToByteArray(node);
            fileOut.Write(nodeBytes);
        }

        public void Dispose() {
            stdOut.Dispose();
        }
    }

    class Program {
        internal static string SLNPath = @"c:\Users\Jakub\Documents\Visual Studio 2013\Projects\MFF-Huffman\";

        /// <summary>
        /// Prints header of Huffman encoded file.
        /// </summary>
        /// <param name="fileOut">Stream to file output.</param>
        internal static void PrintHeaderBinary(BinaryWriter fileOut) {
            byte[] headBytes = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
            fileOut.Write(headBytes);
        }

        /// <summary>
        /// Checks header of Huffman encoded file.
        /// </summary>
        /// <param name="fileIn"></param>
        /// <returns>true if ok, false if header is wrong</returns>
        internal static bool CheckHeader(BinaryReader fileIn) {
            fileIn.BaseStream.Position = 0;
            byte[] headBytes = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
            ulong headLong = BitConverter.ToUInt64(headBytes, 0);
            ulong headLongLoaded = fileIn.ReadUInt64();

            if(headLong != headLongLoaded)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Reads the input file and creates Dictionary<byte,long> of all byte's frequencies.
        /// Using buffer for quicker reading (BEWARE OF OUT OF MEMORY EXCEPTIONS).
        /// </summary>
        /// <param name="fileIn">Input stream.</param>
        /// <exception cref="System.OutOfMemoryException">BEWARE OF OUT OF MEMORY EXCEPTION.</exception>
        /// <returns>Dictionary of byte frequencies.</returns>
        internal static Dictionary<byte, ulong> ReadFileByteCounts(BinaryReader fileIn) {
            int maxBufferSize = 100000; //Maximum for Huffman I => 1000000
            var rates = new Dictionary<byte, ulong>(256);
            while(fileIn.BaseStream.Position != fileIn.BaseStream.Length) {
                long diff = fileIn.BaseStream.Length - fileIn.BaseStream.Position;
                int count = diff < maxBufferSize ? (int)(diff) : maxBufferSize;
                byte[] bytesBuffer = fileIn.ReadBytes(count);

                for (int i = 0; i < bytesBuffer.Length; i++) {
                    ulong existNode;
                    if(rates.TryGetValue(bytesBuffer[i], out existNode))
                        rates[bytesBuffer[i]] = existNode + 1;
                    else
                        rates.Add(bytesBuffer[i], 1);
			    }
            }

            return rates;
        }

        /// <summary>
        /// This reads input file and transfers bytes to Huffman code.
        /// </summary>
        /// <param name="nodes">Prepared hash table with codes for every type of byte.</param>
        /// <param name="fileIn">Input stream.</param>
        /// <param name="fileOut">Output stream.</param>
        /// <exception cref="System.OutOfMemoryException">BEWARE OF OUT OF MEMORY EXCEPTION</exception>
        internal static void EncodeFileBytes(BitArray[] nodes, BinaryReader fileIn, BinaryWriter fileOut) {
            int maxBufferSize = 100000; //Maximum for Huffman I => 1000000

            int bufferPos = 0;
            BitArray writeBuffer = new BitArray(maxBufferSize*8);

            while(fileIn.BaseStream.Position != fileIn.BaseStream.Length) {
                long diff = fileIn.BaseStream.Length - fileIn.BaseStream.Position;
                int maxSize = diff < maxBufferSize ? (int)(diff) : maxBufferSize;

                byte[] bytesBuffer = fileIn.ReadBytes(maxSize);

                for(int i = 0; i < bytesBuffer.Length; i++) {
                    BitArray keyStream = nodes[bytesBuffer[i]];

                    BitArray tmp = BitArrayExtender.TryAppend(writeBuffer, bufferPos, keyStream);
                    if(tmp != null) {
                        fileOut.Write(BitArrayExtender.ToByteArray(writeBuffer));
                        BitArrayExtender.TryAppend(writeBuffer, 0, tmp);
                        bufferPos = tmp.Length;
                    }
                    else
                        bufferPos += keyStream.Length;
                }
            }
            writeBuffer.Length = bufferPos;
            fileOut.Write(BitArrayExtender.ToByteArray(writeBuffer));
        }

        /// <summary>
        /// Decodes encoded stream of input stream.
        /// </summary>
        /// <param name="tree">Initialized Huffman tree, loaded from encoded file.</param>
        /// <param name="fileIn">Encoded input stream.</param>
        /// <param name="fileOut">Output stream.</param>
        internal static void DecodeFileBytes(HuffmansTree tree, BinaryReader fileIn, BinaryWriter fileOut) {
            fileIn.BaseStream.Position = 0;
            if(!Program.CheckHeader(fileIn))
                throw new IOException(); //Structural error, path in tree not found

            while(fileIn.ReadUInt64() != 0) ;

            if(tree == null) {
                fileOut.Write("");
                return;
            }

            int maxBufferSize = 100000; //Maximum for Huffman I => 1000000
            
            HuffmansTree.Node actual = tree.root;
            var outBuffer = new List<byte>();

            while(fileIn.BaseStream.Position != fileIn.BaseStream.Length) {
                long diff = fileIn.BaseStream.Length - fileIn.BaseStream.Position;
                bool lastBatch = diff < maxBufferSize ? true : false;
                int maxSize = lastBatch ? (int)(diff) : maxBufferSize;

                byte[] bytesBuffer = fileIn.ReadBytes(maxSize);

                BitArray bits = new BitArray(bytesBuffer);

                for(int i = 0; i < bits.Length; i++) {
                    if(actual.Order == 0) {
                        if(actual.Sum == 0)
                            if(lastBatch && i > bits.Length - 8)
                                break;
                            else
                                throw new IOException(); // Sum shouldn't be zero yet
                        outBuffer.Add(actual.Code);
                        actual.Sum--;
                        actual = tree.root;
                    }

                    if(bits[i] == false) {
                        if(actual.Left == null)
                            throw new IOException(); //Structural error, path in tree not found
                        actual = actual.Left;
                    }
                    else {
                        if(actual.Right == null)
                            throw new IOException(); //Structural error, path in tree not found
                        actual = actual.Right;
                    }
                }

                if(actual.Order == 0 && actual.Sum > 0) {
                    outBuffer.Add(actual.Code);
                    actual.Sum--;
                    actual = tree.root;
                }

                fileOut.Write(outBuffer.ToArray());
                outBuffer = new List<byte>();
            }
        }

        /// <summary>
        /// Main method for Huffman I
        /// </summary>
        /// <param name="args">Program arguments</param>
        /// <param name="stdOut">Standard output stream</param>
        /// <param name="fileIn">Input Binary file stream</param>
        public static void Run_FileReadTreeBuild(string[] args, TextWriter stdOut, BinaryReader fileIn = null) {
            if(args.Length != 1) {
                stdOut.WriteLine("Argument Error");
                return;
            }
            string fileName = args[0];

            try {
                if(fileIn == null)
                    fileIn = new BinaryReader(File.OpenRead(args[0]));

                Dictionary<byte, ulong> byteFrequency = ReadFileByteCounts(fileIn);
                HuffmansTree tree = new HuffmansTree(stdOut, byteFrequency);
                tree.PrintPrefixReadable();
            }
            catch(IOException) {
                stdOut.WriteLine("File Error");
            }
        }

        /// <summary>
        /// Main method for Huffman II
        /// </summary>
        /// <param name="args">Program arguments</param>
        /// <param name="stdOut">Standard output stream</param>
        /// <param name="fileOut">Optimal Output Binary stream</param>
        /// <param name="fileIn">Optimal Input Binary stream</param>
        public static void Run_FileEncodeWithTree(string[] args, TextWriter stdOut, BinaryWriter fileOut = null, BinaryReader fileIn = null) {
            if(args.Length != 1) {
                stdOut.WriteLine("Argument Error");
                return;
            }
            string fileName = args[0];

            try {
                if(fileIn == null)
                    fileIn = new BinaryReader(File.OpenRead(fileName));

                if(fileOut == null)
                    fileOut = new BinaryWriter(File.Open(fileName+".huff", FileMode.Create));

                var byteFrequency = ReadFileByteCounts(fileIn);
                HuffmansTree tree = new HuffmansTree(stdOut, byteFrequency);

                var encodedNodes = tree.GetEncodedNodes();

                PrintHeaderBinary(fileOut);
                tree.PrintPrefixBinary(fileOut);
                fileIn.BaseStream.Position = 0;
                EncodeFileBytes(encodedNodes, fileIn, fileOut);
            }
            catch(IOException) {
                stdOut.WriteLine("File Error");
            }
            finally {
                if(fileIn != null)
                    fileIn.Close();
                if(fileOut != null)
                    fileOut.Close();
                if(stdOut != null)
                    stdOut.Close();
            }
        }

        /// <summary>
        /// Main method for Huffman III
        /// </summary>
        /// <param name="args">Program arguments</param>
        /// <param name="stdOut">Standard output stream</param>
        /// <param name="fileOut">Optimal Output Binary stream</param>
        /// <param name="fileIn">Optimal Input Binary stream</param>
        public static void Run_FileDecodeWithTree(string[] args, TextWriter stdOut, BinaryWriter fileOut = null, BinaryReader fileIn = null) {
            if(args.Length != 1) {
                stdOut.WriteLine("Argument Error");
                return;
            }

            string fileName = args[0];

            if(fileName.Length <= 5 || !fileName.Substring(fileName.Length-5).Equals(".huff")) {
                stdOut.WriteLine("Argument Error");
                return;
            }
            
            try {
                if(fileIn == null)
                    fileIn = new BinaryReader(File.OpenRead(fileName));

                if(fileOut == null)
                    fileOut = new BinaryWriter(File.Open(fileName.Substring(0,fileName.Length-5), FileMode.Create));

                HuffmansTree tree = new HuffmansTree(stdOut, fileIn);

                DecodeFileBytes(tree, fileIn, fileOut);
            }
            catch(IOException) {
                stdOut.WriteLine("File Error");
            }
            finally {
                if(fileIn != null)
                    fileIn.Close();
                if(fileOut != null)
                    fileOut.Close();
                if(stdOut != null)
                    stdOut.Close();
            }
        }

        static void Main(string[] args) {
            //Run_FileReadTreeBuild(args, Console.Out);
            //Run_FileEncodeWithTree(args, Console.Out);
            Run_FileDecodeWithTree(args, Console.Out);
        }
    }
}
