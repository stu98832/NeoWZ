using NeoWZ.Extensions;

namespace NeoWZ.Maths
{
    /// <summary>
    /// Provide hash functions for .wz file
    /// </summary>
    public static class WzHash
    {
        /// <summary>
        /// Hash version string
        /// </summary>
        /// <param name="str"> Version string </param>
        /// <returns></returns>
        public static int VersionHash(string str)
            => str.Aggregate(0, (hash, ch) => (hash << 5) + (byte)ch + 1);

        /// <summary>
        /// Hash hashed version string for package
        /// </summary>
        /// <param name="data"> Hashed version string </param>
        /// <returns></returns>
        public static short PackageHash(int data)
            => Enumerable.Range(0, 4).Aggregate((byte)0xFF, (x, i) => (byte)(x ^ (data >> (8 * i))));

        /// <summary>
        /// Hash offset
        /// </summary>
        /// <param name="cur"> Current position </param>
        /// <param name="hash"> Hashed version string </param>
        /// <returns></returns>
        public static uint OffsetHash(uint cur, int hash) {
            uint key = (uint)(((~cur) * hash) - 0x581C3F6D);
            return key.RotateLeft((byte)(key & 0x1F));
        }
    }
}
