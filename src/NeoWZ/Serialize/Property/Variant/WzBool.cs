using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzBool : WzVariant
    {
        public override VariantType Type => VariantType.Boolean;
        public bool Value { get; set; }

        public WzBool(string name, bool value = false) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => Value;
        public override string ToText(string def = null) => Value ? "true" : "false";
        public override bool Equals(ComVariant obj) => Value == (obj as WzBool)?.Value;
        public override WzVariant Clone() => new WzBool(Name, Value);

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            var value = stream.ReadInt16();
            if (value != 0 && value != -1) {
                throw new InvalidDataException("Invalid boolean value");
            }
            this.Value = value == 0;
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) =>
            stream.WriteInt16((short)(Value ? -1 : 0));
    }
}
