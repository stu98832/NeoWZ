using NeoWZ.Maths;
using System.Collections;
using System.Text;

namespace NeoWZ
{
    public class WzPackage : IEnumerable<WzEntry>
    {
        protected WzStream mStream;

        public string Description { get; set; }
        public long Size { get; set; }
        public uint Offset { get; set; }
        public int VersionHash { get; set; }
        public int Version { get; set; } = 0;
        public WzDirectory Root { get; protected set; }

        public string Name {
            get => this.Root.Name; 
            set => this.Root.Name = value;
        }
        public WzEntry this[string path] => this.Root[path];
        public WzEntry this[int index] => this.Root[index];

        public WzPackage() {
            this.Root = new WzDirectory() {
                Package = this
            };
        }

        public static WzPackage Open(string path, byte[] iv = null, int version = 0) {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var package = new WzPackage() {
                Version = version,
                Name = Path.GetFileName(path),
                mStream = new WzStream(stream, iv)
            };
            package.Read(package.mStream);
            return package;
        }

        public void Save(string path, byte[] iv) {
            var dir = Path.GetDirectoryName(path);
            var name = Path.GetFileName(path);
            var temp = $"{dir}/~{name}";
            if (name == "") {
                throw new ArgumentException("Name could not be empty");
            }

            using (var stream = new WzStream(File.Create(temp), iv)) {
                this.Write(stream);
                stream.Flush();
            }
            File.Move(temp, path, true);

            // reopen stream
            this.mStream?.Dispose();
            this.mStream = new WzStream(File.OpenRead(path), iv);
            this.Read(this.mStream);
        }

        internal void Read(WzStream stream) {
            stream.Base.Seek(0, SeekOrigin.Begin);
            var sign = Encoding.ASCII.GetString(stream.Read(4));

            if (!sign.Equals("PKG1")) {
                throw new InvalidDataException("Invalid package file");
            }

            this.Size = stream.ReadInt64();
            this.Offset = stream.ReadUInt32();
            this.Description = Encoding.ASCII.GetString(stream.Read((int)(this.Offset - stream.Position)));

            ushort key = stream.ReadUInt16();

            this.Root.Offset = 2; // skip key
            var sectionStream = new WzStream(new SectionStream(this.mStream.Base, this.Offset), this.mStream.IV);
            for (int version = this.Version; version < 0xFFFF; version++) {
                this.VersionHash = WzHash.VersionHash(version.ToString());
                if (WzHash.PackageHash(this.VersionHash) == key) {
                    try {
                        sectionStream.StringPool.Clear();
                        this.Root.Read(sectionStream);
                        this.Version = version;
                        return;
                    } catch (InvalidDataException) {
                    }
                }
            }
            throw new InvalidDataException("Package corrupted");
        }

        internal void Write(WzStream stream) {

        }

        public IEnumerator<WzEntry> GetEnumerator() => this.Root.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Root.GetEnumerator();
    }
}
