using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace NeoWZ.Test
{
    [TestClass]
    public class WzStreamTest
    {
        private MemoryStream Memory;
        private WzStream Stream;

        [TestInitialize]
        public void Up() {
            this.Memory = new MemoryStream();
            this.Stream = new WzStream(Memory, null);
        }

        [TestCleanup]
        public void Down() {
            this.Stream?.Dispose();
        }

        [TestMethod]
        public void WriteTest() {
            var datas = new byte[16];
            var buffer = new byte[16];
            Random.Shared.NextBytes(datas);

            this.Stream?.Write(datas);
            Assert.AreEqual(datas.Length, this.Stream?.Length);
            Assert.AreEqual(datas.Length, this.Stream?.Position);

            this.Memory?.Seek(0, SeekOrigin.Begin);
            this.Memory?.Read(buffer, 0, 16);
            CollectionAssert.AreEqual(datas, buffer);
        }

        [TestMethod]
        public void ReadTest() {
            var datas = new byte[16];
            Random.Shared.NextBytes(datas);

            this.Memory?.Write(datas, 0, datas.Length);
            Assert.AreEqual(datas.Length, this.Stream?.Length);
            Assert.AreEqual(datas.Length, this.Stream?.Position);

            this.Memory?.Seek(0, SeekOrigin.Begin);
            var buffer = this.Stream?.Read(16);
            CollectionAssert.AreEqual(datas, buffer);
            Assert.AreEqual(0, this.Stream?.Available);
        }

        private void ReadAssert<T>(T excepted, Func<WzStream, T> callback) {
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(excepted, callback(this.Stream));
            Assert.AreEqual(0, this.Stream?.Available);
        }

        [TestMethod]
        public void ReadSByteTest() {
            var datas = new byte[] { 0x81 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert<sbyte?>(-127, s => s?.ReadSByte());
        }

        [TestMethod]
        public void ReadByteTest() {
            var datas = new byte[] { 0x81 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert<byte?>(0x81, s => s?.ReadByte());
        }

        [TestMethod]
        public void ReadInt16Test() {
            var datas = new byte[] { 0x81, 0x63 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert<short?>(0x6381, s => s?.ReadInt16());
        }

        [TestMethod]
        public void ReadUInt16Test() {
            var datas = new byte[] { 0x81, 0x63 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert<ushort?>(0x6381, s => s?.ReadUInt16());
        }

        [TestMethod]
        public void ReadInt32Test() {
            var datas = new byte[] { 0x81, 0x63, 0x45, 0x67 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x67456381, s => s?.ReadInt32());
        }

        [TestMethod]
        public void ReadUInt32Test() {
            var datas = new byte[] { 0x81, 0x63, 0x45, 0x67 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x67456381u, s => s?.ReadUInt32());
        }

        [TestMethod]
        public void ReadInt64Test() {
            var datas = new byte[] { 0x81, 0x63, 0x45, 0x67, 0x85, 0x45, 0x16, 0x34 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x3416458567456381L, s => s?.ReadInt64());
        }

        [TestMethod]
        public void ReadUInt64Test() {
            var datas = new byte[] { 0x81, 0x63, 0x45, 0x67, 0x85, 0x45, 0x16, 0x34 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x3416458567456381Lu, s => s?.ReadUInt64());
        }

        [TestMethod]
        public void ReadFloatTest() {
            // -128 = 11000011 00000000 00000000 00000000
            var datas = new byte[] { 0x00, 0x00, 0x00, 0xC3 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(-128.0f, s => s?.ReadFloat());
        }

        [TestMethod]
        public void ReadDoubleTest() {
            // -128 = 11000000 01100000 00000000 00000000 00000000 00000000 00000000 00000000
            var datas = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0xC0 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(-128.0, s => s?.ReadDouble());
        }

        [TestMethod]
        public void ReadBoolTest() {
            var datas = new byte[] { 0x80, 0x32, 0x00, 0x60, 0x00 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(true, this.Stream?.ReadBool());
            Assert.AreEqual(true, this.Stream?.ReadBool());
            Assert.AreEqual(false, this.Stream?.ReadBool());
            Assert.AreEqual(true, this.Stream?.ReadBool());
            Assert.AreEqual(false, this.Stream?.ReadBool());
            Assert.AreEqual(0, this.Stream?.Available);
        }

        [TestMethod]
        public void WriteSByteTest() {
            var data = (sbyte)-127;

            this.Stream?.WriteSByte(data);
            Assert.AreEqual(1, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadSByte());
        }

        [TestMethod]
        public void WriteByteTest() {
            var data = (byte)254;

            this.Stream?.WriteByte(data);
            Assert.AreEqual(1, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadByte());
        }

        [TestMethod]
        public void WriteInt16Test() {
            var data = (short)0x6532;

            this.Stream?.WriteInt16(data);
            Assert.AreEqual(2, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadInt16());
        }

        [TestMethod]
        public void WriteUInt16Test() {
            var data = (ushort)0x6532;

            this.Stream?.WriteUInt16(data);
            Assert.AreEqual(2, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadUInt16());
        }

        [TestMethod]
        public void WriteInt32Test() {
            var data = 0x13656532;

            this.Stream?.WriteInt32(data);
            Assert.AreEqual(4, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadInt32());
        }

        [TestMethod]
        public void WriteUInt32Test() {
            var data = 0x85326532u;

            this.Stream?.WriteUInt32(data);
            Assert.AreEqual(4, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadUInt32());
        }

        [TestMethod]
        public void WriteInt64Test() {
            var data = 0x65321513656532;

            this.Stream?.WriteInt64(data);
            Assert.AreEqual(8, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadInt64());
        }

        [TestMethod]
        public void WriteUInt64Test() {
            var data = 0x65321513656532u;

            this.Stream?.WriteUInt64(data);
            Assert.AreEqual(8, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadUInt64());
        }

        [TestMethod]
        public void WriteFloatTest() {
            var data = 0.0f;

            this.Stream?.WriteFloat(data);
            Assert.AreEqual(4, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadFloat());
        }

        [TestMethod]
        public void WriteDoubleTest() {
            var data = 13.0f;

            this.Stream?.WriteDouble(data);
            Assert.AreEqual(8, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadDouble());
        }
    }
}
