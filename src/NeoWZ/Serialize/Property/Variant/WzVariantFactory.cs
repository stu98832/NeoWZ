using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public class WzVariantFactory
    {
        public static WzVariant CreateByType(VariantType vt, string name) {
            switch (vt) {
                case VariantType.Empty:
                    return new WzEmpty(name);
                case VariantType.Null:
                    return new WzNull(name);
                case VariantType.Int16:
                    return new WzShort(name);
                case VariantType.Int32:
                    return new WzInt(name);
                case VariantType.Float:
                    return new WzFloat(name);
                case VariantType.Double:
                    return new WzDouble(name);
                case VariantType.String:
                    return new WzString(name);
                case VariantType.Dispatch:
                    return new WzDispatch(name);
                case VariantType.Boolean:
                    return new WzBool(name);
                case VariantType.Unknown:
                    return new WzUnknown(name);
                case VariantType.UInt32:
                    return new WzUInt(name);
                case VariantType.Int64:
                    return new WzLong(name);
                default: throw new NotImplementedException($"variant {vt} not implemented");
            }
        }
    }
}
