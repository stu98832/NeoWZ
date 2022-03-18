using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace NeoWZ.Extensions.Test
{
    [TestClass]
    public class WzStreamExtensionTest
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

        private void ReadAssert<T>(T excepted, Func<WzStream, T> callback) {
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(excepted, callback(this.Stream));
            Assert.AreEqual(0, this.Stream?.Available);
        }

        [TestMethod]
        public void ReadInt32CompressedTest() {
            var datas = new byte[] { 0x30 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x30, s => s?.ReadCompressedInt32());
        }

        [TestMethod]
        public void ReadInt32CompressedMoreTest() {
            var datas = new byte[] { 0x80, 0x65, 0x45, 0x32, 0x48 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x48324565, s => s?.ReadCompressedInt32());
        }

        [TestMethod]
        public void ReadInt64CompressedTest() {
            var datas = new byte[] { 0x30 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x30L, s => s?.ReadCompressedInt64());
        }

        [TestMethod]
        public void ReadInt64CompressedMoreTest() {
            var datas = new byte[] { 0x80, 0x65, 0x45, 0x32, 0x48, 0, 0, 0, 0 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0x48324565L, s => s?.ReadCompressedInt64());
        }

        [TestMethod]
        public void ReadFloatZeroCompressTest() {
            var datas = new byte[] { 0x00 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(0.0f, s => s?.ReadCompressedFloat());
        }

        [TestMethod]
        public void ReadFloatNonZeroCompressTest() {
            var datas = new byte[] { 0x80, 0x00, 0x00, 0x00, 0xC3 };

            this.Memory?.Write(datas, 0, datas.Length);
            this.ReadAssert(-128.0f, s => s?.ReadCompressedFloat());
        }

        [TestMethod]
        public void WriteInt32CompressTest() {
            var data = 0x72;

            this.Stream?.WriteCompressedInt32(data);
            Assert.AreEqual(1, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedInt32());
        }

        [TestMethod]
        public void WriteInt32CompressMoreTest() {
            var data = 253;

            this.Stream?.WriteCompressedInt32(data);
            Assert.AreEqual(5, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedInt32());
        }

        [TestMethod]
        public void WriteInt64CompressTest() {
            var data = 0x72;

            this.Stream?.WriteCompressedInt64(data);
            Assert.AreEqual(1, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedInt64());
        }

        [TestMethod]
        public void WriteInt64CompressMoreTest() {
            var data = 253;

            this.Stream?.WriteCompressedInt64(data);
            Assert.AreEqual(9, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedInt64());
        }

        [TestMethod]
        public void WriteFloatCompressTest() {
            var data = 0.0f;

            this.Stream?.WriteCompressedFloat(data);
            Assert.AreEqual(1, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedFloat());
        }

        [TestMethod]
        public void WriteFloatCompressMoreTest() {
            var data = 1.0f;

            this.Stream?.WriteCompressedFloat(data);
            Assert.AreEqual(5, this.Stream?.Length);
            this.Memory?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(data, this.Stream?.ReadCompressedFloat());
        }
    }
}
