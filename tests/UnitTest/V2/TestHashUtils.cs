using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoWZ.V2.Utils.Test;

[TestClass]
public class TestHashUtils
{
    [TestMethod]
    public void VersionHashTest() {
        string version = "43";
        int excepted = 1748;
        Assert.AreEqual(excepted, HashUtils.StringHash(version));
    }

    [TestMethod]
    public void PackageHashTest() {
        int hash = 1748;
        int excepted = 45;
        Assert.AreEqual(excepted, HashUtils.XorHash(hash));
    }
}
