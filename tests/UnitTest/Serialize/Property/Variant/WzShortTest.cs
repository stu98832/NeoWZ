using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Serialize.Property.Variant.Test
{
    [TestClass]
    public class WzShortTest
    {
        private WzVariant Variant = new WzShort("variant", 10000);
        private WzVariant Variant2 = new WzShort("variant2", 0);

        [TestMethod]
        public void ToBoolFalseTest() => Assert.AreEqual(true, Variant.ToBool(false));

        [TestMethod]
        public void ToBoolTrueTest() => Assert.AreEqual(false, Variant2.ToBool(true));

        [TestMethod]
        public void ToSByteTest() => Assert.AreEqual<sbyte>(10000 & 0xFF, Variant.ToSByte(123));

        [TestMethod]
        public void ToByteTest() => Assert.AreEqual<byte>(10000 & 0xFF, Variant.ToByte(234));

        [TestMethod]
        public void ToInt16Test() => Assert.AreEqual<short>(10000 & 0xFFFF, Variant.ToInt16(456));

        [TestMethod]
        public void ToUInt16Test() => Assert.AreEqual<ushort>(10000 & 0xFFFF, Variant.ToUInt16(789));

        [TestMethod]
        public void ToInt32Test() => Assert.AreEqual<int>(10000, Variant.ToInt32(456));

        [TestMethod]
        public void ToUInt32Test() => Assert.AreEqual<uint>(10000, Variant.ToUInt32(789));

        [TestMethod]
        public void ToInt64Test() => Assert.AreEqual<long>(10000, Variant.ToInt64(456));

        [TestMethod]
        public void ToUInt64Test() => Assert.AreEqual<ulong>(10000, Variant.ToUInt64(789));

        [TestMethod]
        public void ToFloatTest() => Assert.AreEqual(10000.0f, Variant.ToFloat(456.3f));

        [TestMethod]
        public void ToDoubleTest() => Assert.AreEqual(10000.0, Variant.ToDouble(789.1));

        [TestMethod]
        public void ToTextTest() => Assert.AreEqual("10000", Variant.ToText("text"));

        [TestMethod]
        public void ToComTest() => Assert.AreEqual(null, Variant.ToCom<IComObject>());

        [TestMethod]
        public void EqualsTest() => Assert.AreEqual(new WzShort("variant") { Value = 10000 }, Variant);

        [TestMethod]
        public void EqualsFailedTest() => Assert.AreNotEqual(new WzShort("variant"), Variant);
    }
}
