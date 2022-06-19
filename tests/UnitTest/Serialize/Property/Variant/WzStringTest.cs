﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.Serialize.Property.Variant.Test
{
    [TestClass]
    public class WzStringTest
    {
        private WzVariant Variant = new WzString("variant", "hello");
        private WzVariant Variant2 = new WzString("variant2", "");

        [TestMethod]
        public void ToBoolFalseTest() => Assert.AreEqual(true, Variant.ToBool(false));

        [TestMethod]
        public void ToBoolTrueTest() => Assert.AreEqual(false, Variant2.ToBool(true));

        [TestMethod]
        public void ToSByteTest() => Assert.AreEqual<sbyte>(123, Variant.ToSByte(123));

        [TestMethod]
        public void ToByteTest() => Assert.AreEqual<byte>(234, Variant.ToByte(234));

        [TestMethod]
        public void ToInt16Test() => Assert.AreEqual<short>(456, Variant.ToInt16(456));

        [TestMethod]
        public void ToUInt16Test() => Assert.AreEqual<ushort>(789, Variant.ToUInt16(789));

        [TestMethod]
        public void ToInt32Test() => Assert.AreEqual<int>(456, Variant.ToInt32(456));

        [TestMethod]
        public void ToUInt32Test() => Assert.AreEqual<uint>(789, Variant.ToUInt32(789));

        [TestMethod]
        public void ToInt64Test() => Assert.AreEqual<long>(456, Variant.ToInt64(456));

        [TestMethod]
        public void ToUInt64Test() => Assert.AreEqual<ulong>(789, Variant.ToUInt64(789));

        [TestMethod]
        public void ToFloatTest() => Assert.AreEqual(456.3f, Variant.ToFloat(456.3f));

        [TestMethod]
        public void ToDoubleTest() => Assert.AreEqual(789.1, Variant.ToDouble(789.1));

        [TestMethod]
        public void ToTextTest() => Assert.AreEqual("hello", Variant.ToText("text"));

        [TestMethod]
        public void ToComTest() => Assert.AreEqual(null, Variant.ToCom<IComSerializable>());

        [TestMethod]
        public void EqualsTest() => Assert.AreEqual(new WzString("variant", "hello"), Variant);

        [TestMethod]
        public void EqualsFailedTest() => Assert.AreNotEqual(new WzString("variant"), Variant);
    }
}
