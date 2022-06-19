using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzUnknown : WzComValue
    {
        public override VariantType Type => VariantType.Unknown;

        public WzUnknown(string name, WzSerializable value = null) : base(name, value) {
        }

        public override WzVariant Clone() => new WzUnknown(this.Name, this.Value.Clone().To<WzSerializable>());
    }
}