namespace NeoWZ.Serialize.Property
{
    public class WzString : WzVariant {
        public string Value { get; set; }

        public WzString(string name, string value = "") : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => !string.IsNullOrEmpty(this.Value);
        public override string ToText(string def = null) => this.Value;
        public override bool Equals(WzVariant obj) => this.Value == (obj as WzString)?.Value;
        public override WzVariant Clone() => new WzString(this.Name, this.Value);
    }
}
