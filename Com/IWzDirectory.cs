namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Package Archive Folder </summary>
    public interface IWzDirectory : IWzPackageItem, ICollection<IWzPackageItem>
    {
        /// <summary> Get <see cref="IWzPackageItem"/> object by specific path </summary>
        IWzPackageItem this[string path] { get; }

        /// <summary> Get <see cref="IWzPackageItem"/> object by specific index </summary>
        IWzPackageItem this[int index] { get; }
    }
}
