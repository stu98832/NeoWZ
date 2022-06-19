using NeoWZ.Extensions;
using NeoWZ.Maths;
using NeoWZ.Security;
using System.Collections;
namespace NeoWZ
{
    public class WzDirectory : WzEntry, IEnumerable<WzEntry>
    {
        public List<WzEntry> Entries { get; } = new();

        public WzPackage Package { get; set; }
        public override bool IsDirectory => true;

        internal void Read(WzStream stream) {
            stream.Seek(this.Offset, SeekOrigin.Begin);

            var count = stream.ReadCompressedInt32();
            for (var i = 0; i < count; ++i) {
                byte type;
                var name = stream.StringPool.ReadForDirectory(2, out type);
                var size = stream.ReadCompressedInt32();
                var checksum = stream.ReadCompressedInt32();
                var offKey = WzHash.OffsetHash((uint)stream.Position, this.Package.VersionHash);
                var offset = WzSecurity.DecryptOffset(stream.ReadUInt32(), this.Package.Offset, offKey);

                if ((offset + size) > stream.Length) {
                    throw new InvalidDataException();
                }

                if (type == 3) {
                    var sub = new WzDirectory() {
                        Name = name,
                        Size = size,
                        Checksum = checksum,
                        Offset = offset,
                        Parent = this,
                        Package = this.Package
                    };
                    this.Add(sub);
                } else if (type == 4) {
                    var sub = new WzArchive() {
                        Name = name,
                        Size = size,
                        Checksum = checksum,
                        Offset = offset,
                        Parent = this,
                        Stream = new SectionStream(stream.Base, offset, size)
                    };
                    this.Add(sub);
                } else {
                    throw new InvalidDataException($"Invalid type {type}");
                }
            }

            foreach (var sub in this.Entries.Where(x => x.IsDirectory).Select(x => x as WzDirectory)) {
                sub.Read(stream);
            }
        }

        internal void Write(WzStream stream) {
            // TODO: 
        }

        public int Count => this.Entries.Count;
        public void Add(WzEntry item) => this.Entries.Add(item);
        public void Clear() => this.Entries.Clear();
        public bool Contains(WzEntry item) => this.Entries.Contains(item);
        public bool Remove(WzEntry item) => this.Entries.Remove(item);

        public WzEntry this[int index] => this.Entries[index];
        public WzEntry this[string path] {
            get {
                var segments = path.Replace('\\', '/').Trim('/').Trim().Split('/', 2);
                var parent = this.Entries.FirstOrDefault(x => x.Name == segments[0]);
                return
                    segments[0] == "" ? this :
                    segments.Length == 1 ? parent : 
                    parent?.To<WzDirectory>()?[segments[1]];
            }
        }

        public IEnumerator<WzEntry> GetEnumerator() => this.Entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Entries.GetEnumerator();
    }
}
