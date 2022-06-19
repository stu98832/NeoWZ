using NeoWZ.Extensions;
using NeoWZ.Com;
using System.Numerics;

namespace NeoWZ.Serialize.Shape2D
{
    [ComClass("Shape2D#Vector2D")]
    public class WzVector2D : WzComBase
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2 Vector => new Vector2(this.X, this.Y);

        public override WzComBase Clone() => new WzVector2D() {
            X = this.X, 
            Y = this.Y
        };

        public override void Serialize(WzStream stream, ComSerializer serailizer) {
            stream.WriteCompressedInt32(this.X);
            stream.WriteCompressedInt32(this.Y);
        }

        public override void Deserialize(WzStream stream, ComSerializer serailizer) {
            this.X = stream.ReadCompressedInt32();
            this.Y = stream.ReadCompressedInt32();
        }
    }
}
