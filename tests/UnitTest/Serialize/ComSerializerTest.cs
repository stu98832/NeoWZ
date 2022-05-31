using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NeoWZ.Serialize.Test.Mock;

namespace NeoWZ.Serialize.Test
{
    [TestClass]
    public class ComSerializerTest
    {
        private MemoryStream Memory;
        private WzStream Stream;
        private int Magic = 0x6943642;

        [TestInitialize]
        public void Up() {
            Memory = new MemoryStream();
            Stream = new WzStream(Memory);
            this.PerpareRawMockComClassData();
            Stream.Seek(0, SeekOrigin.Begin);
        }

        private void PerpareRawMockComClassData() {
            Stream.StringPool.Write("Mock", 0x73, 0x7B);
            Stream.WriteInt32(Magic);
        }

        [TestMethod]
        public void SerializeTest() {
            var memory = new MemoryStream();
            var mock = new MockComClass() { Value = Magic };
            ComSerializer.Default.Serialize(memory, mock);
            CollectionAssert.AreEqual(Memory?.ToArray(), memory.ToArray());
        }

        [TestMethod]
        public void DeserializeTest() {
            var mock = ComSerializer.Default.Deserialize<MockComClass>(Stream);
            Assert.AreEqual(0, Stream?.Available);
            Assert.AreEqual(Magic, mock.Value);
        }
    }
}
