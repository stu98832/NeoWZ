using NeoMS.Wz.Com;
using NeoMS.Wz.Com.Variant;
using NeoMS.Framework.Utils;
using System.IO;

namespace NeoMS.Wz.Canvas
{
    /// <summary>  </summary>
    public class WzCanvas : WzSerialize, IWzCanvas
    {
        /// <summary>  </summary>
        public override string ClassName { get { return "Canvas"; } }

        /// <summary>  </summary>
        public byte Unknow1_Byte { get; private set; }

        /// <summary>  </summary>
        public IWzProperty CanvasProperty { get; private set; }

        /// <summary>  </summary>
        public int Width { get; private set; }

        /// <summary>  </summary>
        public int Height { get; private set; }

        /// <summary>  </summary>
        public WzCanvasFormat Format { get; private set; }

        /// <summary>  </summary>
        public byte Scale { get; set; }

        /// <summary> </summary>
        public int Unknow2_Int { get; private set; }

        /// <summary>  </summary>
        public int DataSize { get; private set; }

        /// <summary>  </summary>
        public byte[] CanvasData {
            get {
                if (this.mStream != null) {
                    this.mStream.Seek(this.mCanvasOffset);
                    return this.ResolveData(this.mStream);
                }
                return this.mCanvasData;
            }
            internal set {
                this.mCanvasData = value;
                this.mStream = null;
            }
        }

        /// <summary>  </summary>
        public WzCanvas(string name, WzCanvasFormat format = WzCanvasFormat.B8G8R8A8) : base(name) {
            this.CanvasProperty = new WzProperty(null);
            this.Unknow1_Byte = 0;
            this.Width = 0;
            this.Height = 0;
            this.Format = format;
            this.Scale = 0;
            this.Unknow2_Int = 0;
            this.DataSize = 0;
            this.mCanvasOffset = 0;
            this.mCanvasData = null;
            this.mStream = null;
        }

        /// <summary> </summary>
        public WzVariant this[string path] => this.CanvasProperty[path];

        /// <summary>  </summary>
        public override IWzSerialize Clone() {
            WzCanvas canvas = new WzCanvas(this.Name);

            canvas.Unknow1_Byte = this.Unknow1_Byte;
            canvas.Width = this.Width;
            canvas.Height = this.Height;
            canvas.Format = this.Format;
            canvas.Scale = this.Scale;
            canvas.Unknow2_Int = this.Unknow2_Int;
            canvas.DataSize = this.DataSize;

            byte[] datas = this.CanvasData;
            canvas.mCanvasData = new byte[datas.Length];
            datas.CopyTo(canvas.mCanvasData, 0);

            return canvas;
        }

        /// <summary>  </summary>
        public override void Dispose() {
            if (this.CanvasProperty != null) {
                this.CanvasProperty.Dispose();
            }
            this.mCanvasData = null;
            if (this.mStream != null) {
                this.mStream.Dispose(false);
            }
            base.Dispose();
        }

        /// <summary>  </summary>
        public override void Read(IWzFileStream stream) {
            this.Unknow1_Byte = stream.ReadByte();
            bool hasProperty = stream.ReadBool();
            if (hasProperty) {
                this.CanvasProperty.Read(stream);
            }
            this.Width = stream.ReadInt32(true);
            this.Height = stream.ReadInt32(true);
            this.Format = (WzCanvasFormat)stream.ReadInt32(true);
            this.Scale = stream.ReadByte();
            this.Unknow2_Int = stream.ReadInt32();
            this.DataSize = stream.ReadInt32();
            this.mCanvasOffset = (uint)stream.Tell();

            if (stream.DynamicRead) {
                stream.Skip(this.DataSize);
                this.mStream = stream;
            }
            else {
                this.CanvasData = this.ResolveData(stream);
            }
        }

        /// <summary>  </summary>
        public override void Write(IWzFileStream stream) {
            bool hasprop = this.CanvasProperty.Count > 0;

            stream.WriteByte(this.Unknow1_Byte);
            stream.WriteBool(hasprop);
            if (hasprop) {
                this.CanvasProperty.Write(stream);
            }
            stream.WriteInt32(this.Width, true);
            stream.WriteInt32(this.Height, true);
            stream.WriteInt32((int)this.Format, true);
            stream.WriteByte(this.Scale);
            stream.WriteInt32(this.Unknow2_Int);

            stream.WriteInt32(this.mCanvasData.Length);
            stream.Write(this.mCanvasData);
        }

        private uint mCanvasOffset;
        private byte[] mCanvasData;
        private IWzFileStream mStream;

        private byte[] ResolveData(IWzFileStream zs) {
            byte unk = zs.ReadByte();
            byte cmf = zs.ReadByte();
            byte flg = zs.ReadByte();
            zs.Skip(-3);

            if (ZlibTool.CheckDeflate(unk, cmf, flg)) {
                return zs.Read(this.DataSize);
            }
            else {
                using (MemoryStream ms = new MemoryStream()) {
                    ms.WriteByte(zs.ReadByte());
                    for (int i = 1; i < this.DataSize;) {
                        int blocksize = zs.ReadInt32();
                        ms.Write(zs.Read(blocksize, true), 0, blocksize);
                        i += blocksize + 4;
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
