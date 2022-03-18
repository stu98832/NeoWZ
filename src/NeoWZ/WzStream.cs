using NeoWZ.Constants;
using NeoWZ.Serialize;
using System.Security.Cryptography;

namespace NeoWZ
{
    public class WzStream : IDisposable
    {
        private Aes mAES = null;
        private ICryptoTransform mTransform = null;
        private byte[] mKeyCache = null;

        public byte[] IV { get; private set; }
        public Stream Base { get; }
        public SerializeStringPool StringPool { get; protected set; }
        public virtual long Available => this.Base.Length - this.Base.Position;
        public long Length => this.Base.Length;
        public long Position {
            get => this.Base.Position;
            set => this.Base.Position = value;
        }

        public WzStream(Stream baseStream, byte[] iv = null) {
            this.Base = baseStream;
            this.StringPool = new SerializeStringPool(this);
            this.LoadAES(iv);
        }

        private WzStream(WzStream refer, long offset, long size) {
            this.Base = new SectionStream(refer.Base, offset, size);
            this.StringPool = refer.StringPool;
            this.LoadAES(refer.IV);
        }

        public WzStream OpenSection(long offset, long size = -1) => new WzStream(this, offset, size);
        public long Seek(long offset, SeekOrigin origin) => this.Base.Seek(offset, SeekOrigin.Begin);
        public void Close() => this.Base.Close();
        public void Flush() => this.Base.Flush();

        public byte[] Read(int size, bool encrypted = false) {
            var buffer = new byte[size];
            this.Base.Read(buffer, 0, size);
            if (encrypted) {
                this.Crypt(buffer);
            }
            return buffer;
        }

        public bool ReadBool() {
            return this.ReadByte() != 0;
        }

        public byte ReadByte() {
            if (this.Available < 1) {
                throw new EndOfStreamException("End od stream");
            }
            return (byte)this.Base.ReadByte();
        }

        public ushort ReadUInt16() {
            byte[] buf = this.Read(2);
            return (ushort)(buf[0] | (buf[1] << 8));
        }

        public uint ReadUInt32() {
            byte[] buf = this.Read(4);
            return (uint)(buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24));
        }

        public ulong ReadUInt64() {
            byte[] buf = this.Read(8);
            return (
                buf[0] | ((ulong)buf[1] << 8) | ((ulong)buf[2] << 16) | ((ulong)buf[3] << 24) | 
                ((ulong)buf[4] << 32) | ((ulong)buf[5] << 40) | ((ulong)buf[6] << 48) | ((ulong)buf[7] << 56)
            );
        }

        public sbyte ReadSByte() {
            return (sbyte)this.ReadByte();
        }

        public short ReadInt16() {
            return (short)this.ReadUInt16();
        }

        public int ReadInt32() {
            return (int)this.ReadUInt32();
        }

        public long ReadInt64() {
            return (long)this.ReadUInt64();
        }

        public float ReadFloat() {
            return BitConverter.ToSingle(this.Read(4), 0);
        }

        public double ReadDouble() {
            return BitConverter.ToDouble(this.Read(8), 0);
        }

        public void Write(byte[] buffer, bool encrypted = false) {
            if (encrypted) {
                this.Crypt(buffer);
            }
            this.Base.Write(buffer, 0, buffer.Length);
        }

        public void WriteBool(bool b) {
            this.WriteByte((byte)(b ? 1 : 0));
        }

        public void WriteByte(byte b) {
            this.Base.WriteByte(b);
        }

        public void WriteUInt16(ushort v) {
            var b = new byte[2];
            for (var i = 0; i < 2; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }
            this.Write(b);
        }

        public void WriteUInt32(uint v) {
            var b = new byte[4];
            for (var i = 0; i < 4; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }
            this.Write(b);
        }

        public void WriteUInt64(ulong v) {
            var b = new byte[8];
            for (var i = 0; i < 8; ++i) {
                b[i] = (byte)((v >> i * 8) & 0xFF);
            }
            this.Write(b);
        }

        public void WriteSByte(sbyte v) {
            this.WriteByte((byte)v);
        }

        public void WriteInt16(short v) {
            this.WriteUInt16((ushort)v);
        }

        public void WriteInt32(int v) {
            this.WriteUInt32((uint)v);
        }

        public void WriteInt64(long v) {
            this.WriteUInt64((ulong)v);
        }

        public void WriteFloat(float v) {
            this.Write(BitConverter.GetBytes(v));
        }

        public void WriteDouble(double v) {
            this.Write(BitConverter.GetBytes(v));
        }

        private void Crypt(byte[] data) {
            if (this.IV == null) {
                return;
            }
            if (this.mKeyCache == null || this.mKeyCache.Length < data.Length) {
                this.ExpendKey(data.Length);
            }
            for (var i=0;i<data.Length;++i) {
                data[i] ^= this.mKeyCache[i];
            }
        }

        private void ExpendKey(int length) {
            var blockLength = (int)Math.Ceiling(length / 16.0) * 16;
            var key = new byte[blockLength];
            var iv = this.IV;
            var offset = 0;

            if (this.mKeyCache != null) {
                this.mKeyCache.CopyTo(key, 0);
                offset = this.mKeyCache.Length;
                iv = new byte[16];
                Array.Copy(iv, 0, this.mKeyCache, this.mKeyCache.Length - 16, 16);
            }
            for (var i = offset; i < length; i += 16) {
                iv = this.mTransform.TransformFinalBlock(iv, 0, 16);
                Array.Copy(iv, 0, key, i, 16);
            }
            this.mKeyCache = key;
        }

        private void LoadAES(byte[] iv) {
            if (iv == null) {
                return;
            }
            if (iv.Length == 4) {
                var expandIv = new byte[16];
                for (int i = 0; i < 4; ++i) {
                    iv.CopyTo(expandIv, 4 * i);
                }
                iv = expandIv;
            }
            if (iv.Length != 16) {
                throw new ArgumentException("Invalid AES IV");
            }
            this.mAES = Aes.Create();
            this.mAES.Key = WzConstants.AESKey;
            this.mAES.Mode = CipherMode.ECB;
            this.mTransform = this.mAES.CreateEncryptor();
            this.IV = iv;
        }

        public void Dispose() {
            this.Base?.Flush();
            this.Base?.Close();
            this.Base?.Dispose();
        }
    }
}
