using NeoMS.Wz.Com;
using System.ComponentModel;

namespace NeoMS.Wz.Shape2D
{
    /// <summary> </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WzVector2D : WzShape2D, IWzVector2D
    {
        /// <summary> </summary>
        public override string ClassName { get { return "Shape2D#Vector2D"; } }

        /// <summary> </summary>
        public int X { get; set; }

        /// <summary> </summary>
        public int Y { get; set; }

        /// <summary> </summary>
        public WzVector2D(string name) : this(name, 0, 0) { }

        /// <summary> </summary>
        public WzVector2D(string name, int x, int y) :
            base(name) {
            this.X = x;
            this.Y = y;
        }

        /// <summary> </summary>
        public override IWzSerialize Clone() {
            return new WzVector2D(this.Name, this.X, this.Y);
        }

        /// <summary> </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary> </summary>
        public override void Read(IWzFileStream stream) {
            this.X = stream.ReadInt32(true);
            this.Y = stream.ReadInt32(true);
        }

        /// <summary> </summary>
        public override void Write(IWzFileStream stream) {
            stream.WriteInt32(this.X, true);
            stream.WriteInt32(this.Y, true);
        }
    }
}
