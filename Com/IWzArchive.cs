using System.IO;

namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Package Archive File </summary>
    public interface IWzArchive : IWzPackageItem
    {
        /// <summary> Get key type of this file </summary>
        byte[] IV { get; }

        /// <summary> Get <see cref="System.IO.Stream"/> of this file </summary>
        Stream Stream { get; }

        bool Dump(string path);
    }
}
