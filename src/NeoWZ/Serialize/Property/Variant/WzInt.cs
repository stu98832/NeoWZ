using NeoWZ.Com;
using NeoWZ.Extensions;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzInt : WzVariant
    {
        public override VariantType Type => VariantType.Int32;
        public int Value { get; set; }

        public WzInt(string name, int value = 0) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != 0;
        public override sbyte ToSByte(sbyte def = 0) => (sbyte)(this.Value & 0xFF);
        public override byte ToByte(byte def = 0) => (byte)(this.Value & 0xFF);
        public override short ToInt16(short def = 0) => (short)(this.Value & 0xFFFF);
        public override ushort ToUInt16(ushort def = 0) => (ushort)(this.Value & 0xFFFF);
        public override int ToInt32(int def = 0) => this.Value;
        public override uint ToUInt32(uint def = 0) => (uint)this.Value;
        public override long ToInt64(long def = 0) => this.Value;
        public override ulong ToUInt64(ulong def = 0) => (ulong)(this.Value & 0xFFFFFFFF);
        public override float ToFloat(float def = 0) => this.Value;
        public override double ToDouble(double def = 0) => this.Value;
        public override string ToText(string def = null) => this.Value.ToString();
        public override bool Equals(ComVariant obj) => this.Value == (obj as WzInt).Value;
        public override WzVariant Clone() => new WzInt(this.Name, this.Value);
        public override void Deserialize(WzStream stream, ComSerializer serializer) =>
            this.Value = stream.ReadCompressedInt32();
        public override void Serialize(WzStream stream, ComSerializer serializer) =>
            stream.WriteCompressedInt32(this.Value);
    }
}
