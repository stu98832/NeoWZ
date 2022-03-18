namespace NeoWZ.Serialize
{
    public class SerializeStringPool : IDisposable
    {
        private Dictionary<uint, KeyValuePair<byte, string>> mStrTable;
        private Dictionary<KeyValuePair<byte, string>, uint> mRefTable;
        private WzStream mStream;

        public SerializeStringPool(WzStream stream) {
            this.mStrTable = new();
            this.mRefTable = new();
            this.mStream = stream;
        }

        public void Clear() {
            this.mStrTable.Clear();
            this.mRefTable.Clear();
        }

        public string Read(byte readSign, byte refSign) {
            var sign = this.mStream.ReadByte();
            var offset = (uint)this.mStream.Position;

            if (sign == readSign) {
                var str = SerializeString.Read(this.mStream);
                this.mStrTable.Add(offset, KeyValuePair.Create(readSign, str));
                return str;
            } else if (sign == refSign) {
                var key = this.mStream.ReadUInt32();
                var pair = this.mStrTable[key];
                return pair.Value;
            } else {
                throw new InvalidDataException("Invalid sign : " + sign);
            }
        }

        public string ReadForDirectory(byte refSign, out byte realSign) {
            var off = (uint)this.mStream.Position;
            var sign = (byte)this.mStream.ReadByte();

            if (sign == refSign) {
                var key = this.mStream.ReadUInt32();
                var pair = this.mStrTable[key];
                realSign = pair.Key;
                return pair.Value;
            } else {
                var str = SerializeString.Read(this.mStream);
                this.mStrTable.Add(off, KeyValuePair.Create(sign, str));
                realSign = sign;
                return str;
            }
        }

        public void Write(string str, byte readSign, byte refSign) {
            if (str == null) {
                str = "";
            }

            var pair = KeyValuePair.Create(readSign, str);
            var cacheable = this.mRefTable.ContainsKey(pair);

            this.mStream.WriteByte(cacheable ? refSign : readSign);
            var offset = (uint)(this.mStream.Position);

            if (cacheable) {
                this.mStream.WriteUInt32(this.mRefTable[pair]);
            } else {
                SerializeString.Write(this.mStream, str);
                this.mRefTable.Add(pair, offset);
            }
        }

        public void WriteForDirectory(string str, byte readSign, byte refSign) {
            if (str == null) {
                str = "";
            }

            var offset = (uint)this.mStream.Position;
            var pair = KeyValuePair.Create(readSign, str);
            var cacheable = this.mRefTable.ContainsKey(pair);

            this.mStream.WriteByte(cacheable ? refSign : readSign);

            if (cacheable) {
                this.mStream.WriteUInt32(this.mRefTable[pair]);
            } else {
                SerializeString.Write(this.mStream, str);
                this.mRefTable.Add(pair, offset);
            }
        }

        public void Dispose() {
            this.Clear();
            this.mStream = null;
        }
    }
}
