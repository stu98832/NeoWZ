using NeoMS.Wz.Com.Variant;
using System;

namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Image Interface </summary>
    public interface IWzImage : IDisposable
    {
        /// <summary> Get serialize object of this image file </summary>
        IWzSerialize Data { get; }

        /// <summary> Get <see cref="WzVariant"/> object from this image file by specific path </summary>
        WzVariant this[string path] { get; }
    }
}
