namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzUInt : WzVariant
    {
        /// <summary> </summary>
        public uint Value { get; set; }

        /// <summary> </summary>
        public WzUInt(string name, uint value = 0) :
            base(WzVariantType.UInt, name)
        {
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
        public override int ToInt(int def = 0) => (int)this.Value;

        /// <summary> </summary>
        public override uint ToUInt(uint def = 0) => this.Value;

        /// <summary> </summary>
        public override long ToLong(long def = 0) => this.Value;

        /// <summary> </summary>
        public override ulong ToULong(ulong def = 0) => this.Value;

        /// <summary> </summary>
        public override string ToString() => this.Value.ToString();

        /// <summary> </summary>
        public override bool Equals(WzVariant obj)
        {
            return this.Value == (obj as WzUInt).Value;
        }

        /// <summary> </summary>
        public override WzVariant Clone()
        {
            return new WzUInt(this.Name, this.Value);
        }

        internal override void Read(IWzFileStream fs)
        {
            this.Value = (uint)fs.ReadInt32(true);
        }

        internal override void Write(IWzFileStream fs)
        {
            base.Write(fs);
            fs.WriteInt32((int)this.Value, true);
        }
    }
}
