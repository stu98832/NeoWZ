namespace NeoWZ.Serialize.Property
{
    public class WzDispatch : WzVariant
    {
        public override WzComBase Parent {
            get => this.Value?.Parent ?? base.Parent;
            set {
                if (this.Value != null) {
                    this.Value.Parent = value;
                }
                base.Parent = value;
            }
        }

        public WzComBase Value { get; set; }

        public WzDispatch(string name, WzComBase value = null) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != null;
        public override string ToText(string def = null) => this.Value?.ToString();
        public override bool Equals(WzVariant obj) => object.Equals(this.Value, obj.To<WzDispatch>()?.Value);
        public override T ToCom<T>(T def = null) => this.Value as T ?? def;
        public override WzVariant Clone() => new WzDispatch(this.Name, this.Value.Clone().To<WzComBase>());
    }
}