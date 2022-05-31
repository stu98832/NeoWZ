using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace NeoWZ.Serialize.Property.Test
{
    [TestClass]
    public class WzPropertyTest
    {
        [TestMethod]
        public void IndexerTest() {
            var property = new WzProperty();
            var items = new WzVariant[] { 
                new WzInt("int", 32), 
                new WzLong("long", 32), 
                new WzLong("value", 32) 
            };

            foreach (var item in items) {
                property.Add(item);
            }

            var i = 0;
            foreach (var item in items) {
                Assert.AreEqual(item.Name, property[i].Name);
                Assert.AreEqual(item, property[i]);
                ++i;
            }
        }

        [TestMethod]
        public void PathSearchTest() {
            var property = new WzProperty();
            var items = new WzVariant[] {
                new WzInt("int", 32),
                new WzLong("long", 32),
                new WzLong("value", 32)
            };

            foreach (var item in items) {
                property.Add(item);
            }

            Assert.AreEqual(items[2], property["value"]);
        }

        [TestMethod]
        public void PathSearchFailedTest() {
            var property = new WzProperty();
            var items = new WzVariant[] {
                new WzInt("int", 32),
                new WzLong("long", 32),
                new WzLong("value", 32)
            };

            foreach (var item in items) {
                property.Add(item);
            }

            Assert.AreEqual(WzVariant.Invalid, property["null"]);
        }

        [TestMethod]
        public void PathSearchDeepTest() {
            var property = new WzProperty();
            var propertySub = new WzProperty();
            var items = new WzVariant[] {
                new WzInt("int", 32),
                new WzLong("long", 32),
                new WzLong("value", 32)
            };

            foreach (var item in items) {
                propertySub.Add(item);
            }
            property.Add(new WzDispatch("sub", propertySub));

            Assert.AreEqual(items[2], property["sub/value"]);
        }

        [TestMethod]
        public void PathSearchPrevTest() {
            var property = new WzProperty();
            var propertySub = new WzProperty();
            var items = new WzVariant[] {
                new WzInt("int", 32),
                new WzLong("long", 32),
                new WzLong("value", 32)
            };

            foreach (var item in items) {
                propertySub.Add(item);
            }
            property.Add(new WzDispatch("sub", propertySub) { Parent = property });

            Assert.AreEqual(property, propertySub[".."].ToCom<WzProperty>());
        }

        [TestMethod]
        public void SerializeDeserializeTest() {
            var property = new WzProperty();
            var stream = new MemoryStream();
            property.Add(new WzInt("int", 32));
            property.Add(new WzLong("long", 32));
            property.Add(new WzLong("value", 32));
            ComSerializer.Default.Serialize(stream, property);
            stream.Seek(0, SeekOrigin.Begin);
            var anotherProperty = ComSerializer.Default.Deserialize<WzProperty>(stream);
            Assert.AreEqual(property.Count, anotherProperty.Count);
            for (var i=0;i<property.Count;++i) {
                Assert.AreEqual(property[i].Name, anotherProperty[i].Name);
                Assert.AreEqual(property[i], anotherProperty[i]);
            }
        }
    }
}
