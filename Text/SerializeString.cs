using NeoMS.Wz.Com;
using System;
using System.Text;

namespace NeoMS.Wz.Text
{
    /// <summary> </summary>
    public class SerializeString
    {
        public const int ExtendSign = 0x7F;

        /// <summary> </summary>
        public static string Read(IWzFileStream stream) {
            int length = stream.ReadSByte();
            bool unicode = length > 0;
            int flag = (unicode ? length : ~length);

            if (length == 0) {
                return "";
            }

            length = flag == ExtendSign ? stream.ReadInt32() : Math.Abs(length);

            var str = stream.Read(length * (unicode ? 2 : 1), true);

            Process(str, length, unicode);

            return (unicode ? Encoding.Unicode : Encoding.ASCII).GetString(str);
        }

        /// <summary> </summary>
        public static void Write(IWzFileStream stream, string str) {
            int len = str.Length;
            bool uni = false;

            foreach (char ch in str) {
                if ((int)ch > 0xFF) {
                    uni = true;
                    break;
                }
            }

            byte[] chars = (uni ? Encoding.Unicode : Encoding.ASCII).GetBytes(str);

            Process(chars, len, uni);

            if ((uni && len <= 127) || (!uni && len <= 128)) {
                stream.WriteSByte((sbyte)(uni ? len : -len));
            }
            else {
                stream.WriteByte((byte)(uni ? ExtendSign : ~ExtendSign));
                stream.WriteInt32(len);
            }

            stream.Write(chars, true);
        }

        // Process string to serialize string
        private static unsafe void Process(byte[] src, int len, bool unicode) {
            uint chkey = 0xAAAA;
            fixed (byte* pointer = src) {
                byte* ch = pointer;
                ushort* wch = (ushort*)pointer;
                for (int i = 0; i < len; ++i) {
                    if (unicode) {
                        *(wch++) ^= (ushort)chkey++;
                    }
                    else {
                        *(ch++) ^= (byte)chkey++;
                    }
                }
            }
        }
    }
}
