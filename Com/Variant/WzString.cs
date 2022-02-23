namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzString : WzVariant {
        /// <summary> </summary>
        public string Value { get; set; }

        /// <summary> </summary>
        public WzString(string name, string value = "") :
            base(WzVariantType.String, name) {
            this.Value = value;
        }

        /// <summary> </summary>
        public override string ToString() => this.Value;

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => this.ToString().Equals(obj.ToString());

        /// <summary> </summary>
        public override WzVariant Clone() => new WzString(this.Name, this.Value);

        internal override void Read(IWzFileStream fs) => this.Value = fs.ReadSerializeString();

        internal override void Write(IWzFileStream fs) {
            base.Write(fs);
            fs.WriteSerializeString(this.Value, 0, 1);
        }
    }
}
