namespace NeoMS.Wz.Utils
{
    /// <summary> </summary>
    public class HashTool
    {
        /// <summary> </summary>
        public static uint GenerateChecksum(Stream stream, long offset, int size) {
            int blockSize = 0x1000;
            long orgloc = stream.Position;

            stream.Seek(offset, SeekOrigin.Begin);

            uint checksum = 0u;
            int blockCount = (size / blockSize);
            int remains = (size % blockSize);

            byte[] buffer = new byte[blockSize];

            for (int i = 0; i < blockCount; ++i) {
                stream.Read(buffer, 0, blockSize);

                for (int j = 0; j < blockSize; ++j) {
                    checksum += buffer[j];
                }
            }

            if (remains > 0) {
                stream.Read(buffer, 0, remains);

                for (int j = 0; j < remains; ++j) {
                    checksum += buffer[j];
                }
            }

            return checksum;
        }

        /// <summary> </summary>
        public static int GeneratePackageVersionHash(string str) 
            => str.Aggregate(0, (hash, ch) => (hash << 5) + (byte)ch + 1);

        /// <summary> </summary>
        public static short EncryptPackageVersionHash(int hash) 
            => Enumerable
                .Range(0, 4)
                .Aggregate((byte)0xFF, (x, i) => (byte)(x ^ (hash >> (8 * i))));

        /// <summary> </summary>
        public static uint GenerateOffsetKey(uint cur, uint off, int hash) {
            uint key = (uint)(((~(cur - off)) * hash) - 0x581C3F6D);
            return BitTool.RotateL(key, (byte)(key & 0x1F));
        }

        /// <summary> </summary>
        public static uint DecryptOffsetHash(uint factor, uint offkey, uint off)
            => (factor ^ offkey) + off * 2;

        /// <summary> </summary>
        public static uint EncryptOffsetHash(uint loc, uint offkey, uint off)
            => loc - off * 2 ^ offkey;
    }
}
