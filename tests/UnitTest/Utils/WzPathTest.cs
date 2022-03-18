using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoWZ.Utils;
namespace NeoWZ.Utils.Test
{
    [TestClass]
    public class WzPathTest
    {
        [TestMethod]
        public void GetImageFilePathTest() {
            var path = "/dir/file.img/property/item";
            Assert.AreEqual("dir/file.img", WzPath.GetImageFilePath(path));
        }

        [TestMethod]
        public void GetPropertyPathTest() {
            var path = "/dir/file.img/property/item";
            Assert.AreEqual("property/item", WzPath.GetPropertyPath(path));
        }
    }
}
