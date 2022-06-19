namespace NeoWZ.Serialize
{
    public abstract class WzSerializable : IComSerializable
    {
        public virtual string Name { get; set; }
        public virtual WzSerializable Parent { get; set; }
        public abstract IComSerializable Clone();

        public T To<T>() where T : class, IComSerializable => this as T;
        public abstract void Serialize(WzStream stream, ComSerializer serializer);
        public abstract void Deserialize(WzStream stream, ComSerializer serializer);

        public virtual void Dispose() { }
    }
}
