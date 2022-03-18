namespace NeoWZ.Serialize.Property
{
    public class WzEmpty : WzVariant
    {
        public WzEmpty(string name) : base(name) {
        }

        public override string ToText(string def = null) => "";
        public override bool ToBool(bool def = false) => false;
        public override WzVariant Clone() => new WzEmpty(this.Name);
        public override bool Equals(WzVariant obj) => true;
    }
}
