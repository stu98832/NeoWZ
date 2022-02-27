namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzNull : WzVariant
    {
        /// <summary> </summary>
        public WzNull(string name) :
            base(WzVariantType.Null, name) {
        }

        /// <summary> </summary>
        public override string ToString() => null;

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => true;

        /// <summary> </summary>
        public override WzVariant Clone() => new WzNull(this.Name);
    }
}
