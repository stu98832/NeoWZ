using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzString : WzVariant
    {
        public override VariantType Type => VariantType.String;
        public string Value { get; set; }

        public WzString(string name, string value = "") : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => !string.IsNullOrEmpty(this.Value);
        public override string ToText(string def = null) => this.Value;
        public override bool Equals(ComVariant obj) => this.Value == (obj as WzString)?.Value;
        public override WzVariant Clone() => new WzString(this.Name, this.Value);
        public override void Deserialize(WzStream stream, ComSerializer serializer) =>
            this.Value = stream.StringPool.Read(0, 1);
        public override void Serialize(WzStream stream, ComSerializer serializer) =>
            stream.StringPool.Write(this.Value, 0, 1);
    }
}
