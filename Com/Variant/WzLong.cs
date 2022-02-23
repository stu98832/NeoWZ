namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzLong : WzVariant
    {
        /// <summary> </summary>
        public long Value { get; set; }

        /// <summary> </summary>
        public WzLong(string name, long value = 0) :
            base(WzVariantType.Long, name) {
            this.Value = value;
        }

        /// <summary> </summary>
        public override bool ToBool(bool def = false) => this.Value != 0;

        /// <summary> </summary>
        public override sbyte ToSByte(sbyte def = 0) => (sbyte)(this.Value & 0xFF);

        /// <summary> </summary>
        public override byte ToByte(byte def = 0) => (byte)(this.Value & 0xFF);

        /// <summary> </summary>
        public override short ToShort(short def = 0) => (short)(this.Value & 0xFFFF);

        /// <summary> </summary>
        public override ushort ToUShort(ushort def = 0) => (ushort)(this.Value & 0xFFFF);

        /// <summary> </summary>
        public override int ToInt(int def = 0) => (int)(this.Value & 0xFFFFFFFF);

        /// <summary> </summary>
        public override uint ToUInt(uint def = 0) => (uint)(this.Value & 0xFFFFFFFF);

        /// <summary> </summary>
        public override long ToLong(long def = 0) => this.Value;

        /// <summary> </summary>
        public override ulong ToULong(ulong def = 0) => (ulong)this.Value;

        /// <summary> </summary>
        public override string ToString() => this.Value.ToString();

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => this.Value == (obj as WzLong).Value;

        /// <summary> </summary>
        public override WzVariant Clone() => new WzLong(this.Name, this.Value);

        internal override void Read(IWzFileStream fs) => this.Value = fs.ReadInt64(true);

        internal override void Write(IWzFileStream fs) {
            base.Write(fs);
            fs.WriteInt64(this.Value, true);
        }
    }
}
