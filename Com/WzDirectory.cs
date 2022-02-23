using NeoMS.Framework.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NeoMS.Wz.Com
{
    /// <summary> </summary>
    public class WzDirectory : WzPackageItem, IWzDirectory
    {
        /// <summary> </summary>
        public int Count => this.mItems.Count;

        public bool IsReadOnly => ((ICollection<IWzPackageItem>)mItems).IsReadOnly;

        /// <summary> </summary>
        public WzDirectory(string name) : base(name, WzPackageItemType.Directory) {
            this.mItems = new List<IWzPackageItem>();
        }

        /// <summary> </summary>
        public IWzPackageItem this[int index] => this.mItems[index];

        /// <summary> </summary>
        public IWzPackageItem this[string path] {
            get {
                IWzDirectory dir = this;
                string[] names = path.Replace('\\', '/').Split('/');
                int i;

                for (i = 0; i < names.Length - 1 && dir != null; ++i) {
                    dir = dir[names[i]] as WzDirectory;
                }

                if (dir != null) {
                    for (int j = 0; j < dir.Count; ++j) {
                        if (dir[j].Name == names[i]) {
                            return dir[j];
                        }
                    }
                }
                return null;
            }
        }

        /// <summary> </summary>
        public void Add(IWzPackageItem item) {
            bool samename = this[item.Name] != null;
            if (!samename) {
                item.Parent = this;
                item.Archive = this.Archive;
                this.mItems.Add(item);
            }
        }

        /// <summary> </summary>
        public void Clear() {
            this.mItems.Clear();
        }

        /// <summary> </summary>
        public void Remove(IWzPackageItem item) {
            this.mItems.Remove(item);
        }

        /// <summary>  </summary>
        public override void Read(IWzFileStream zs) {
            // this.Items.Clear();
            zs.Seek(this.Offset);

            int count = zs.ReadInt32(true);
            WzPackageItem[] items = new WzPackageItem[count];

            for (int i = 0; i < count; i++) {
                WzPackageItem item = null;
                WzPackageItemType itemtype;
                string itemname = zs.ReadDirectoryString(out itemtype, WzPackageItemType.Reference);

                if (itemtype == WzPackageItemType.Directory) {
                    item = new WzDirectory(itemname);
                }
                else if (itemtype == WzPackageItemType.File) {
                    item = new WzArchive(itemname, zs.IV) { Stream = zs.BaseStream };
                }
                else {
                    throw new InvalidDataException("Undefined item type : " + (int)itemtype);
                }

                item.Size = zs.ReadInt32(true);
                item.Checksum = zs.ReadInt32(true);
                uint offKey = HashTool.GenerateOffsetKey((uint)zs.Tell(), this.Archive.DataOffset, this.Archive.Hash);
                item.Offset = HashTool.DecryptOffsetHash(zs.ReadUInt32(), offKey, this.Archive.DataOffset);

                if ((item.Offset + item.Size) > zs.Length) {
                    throw new InvalidDataException();
                }

                items[i] = item;
            }
            for (int i = 0; i < items.Length; i++) {
                this.Add(items[i]);
                if (items[i].Type == WzPackageItemType.Directory) {
                    items[i].Read(zs);
                }
            }
        }

        /// <summary>  </summary>
        public override void Write(IWzFileStream zf) {
            // start write
            int count = this.mItems.Count;
            long[] itemoff = new long[count];

            this.Offset = (uint)zf.Tell();

            zf.WriteInt32(count, true);

            // write header
            for (int i = 0; i < count; i++) {
                IWzPackageItem item = this.mItems[i];
                zf.WriteDirectoryString(item.Name, item.Type, WzPackageItemType.Reference);
                zf.WriteInt32(item.Size, true);
                zf.WriteInt32(item.Checksum, true);

                itemoff[i] = zf.Tell();
                zf.WriteUInt32(0u); // reserve
            }

            // package items
            for (int i = 0; i < count; i++) {
                this.mItems[i].Write(zf);
            }

            long endoff = zf.Tell();

            // rewrite offset
            for (int i = 0; i < count; i++) {
                zf.Seek(itemoff[i]);
                uint offKey = HashTool.GenerateOffsetKey((uint)zf.Tell(), this.Archive.DataOffset, this.Archive.Hash);
                uint encoff = HashTool.EncryptOffsetHash(this.mItems[i].Offset, offKey, this.Archive.DataOffset);
                zf.WriteUInt32(encoff);
            }

            // end write
            zf.Seek(endoff);
        }

        /// <summary>  </summary>
        public override void Update() {
            uint checksum = 0u;
            int size = 0;

            for (int i = 0; i < this.mItems.Count; i++) {
                this.mItems[i].Update();
                checksum += (uint)this.mItems[i].Checksum;
                size += this.mItems[i].Size;
            }

            this.Checksum = (int)checksum;
            this.Size = size;
        }

        public bool Contains(IWzPackageItem item) {
            return ((ICollection<IWzPackageItem>)mItems).Contains(item);
        }

        public void CopyTo(IWzPackageItem[] array, int arrayIndex) {
            ((ICollection<IWzPackageItem>)mItems).CopyTo(array, arrayIndex);
        }

        bool ICollection<IWzPackageItem>.Remove(IWzPackageItem item) {
            return ((ICollection<IWzPackageItem>)mItems).Remove(item);
        }

        public IEnumerator<IWzPackageItem> GetEnumerator() {
            return ((IEnumerable<IWzPackageItem>)mItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)mItems).GetEnumerator();
        }

        private List<IWzPackageItem> mItems;
    }
}
