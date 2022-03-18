namespace NeoWZ
{
    public abstract class WzEntry
    {
        public WzEntry Parent { get; set; }
        public string Name { get; set; }

        public int Size { get; set; }
        public int Checksum { get; set; }
        public uint Offset { get; set; }
        public abstract bool IsDirectory { get; }

        public T To<T>(T def = null) where T : WzEntry => this as T;
    }
}
