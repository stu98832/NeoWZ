namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzEmpty : WzVariant
    {
        /// <summary> </summary>
        public WzEmpty(string name) :
            base(WzVariantType.Empty, name) {
        }

        /// <summary> </summary>
        public override string ToString() => "";

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => true;

        /// <summary> </summary>
        public override WzVariant Clone() => new WzEmpty(this.Name);
    }
}
