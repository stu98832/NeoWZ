namespace NeoWZ.Serialize.Property
{
    public class WzNull : WzVariant
    {
        public WzNull(string name) : base(name) {
        }

        public override WzVariant Clone() => new WzNull(this.Name);
        public override bool Equals(WzVariant obj) => true;
    }
}
