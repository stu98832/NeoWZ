using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzDispatch : WzComValue
    {
        public override VariantType Type => VariantType.Dispatch;

        public WzDispatch(string name, WzSerializable value = null) : base(name, value) {
        }

        public override WzVariant Clone() => new WzDispatch(this.Name, this.Value.Clone().To<WzSerializable>());
    }
}