using NeoMS.Wz.Canvas;
using NeoMS.Wz.Com;
using NeoMS.Wz.Shape2D;
using NeoMS.Wz.Sound;

namespace NeoMS.Wz.Utils
{
    /// <summary> </summary>
    class WzSerializeFactory
    {
        /// <summary> </summary>
        public static IWzSerialize Create(string classname, string name = null) {
            switch (classname) {
                case "Property":
                    return new WzProperty(name);
                case "Canvas":
                    return new WzCanvas(name);
                case "Shape2D#Vector2D":
                    return new WzVector2D(name);
                case "Shape2D#Convex2D":
                    return new WzConvex2D(name);
                case "UOL":
                    return new WzUOL(name);
                case "Sound_DX8":
                    return new WzSound(name);
                default:
                    throw new NotSupportedException("Not supported class. \nName:" + classname);
            }
        }
    }
}
