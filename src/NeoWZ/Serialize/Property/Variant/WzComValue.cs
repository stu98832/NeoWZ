using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    public abstract class WzComValue : WzVariant
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

        public WzComValue(string name, WzComBase value = null) : base(name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != null;
        public override string ToText(string def = null) => this.Value?.ToString();
        public override bool Equals(ComVariant obj) => Equals(this.Value, obj.To<WzComValue>()?.Value);
        public override T ToCom<T>(T def = null) => this.Value as T ?? def;

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            var size = stream.ReadInt32();
            var off = stream.Position;
            this.Value = serializer.Deserialize<WzComBase>(stream);

            // prevent incomplete deserialize
            stream.Seek(off + size, SeekOrigin.Begin);
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteInt32(0);
            var off = stream.Base.Position;

            serializer.Serialize(stream, this.Value);

            var size = stream.Base.Position - off;
            stream.Base.Seek(off - 4, SeekOrigin.Begin);
            stream.WriteInt32((int)size);

            stream.Base.Seek(off + size, SeekOrigin.Begin);
        }
    }
}
