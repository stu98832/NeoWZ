using NeoWZ.Com;
using NeoWZ.Extensions;
using NeoWZ.Serialize.Property.Variant;
using System.Collections;

namespace NeoWZ.Serialize.Property
{
    [ComClass("Property")]
    public class WzProperty : WzSerializable, IEnumerable<WzVariant>
    {
        private List<WzVariant> mItems = new();

        public WzVariant this[int index] => this.mItems[index];
        public WzVariant this[string path] {
            get {
                var section = path.Replace('\\', '/').Trim('/').Trim().Split('/', 2);
                if (section[0] == "") {
                    return new WzDispatch(this.Name, this);
                } else if (section[0] == "..") {
                    return this.Parent?.To<WzProperty>()?[section.Length == 1 ? "" : section[1]];
                }
                var item = this.mItems.FirstOrDefault(x => x.Name == section[0]);
                return section.Length == 1 ? item : item?.ToCom<WzProperty>()?[section[1]];
            }
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.WriteCompressedInt32(this.Count);
            foreach (var v in this.mItems) {
                this.WriteVariant(stream, v, serializer);
            }
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            this.mItems.Clear();

            stream.ReadByte(); // ascii code for plain text property
            stream.ReadByte();
            var count = stream.ReadCompressedInt32();
            for (int i=0;i<count;++i) {
                var variant = this.ReadVariant(stream, serializer);
                this.mItems.Add(variant);
            }
        }

        public int Count => this.mItems.Count;
        public void Add(WzVariant item) => this.mItems.Add(item);
        public void Clear() => this.mItems.Clear();
        public bool Contains(WzVariant item) => this.mItems.Contains(item);
        public bool Remove(WzVariant item) => this.mItems.Remove(item);

        private WzVariant ReadVariant(WzStream stream, ComSerializer serializer) {
            var name = stream.StringPool.Read(0, 1);
            var type = (VariantType)stream.ReadByte();
            var variant = WzVariantFactory.CreateByType(type, name);
            variant.Deserialize(stream, serializer);
            return variant;
        }

        private void WriteVariant(WzStream stream, WzVariant variant, ComSerializer serializer) {
            stream.StringPool.Write(variant.Name, 0, 1);
            stream.WriteByte((byte)variant.Type);
            variant.Serialize(stream, serializer);
        }

        public override WzSerializable Clone() {
            var prop = new WzProperty() { Name = this.Name };
            prop.mItems.AddRange(this.mItems.Select(v => v.Clone()));

            return prop;
        }

        public IEnumerator<WzVariant> GetEnumerator() => this.mItems.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.mItems.GetEnumerator();
    }
}
