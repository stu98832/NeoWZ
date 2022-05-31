using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Serialize.Property.Variant.Test
{
    [TestClass]
    public class WzLongTest
    {
        private WzVariant Variant = new WzLong("variant", 10000000000);
        private WzVariant Variant2 = new WzLong("variant2", 0);

        [TestMethod]
        public void ToBoolFalseTest() => Assert.AreEqual(true, Variant.ToBool(false));

        [TestMethod]
        public void ToBoolTrueTest() => Assert.AreEqual(false, Variant2.ToBool(true));

        [TestMethod]
        public void ToSByteTest() => Assert.AreEqual<sbyte>(unchecked((sbyte)(10000000000 & 0xFF)), Variant.ToSByte(123));

        [TestMethod]
        public void ToByteTest() => Assert.AreEqual<byte>((byte)(10000000000 & 0xFF), Variant.ToByte(234));

        [TestMethod]
        public void ToInt16Test() => Assert.AreEqual<short>(unchecked((short)(10000000000 & 0xFFFF)), Variant.ToInt16(456));

        [TestMethod]
        public void ToUInt16Test() => Assert.AreEqual<ushort>((ushort)(10000000000 & 0xFFFF), Variant.ToUInt16(789));

        [TestMethod]
        public void ToInt32Test() => Assert.AreEqual<int>((int)(10000000000 & 0xFFFFFFFF), Variant.ToInt32(456));

        [TestMethod]
        public void ToUInt32Test() => Assert.AreEqual<uint>((uint)(10000000000 & 0xFFFFFFFF), Variant.ToUInt32(789));

        [TestMethod]
        public void ToInt64Test() => Assert.AreEqual<long>(10000000000, Variant.ToInt64(456));

        [TestMethod]
        public void ToUInt64Test() => Assert.AreEqual<ulong>(10000000000u, Variant.ToUInt64(789));

        [TestMethod]
        public void ToFloatTest() => Assert.AreEqual(10000000000.0f, Variant.ToFloat(456.3f));

        [TestMethod]
        public void ToDoubleTest() => Assert.AreEqual(10000000000.0, Variant.ToDouble(789.1));

        [TestMethod]
        public void ToTextTest() => Assert.AreEqual("10000000000", Variant.ToText("text"));

        [TestMethod]
        public void ToComTest() => Assert.AreEqual(null, Variant.ToCom<IComObject>());

        [TestMethod]
        public void EqualsTest() => Assert.AreEqual(new WzLong("variant") { Value = 10000000000 }, Variant);

        [TestMethod]
        public void EqualsFailedTest() => Assert.AreNotEqual(new WzLong("variant"), Variant);
    }
}
