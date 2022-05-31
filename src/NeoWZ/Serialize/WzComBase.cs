namespace NeoWZ.Serialize
{
    public abstract class WzComBase : IComObject
    {
        public virtual string Name { get; set; }
        public virtual WzComBase Parent { get; set; }
        public abstract IComObject Clone();

        public T To<T>() where T : class, IComObject => this as T;
        public abstract void Serialize(WzStream stream, ComSerializer serializer);
        public abstract void Deserialize(WzStream stream, ComSerializer serializer);

        public virtual void Dispose() { }
    }
}
