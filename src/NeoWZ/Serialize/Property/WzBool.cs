namespace NeoWZ.Serialize.Property
{
    public class WzBool : WzVariant
    {
        public bool Value { get; set; }

        public WzBool(string name, bool value = false) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value;
        public override string ToText(string def = null) => this.Value ? "true" : "false";
        public override bool Equals(WzVariant obj) => this.Value == (obj as WzBool)?.Value;
        public override WzVariant Clone() => new WzBool(this.Name, this.Value);
    }
}
