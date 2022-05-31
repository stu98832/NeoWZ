namespace NeoWZ
{
    public class WzArchive : WzEntry
    {
        public override bool IsDirectory => false;
        public Stream Stream { get; set; }

        public void Save(string path) {
            var stream = File.OpenWrite(path);
            var buffer = new byte[this.Size];
            this.Stream.Seek(0, SeekOrigin.Begin);
            this.Stream.Read(buffer, 0, buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            stream.Dispose();
        }
    }
}
