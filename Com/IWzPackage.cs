namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Package Archive Interface </summary>
    public interface IWzPackage
    {
        /// <summary> Get size of this archive </summary>
        long DataSize { get; }

        /// <summary> Get data offset of this archive </summary>
        uint DataOffset { get; }

        /// <summary> Get or set archive description </summary>
        string Description { get; set; }

        /// <summary> Get root directory of this archive </summary>
        IWzDirectory RootDirectory { get; }

        /// <summary> Get hash of this archive </summary>
        int Hash { get; }

        /// <summary> Get game version of this archive </summary>
        int Version { get; }

        /// <summary> Get a archive item from this archive's root directory </summary>
        IWzPackageItem this[string path] { get; }
    }
}
