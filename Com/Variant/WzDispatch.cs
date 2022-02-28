using NeoMS.Wz.Utils;

namespace NeoMS.Wz.Com.Variant
{
    /// <summary> </summary>
    public class WzDispatch : WzVariant
    {
        /// <summary> </summary>
        public override IWzProperty Parent {
            get {
                if (this.Value != null) {
                    return this.Value.Parent as IWzProperty;
                }

                return this.mParent;
            }
            set {
                if (this.Value != null) {
                    this.Value.Parent = value;
                }

                this.mParent = value;
            }
        }

        /// <summary> </summary>
        public IWzSerialize Value { get; set; }

        /// <summary> </summary>
        public WzDispatch(string name, IWzSerialize value = null) :
            base(WzVariantType.Dispatch, name) {
            this.Value = value;
        }

        public override bool ToBool(bool def = false) => this.Value != null;

        /// <summary> </summary>
        public override string ToString() => this.Value?.ToString();

        /// <summary> </summary>
        public override bool Equals(WzVariant obj) => this.Value.Equals((obj as WzDispatch).Value);

        /// <summary> </summary>
        public override IWzSerialize ToDispatch(IWzSerialize def = null) => this.Value ?? def;

        /// <summary> </summary>
        public override WzVariant Clone() => new WzDispatch(this.Name, this.Value.Clone());

        /// <summary> </summary>
        internal override void Read(IWzFileStream fs) {
            int blockSize = fs.ReadInt32();
            long off = fs.Tell();

            string classname = fs.ReadSerializeString();
            IWzSerialize obj = WzSerializeFactory.Create(classname, this.Name);
            obj.ImageFile = this.Parent.ImageFile;
            obj.Read(fs);

            if (fs.Tell() != (off + blockSize)) {
                fs.Seek(off + blockSize);
                // TODO: throw error?
            }

            this.Value = obj;
        }

        /// <summary> </summary>
        internal override void Write(IWzFileStream fs) {
            base.Write(fs);
            fs.WriteUInt32(0); // Size Reserve

            long startBlock = fs.Tell();
            fs.WriteSerializeString(this.Value.ClassName, 0x73, 0x1B);
            this.Value.Write(fs);
            long endBlock = fs.Tell();

            fs.Seek(startBlock - 4);
            fs.WriteUInt32((uint)(endBlock - startBlock));
            fs.Seek(endBlock);
        }
    }
}