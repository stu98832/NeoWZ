using NeoMS.Wz.Com.Variant;

namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public class WzUOL : WzSerialize, IWzUOL
    {
        /// <summary> </summary>
        public override string ClassName => "UOL";

        /// <summary> </summary>
        public byte Unknow1_Byte { get; private set; }

        /// <summary> </summary>
        public string Path { get; set; }

        /// <summary> </summary>
        public WzUOL(string name, string path = null) :
            base(name) {
            this.Unknow1_Byte = 0;
            this.Path = path;
        }

        /// <summary> </summary>
        public WzVariant GetVariant() {
            string[] names = this.Path.Split('/');
            IWzProperty parent = this.Parent as IWzProperty;
            int i;

            for (i = 0; i < names.Length - 1; ++i) {
                if (parent == null) {
                    break;
                }
                if (names[i] == "..") {
                    parent = parent.Parent as IWzProperty;
                }
                else {
                    parent = parent[names[i]].ToDispatch() as IWzProperty;
                }
            }

            return parent == null ? WzVariant.Null : parent[names[i]];
        }

        /// <summary> </summary>
        public void LinkVariant(WzVariant variant) {
            string path = "";
            IWzSerialize myparent = this.Parent;
            while (myparent != null) {
                string path2 = path;
                IWzSerialize vparent = variant.Parent;
                while (vparent != null) {
                    if (myparent == vparent) {
                        this.Path = path2 + variant.Name;
                        return;
                    }

                    path2 = vparent.Name + "/" + path2;
                    vparent = vparent.Parent;
                }
                path = "../" + path;
                myparent = myparent.Parent;
            }
            this.Path = "";
            //throw new Exception("Can't find variant '" + v.Name + "' in image file");
        }

        /// <summary> </summary>
        public override IWzSerialize Clone() {
            WzUOL uol = new WzUOL(this.Path);

            uol.Unknow1_Byte = this.Unknow1_Byte;

            return uol;
        }

        /// <summary> </summary>
        public override void Dispose() {
            this.Path = null;
            base.Dispose();
        }

        /// <summary> </summary>
        public override void Read(IWzFileStream stream) {
            this.Unknow1_Byte = stream.ReadByte();
            this.Path = stream.ReadSerializeString();
        }

        /// <summary> </summary>
        public override void Write(IWzFileStream stream) {
            stream.WriteByte(this.Unknow1_Byte);
            stream.WriteSerializeString(this.Path, 0, 1);
        }

    }
}
