using System.IO;
using System.IO.Compression;

namespace NeoMS.Framework.Utils
{
    /// <summary> </summary>
    public class ZlibTool
    {
        /// <summary> </summary>
        public const byte Z_DEFLATE = 8;

        /// <summary> </summary>
        public static bool CheckDeflate(byte unk, byte cmf, byte flg)
        {
            return (cmf == 0x78) && (flg == 0x9C || flg == 0xDA || flg == 0x01 || flg == 0x5E);
        }

        /// <summary> </summary>
        public static byte[] Compress(byte[] datas, int len)
        {
            MemoryStream stream = new MemoryStream(), comp = new MemoryStream();
            DeflateStream zlib = new DeflateStream(comp, CompressionMode.Compress);
            byte[] aComp, result;

            zlib.Write(datas, 0, len);
            zlib.Flush();
            zlib.Close();

            aComp = comp.ToArray();

            stream.WriteByte(0);
            stream.WriteByte(0x78); // Z_DEFLATE(8)    | (0x7 << 4)
            stream.WriteByte(0x9C); // Z_CHECK(11100b) | (Z_DIR(0) << 5) | (Z_LEVEL(2) << 6);
            stream.Write(aComp, 0, aComp.Length);

            result = stream.ToArray();
            stream.Dispose();
            comp.Dispose();
            zlib.Dispose();
            return result;
        }

        /// <summary> </summary>
        public static byte[] Decompress(byte[] datas, int len)
        {
            MemoryStream stream = new MemoryStream(datas);
            DeflateStream zlib = new DeflateStream(stream, CompressionMode.Decompress);
            int unk_byte = stream.ReadByte();
            int zCMF = stream.ReadByte();
            int zFLG = stream.ReadByte();

            if ((zCMF & 0xF) != Z_DEFLATE)
                throw new InvalidDataException("Not 'Deflate' method.");

            if (zCMF != 0x78)
                throw new InvalidDataException("Invalid CMF.");

            if ((zFLG & 0x20) != 0)
                throw new InvalidDataException("FLG 'Directory preset' is true.");

            byte[] rtn = new byte[len];
            zlib.Read(rtn, 0, len);

            zlib.Dispose();
            stream.Dispose();
            return rtn;
        }
    }
}
