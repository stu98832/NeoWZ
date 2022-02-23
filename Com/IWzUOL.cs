using NeoMS.Wz.Com.Variant;

namespace NeoMS.Wz.Com
{
    /// <summary> Wizet Uniform Object Locator Interface </summary>
    public interface IWzUOL : IWzSerialize
    {
        /// <summary> Get or set tha path of <see cref="WzVariant"/> object which this object links to </summary>
        string Path { get; set; }

        /// <summary> Get <see cref="WzVariant"/> object which this object links to </summary>
        WzVariant GetVariant();

        /// <summary> Link to a <see cref="WzVariant"/> object </summary>
        void LinkVariant(WzVariant variant);
    }
}
