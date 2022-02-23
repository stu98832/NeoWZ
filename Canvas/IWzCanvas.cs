using NeoMS.Wz.Com;
using NeoMS.Wz.Com.Variant;

namespace NeoMS.Wz.Canvas
{
    /// <summary>  </summary>
    public interface IWzCanvas : IWzSerialize
    {
        /// <summary>  </summary>
        IWzProperty CanvasProperty { get; }

        /// <summary>  </summary>
        int Width { get; }

        /// <summary>  </summary>
        int Height { get; }

        /// <summary>  </summary>
        WzCanvasFormat Format { get; }

        /// <summary>  </summary>
        byte Scale { get; set; }

        /// <summary>  </summary>
        int DataSize { get; }

        /// <summary>  </summary>
        byte[] CanvasData { get; }

        /// <summary>  </summary>
        WzVariant this[string path] { get; }
    }
}
