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
        public static int GeneratePackageVersionHash(string str) {
            int hash = 0;
            foreach (char ch in str) {
                hash = ((hash << 5) + ((byte)ch)) + 1;
            }
            return hash;
        }

        /// <summary> </summary>
        public static short EncryptPackageVersionHash(int hash) {
            int val = 0;
            for (int i = 0; i < 4; ++i) {
                val ^= (hash >> (8 * i)) & 0xff;
            }
            return (byte)~val;
        }

        /// <summary> </summary>
        public static uint GenerateOffsetKey(uint cur, uint off, int hash) {
            uint key = (uint)(((~(cur - off)) * hash) - 0x581C3F6D);
            return BitTool.RotateL(key, (byte)(key & 0x1F));
        }

        /// <summary> </summary>
        public static uint DecryptOffsetHash(uint factor, uint offkey, uint off) {
            return (factor ^ offkey) + off * 2;
        }

        /// <summary> </summary>
        public static uint EncryptOffsetHash(uint loc, uint offkey, uint off) {
            return ((loc - off * 2) ^ offkey);
        }
    }
}
