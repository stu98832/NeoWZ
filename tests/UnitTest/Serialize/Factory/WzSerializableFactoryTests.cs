using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoWZ.Serialize.Canvas;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.Shape2D;
using NeoWZ.Serialize.Sound;
using NeoWZ.Serialize.UOL;

namespace NeoWZ.Serialize.Factory.Tests
{
    [TestClass]
    public class WzSerializableFactoryTests 
    {
        private IComSerializableFactory factory;

        [TestInitialize]
        public void InitializeFactory() {
            this.factory = new WzSerializableFactory();
        }

        [TestMethod]
        public void CreatePropertyTest() {
            Assert.IsInstanceOfType(this.factory.CreateByName("Property"), typeof(WzProperty));
        }

        [TestMethod]
        public void CreateCanvasTest()
        {
            Assert.IsInstanceOfType(this.factory.CreateByName("Canvas"), typeof(WzCanvas));
        }

        [TestMethod]
        public void CreateSoundTest()
        {
            Assert.IsInstanceOfType(this.factory.CreateByName("Sound_DX8"), typeof(WzSound));
        }

        [TestMethod]
        public void CreateVector2DTest()
        {
            Assert.IsInstanceOfType(this.factory.CreateByName("Shape2D#Vector2D"), typeof(WzVector2D));
        }

        [TestMethod]
        public void CreateConvex2DTest()
        {
            Assert.IsInstanceOfType(this.factory.CreateByName("Shape2D#Convex2D"), typeof(WzConvex2D));
        }

        [TestMethod]
        public void CreateUOLTest()
        {
            Assert.IsInstanceOfType(this.factory.CreateByName("UOL"), typeof(WzUOL));
        }
    }
}