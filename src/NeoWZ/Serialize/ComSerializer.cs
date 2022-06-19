using NeoWZ.Com;
using NeoWZ.Serialize.Factory;
using System.Reflection;

namespace NeoWZ.Serialize
{
    /// <summary>
    /// Can serialize or deserialize any <see cref="IComSerializable"/> object
    public class ComSerializer
    {
        public static ComSerializer Default = new ComSerializer();

        /// <summary>
        /// AES iv
        /// </summary>
        public byte[] IV { get; protected set; }

        private IComSerializableFactory Factory { get; init; }

        /// <summary> Create serializer with default factory </summary>
        /// <param name="iv">crypt iv</param>
        public ComSerializer(byte[] iv = null) : this(new WzSerializableFactory(), iv) {
        }

        /// <summary> Create serializer with specific factory </summary>
        /// <param name="iv">crypt iv</param>
        public ComSerializer(IComSerializableFactory factory, byte[] iv = null) {
            this.IV = iv;
            this.Factory = factory;
        }

        /// <summary>
        /// Serialize a <see cref="IComSerializable"/> object to file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">File path</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(string path, T obj) where T : class, IComSerializable
            => this.Serialize<T>(File.OpenWrite(path), obj);

        /// <summary>
        /// Serialize a <see cref="IComSerializable"/> object to stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(Stream stream, T obj) where T : class, IComSerializable
            => this.Serialize<T>(new WzStream(stream, this.IV), obj);

        /// <summary>
        /// Serialize a <see cref="IComSerializable"/> object to <see cref="WzStream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <param name="obj">Object which would be serialized</param>
        public void Serialize<T>(WzStream stream, T obj) where T : class, IComSerializable {
            var attr = obj.GetType().GetCustomAttribute<ComClassAttribute>();
            if (attr == null) {
                throw new ArgumentException($"Type {obj.GetType()} not a com class");
            }
            stream.StringPool.Write(attr.ClassName, 0x73, 0x1B);
            obj.Serialize(stream, this);
        }

        /// <summary>
        /// Deserialize a <see cref="IComSerializable"/> object from file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">File path</param>
        public T Deserialize<T>(string path) where T : class, IComSerializable
            => this.Deserialize<T>(File.OpenRead(path));

        /// <summary>
        /// Deserialize a <see cref="IComSerializable"/> object from <see cref="Stream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <returns><see cref="IComSerializable"/> object</returns>
        public T Deserialize<T>(Stream stream) where T : class, IComSerializable
            => this.Deserialize<T>(new WzStream(stream, this.IV));

        /// <summary>
        /// Deserialize a <see cref="IComSerializable"/> object from <see cref="WzStream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Stream</param>
        /// <returns><see cref="IComSerializable"/> object</returns>
        public T Deserialize<T>(WzStream stream) where T : class, IComSerializable {
            var attr = typeof(T).GetCustomAttribute<ComClassAttribute>();
            var className = stream.StringPool.Read(0x73, 0x1B);
            T obj;
            if (attr == null) {
                obj = this.Factory?.CreateByName(className) as T ?? throw new Exception("No class found");
            } else if (attr.ClassName != className) {
                throw new ArgumentException($"Wrong com class {className}, real: {attr.ClassName}");
            } else {
                obj = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;
            }
            obj.Deserialize(stream, this);
            return obj;
        }
    }
}
