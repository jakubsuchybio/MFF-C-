using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Huffman;

namespace MFF_Huffman_Tests {
    /// <summary>
    /// Summary description for HuffmanTreeTests
    /// </summary>
    [TestClass]
    public class HuffmanTreeTests {
        public string ToBitString(BitArray bits) {
            var sb = new StringBuilder();

            for(int i = 0; i < bits.Count; i++) {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        //Checks tree for "aaabbc" which is "6 *97:3 3 *99:1 *98:2"
        public void CheckSimpleTree(HuffmansTree.Node root) {
            Assert.IsNotNull(root);
            Assert.IsNull(root.Left.Left);
            Assert.IsNull(root.Left.Right);
            Assert.IsNull(root.Right.Left.Left);
            Assert.IsNull(root.Right.Left.Right);
            Assert.IsNull(root.Right.Right.Left);
            Assert.IsNull(root.Right.Right.Right);
            Assert.AreEqual((ulong)6, root.Sum);
            Assert.AreEqual((ulong)3, root.Left.Sum);
            Assert.AreEqual((ulong)3, root.Right.Sum);
            Assert.AreEqual((ulong)1, root.Right.Left.Sum);
            Assert.AreEqual((ulong)2, root.Right.Right.Sum);
            Assert.AreEqual((ulong)0, root.Code);
            Assert.AreEqual((byte)97, root.Left.Code);
            Assert.AreEqual((byte)0, root.Right.Code);
            Assert.AreEqual((byte)99, root.Right.Left.Code);
            Assert.AreEqual((byte)98, root.Right.Right.Code);
            Assert.AreNotEqual((ulong)0, root.Order);
            Assert.AreEqual((ulong)0, root.Left.Order);
            Assert.AreNotEqual((ulong)0, root.Right.Order);
            Assert.AreEqual((ulong)0, root.Right.Left.Order);
            Assert.AreEqual((ulong)0, root.Right.Right.Order);
        }

        [TestMethod]
        public void Node_CompareTo_LesserGreater() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 0,
                Sum = 1,
                Order = 0
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 0,
                Sum = 2,
                Order = 0
            };

            int expected = -1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_GreaterLesser() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 0,
                Sum = 3,
                Order = 0
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 0,
                Sum = 2,
                Order = 0
            };

            int expected = 1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsListsCodesLesserGreater() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 1,
                Sum = 2,
                Order = 0
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 0
            };

            int expected = -1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsListsCodesGreaterLesser() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 3,
                Sum = 2,
                Order = 0
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 0
            };

            int expected = 1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsListInner() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 3,
                Sum = 2,
                Order = 0
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 1
            };

            int expected = -1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsInnerList() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 3,
                Sum = 2,
                Order = 1
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 0
            };

            int expected = 1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsBothInnerOrderLesserGreater() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 3,
                Sum = 2,
                Order = 1
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 2
            };

            int expected = -1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Node_CompareTo_EqualsBothInnerOrderGreaterLesser() {
            HuffmansTree.Node lesser = new HuffmansTree.Node() {
                Code = 3,
                Sum = 2,
                Order = 3
            };
            HuffmansTree.Node greater = new HuffmansTree.Node() {
                Code = 2,
                Sum = 2,
                Order = 2
            };

            int expected = 1;
            int actual = lesser.CompareTo(greater);

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void EncodeNodes_Simple1() {
            string inFile = Program.SLNPath + @"CodEx\data\simple.in";
            
            BinaryReader fileIn = new BinaryReader(File.OpenRead(inFile));
            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter fileOut = new BinaryWriter(File.Open(tempFileName, FileMode.Create));

            var byteFrequency = Program.ReadFileByteCounts(fileIn);
            HuffmansTree tree = new HuffmansTree(Console.Out, byteFrequency);

            var encodedNodes = tree.GetEncodedNodes();

            Assert.AreEqual(ToBitString(encodedNodes[97]), "0");
            Assert.AreEqual(ToBitString(encodedNodes[98]), "11");
            Assert.AreEqual(ToBitString(encodedNodes[99]), "10");
            
            fileOut.Close();
            fileIn.Close();
            File.Delete(tempFileName);
        }

        [TestMethod]
        public void EncodeNodes_Simple2() {
            string inFile = Program.SLNPath + @"CodEx\data\simple2.in";

            BinaryReader fileIn = new BinaryReader(File.OpenRead(inFile));
            string tempFileName = System.IO.Path.GetTempFileName();
            BinaryWriter fileOut = new BinaryWriter(File.Open(tempFileName, FileMode.Create));

            var byteFrequency = Program.ReadFileByteCounts(fileIn);
            HuffmansTree tree = new HuffmansTree(Console.Out, byteFrequency);

            var encodedNodes = tree.GetEncodedNodes();
            
            Assert.AreEqual(ToBitString(encodedNodes[112]), "0000");
            Assert.AreEqual(ToBitString(encodedNodes[46]), "00010");
            Assert.AreEqual(ToBitString(encodedNodes[83]), "00011");
            Assert.AreEqual(ToBitString(encodedNodes[84]), "00100");
            Assert.AreEqual(ToBitString(encodedNodes[102]), "00101");
            Assert.AreEqual(ToBitString(encodedNodes[109]), "00110");
            Assert.AreEqual(ToBitString(encodedNodes[111]), "00111");

            fileOut.Close();
            fileIn.Close();
            File.Delete(tempFileName);
        }
    
        [TestMethod]
        public void ReCreateNode_zero() {
            HuffmansTree.Node actual = HuffmansTree.ReCreateNode(0);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ReCreateNode_one() {
            HuffmansTree.Node actual = HuffmansTree.ReCreateNode(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual((ulong)0, actual.Order);
            Assert.AreEqual((ulong)0, actual.Sum);
            Assert.AreEqual((byte)0, actual.Code);
        }

        [TestMethod]
        public void ReCreateNode_list() {
            HuffmansTree.Node actual = HuffmansTree.ReCreateNode(12);

            Assert.IsNotNull(actual);
            Assert.AreEqual((ulong)1, actual.Order);
            Assert.AreEqual((ulong)6, actual.Sum);
            Assert.AreEqual((byte)0, actual.Code);
        }

        [TestMethod]
        public void ReCreateNode_inner() {
            // From 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62
            HuffmansTree.Node actual = HuffmansTree.ReCreateNode(7061644215716937733);

            Assert.IsNotNull(actual);
            Assert.AreEqual((ulong)0, actual.Order);
            Assert.AreEqual((ulong)2, actual.Sum);
            Assert.AreEqual((byte)98, actual.Code);
        }

        [TestMethod]
        public void ReCreateTree_simple() {
            byte[] fileInBytes = new byte[] {   0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61,
                                                0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x63,
                                                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));

            HuffmansTree.Node actual = HuffmansTree.ReCreateTree(fileIn);
            CheckSimpleTree(actual);
        }

        [TestMethod]
        public void FromFile_simple() {
            byte[] fileInBytes = new byte[] {   0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66,
                                                0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61,
                                                0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x63,
                                                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));
            TextWriter stdOut = new StringWriter(); ;

            HuffmansTree.Node actual = HuffmansTree.FromFile(fileIn);

            CheckSimpleTree(actual);
        }

        [TestMethod]
        public void HuffmansTree_simple() {
            byte[] fileInBytes = new byte[] {   0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66,
                                                0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61,
                                                0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x63,
                                                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));
            TextWriter stdOut = new StringWriter(); ;

            HuffmansTree actual = new HuffmansTree(stdOut, fileIn);

            CheckSimpleTree(actual.root);
        }
    }
}
