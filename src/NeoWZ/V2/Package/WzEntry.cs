namespace NeoWZ.V2.Package;

public class WzEntry
{
    public WzPackage Package { get; init; }
    public WzEntryType Type { get; init; }
    public string Name { get; set; } = "";
    public int Size { get; set; } = 0;
    public int Checksum { get; set; } = 0;
    public uint Offset { get; set; } = 0;
    public WzEntry Parent { get; set; } = null;
    public List<WzEntry> Children { get; set; } = new();
}