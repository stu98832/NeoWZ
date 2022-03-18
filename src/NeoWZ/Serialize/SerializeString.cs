using System.Text;

namespace NeoWZ.Serialize
{
    public class SerializeString
    {
        public const int ExtendSign = 0x7F;

        public static string Read(WzStream stream) {
            int length = stream.ReadSByte();
            var unicode = length > 0;
            var flag = (unicode ? length : ~length);

            if (length == 0) {
                return "";
            }

            length = flag == ExtendSign ? stream.ReadInt32() : Math.Abs(length);

            var str = stream.Read(length * (unicode ? 2 : 1), true);

            Process(str, length, unicode);

            return (unicode ? Encoding.Unicode : Encoding.ASCII).GetString(str);
        }

        public static void Write(WzStream stream, string str) {
            var len = str.Length;
            var uni = str.Any(x => x > 0xFF);
            var chars = (uni ? Encoding.Unicode : Encoding.ASCII).GetBytes(str);

            Process(chars, len, uni);

            if ((uni && len <= 127) || (!uni && len <= 128)) {
                stream.WriteSByte((sbyte)(uni ? len : -len));
            } else {
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
                    } else {
                        *(ch++) ^= (byte)chkey++;
                    }
                }
            }
        }
    }
}
