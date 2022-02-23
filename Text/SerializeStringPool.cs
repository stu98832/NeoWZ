using NeoMS.Wz.Com;
using System;
using System.Collections.Generic;

namespace NeoMS.Wz.Text
{
    /// <summary> </summary>
    public class SerializeStringPool : IDisposable
    {
        /// <summary> </summary>
        public SerializeStringPool(IWzFileStream stream) {
            this.mStrTable = new Dictionary<uint, string>();
            this.mRefTable = new Dictionary<string, uint>();
            this.mStream = stream;
        }

        /// <summary> 清除所有的緩衝資料 </summary>
        public void Clear() {
            this.mStrTable.Clear();
            this.mRefTable.Clear();
        }

        /// <summary> </summary>
        public string Read() {
            byte header = this.mStream.ReadByte();

            switch (header) {
                case 0x00: // normal serialize string
                case 0x73: // class  serialize string
                    uint off = (uint)this.mStream.Tell(true);
                    string str = SerializeString.Read(this.mStream);
                    this.mStrTable.Add(off, str);
                    return str;
                case 0x01: // reference normal serialize string
                case 0x1B: // reference class  serialize string
                    return this.mStrTable[this.mStream.ReadUInt32()];
                default:

                    throw new ArgumentException("invalid header : " + header);
            }
        }

        /// <summary> </summary>
        public void Write(string str, byte readsign, byte refsign) {
            if (str == null) {
                str = "";
            }

            bool cacheable = this.mRefTable.ContainsKey(str);

            this.mStream.WriteByte(cacheable ? refsign : readsign);

            if (cacheable) {
                this.mStream.WriteUInt32(this.mRefTable[str]);
            }
            else {
                this.mRefTable.Add(str, (uint)(this.mStream.Tell(true)));
                SerializeString.Write(this.mStream, str);
            }
        }

        /// <summary> </summary>
        public void Dispose() {
            this.Clear();
            this.mStream = null;
        }

        // For WZDirectory
        internal string DirectoryRead(out WzPackageItemType type, WzPackageItemType reftype) {
            uint off = (uint)this.mStream.Tell(true);
            type = (WzPackageItemType)this.mStream.ReadByte();
            if (type == reftype) {
                uint cacheoff = this.mStream.ReadUInt32();
                long org = this.mStream.Tell();
                this.mStream.Seek(cacheoff, true);
                type = (WzPackageItemType)this.mStream.ReadByte();
                this.mStream.Seek(org);
                return this.mStrTable[cacheoff];
            }
            else {
                string str = SerializeString.Read(this.mStream);
                this.mStrTable.Add(off, str);
                return str;
            }
        }
        internal void DirectoryWrite(string str, WzPackageItemType type, WzPackageItemType cachetype) {
            bool cacheable = false;
            bool hascache = this.mRefTable.ContainsKey(str);
            long org = this.mStream.Tell(true);

            if (hascache) {
                this.mStream.Seek(this.mRefTable[str], true);
                cacheable = this.mStream.ReadByte() == (byte)type;
                this.mStream.Seek(org, true);
            }

            this.mStream.WriteByte((byte)(cacheable ? cachetype : type));

            if (cacheable)
                this.mStream.WriteUInt32(this.mRefTable[str]);
            else {
                this.mRefTable.Add(str, (uint)org);
                SerializeString.Write(this.mStream, str);
            }
        }

        private Dictionary<uint, string> mStrTable;
        private Dictionary<string, uint> mRefTable;
        private IWzFileStream mStream;
    }
}
