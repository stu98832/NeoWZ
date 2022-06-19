using NeoWZ.Com;
using NeoWZ.Extensions;

namespace NeoWZ.Serialize.Property.Variant
{
    /// <summary> </summary>
    public class WzUInt : WzVariant
    {
        public override VariantType Type => VariantType.UInt32;
        public uint Value { get; set; }

        public WzUInt(string name, uint value = 0) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != 0;
        public override sbyte ToSByte(sbyte def = 0) => (sbyte)(this.Value & 0xFF);
        public override byte ToByte(byte def = 0) => (byte)(this.Value & 0xFF);
        public override short ToInt16(short def = 0) => (short)(this.Value & 0xFFFF);
        public override ushort ToUInt16(ushort def = 0) => (ushort)(this.Value & 0xFFFF);
        public override int ToInt32(int def = 0) => (int)this.Value;
        public override uint ToUInt32(uint def = 0) => this.Value;
        public override long ToInt64(long def = 0) => this.Value;
        public override ulong ToUInt64(ulong def = 0) => this.Value;
        public override float ToFloat(float def = 0) => this.Value;
        public override double ToDouble(double def = 0) => this.Value;
        public override string ToText(string def = null) => this.Value.ToString();
        public override bool Equals(ComVariant obj) => this.Value == (obj as WzUInt).Value;
        public override WzVariant Clone() => new WzUInt(this.Name, this.Value);
        public override void Deserialize(WzStream stream, ComSerializer serializer) =>
            this.Value = (uint)stream.ReadCompressedInt32();
        public override void Serialize(WzStream stream, ComSerializer serializer) =>
            stream.WriteCompressedInt32((int)this.Value);
    }
}
