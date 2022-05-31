using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoWZ.Json;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.UOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoWZ.Json.Test
{
    [TestClass]
    public class WzJsonExtensionTest
    {
        [TestMethod]
        public void ToJsonTest() {
            var obj = new WzProperty();
            var obj2 = new WzProperty();
            obj.Add(new WzInt("int32", 32));
            obj.Add(new WzString("str", "text"));
            obj.Add(new WzDouble("double", 3.2));
            obj.Add(new WzDispatch("obj", new WzUOL() { Path = "int32" }));
            obj.Add(new WzDispatch("sub", obj2));
            obj2.Add(new WzString("substr", "text2"));
            var real = obj.ToJson();
            var excepted =
@"{
  ""int32"": 32,
  ""str"": ""text"",
  ""double"": 3.2,
  ""obj"": {
    ""path"": ""int32""
  },
  ""sub"": {
    ""substr"": ""text2""
  }
}";
            Assert.AreEqual(excepted, real);
        }
    }
}