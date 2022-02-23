using NeoMS.Wz.Com.Variant;
using System.Collections;
using System.Collections.Generic;

namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public class WzProperty : WzSerialize, IWzProperty
    {
        /// <summary> </summary>
        public override string ClassName => "Property";

        /// <summary> </summary>
        public ushort Unknow1_UShort { get; set; }

        /// <summary> </summary>
        public WzProperty(string name) : base(name) {
            this.Unknow1_UShort = 0;
            this.mList = new List<WzVariant>();
        }

        /// <summary> </summary>
        public WzVariant this[string path] {
            get {
                IWzProperty sub = this;
                string[] names = path.Replace('\\', '/').Trim('/').Split('/');
                int i;

                for (i = 0; i < names.Length - 1 && sub != null; ++i) {
                    sub = sub[names[i]].ToDispatch() as IWzProperty;
                }

                if (sub != null) {
                    foreach (WzVariant v in sub) {
                        if (v.Name == names[i]) {
                            return v;
                        }
                    }
                }

                return WzVariant.Null;
            }
        }

        /// <summary> </summary>
        public int Count => this.mList.Count;

        /// <summary> </summary>
        public bool IsReadOnly => false;

        /// <summary> </summary>
        public WzVariant this[int index] {
            get => this.mList[index];
            set => this.mList[index] = value;
        }

        /// <summary> </summary>
        public void Add(WzVariant variant) {
            variant.Parent = this;
            this.mList.Add(variant);
        }

        /// <summary> </summary>
        public void AddRange(IEnumerable item) {
            foreach (WzVariant v in item) {
                this.Add(v);
            }
        }

        /// <summary> </summary>
        public void Clear() => this.mList.Clear();

        /// <summary> </summary>
        public bool Contains(WzVariant variant) => this.mList.Contains(variant);

        /// <summary> </summary>
        public void CopyTo(WzVariant[] array, int arrayIndex) => this.mList.CopyTo(array, arrayIndex);

        /// <summary> </summary>
        public IEnumerator<WzVariant> GetEnumerator() => this.mList.GetEnumerator();

        /// <summary> </summary>
        public bool Remove(WzVariant variant) => this.mList.Remove(variant);

        /// <summary> </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.mList.GetEnumerator();

        /// <summary> </summary>
        public override IWzSerialize Clone() {
            WzProperty prop = new WzProperty(this.Name);

            prop.Unknow1_UShort = this.Unknow1_UShort;

            foreach (WzVariant v in this.mList) {
                prop.Add(v.Clone());
            }

            return prop;
        }

        /// <summary> </summary>
        public override void Dispose() {
            foreach (WzVariant v in this.mList) {
                if (v.Type == WzVariantType.Dispatch) {
                    v.ToDispatch().Dispose();
                }
            }

            this.mList.Clear();
            this.mList = null;
            base.Dispose();
        }

        /// <summary> </summary>
        public override void Read(IWzFileStream stream) {
            this.mList.Clear();

            this.Unknow1_UShort = stream.ReadUInt16(); //0
            int count = stream.ReadInt32(true);

            for (int i = 0; i < count; ++i) {
                string name = stream.ReadSerializeString();
                WzVariantType type = (WzVariantType)stream.ReadByte();
                WzVariant variant = WzVariant.Create(name, type);

                if (variant != null) {
                    variant.Parent = this;
                    variant.Read(stream);
                    this.Add(variant);
                }
            }
        }

        /// <summary> </summary>
        public override void Write(IWzFileStream stream) {
            stream.WriteUInt16(0);
            stream.WriteInt32(this.mList.Count, true);

            foreach (WzVariant v in this.mList) {
                v.Write(stream);
            }
        }

        private List<WzVariant> mList;
    }
}
