namespace NeoWZ.Serialize.Property
{
    public class WzDouble : WzVariant
    {
        public double Value { get; set; }

        public WzDouble(string name, double value = 0) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != 0;
        public override sbyte ToSByte(sbyte def = 0) => Convert.ToSByte(this.Value);
        public override byte ToByte(byte def = 0) => Convert.ToByte(this.Value);
        public override short ToInt16(short def = 0) => Convert.ToInt16(this.Value);
        public override ushort ToUInt16(ushort def = 0) => Convert.ToUInt16(this.Value);
        public override int ToInt32(int def = 0) => Convert.ToInt32(this.Value);
        public override uint ToUInt32(uint def = 0) => Convert.ToUInt32(this.Value);
        public override long ToInt64(long def = 0) => Convert.ToInt64(this.Value);
        public override ulong ToUInt64(ulong def = 0) => Convert.ToUInt64(this.Value);
        public override float ToFloat(float def = 0) => (float)this.Value;
        public override double ToDouble(double def = 0) => this.Value;
        public override string ToText(string def = null) => this.Value.ToString();
        public override bool Equals(WzVariant obj) => this.Value == (obj as WzDouble).Value;
        public override WzVariant Clone() => new WzDouble(this.Name, this.Value);
    }
}
