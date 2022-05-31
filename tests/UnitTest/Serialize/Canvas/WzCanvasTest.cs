using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoWZ.Serialize;
using NeoWZ.Serialize.Canvas;
using NeoWZ.Serialize.Property;
using System.IO;

namespace NeoWZ.Serialize.Canvas.Test
{
    [TestClass]
    public class WzCanvasTest
    {
        [TestMethod]
        public void SerializeDeserializeTest() {
            var memory = new MemoryStream();
            var variant = new WzInt("hello", 32);
            var canvas = new WzCanvas() {
                Width = 32,
                Height = 32,
                CanvasData = new byte[32 * 32],
                Format = WzCanvasFormat.B4G4R4A4,
                Scale = 3,
            };
            canvas.Property.Add(variant);
            ComSerializer.Default.Serialize(memory, canvas);

            memory.Seek(0, SeekOrigin.Begin);
            var anotherConvas = ComSerializer.Default.Deserialize<WzCanvas>(memory);
            Assert.AreEqual(canvas.Width, anotherConvas.Width);
            Assert.AreEqual(canvas.Height, anotherConvas.Height);
            CollectionAssert.AreEqual(canvas.CanvasData, anotherConvas.CanvasData);
            Assert.AreEqual(canvas.Format, anotherConvas.Format);
            Assert.AreEqual(canvas.Scale, anotherConvas.Scale);
            Assert.AreEqual(canvas.Property.Count, anotherConvas.Property.Count);
            Assert.AreEqual(canvas.Property[0], anotherConvas.Property[0]);
        }
    }
}
