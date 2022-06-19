using NeoWZ.Extensions;
using NeoWZ.Com;
using System.Collections;

namespace NeoWZ.Serialize.Shape2D
{
    [ComClass("Shape2D#Convex2D")]
    public class WzConvex2D : WzComBase, IEnumerable<WzVector2D>
    {
        private List<WzVector2D> mVertices = new();

        public int Count => this.mVertices.Count;
        public WzVector2D this[int index] => this.mVertices[index];

        public void Add(WzVector2D v) => this.mVertices.Add(v);

        public override WzComBase Clone() {
            var convex = new WzConvex2D() { Name = this.Name };
            convex.mVertices.AddRange(this.mVertices.Select(x => x.Clone() as WzVector2D));
            return convex;
        }

        public override void Dispose() {
            foreach (var obj in this.mVertices) {
                obj.Dispose();
            }
            base.Dispose();
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteCompressedInt32(this.Count);
            foreach (var v in this.mVertices) {
                serializer.Serialize(stream, v);
            }
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            var count = stream.ReadCompressedInt32();
            for (int i = 0; i < count; ++i) {
                var vec = serializer.Deserialize<WzVector2D>(stream);
                vec.Name = i.ToString();
                vec.Parent = this;
                this.mVertices.Add(vec);
            }
        }

        public IEnumerator<WzVector2D> GetEnumerator() => this.mVertices.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.mVertices.GetEnumerator();
    }
}
