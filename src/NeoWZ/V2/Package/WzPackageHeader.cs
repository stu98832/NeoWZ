namespace NeoWZ.V2.Package;

public record WzPackageHeader(
    long FileSize,
    uint DataOffset,
    string Description)
{
    public const string HEADER = "PKG1";
}