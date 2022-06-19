using NeoWZ.Com;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.Property.Variant;

namespace NeoWZ.Serialize.UOL
{
    [ComClass("UOL")]
    public class WzUOL : WzComBase
    {
        public string Path { get; set; } = null;

        public WzVariant Variant {
            get {
                var names = this.Path.Split('/');
                var parent = this.Parent.To<WzProperty>();

                for (var i = 1; i < names.Length; ++i) {
                    if (names[i - 1] == "..") {
                        parent = parent?.Parent.To<WzProperty>();
                    } else {
                        parent = parent?[names[i - 1]].ToCom<WzProperty>();
                    }
                }

                return parent?[names[names.Length - 1]];
            }
            set {
                string comPath = "";
                var parent = this.Parent;
                if (parent == null) {
                    throw new NullReferenceException("No parent");
                }
                while (parent != null) {
                    string subPath = value.Name;
                    var vParent = value.Parent;
                    while (vParent != null && parent != vParent) {
                        subPath = $"{vParent.Name}/{subPath}";
                        vParent = vParent.Parent;
                    }
                    comPath = "../" + comPath;
                    if (parent == vParent) {
                        this.Path = $"{comPath.Trim('/')}/{subPath}";
                        return;
                    }
                    parent = parent.Parent;
                }
                throw new ArgumentException("Variant not found in data tree");
            }
        }

        public override WzComBase Clone() => new WzUOL() {
            Path = this.Path
        };

        public override void Dispose() {
            this.Path = null;
            base.Dispose();
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteByte(0);
            stream.StringPool.Write(this.Path, 0, 1);
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            stream.ReadByte();
            this.Path = stream.StringPool.Read(0, 1);
        }
    }
}
