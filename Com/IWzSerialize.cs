using NeoMS.Wz.Com.Variant;
using System;

namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Serialize Object Interface </summary>
    public interface IWzSerialize : IDisposable
    {
        /// <summary> Get of set the image file which this object belong to </summary>
        IWzImage ImageFile { get; set; }

        /// <summary> Get or set name of this object </summary>
        string Name { get; set; }

        /// <summary> Get or set parent serialize object of this object </summary>
        IWzSerialize Parent { get; set; }

        /// <summary> Get the class name of this object </summary>
        string ClassName { get; }

        /// <summary> convert this object to <see cref="WzVariant"/> </summary>
        WzVariant ToVariant();

        /// <summary> Clone this object </summary>
        IWzSerialize Clone();

        /// <summary> Write data to specific file stream </summary>
        void Write(IWzFileStream stream);

        /// <summary> Read data from specific file stream </summary>
        /// <param name="stream"></param>
        void Read(IWzFileStream stream);
    }
}
