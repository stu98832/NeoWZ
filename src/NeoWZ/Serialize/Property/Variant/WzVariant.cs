using NeoWZ.Com;

namespace NeoWZ.Serialize.Property.Variant
{
    /// <summary>
    /// Variant object for WZ
    /// </summary>
    public abstract class WzVariant : ComVariant
    {
        /// <summary>
        /// Gets or sets parent of this variant
        /// </summary>
        public virtual WzComBase Parent { get; set; } = null;

        /// <summary>
        /// Gets or sets name of this variant
        /// </summary>
        public string Name { get; set; }

        public WzVariant(string name) {
            this.Name = name;
        }

        /// <summary>
        /// Clone current variant
        /// </summary>
        /// <returns></returns>
        public abstract WzVariant Clone();

        public override bool Equals(object obj) =>
            base.Equals(obj) && this.Name == (obj as WzVariant).Name;

        public override int GetHashCode() => base.GetHashCode();

        public virtual void Deserialize(WzStream stream, ComSerializer serializer) {
        }

        public virtual void Serialize(WzStream stream, ComSerializer serializer) {
        }
    }
}
