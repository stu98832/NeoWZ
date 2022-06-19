namespace NeoWZ.Com
{
    /// <summary>
    /// PCom class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ComClassAttribute : Attribute
    {
        public string ClassName { get; }

        /// <summary></summary>
        /// <param name="className">Class name of PCom object</param>
        public ComClassAttribute(string className) {
            this.ClassName = className;
        }
    }
}
