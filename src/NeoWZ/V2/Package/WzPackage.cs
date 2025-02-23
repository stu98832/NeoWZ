using NeoWZ.V2.IO;
using NeoWZ.V2.Utils;
using System.Text;

namespace NeoWZ.V2.Package;

public class WzPackage : IDisposable
{
    private const string SIGN = "PKG1";

    public static bool Validate(string filename) {
        using var stream = File.OpenRead(filename);
        return Validate(stream);
    }

    public static bool Validate(Stream stream) {
        var pos = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);
        var buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        var sign = Encoding.ASCII.GetString(buffer);
        stream.Seek(pos, SeekOrigin.Begin);
        return sign == SIGN;
    }

    public WzPackageHeader Header { get; private set; } = null;
    public string GameVersion { get; set; } = "";
    private List<WzEntry> entries = new();
    public IReadOnlyList<WzEntry> Entries => this.entries;

    private Stream inputStream;

    private WzPackage(Stream stream) {
        this.inputStream = stream;
    }

    public static WzPackage Open(string path, string version = "") {
        var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        return Open(stream, version);
    }

    public static WzPackage Open(Stream stream, string version = "") {
        var package = new WzPackage(stream);
        package.GameVersion = version;
        package.Read(stream);
        return package;
    }

    private void Read(Stream stream) {
        WzStreamReader reader = new(stream);
        if (!ReadHeader(reader)) {
            throw new InvalidDataException("Invalid package file.");
        }

        stream.Seek(this.Header.DataOffset, SeekOrigin.Begin);
        this.ReadBlock(reader);
    }

    private bool ReadHeader(WzStreamReader reader) {
        var sign = Encoding.ASCII.GetString(reader.ReadBytes(4));
        if (!sign.Equals(SIGN)) {
            return false;
        }

        long fileSize = reader.ReadInt64();
        uint dataOffset = reader.ReadUInt32();
        long descLength = dataOffset - reader.BaseStream.Position;
        string desc = Encoding.ASCII.GetString(reader.ReadBytes((int)descLength));

        this.Header = new WzPackageHeader(fileSize, dataOffset, desc);
        return true;
    }

    private void ReadBlock(WzStreamReader reader) {
        List<WzEntry> entries = new();

        int versionHash = HashUtils.StringHash(this.GameVersion);
        ushort hash = reader.ReadUInt16();
        if (HashUtils.XorHash(versionHash) != hash) {
            throw new InvalidDataException("Invalid version.");
        }

        int count = reader.ReadCompressedInt32();
        for (var i = 0; i < count; ++i) {
            WzEntry entry = this.ReadEntry(reader, versionHash);
            entries.Add(entry);
        }

        // Read all sub entries
        Queue<WzEntry> queue = new(entries.Where(x => x.Type == WzEntryType.Directory));
        while (queue.Count > 0) {
            WzEntry parent = queue.Dequeue();
            reader.BaseStream.Seek(this.Header.DataOffset + parent.Offset, SeekOrigin.Begin);
            count = reader.ReadCompressedInt32();
            for (var i = 0; i < count; ++i) {
                WzEntry entry = this.ReadEntry(reader, hash);
                entry.Parent = parent;
                parent?.Children.Add(entry);
                if (entry.Type == WzEntryType.Directory) {
                    queue.Enqueue(entry);
                }
            }
        }

        this.entries.AddRange(entries);
    }

    private WzEntry ReadEntry(WzStreamReader reader, int versionHash) {
        WzEntryType type = (WzEntryType)reader.ReadByte();
        string name;
        if (type == WzEntryType.Reference) {
            uint refer = reader.ReadUInt32();
            long origin = reader.BaseStream.Position;

            reader.BaseStream.Seek(refer, SeekOrigin.Begin);
            type = (WzEntryType)reader.ReadByte();
            name = reader.ReadSerializeString();
            reader.BaseStream.Seek(origin, SeekOrigin.Begin);
        } else {
            name = reader.ReadSerializeString();
        }

        int size = reader.ReadCompressedInt32();
        int checksum = reader.ReadCompressedInt32();
        uint offsetKey = WzPackageUtils.GetOffsetKey((uint)reader.BaseStream.Position - this.Header.DataOffset, versionHash);
        uint offset = this.Header.DataOffset + (reader.ReadUInt32() ^ offsetKey);

        if (offset + size > this.Header.FileSize) {
            throw new InvalidDataException("Invalid entry.");
        }

        WzEntry entry = new() {
            Package = this,
            Type = type,
            Name = name,
            Size = size,
            Checksum = checksum,
            Offset = offset
        };
        return entry;
    }

    public MemoryStream CreateMemoryStream(WzEntry entry) {
        if (entry.Package != this) {
            throw new ArgumentException("Entry does not belong to this package.");
        } else if (entry.Type != WzEntryType.Archive) {
            throw new ArgumentException("Only archive entry can be read as stream.");
        }
        this.inputStream.Seek(this.Header.DataOffset + entry.Offset, SeekOrigin.Begin);
        var buffer = new byte[entry.Size];
        this.inputStream.Read(buffer, 0, entry.Size);
        return new MemoryStream(buffer);
    }

    public PartialStream CreatePartialStream(WzEntry entry) {
        if (entry.Package != this) {
            throw new ArgumentException("Entry does not belong to this package.");
        } else if (entry.Type != WzEntryType.Archive) {
            throw new ArgumentException("Only archive entry can be read as stream.");
        }
        return new PartialStream(this.inputStream, this.Header.DataOffset + entry.Offset, entry.Size);
    }

    public void Dispose() {
        this.inputStream?.Dispose();
    }
}
