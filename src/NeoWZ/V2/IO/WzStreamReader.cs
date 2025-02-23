using NeoWZ.V2.Crypt;
using System.Text;

namespace NeoWZ.V2.IO;

public class WzStreamReader
{
    public Stream BaseStream { get; }

    public AesCrypter Crypter { get; set; }

    public WzStreamReader(Stream stream) {
        this.BaseStream = stream;
    }

    public byte[] ReadBytes(int size) {
        byte[] buffer = new byte[size];
        int readSize = this.BaseStream.Read(buffer, 0, size);
        if (readSize != size) {
            throw new EndOfStreamException();
        }
        return buffer;
    }

    public byte[] ReadCryptBytes(int size) {
        byte[] buffer = this.ReadBytes(size);
        this.Crypter?.Transform(buffer);
        return buffer;
    }

    public sbyte ReadSByte() {
        return (sbyte)this.ReadByte();
    }

    public byte ReadByte() {
        int val = BaseStream.ReadByte();
        if (val == -1) {
            throw new EndOfStreamException();
        }
        return (byte)val;
    }

    public short ReadInt16() {
        int b1 = this.ReadByte();
        int b2 = this.ReadByte();
        return (short)((b2 << 8) | b1);
    }

    public ushort ReadUInt16() {
        int b1 = this.ReadByte();
        int b2 = this.ReadByte();
        return (ushort)((b2 << 8) | b1);
    }

    public int ReadInt32() {
        int s1 = this.ReadUInt16();
        int s2 = this.ReadUInt16();
        return (s2 << 16) | s1;
    }

    public uint ReadUInt32() {
        uint i1 = this.ReadUInt16();
        uint i2 = this.ReadUInt16();
        return (i2 << 16) | i1;
    }

    public long ReadInt64() {
        long i1 = this.ReadUInt32();
        long i2 = this.ReadUInt32();
        return (i2 << 32) | i1;
    }

    public ulong ReadUInt64() {
        ulong i1 = this.ReadUInt32();
        ulong i2 = this.ReadUInt32();
        return (i2 << 32) | i1;
    }

    public float ReadFloat() {
        byte[] buffer = new byte[4];
        BaseStream.Read(buffer, 0, 4);
        return BitConverter.ToSingle(buffer, 0);
    }

    public double ReadDouble() {
        byte[] buffer = new byte[8];
        BaseStream.Read(buffer, 0, 8);
        return BitConverter.ToDouble(buffer, 0);
    }

    public int ReadCompressedInt32() {
        sbyte val = this.ReadSByte();
        return val != -128 ? val : this.ReadInt32();
    }

    public long ReadCompressedInt64() {
        sbyte val = this.ReadSByte();
        return val != -128 ? val : this.ReadInt64();
    }

    public float ReadCompressedFloat() {
        sbyte val = this.ReadSByte();
        return val != -128 ? val : this.ReadFloat();
    }

    public string ReadSerializeString() {
        int length = this.ReadSByte();
        var isUnicode = length > 0;
        var flag = isUnicode ? length : ~length;

        if (length == 0) {
            return "";
        }

        length = flag == 0x7F ? this.ReadInt32() : Math.Abs(length);

        byte[] buffer = this.ReadCryptBytes(length * (isUnicode ? 2 : 1));

        this.SerializeString(buffer, length, isUnicode);

        return (isUnicode ? Encoding.Unicode : Encoding.ASCII).GetString(buffer);
    }

    private unsafe void SerializeString(byte[] src, int len, bool isWChar) {
        uint chKey = 0xAAAA;
        fixed (byte* pointer = src) {
            byte* ch = pointer;
            ushort* wch = (ushort*)pointer;
            for (int i = 0; i < len; ++i) {
                if (isWChar) {
                    *(wch++) ^= (ushort)chKey++;
                } else {
                    *(ch++) ^= (byte)chKey++;
                }
            }
        }
    }
}