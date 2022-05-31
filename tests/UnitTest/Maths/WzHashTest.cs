using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Maths.Test
{
    [TestClass]
    public class WzHashTest
    {
        [TestMethod]
        public void VersionHashTest() {
            string version = "43";
            int excepted = 1748;
            Assert.AreEqual(excepted, WzHash.VersionHash(version));
        }

        [TestMethod]
        public void PackageHashTest() {
            int hash = 1748;
            int excepted = 45;
            Assert.AreEqual(excepted, WzHash.PackageHash(hash));
        }

        [TestMethod]
        public void OffsetHashTest() {
            uint position = 24;
            int hash = 53970;
            Assert.AreEqual(1411600286u, WzHash.OffsetHash(position, hash));
        }
    }
}
