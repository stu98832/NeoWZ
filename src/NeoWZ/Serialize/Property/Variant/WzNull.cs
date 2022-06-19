using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzNull : WzVariant
    {
        public override VariantType Type => VariantType.Null;

        public WzNull(string name) : base(name) {
        }

        public override WzVariant Clone() => new WzNull(this.Name);
        public override bool Equals(ComVariant obj) => true;
    }
}
