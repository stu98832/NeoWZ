using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NeoWZ.Serialize.Property.Variant.Test
{
    [TestClass]
    public class WzDoubleTest
    {
        private WzVariant Variant = new WzDouble("variant", 128.46);
        private WzVariant Variant2 = new WzDouble("variant2", 0.0);

        [TestMethod]
        public void ToBoolFalseTest() => Assert.AreEqual(true, Variant.ToBool(false));

        [TestMethod]
        public void ToBoolTrueTest() => Assert.AreEqual(false, Variant2.ToBool(true));

        [TestMethod]
        public void ToSByteTest() => Assert.ThrowsException<OverflowException>(() => Variant.ToSByte(123));

        [TestMethod]
        public void ToByteTest() => Assert.AreEqual<byte>((byte)128.46f, Variant.ToByte(234));

        [TestMethod]
        public void ToInt16Test() => Assert.AreEqual<short>((short)128.46f, Variant.ToInt16(456));

        [TestMethod]
        public void ToUInt16Test() => Assert.AreEqual<ushort>((ushort)128.46f, Variant.ToUInt16(789));

        [TestMethod]
        public void ToInt32Test() => Assert.AreEqual<int>((int)128.46f, Variant.ToInt32(456));

        [TestMethod]
        public void ToUInt32Test() => Assert.AreEqual<uint>((uint)128.46f, Variant.ToUInt32(789));

        [TestMethod]
        public void ToInt64Test() => Assert.AreEqual<long>((long)128.46f, Variant.ToInt64(456));

        [TestMethod]
        public void ToUInt64Test() => Assert.AreEqual<ulong>((ulong)128.46f, Variant.ToUInt64(789));

        [TestMethod]
        public void ToFloatTest() => Assert.AreEqual(128.46f, Variant.ToFloat(456.3f));

        [TestMethod]
        public void ToDoubleTest() => Assert.AreEqual(128.46, Variant.ToDouble(789.1));

        [TestMethod]
        public void ToTextTest() => Assert.AreEqual("128.46", Variant.ToText("text"));

        [TestMethod]
        public void ToComTest() => Assert.AreEqual(null, Variant.ToCom<IComSerializable>());

        [TestMethod]
        public void EqualsTest() => Assert.AreEqual(new WzDouble("variant", 128.46), Variant);

        [TestMethod]
        public void EqualsFailedTest() => Assert.AreNotEqual(new WzDouble("variant"), Variant);
    }
}
