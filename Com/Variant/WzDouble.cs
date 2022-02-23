namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzDouble : WzVariant
    {
        /// <summary> </summary>
        public double Value { get; set; }

        /// <summary> </summary>
        public WzDouble(string name, double value = 0) :
            base(WzVariantType.Double, name) {
            this.Value = value;
        }

        /// <summary> </summary>
        public override bool ToBool(bool def = false) => this.Value != 0;

        /// <summary> </summary>
        public override sbyte ToSByte(sbyte def = 0) => (sbyte)this.Value;

        /// <summary> </summary>
        public override byte ToByte(byte def = 0) => (byte)this.Value;

        /// <summary> </summary>
        public override short ToShort(short def = 0) => (short)this.Value;

        /// <summary> </summary>
        public override ushort ToUShort(ushort def = 0) => (ushort)this.Value;

        /// <summary> </summary>
        public override int ToInt(int def = 0) => (int)this.Value;

        /// <summary> </summary>
        public override uint ToUInt(uint def = 0) => (uint)this.Value;

        /// <summary> </summary>
        public override long ToLong(long def = 0) => (long)this.Value;

        /// <summary> </summary>
        public override ulong ToULong(ulong def = 0) => (ulong)this.Value;

        /// <summary> </summary>
        public override float ToFloat(float def = 0) => (float)this.Value;

        /// <summary> </summary>
        public override double ToDouble(double def = 0) => this.Value;

        /// <summary> </summary>
        public override string ToString() => this.Value.ToString();

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => this.Value == (obj as WzDouble).Value;

        /// <summary> </summary>
        public override WzVariant Clone() => new WzDouble(this.Name, this.Value);

        internal override void Read(IWzFileStream fs) => this.Value = fs.ReadDouble();

        internal override void Write(IWzFileStream fs) {
            base.Write(fs);
            fs.WriteDouble(this.Value);
        }
    }
}
