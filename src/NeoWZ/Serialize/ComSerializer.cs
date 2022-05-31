using NeoWZ.Serialize.Attributes;
using System.Reflection;

namespace NeoWZ.Serialize
{
    /// <summary>
    /// Can serialize or deserialize any <see cref="IComObject"/> object
    /// </summary>
    public abstract class ComSerializer
    {
        /// <summary>
        /// Default <see cref="ComSerializer"/> without cipher
        /// </summary>
        public static ComSerializer Default { get; } = new WzSerializer();

        /// <summary>
        /// Create a build-in <see cref="ComSerializer"/> with cipher
        /// </summary>
        /// <param name="iv">AES iv</param>
        /// <returns>Build-in serializer</returns>
        public static ComSerializer Create(byte[] iv) => new WzSerializer(iv);

        /// <summary>
        /// AES iv
        /// </summary>
        public byte[] IV { get; protected set; }

        public ComSerializer(byte[] iv = null) {
            this.IV = iv;
        }

        /// <summary>
        /// Serialize a <see cref="IComObject"/> object to file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">File path</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(string path, T obj) where T : class, IComObject
            => this.Serialize<T>(File.OpenWrite(path), obj);

        /// <summary>
        /// Serialize a <see cref="IComObject"/> object to stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(Stream stream, T obj) where T : class, IComObject
            => this.Serialize<T>(new WzStream(stream, this.IV), obj);

        /// <summary>
        /// Serialize a <see cref="IComObject"/> object to <see cref="WzStream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(WzStream stream, T obj) where T : class, IComObject {
            var attr = obj.GetType().GetCustomAttribute<ComClassAttribute>();
            if (attr == null) {
                throw new ArgumentException($"Type {obj.GetType()} not a com class");
            }
            stream.StringPool.Write(attr.ClassName, 0x73, 0x1B);
            obj.Serialize(stream, this);
        }

        /// <summary>
        /// Deserialize a <see cref="IComObject"/> object from file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">File path</param>
        public T Deserialize<T>(string path) where T : class, IComObject
            => this.Deserialize<T>(File.OpenRead(path));

        /// <summary>
        /// Deserialize a <see cref="IComObject"/> object from <see cref="Stream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <returns><see cref="IComObject"/> object</returns>
        public T Deserialize<T>(Stream stream) where T : class, IComObject
            => this.Deserialize<T>(new WzStream(stream, this.IV));

        /// <summary>
        /// Deserialize a <see cref="IComObject"/> object from <see cref="WzStream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <returns><see cref="IComObject"/> object</returns>
        public T Deserialize<T>(WzStream stream) where T : class, IComObject {
            var attr = typeof(T).GetCustomAttribute<ComClassAttribute>();
            var className = stream.StringPool.Read(0x73, 0x1B);
            T obj;
            if (attr == null) {
                obj = this.GetUnknown(className) as T;
            } else if (attr.ClassName != className) {
                throw new ArgumentException($"Wrong com class {className}, real: {attr.ClassName}");
            } else {
                obj = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
            }
            obj.Deserialize(stream, this);
            return obj;
        }

        /// <summary>
        /// Create <see cref="IComObject"/> object when serializer could not comfirm object type
        /// </summary>
        /// <param name="className"></param>
        /// <returns><see cref="IComObject"/> object</returns>
        protected abstract IComObject GetUnknown(string className);
    }
}
