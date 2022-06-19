using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzEmpty : WzVariant
    {
        public override VariantType Type => VariantType.Empty;

        public WzEmpty(string name) : base(name) {
        }

        public override string ToText(string def = null) => "";
        public override bool ToBool(bool def = false) => false;
        public override WzVariant Clone() => new WzEmpty(this.Name);
        public override bool Equals(ComVariant obj) => true;
    }
}
