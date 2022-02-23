using NeoMS.Wz.Com.Variant;
using System.Collections.Generic;

namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Property Interface </summary>
    public interface IWzProperty : IWzSerialize, ICollection<WzVariant>
    {
        /// <summary> Get <see cref="WzVariant"/> object by specific path </summary>
        WzVariant this[string path] { get; }

        /// <summary> Get <see cref="WzVariant"/> object by specific index </summary>
        WzVariant this[int index] { get; }

        /// <summary> Get the path of this object(start from image) </summary>
        string GetImagePath();
    }
}
