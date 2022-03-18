using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace NeoWZ.Serialize.Shape2D.Tests
{
    [TestClass]
    public class WzVector2DTests
    {
        [TestMethod]
        public void CloneTest() {
            var vector = new WzVector2D() { X = 32, Y = 64 };
            var memory = new MemoryStream();
            ComSerializer.Default.Serialize(memory, vector);
            memory.Seek(0, SeekOrigin.Begin);

            var anotherVector = ComSerializer.Default.Deserialize<WzVector2D>(memory);
            Assert.AreEqual(vector.X, anotherVector.X);
            Assert.AreEqual(vector.Y, anotherVector.Y);
        }
    }
}