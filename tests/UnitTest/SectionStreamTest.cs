using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace NeoWZ.Test
{
    [TestClass]
    public class SectionStreamTest
    {
        private MemoryStream Memory;
        private SectionStream Section;

        [TestInitialize]
        public void Up() {
            Memory = new MemoryStream();
            Memory.Write(Enumerable.Range(0, 2000).Select(x => (byte)x).ToArray(), 0, 2000);
            Memory.Seek(1000, SeekOrigin.Begin);
            Section = new SectionStream(Memory, 1000);
        }

        [TestCleanup]
        public void Down() {
            Section?.Dispose();
            Memory?.Dispose();
        }

        [TestMethod]
        public void CreateWithoutSizeTest() {
            var mem = new MemoryStream();
            var sec = new SectionStream(mem, 1000);
            mem.Write(new byte[2000], 0, 2000);
            mem.Seek(1000, SeekOrigin.Begin);
            Assert.AreEqual(0, sec.Position);
            Assert.AreEqual(1000, sec.Base.Position);
            Assert.AreEqual(1000, sec.Offset);
            Assert.AreEqual(1000, sec.Length);
        }

        [TestMethod]
        public void CreateWithSizeTest() {
            var mem = new MemoryStream();
            var sec = new SectionStream(mem, 500, 500);
            mem.Write(new byte[2000], 0, 2000);
            mem.Seek(500, SeekOrigin.Begin);
            Assert.AreEqual(0, sec.Position);
            Assert.AreEqual(500, sec.Base.Position);
            Assert.AreEqual(500, sec.Offset);
            Assert.AreEqual(500, sec.Length);
        }

        [TestMethod]
        public void SeekBeginTest() {
            Section?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0, Section?.Position);
            Assert.AreEqual(Section?.Offset, Memory?.Position);
        }

        [TestMethod]
        public void SeekEndTest() {
            Section?.Seek(0, SeekOrigin.End);
            Assert.AreEqual(1000, Section?.Position);
            Assert.AreEqual(2000, Memory?.Position);
        }

        [TestMethod]
        public void SeekOutBoundErrorTest() {
            Section?.Seek(50, SeekOrigin.Begin);
            Assert.ThrowsException<IOException>(() => Section?.Seek(-1, SeekOrigin.Begin));
            Assert.ThrowsException<IOException>(() => Section?.Seek(-51, SeekOrigin.Current));
            Assert.ThrowsException<IOException>(() => Section?.Seek(-1001, SeekOrigin.End));
        }

        [TestMethod]
        public void ReadTest() {
            Section?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(1000 & 0xFF, Section?.ReadByte());
        }

        [TestMethod]
        public void WriteTest() {
            Section?.Seek(0, SeekOrigin.Begin);
            Section?.WriteByte(64);
            Section?.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(64, Section?.ReadByte());
        }

        [TestMethod]
        public void WriteOutOfBoundTest() {
            Section?.Seek(0, SeekOrigin.End);
            Section?.WriteByte(64);
            Section?.Seek(-1, SeekOrigin.End);
            Assert.AreEqual(64, Section?.ReadByte());
            Assert.AreEqual(1001, Section?.Length);
            Assert.AreEqual(2001, Memory?.Length);
        }
    }
}
