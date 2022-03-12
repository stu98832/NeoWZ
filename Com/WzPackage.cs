using NeoMS.Wz.Com.Variant;
using NeoMS.Wz.Utils;
using System.IO;

namespace NeoMS.Wz.Com
{
    /// <summary>  </summary>
    public class WzPackage : IWzPackage
    {
        /// <summary>  </summary>
        public long DataSize { get; private set; }

        /// <summary>  </summary>
        public uint DataOffset { get; private set; }

        /// <summary> 文件描述 </summary>
        public string Description { get; set; }

        /// <summary> 根目錄 </summary>
        public IWzDirectory RootDirectory => this.mRoot;

        /// <summary> 建立一個<see cref="WzPackage"/>實體 </summary>
        public WzPackage() {
            this.Init(new WzDirectory(""));
        }

        /// <summary> 建立一個連接指定<see cref="WzDirectory"/>的<see cref="WzPackage"/>實體 </summary>
        /// <param name="link"> 要進行連結的<see cref="WzDirectory"/>實例 </param>
        public WzPackage(WzDirectory link) {
            this.Init(link);
        }

        /// <summary> 文件雜湊碼 </summary>
        public int Hash { get; private set; }

        /// <summary> 文件版本 </summary>
        public int Version { get; set; }

        /// <summary> 文件資料流 </summary>
        public WzFileStream Stream { get; set; }

        /// <summary></summary>
        public IWzPackageItem this[string path] {
            get {
                path = path.Replace('\\', '/');

                int index = path.IndexOf("/"), i = 0;

                if (index == -1) {
                    return this.RootDirectory[path];
                }

                string linkName = path.Substring(0, index);
                string linkPath = path.Substring(index + 1);
                IWzPackageItem obj = null;

                do {
                    IWzDirectory zcLink = this.RootDirectory[linkName + (++i == 1 ? "" : i.ToString())] as IWzDirectory;
                    if (zcLink == null) {
                        return null;
                    }
                    obj = zcLink[linkPath];
                } while (obj == null);

                return obj;
            }
        }

        /// <summary> 從指定路徑建立<see cref="WzStream"/>並讀取資料 </summary>
        public bool Open(string path, byte[] iv) {
            this.Stream = new WzFileStream(path, FileMode.Open, iv);
            return this.Read(this.Stream);
        }

        /// <summary> 儲存整個<see cref="WzPackage"/>並建立.wz檔案 </summary>
        public bool Save(string path, byte[] iv) {
            IWzFileStream stream = new WzFileStream(path, FileMode.Create, iv);
            return this.Write(stream);
        }

        /// <summary> 使用自身的<see cref="WzStream"/>讀取資料 </summary>
        public bool Read() {
            return this.Read(this.Stream);
        }

        /// <summary> 使用外部的<see cref="WzStream"/>讀取資料 </summary>
        public bool Read(IWzFileStream stream) {
            string identifier = stream.ReadString(4);

            if (!identifier.Equals("PKG1")) {
                return false;
            }

            this.DataSize = stream.ReadInt64();
            this.DataOffset = stream.ReadUInt32();
            this.Description = stream.ReadString((int)(this.DataOffset - stream.Tell()));

            stream.BaseOffset = this.DataOffset;

            ushort cryptedHash = stream.ReadUInt16();

            this.mRoot.Offset = (uint)stream.Tell();
            for (int j = 0; j < 0xFFFF; j++) {
                this.Hash = HashTool.GeneratePackageVersionHash(j.ToString());
                if (HashTool.EncryptPackageVersionHash(this.Hash) == cryptedHash) {
                    stream.ClearStringPool();
                    try {
                        this.mRoot.Read(stream);
                        this.Version = j;
                        return true;
                    }
                    catch (InvalidDataException) { }
                }
            }

            return false;
        }

        /// <summary> 使用自身的<see cref="WzStream"/>儲存資料 </summary>
        public bool Write() {
            return this.Write(this.Stream);
        }

        /// <summary> 使用外部的的<see cref="WzStream"/>儲存資料 </summary>
        public bool Write(IWzFileStream zs) {
            this.Hash = HashTool.GeneratePackageVersionHash(this.Version.ToString());

            // header
            zs.Write(new byte[] { (byte)'P', (byte)'K', (byte)'G', (byte)'1' }, 4);
            zs.WriteInt64(0);//Reserve
            zs.WriteUInt32(0);//Reserve
            zs.WriteString(this.Description);

            // data
            long off = zs.Tell();
            this.DataOffset = (uint)off;
            zs.BaseOffset = this.DataOffset;

            zs.WriteUInt16((ushort)HashTool.EncryptPackageVersionHash(this.Hash));
            this.mRoot.Update();
            this.mRoot.Write(zs);

            long endoff = zs.Tell();

            // rewrite size, offset
            zs.Seek(4);
            zs.WriteInt64(endoff - off);
            zs.WriteUInt32((uint)off);

            // end write
            zs.Flush();

            return true;
        }

        /// <summary> 透過指定路徑取得對應的<see cref="WzVariant"/> </summary>
        /// <param name="path"> <see cref="WzVariant"/>的所在位置 </param>
        /// <param name="img"> 尋找時所開啟的<see cref="WzImage"/>物件 </param>
        public WzVariant GetVariant(string path, ref WzImage img) {
            WzArchive file = this[WzTool.GetImageFilePath(path)] as WzArchive;
            if (file != null) {
                img = WzImage.FromWzFile(file);
                return img[WzTool.GetPropertyPath(path)];
            }
            return WzVariant.Null;
        }

        /// <summary> 取得指定路徑的<see cref="WzVariant"/>拷貝 </summary>
        /// <param name="path"> <see cref="WzVariant"/>的所在位置 </param>
        public WzVariant CloneVariant(string path) {
            WzImage img = null;
            WzVariant v = GetVariant(path, ref img).Clone();
            if (img != null) {
                img.Dispose();
            }
            return v;
        }

        private void Init(WzDirectory root) {
            root.Archive = this;
            this.Description = "\0";
            this.Version = 0;
            this.mRoot = root;
        }

        /// <summary> </summary>
        public WzPackage LinkArchive(string linkname, string archiveFullPath, byte[] iv) {
            WzDirectory import = this.RootDirectory[linkname] as WzDirectory;
            WzPackage archive = null;
            if (import != null) {
                archive = new WzPackage(import);
                archive.Stream = new WzFileStream(archiveFullPath, FileMode.Open, iv);
                archive.Read();
            }
            return archive;
        }

        private WzDirectory mRoot;
    }
}
