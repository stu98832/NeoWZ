namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Package Archive Item Interface </summary>
    public interface IWzPackageItem
    {
        /// <summary> Get type of this item </summary>
        WzPackageItemType Type { get; }
        
        /// <summary> Get or set name of this item </summary>
        string Name { get; set; }
        
        /// <summary> Get size of this item </summary>
        int Size { get; }
        
        /// <summary> Get check sum of this item </summary>
        int Checksum { get; }
        
        /// <summary> Get offset of this item </summary>
        uint Offset { get; }

        /// <summary> Get the archive which this item belong to </summary>
        IWzPackage Archive { get; set; }
        
        /// <summary> Get the parent directory of this item </summary>
        IWzDirectory Parent { get; set; }

        /// <summary> Update informations of this item </summary>
        void Update();

        /// <summary> Write data to specific file stream </summary>
        void Write(IWzFileStream zf);

        /// <summary> Read data from specific file stream </summary>
        void Read(IWzFileStream zf);
    }
}
