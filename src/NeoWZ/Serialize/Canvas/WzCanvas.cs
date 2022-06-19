using NeoWZ.Extensions;
using NeoWZ.Com;
using NeoWZ.Serialize.Property;
using NeoWZ.Serialize.Property.Variant;
using System.Collections;

namespace NeoWZ.Serialize.Canvas
{
    [ComClass("Canvas")]
    public class WzCanvas : WzComBase, IEnumerable<WzVariant>
    {
        public virtual WzProperty Property { get; init; } = new WzProperty();
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual WzCanvasFormat Format { get; set; } = WzCanvasFormat.B8G8R8A8;
        public virtual int Scale { get; set; }
        public virtual byte[] CanvasData { get; set; }

        public WzVariant this[int index] => this.Property[index];
        public WzVariant this[string path] => this.Property[path];

        public WzCanvas() {
        }

        public override WzComBase Clone() {
            var canvas = new WzCanvas() {
                Name = this.Name,
                Width = this.Width,
                Height = this.Height,
                Format = this.Format,
                Scale = this.Scale,
                CanvasData = new byte[this.CanvasData.Length],
                Property = this.Property.Clone() as WzProperty
            };
            this.CanvasData.CopyTo(canvas.CanvasData, 0);

            return canvas;
        }

        public override void Dispose() {
            this.Property?.Dispose();
            this.CanvasData = null;
            base.Dispose();
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            stream.ReadByte(); // 0
            bool hasProperty = stream.ReadBool();
            if (hasProperty) {
                this.Property.Clear();
                this.Property.Deserialize(stream, serializer);
            }
            this.Width = stream.ReadCompressedInt32();
            this.Height = stream.ReadCompressedInt32();
            this.Format = (WzCanvasFormat)stream.ReadCompressedInt32();
            this.Scale = stream.ReadCompressedInt32();
            for (int i = 0; i < 4; ++i) {
                stream.ReadCompressedInt32(); // non-zero throw error in PCOM
            }
            this.CanvasData = stream.Read(stream.ReadInt32());
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteByte(0);
            bool hasprop = this.Property.Count > 0;
            stream.WriteBool(hasprop);
            if (hasprop) {
                this.Property.Serialize(stream, serializer);
            }
            stream.WriteCompressedInt32(this.Width);
            stream.WriteCompressedInt32(this.Height);
            stream.WriteCompressedInt32((int)this.Format);
            stream.WriteCompressedInt32(this.Scale);
            for (int i = 0; i < 4; ++i) {
                stream.WriteCompressedInt32(0);
            }
            stream.WriteInt32(this.CanvasData.Length);
            stream.Write(this.CanvasData);
        }

        public IEnumerator<WzVariant> GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
