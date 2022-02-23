using NeoMS.Wz.Com.Variant;
using NeoMS.Wz.Utils;
using System;

namespace NeoMS.Wz.Com
{
    /// <summary>  </summary>
    public class WzImage : IWzImage
    {
        /// <summary>  </summary>
        public IWzSerialize Data { get; private set; }

        /// <summary>  </summary>
        public WzImage(IWzSerialize data = null) {
            if (data != null) {
                data.ImageFile = this;
            }
            this.Data = data;
        }

        /// <summary>  </summary>
        public void Read(IWzFileStream stream) {
            stream.Seek(0, true);
            stream.ClearStringPool();
            this.Data = WzSerializeFactory.Create(stream.ReadSerializeString());
            this.Data.ImageFile = this;
            this.Data.Read(stream);
        }

        /// <summary>  </summary>
        public void Write(IWzFileStream stream) {
            stream.ClearStringPool();
            stream.WriteSerializeString(this.Data.ClassName, 0x73, 0x1B);
            this.Data.Write(stream);
        }

        /// <summary>  </summary>
        public WzImage Clone() {
            WzImage img = new WzImage();

            img.Data = this.Data.Clone() as WzSerialize;
            return img;
        }

        /// <summary>  </summary>
        public void Dispose() {
            this.Data.Dispose();
            this.Data = null;

            GC.SuppressFinalize(this);

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        /// <summary> </summary>
        public WzVariant this[string path] {
            get {
                WzProperty obj = this.Data as WzProperty;

                return obj == null ? WzVariant.Null : obj[path];
            }
        }

        /// <summary>  </summary>
        public WzArchive ToWzFile(string name, byte[] iv = null) {
            WzArchive file = new WzArchive(name, iv);
            WzFileStream stream = new WzFileStream(file.Stream, iv);

            this.Write(stream);
            file.Size = (int)stream.Length;
            stream.Dispose(false);

            return file;
        }

        /// <summary>  </summary>
        public static WzImage FromWzFile(IWzArchive file, bool dynamic = false) {
            if (file == null) {
                return null;
            }

            WzImage img = new WzImage();
            WzFileStream fs = new WzFileStream(file.Stream, file.IV);

            fs.BaseOffset = file.Offset;
            fs.Seek(file.Offset);
            fs.DynamicRead = dynamic;
            img.Read(fs);

            if (!dynamic) {
                fs.Dispose(false);
            }
            return img;
        }

        /// <summary>  </summary>
        public static WzImage FromFile(string path, bool dynamic = false, byte[] iv = null) {
            if (path == null) {
                return null;
            }

            WzImage img = new WzImage();
            WzFileStream fs = new WzFileStream(File.OpenRead(path), iv);

            fs.BaseOffset = 0;
            fs.DynamicRead = dynamic;
            img.Read(fs);

            if (!dynamic) {
                fs.Dispose(false);
            }
            return img;
        }
    }
}
