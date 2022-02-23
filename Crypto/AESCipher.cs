using System.Security.Cryptography;

namespace NeoMS.Wz.Crypto
{
    public class AESCipher
    {
        private static readonly byte[] sKey = new byte[] {
            0x13, 0x00, 0x00, 0x00,
            0x08, 0x00, 0x00, 0x00,
            0x06, 0x00, 0x00, 0x00,
            0xB4, 0x00, 0x00, 0x00,
            0x1B, 0x00, 0x00, 0x00,
            0x0F, 0x00, 0x00, 0x00,
            0x33, 0x00, 0x00, 0x00,
            0x52, 0x00, 0x00, 0x00
        };
        private static readonly Aes s_AES;
        private static readonly ICryptoTransform s_Transform;

        static AESCipher() {
            s_AES = Aes.Create();
            s_AES.Mode = CipherMode.ECB;
            s_AES.Key = sKey;
            s_Transform = s_AES.CreateEncryptor();
        }

        public static byte[] CreateKey(byte[] iv, int length) {
            var start = 0;
            var key = new byte[length];
            while (length > start) {
                iv = s_Transform.TransformFinalBlock(iv, 0, 16);
                Array.Copy(iv, 0, key, start, Math.Min(16, length));
                start += 16;
            }
            return key;
        }
    }
}