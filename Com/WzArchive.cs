using NeoMS.Framework.Utils;
using System.IO;

namespace NeoMS.Wz.Com
{
    /// <summary>  </summary>
    public class WzArchive : WzPackageItem, IWzArchive
    {
        /// <summary> </summary>
        public byte[] IV { get; private set; }

        /// <summary> </summary>
        public Stream Stream { get; internal set; }

        /// <summary> </summary>
        public WzArchive(string name, byte[] iv = null) : base(name, WzPackageItemType.File) {
            this.IV = iv;
            this.Stream = new MemoryStream();
        }

        /// <summary> </summary>
        public bool Dump(string path) {
            if (File.Exists(path) || this.Stream == null) {
                return false;
            }
            var org = this.Stream.Position;
            try {
                this.Stream.Position = this.Offset;
                using (var fp = File.OpenWrite(path)) {
                    for (int i = 0; i < this.Size; i += 4096) {
                        var buffer = new byte[4096];
                        this.Stream.Read(buffer, 0, 4096);
                        fp.Write(buffer, 0, Math.Min(this.Size - i, 4096));
                    }
                }
            } catch(Exception ex) {
                return false;
            } finally {
                this.Stream.Position = org;
            }
            return true;
        }

        /// <summary> </summary>
        public static WzArchive FromFile(string path, byte[] iv = null) {
            WzArchive file = new WzArchive(Path.GetFileName(path), iv);

            file.Stream = File.OpenRead(path);
            file.Size = (int)file.Stream.Length;

            return file;
        }

        /// <summary>  </summary>
        public override void Update() {
            if (this.Stream != null) {
                this.Checksum = (int)HashTool.GenerateChecksum(this.Stream, this.Offset, this.Size);
            }
        }

        /// <summary>  </summary>
        public override void Write(IWzFileStream zs) {
            uint newoff = (uint)zs.Tell();

            if (this.Stream != null) {
                zs.WriteDataFromStream(this.Stream, this.Offset, this.Size);
            }

            this.Offset = newoff;
        }

        /// <summary>  </summary>
        public override void Read(IWzFileStream zf) {

        }
    }
}
