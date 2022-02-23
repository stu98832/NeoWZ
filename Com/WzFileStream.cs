using NeoMS.Wz.Crypto;
using NeoMS.Wz.Text;
using System.Text;

namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public class WzFileStream : IWzFileStream
    {
        /// <summary> </summary>
        public SerializeStringPool StringPool { get; private set; }

        /// <summary> </summary>
        public Stream BaseStream { get; private set; }

        /// <summary> </summary>
        public byte[] IV { get; private set; }

        /// <summary> </summary>
        public byte[] Key { get; private set; }

        /// <summary> </summary>
        public long BaseOffset { get; set; }

        /// <summary> </summary>
        public bool DynamicRead { get; set; }

        /// <summary> </summary>
        public long Length {
            get {
                if (this.BaseStream != null) {
                    return this.BaseStream.Length;
                }
                else {
                    return -1L;
                }
            }
        }

        /// <summary> </summary>
        public long Position {
            get {
                if (this.BaseStream != null) {
                    return this.BaseStream.Position;
                }
                else {
                    return -1L;
                }
            }
            set {
                if (this.BaseStream != null) {
                    this.BaseStream.Position = value;
                }
            }
        }

        /// <summary> </summary>
        public WzFileStream(string path, FileMode mode, byte[] iv = null) {
            this.Init(File.Open(path, mode), iv);
        }

        /// <summary> </summary>
        public WzFileStream(Stream stream, byte[] iv = null) {
            this.Init(stream, iv);
        }

        /// <summary> </summary>
        public void Seek(long off, bool usebase = false) {
            if (this.BaseStream != null) {
                this.BaseStream.Position = off + (usebase ? this.BaseOffset : 0);
            }
        }

        /// <summary> </summary>
        public void Skip(long size) {
            if (this.BaseStream != null)
                this.BaseStream.Position += size;
        }

        /// <summary> </summary>
        public long Tell(bool usebase = false) {
            if (this.BaseStream != null)
                return this.BaseStream.Position - (usebase ? this.BaseOffset : 0);
            else
                return -1L;
        }

        /// <summary> </summary>
        public void Flush() {
            if (this.BaseStream != null)
                this.BaseStream.Flush();
        }

        /// <summary> </summary>
        public void Dispose() {
            this.Dispose(true);
        }

        /// <summary> </summary>
        public void Dispose(bool closeBase) {
            if (closeBase) {
                if (this.BaseStream != null) {
                    this.BaseStream.Flush();
                    this.BaseStream.Close();
                    this.BaseStream.Dispose();
                }
                if (this.StringPool != null) {
                    this.StringPool.Dispose();
                    this.StringPool = null;
                }
            }
            this.BaseStream = null;
        }

        // read functions
        /// <summary> </summary>
        public void Read(byte[] buffer, int length, bool decrypt = false) {
            this.BaseStream.Read(buffer, 0, length);

            if (decrypt) {
                this.CryptData(buffer);
            }
        }

        /// <summary> </summary>
        public byte[] Read(int length, bool decrypt = false) {
            byte[] buffer = new byte[length];
            this.Read(buffer, length, decrypt);
            return buffer;
        }

        /// <summary> </summary>
        public bool ReadBool() {
            return this.ReadByte() == 1;
        }

        /// <summary> </summary>
        public byte ReadByte() {
            return (byte)this.Read(1)[0];
        }

        /// <summary> </summary>
        public ushort ReadUInt16() {
            byte[] buf = this.Read(2);
            return (ushort)(buf[0] | (buf[1] << 8));
        }

        /// <summary> </summary>
        public uint ReadUInt32() {
            byte[] buf = this.Read(4);
            return (uint)(buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24));
        }

        /// <summary> </summary>
        public ulong ReadUInt64() {
            byte[] buf = this.Read(8);
            return (buf[0] | ((ulong)buf[1] << 8) | ((ulong)buf[2] << 16) | ((ulong)buf[3] << 24) | ((ulong)buf[4] << 32) | ((ulong)buf[5] << 40) | ((ulong)buf[6] << 48) | ((ulong)buf[7] << 56));
        }

        /// <summary> </summary>
        public sbyte ReadSByte() {
            return (sbyte)this.ReadByte();
        }

        /// <summary> </summary>
        public short ReadInt16() {
            return (short)this.ReadUInt16();
        }

        /// <summary> </summary>
        public int ReadInt32(bool compressed = false) {
            if (compressed) {
                int val = this.ReadSByte();
                if (val != -128) {
                    return val;
                }
            }

            return (int)this.ReadUInt32();
        }

        /// <summary> </summary>
        public long ReadInt64(bool compressed = false) {
            if (compressed) {
                long val = this.ReadSByte();
                if (val != -128) {
                    return val;
                }
            }

            return (long)this.ReadUInt64();
        }

        /// <summary> </summary>
        public float ReadFloat(bool compressed = false) {
            if (compressed) {
                int val = this.ReadSByte();
                if (val != -128) {
                    return val;
                }
            }

            return BitConverter.ToSingle(this.Read(4), 0);
        }

        /// <summary> </summary>
        public double ReadDouble() {
            return BitConverter.ToDouble(this.Read(8), 0);
        }

        /// <summary> </summary>
        public string ReadString(int len, Encoding codepage = null, bool decrypt = false) {
            if (codepage == null) {
                codepage = Encoding.ASCII;
            }

            return codepage.GetString(this.Read(len, decrypt));
        }

        /// <summary> </summary>
        public void ReadDataToStream(Stream dest, long offset, int size) {
            const int BufferSize = 0x1000;

            this.Seek(offset);

            // read data
            int blockCount = (size / BufferSize);
            int remains = size % BufferSize;
            byte[] buffer = new byte[BufferSize];

            for (int i = 0; i < blockCount; ++i) {
                this.Read(buffer, BufferSize);
                dest.Write(buffer, 0, BufferSize);
            }
            if (remains > 0) {
                this.Read(buffer, remains);
                dest.Write(buffer, 0, remains);
            }
        }

        // write function
        /// <summary> </summary>
        public void Write(byte[] buffer, int length, bool encrypt = false) {
            if (encrypt) {
                this.CryptData(buffer);
            }

            this.BaseStream.Write(buffer, 0, length);
        }

        /// <summary> </summary>
        public void Write(byte[] buffer, bool encrypt = false) {
            byte[] t = new byte[buffer.Length];

            buffer.CopyTo(t, 0);
            this.Write(t, t.Length, encrypt);
        }

        /// <summary> </summary>
        public void WriteBool(bool b) {
            this.WriteByte((byte)(b ? 1 : 0));
        }

        /// <summary> </summary>
        public void WriteByte(byte v) {
            byte[] b = { v };

            this.Write(b, 1);
        }

        /// <summary> </summary>
        public void WriteUInt16(ushort v) {
            byte[] b = new byte[2];

            for (int i = 0; i < 2; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }

            this.Write(b, 2);
        }

        /// <summary> </summary>
        public void WriteUInt32(uint v) {
            byte[] b = new byte[4];

            for (int i = 0; i < 4; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }

            this.Write(b, 4);
        }

        /// <summary> </summary>
        public void WriteUInt64(ulong v) {
            byte[] b = new byte[8];

            for (int i = 0; i < 8; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }

            this.Write(b, 8);
        }

        /// <summary> </summary>
        public void WriteSByte(sbyte v) {
            this.WriteByte((byte)v);
        }

        /// <summary> </summary>
        public void WriteInt16(short v) {
            this.WriteUInt16((ushort)v);
        }

        /// <summary> </summary>
        public void WriteInt32(int v, bool compressed = false) {
            if (compressed) {
                bool compressable = -127 <= v && v <= 127;

                this.WriteSByte(compressable ? (sbyte)v : (sbyte)-128);

                if (compressable) {
                    return;
                }
            }
            this.WriteUInt32((uint)v);
        }

        /// <summary> </summary>
        public void WriteInt64(long v, bool compressed = false) {
            if (compressed) {
                bool compressable = -127 <= v && v <= 127;

                this.WriteSByte(compressable ? (sbyte)v : (sbyte)-128);

                if (compressable) {
                    return;
                }
            }

            this.WriteUInt64((ulong)v);
        }

        /// <summary> </summary>
        public void WriteFloat(float v, bool compressed = false) {
            if (compressed) {
                bool compressable = v == 0.0f;

                this.WriteSByte(compressable ? (sbyte)0 : (sbyte)-128);

                if (compressable) {
                    return;
                }
            }
            this.Write(BitConverter.GetBytes(v));
        }

        /// <summary> </summary>
        public void WriteDouble(double v) {
            this.Write(BitConverter.GetBytes(v));
        }

        /// <summary> </summary>
        public void WriteString(string str, Encoding codepage = null, bool encrypt = false) {
            if (codepage == null) {
                codepage = Encoding.ASCII;
            }

            byte[] strbuffer = codepage.GetBytes(str);
            this.Write(strbuffer, strbuffer.Length, encrypt);
        }

        /// <summary> </summary>
        public void WriteDataFromStream(Stream source, long offset, int size) {
            const int BufferSize = 0x1000;

            source.Seek(offset, SeekOrigin.Begin);

            // write data
            int blockCount = (size / BufferSize);
            int remains = size % BufferSize;
            byte[] buffer = new byte[BufferSize];

            for (int i = 0; i < blockCount; ++i) {
                source.Read(buffer, 0, BufferSize);
                this.Write(buffer, BufferSize);
            }
            if (remains > 0) {
                source.Read(buffer, 0, remains);
                this.Write(buffer, remains);
            }
        }

        /// <summary> </summary>
        public string ReadSerializeString() {
            return this.StringPool.Read();
        }

        /// <summary> </summary>
        public void WriteSerializeString(string str, byte readsign, byte refsign) {
            this.StringPool.Write(str, readsign, refsign);
        }

        /// <summary> </summary>
        public string ReadDirectoryString(out WzPackageItemType type, WzPackageItemType reftype) {
            return this.StringPool.DirectoryRead(out type, reftype);
        }

        /// <summary> </summary>
        public void WriteDirectoryString(string str, WzPackageItemType type, WzPackageItemType reftype) {
            this.StringPool.DirectoryWrite(str, type, reftype);
        }

        public void ClearStringPool() {
            this.StringPool.Clear();
        }

        private void Init(Stream stream, byte[] iv) {
            this.IV = iv;
            this.BaseOffset = 0;
            this.BaseStream = stream;
            this.DynamicRead = false;
            this.StringPool = new SerializeStringPool(this);
        }

        private void CryptData(byte[] data) {
            if (this.IV == null) {
                return;
            }
            if (data.Length > this.Key.Length) {
                this.Key = AESCipher.CreateKey(this.IV, (int)(this.Key.Length * 1.2));
            }
            for (int i = 0; i < data.Length; ++i) {
                data[i] ^= this.Key[i];
            }
        }
    }
}
