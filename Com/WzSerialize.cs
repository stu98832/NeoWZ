using NeoMS.Wz.Com.Variant;
using System;

namespace NeoMS.Wz.Com
{
    /// <summary>  </summary>
    public abstract class WzSerialize : IWzSerialize
    {
        /// <summary>  </summary>
        public IWzImage ImageFile { get; set; }

        /// <summary>  </summary>
        public string Name { get; set; }

        /// <summary>  </summary>
        public IWzSerialize Parent { get; set; }

        /// <summary>  </summary>
        public abstract string ClassName { get; }

        /// <summary>  </summary>
        public WzSerialize(string name) {
            this.Name = name;
            this.Parent = null;
        }

        /// <summary>  </summary>
        public WzVariant ToVariant() {
            return new WzDispatch(this.Name, this);
        }

        /// <summary>  </summary>
        public string GetImagePath() {
            IWzSerialize obj = this.Parent;
            string pathstring = "";

            if (obj != null) {
                pathstring += this.Name;
                while (obj.Parent != null) {
                    pathstring = obj.Name + "/" + pathstring;
                    obj = obj.Parent;
                }
            }
            return pathstring;
        }

        /// <summary>  </summary>
        public abstract IWzSerialize Clone();

        /// <summary>  </summary>
        public virtual void Dispose() {
            this.Name = null;
            this.Parent = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>  </summary>
        public abstract void Read(IWzFileStream stream);

        /// <summary>  </summary>
        public abstract void Write(IWzFileStream stream);
    }
}
