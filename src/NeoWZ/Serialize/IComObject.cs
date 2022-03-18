namespace NeoWZ.Serialize
{
    /// <summary>
    /// Repersent a serializable PCOM object
    /// </summary>
    public interface IComObject : IDisposable
    {
        /// <summary>
        /// Convert this object to other com class which inherits from <see cref="IComObject"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="def"></param>
        /// <returns></returns>
        T To<T>() where T : class, IComObject => this as T;

        /// <summary>
        /// Serialize this object by specific serializer
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <param name="serializer">Serializer</param>
        void Serialize(WzStream stream, ComSerializer serializer);

        /// <summary>
        /// Deserialize this object by specific serializer
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <param name="serializer">Serializer</param>
        void Deserialize(WzStream stream, ComSerializer serializer);
    }
}
