namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public abstract class WzPackageItem : IWzPackageItem
    {
        /// <summary> </summary>
        public WzPackageItemType Type { get; private set; }

        /// <summary> </summary>
        public string Name { get; set; }

        /// <summary> </summary>
        public int Size { get; internal set; }

        /// <summary> </summary>
        public int Checksum { get; internal set; }

        /// <summary> </summary>
        public uint Offset { get; internal set; }

        /// <summary> </summary>
        public IWzPackage Archive { get; set; }

        /// <summary> </summary>
        public IWzDirectory Parent {
            get => this.mParent;
            set => this.mParent = value;
        }

        /// <summary> </summary>
        public WzPackageItem(string name, WzPackageItemType type) {
            this.Type = type;
            this.Name = name;
            this.Offset = 0;
            this.Checksum = 0;
            this.Size = 0;
            this.Archive = null;
            this.mParent = null;
        }

        /// <summary> </summary>
        public abstract void Update();

        /// <summary> </summary>
        public abstract void Write(IWzFileStream zs);

        /// <summary> </summary>
        public abstract void Read(IWzFileStream zf);

        private IWzDirectory mParent;
    }
}
