using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_Huffman;

namespace MFF_Huffman_Tests {
    [TestClass]
    public class ProgramTests {

        public void Assert_Binary_Streams_Are_Equals(BinaryReader expected, BinaryReader actual) {
            Assert.AreEqual(expected.BaseStream.Length, actual.BaseStream.Length, "Size not equals");
            expected.BaseStream.Position = 0;
            actual.BaseStream.Position = 0;
            while(expected.BaseStream.Length > expected.BaseStream.Position && actual.BaseStream.Length > actual.BaseStream.Position) {
                Assert.AreEqual(expected.ReadByte(), actual.ReadByte());
            }
        }

        public bool[] StringToArrayOfBits(string str) {
            string tmp = str.Replace(" ","");
            bool[] ret = new bool[tmp.Length];

            for(int i = 0; i < tmp.Length; i++)
                ret[i] = tmp[i] == '1' ? true : false;

            return ret;
        }

        [TestMethod]
        public void PrintHeaderBinary() {
            BinaryWriter fileOut = new BinaryWriter(new MemoryStream());
            Program.PrintHeaderBinary(fileOut);

            BinaryReader actual = new BinaryReader(fileOut.BaseStream);
            BinaryReader expected = new BinaryReader(new MemoryStream(new byte[] { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 }));
            
            Assert_Binary_Streams_Are_Equals(expected, actual);

            expected.Close();
            actual.Close();
            fileOut.Close();
        }

        [TestMethod]
        public void BitArrayToByteArray_One() {
            byte[] expected = { 10 };
            BitArray testBits = new BitArray(StringToArrayOfBits("0101 0000"));
            byte[] actual = BitArrayExtender.ToByteArray(testBits);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BitArrayToByteArray_Multiple() {
            byte[] expected = { 0x4B, 0x58, 0x07 };
            BitArray testBits = new BitArray(StringToArrayOfBits("1101 0010 0001 1010 111"));
            byte[] actual = BitArrayExtender.ToByteArray(testBits);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeFileBytes_OneChar() {
            byte[] fileInBytes = new byte[] { 5 };
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));

            BitArray[] nodes = new BitArray[6];
            nodes[5] = new BitArray(StringToArrayOfBits("10"));


            BinaryWriter fileOut = new BinaryWriter(new MemoryStream());

            Program.EncodeFileBytes(nodes, fileIn, fileOut);
            
            byte[] expected_bytes = new byte[] { 1 };
            BinaryReader expected = new BinaryReader(new MemoryStream(expected_bytes));
            BinaryReader actual = new BinaryReader(fileOut.BaseStream);
            
            Assert_Binary_Streams_Are_Equals(expected, actual);

            expected.Close();
            actual.Close();
            fileIn.Close();
            fileOut.Close();
        }

        [TestMethod]
        public void EncodeFileBytes_Codex1() {
            byte[] fileInBytes = new byte[] { 2, 4, 1, 1, 3, 2 };
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));

            BitArray[] nodes = new BitArray[5];
            nodes[1] = new BitArray(StringToArrayOfBits("0"));
            nodes[2] = new BitArray(StringToArrayOfBits("11"));
            nodes[3] = new BitArray(StringToArrayOfBits("100"));
            nodes[4] = new BitArray(StringToArrayOfBits("101"));

            BinaryWriter fileOut = new BinaryWriter(new MemoryStream());

            Program.EncodeFileBytes(nodes, fileIn, fileOut);

            byte[] expected_bytes = new byte[] { 0x97, 0x0C };
            BinaryReader expected = new BinaryReader(new MemoryStream(expected_bytes));
            BinaryReader actual = new BinaryReader(fileOut.BaseStream);

            Assert_Binary_Streams_Are_Equals(expected, actual);

            expected.Close();
            actual.Close();
            fileIn.Close();
            fileOut.Close();
        }

        [TestMethod]
        public void EncodeFileBytes_Codex2() {
            byte[] fileInBytes = new byte[] { 2, 4, 1, 1, 3, 2, 1, 1 };
            BinaryReader fileIn = new BinaryReader(new MemoryStream(fileInBytes));

            BitArray[] nodes = new BitArray[5];
            nodes[1] = new BitArray(StringToArrayOfBits("0"));
            nodes[2] = new BitArray(StringToArrayOfBits("11"));
            nodes[3] = new BitArray(StringToArrayOfBits("100"));
            nodes[4] = new BitArray(StringToArrayOfBits("101"));

            BinaryWriter fileOut = new BinaryWriter(new MemoryStream());

            Program.EncodeFileBytes(nodes, fileIn, fileOut);

            byte[] expected_bytes = new byte[] { 0x97, 0x0C };
            BinaryReader expected = new BinaryReader(new MemoryStream(expected_bytes));
            BinaryReader actual = new BinaryReader(fileOut.BaseStream);

            Assert_Binary_Streams_Are_Equals(expected, actual);

            expected.Close();
            actual.Close();
            fileIn.Close();
            fileOut.Close();
        }

        [TestMethod]
        public void TryAppend_NoOwerflow() {
            BitArray outer = new BitArray(10);
            BitArray inner = new BitArray(StringToArrayOfBits("110011"));
            BitArray tmp = BitArrayExtender.TryAppend(outer, 0, inner);

            Assert.IsNull(tmp);

            for(int i = 0; i < inner.Length; i++) {
                Assert.AreEqual(inner[i], outer[i]);
            }
        }

        [TestMethod]
        public void TryAppend_Owerflow() {
            BitArray outer = new BitArray(10);
            BitArray inner = new BitArray(StringToArrayOfBits("110011"));
            BitArray tmp = BitArrayExtender.TryAppend(outer, 5, inner);

            Assert.IsNotNull(tmp);

            for(int i = 0, j = 5; j < outer.Length && i < inner.Length; i++, j++) {
                Assert.AreEqual(inner[i], outer[j]);
            }

            Assert.AreEqual(true, tmp[0]);
        }
        
        [TestMethod]
        public void TryAppend_ExactSizeMatch() {
            BitArray outer = new BitArray(10);
            BitArray inner = new BitArray(StringToArrayOfBits("110011"));
            BitArray tmp = BitArrayExtender.TryAppend(outer, 4, inner);

            Assert.IsNull(tmp);

            for(int i = 0, j = 4; j < outer.Length && i < inner.Length; i++, j++) {
                Assert.AreEqual(inner[i], outer[j]);
            }
        }

        [TestMethod]
        public void TryAppend_ExactSizeMatchAndThenOwerflow() {
            BitArray outer = new BitArray(10);
            BitArray inner = new BitArray(StringToArrayOfBits("110011"));
            int position = 4;
            BitArray tmp = BitArrayExtender.TryAppend(outer, position, inner);

            Assert.IsNull(tmp);

            for(int i = 0, j = position; j < outer.Length && i < inner.Length; i++, j++) {
                Assert.AreEqual(inner[i], outer[j]);
            }
            position += inner.Length;

            tmp = BitArrayExtender.TryAppend(outer, position, inner);
            Assert.IsNotNull(tmp);

            for(int i = 0; i < tmp.Length; i++)
                Assert.AreEqual(inner[i], tmp[i]);
        }
    }
}
