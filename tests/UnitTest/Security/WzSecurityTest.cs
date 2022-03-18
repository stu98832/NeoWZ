using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Security.Test
{
    [TestClass]
    public class WzSecurityTest
    {
        [TestMethod]
        public void EncryptOffsetTest() {
            uint position = 138;
            uint off = 60;
            uint key = 1411600286;
            Assert.AreEqual(1411600268u, WzSecurity.EncryptOffset(position, off, key));
        }

        [TestMethod]
        public void DecryptOffsetTest() {
            uint encrypted = 1411600268;
            uint off = 60;
            uint key = 1411600286;
            Assert.AreEqual(138u, WzSecurity.DecryptOffset(encrypted, off, key));
        }
    }
}
