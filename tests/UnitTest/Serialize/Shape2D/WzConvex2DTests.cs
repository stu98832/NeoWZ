using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace NeoWZ.Serialize.Shape2D.Tests
{
    [TestClass]
    public class WzConvex2DTests
    {
        [TestMethod]
        public void SerializeDeserializeTest() {
            var memory = new MemoryStream();
            var convex = new WzConvex2D();
            convex.Add(new WzVector2D() { X = 32, Y = 64 });
            convex.Add(new WzVector2D() { X = 72, Y = 96 });
            ComSerializer.Default.Serialize(memory, convex);
            memory.Seek(0, SeekOrigin.Begin);

            var anotherConvex = ComSerializer.Default.Deserialize<WzConvex2D>(memory);
            Assert.AreEqual(convex.Count, anotherConvex.Count);
            for (var i=0;i<convex.Count;++i) {
                Assert.AreEqual(convex[i].X, anotherConvex[i].X);
                Assert.AreEqual(convex[i].Y, anotherConvex[i].Y);
            }
        }
    }
}