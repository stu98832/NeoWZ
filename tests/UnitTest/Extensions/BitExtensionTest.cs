using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Extensions.Test
{
    [TestClass]
    public class BitExtensionTest
    {
        [TestMethod]
        public void UIntRotateLeftTest() {
            uint input = 0x0F;  // 00000000 00000000 00000000 00001111
            uint output = 0x3C; // 00000000 00000000 00000000 00111100
            Assert.AreEqual(input.RotateLeft(2), output);
        }

        [TestMethod]
        public void UIntRotateLeftOverflowTest() {
            uint input = 0x0F;  // 00000000 00000000 00000000 00001111
            uint output = 0x3C; // 00000000 00000000 00000000 00111100
            Assert.AreEqual(input.RotateLeft(32 + 2), output);
        }
    }
}
