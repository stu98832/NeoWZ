using NeoMS.Wz.Com;

namespace NeoMS.Wz.Shape2D
{
    /// <summary>  </summary>
    public abstract class WzShape2D : WzSerialize, IWzShape2D
    {
        /// <summary>  </summary>
        public override string ClassName { get { return "Shape2D"; } }

        /// <summary>  </summary>
        public WzShape2D(string name) : base(name) { }

        /// <summary>  </summary>
        public abstract override IWzSerialize Clone();

        /// <summary>  </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary>  </summary>
        public abstract override void Read(IWzFileStream stream);

        /// <summary>  </summary>
        public abstract override void Write(IWzFileStream stream);
    }
}