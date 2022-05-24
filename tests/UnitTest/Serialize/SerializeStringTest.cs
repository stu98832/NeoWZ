using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace NeoWZ.Serialize.Test
{
    [TestClass]
    public class SerializeStringTest
    {
        public WzStream Stream;

        [TestInitialize]
        public void Up() {
            this.Stream = new WzStream(new MemoryStream());
        }

        [TestCleanup]
        public void Down() {
            this.Stream?.Dispose();
        }

        [TestMethod]
        public void ReadAsciiTest() {
            this.WriteAsciiHelloString(this.Stream);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("Hello", SerializeString.Read(this.Stream));
        }

        private void WriteAsciiHelloString(WzStream stream) {
            var datas = new byte[] { 0xE2, 0xCE, 0xC0, 0xC1, 0xC1 };
            stream.WriteSByte(-5);
            stream.Write(datas);
        }

        [TestMethod]
        public void ReadUnicodeTest() {
            this.WriteUnicodeHelloString(this.Stream);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("Hello", SerializeString.Read(this.Stream));
        }

        private void WriteUnicodeHelloString(WzStream stream) {
            var datas = new byte[] { 0xE2, 0xAA, 0xCE, 0xAA, 0xC0, 0xAA, 0xC1, 0xAA, 0xC1, 0xAA };
            stream.WriteSByte(5);
            stream.Write(datas);
        }

        [TestMethod]
        public void WriteTest() {
            SerializeString.Write(this.Stream, "Hello");
            Assert.AreEqual(6, this.Stream?.Length);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("Hello", SerializeString.Read(this.Stream));
        }

        [TestMethod]
        public void WriteUnicodeTest() {
            SerializeString.Write(this.Stream, "中文");
            Assert.AreEqual(5, this.Stream?.Length);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("中文", SerializeString.Read(this.Stream));
        }

        [TestMethod]
        public void ReadWriteTest() {
            var text = "".PadLeft(1000, '1');
            SerializeString.Write(this.Stream, text);
            Assert.AreEqual(1005, this.Stream?.Length);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(text, SerializeString.Read(this.Stream));
        }

        [TestMethod]
        public void ReadWriteUnicodeTest() {
            var text = "".PadLeft(1000, '中');
            SerializeString.Write(this.Stream, text);
            Assert.AreEqual(2005, this.Stream?.Length);
            this.Stream?.Base.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(text, SerializeString.Read(this.Stream));
        }
    }
}
