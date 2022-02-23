using NeoMS.Wz.Com;
using NeoMS.Wz.Utils;
using System.Collections.Generic;

namespace NeoMS.Wz.Shape2D
{
    /// <summary> </summary>
    public class WzConvex2D : WzShape2D, IWzConvex2D
    {
        /// <summary>  </summary>
        public override string ClassName { get { return "Shape2D#Convex2D"; } }

        /// <summary>  </summary>
        public int Count { get { return this.mVertices.Count; } }

        /// <summary>  </summary>
        public IWzVector2D this[int index] { get { return this.mVertices[index]; } }

        /// <summary>  </summary>
        public WzConvex2D(string name) : base(name) {
            this.mVertices = new List<IWzVector2D>();
        }

        /// <summary>  </summary>
        public void Add(WzVector2D v) {
            this.mVertices.Add(v);
        }

        /// <summary>  </summary>
        public override IWzSerialize Clone() {
            WzConvex2D convex = new WzConvex2D(this.Name);

            foreach (IWzVector2D vec in this.mVertices) {
                convex.mVertices.Add(vec.Clone() as IWzVector2D);
            }

            return convex;
        }

        /// <summary>  </summary>
        public override void Dispose() {
            foreach (WzSerialize obj in this.mVertices) {
                obj.Dispose();
            }

            base.Dispose();
        }

        /// <summary>  </summary>
        public override void Read(IWzFileStream stream) {
            int nSize = stream.ReadInt32(true);

            for (int i = 0; i < nSize; ++i) {
                IWzVector2D vec = WzSerializeFactory.Create(stream.ReadSerializeString()) as IWzVector2D;
                vec.Read(stream);
                this.mVertices.Add(vec);
            }
        }

        /// <summary>  </summary>
        public override void Write(IWzFileStream stream) {
            int nSize = this.mVertices.Count;

            stream.WriteInt32(nSize, true);
            for (int i = 0; i < nSize; ++i) {
                stream.WriteSerializeString(this.mVertices[i].ClassName, 0x73, 0x1B);
                this.mVertices[i].Write(stream);
            }
        }

        private List<IWzVector2D> mVertices;
    }
}
