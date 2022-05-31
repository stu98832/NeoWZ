namespace NeoWZ
{
    /// <summary>
    /// Part of stream
    /// </summary>
    public class SectionStream : Stream
    {
        protected long mSize;

        /// <summary>
        /// Base stream
        /// </summary>
        public Stream Base { get; protected set; }

        /// <summary>
        /// Offset of stream
        /// </summary>
        public virtual long Offset { get; }

        /// <summary>
        /// Repersent available bytes of stream
        /// </summary>
        public virtual long Available => this.Length - this.Position;

        public override long Length => this.mSize == -1 ? (this.Base.Length - this.Offset) : this.mSize;
        public override long Position {
            get => this.Base.Position - this.Offset;
            set => this.Base.Position = value + this.Offset;
        }
        public override bool CanRead => Base.CanRead;
        public override bool CanSeek => Base.CanSeek;
        public override bool CanWrite => Base.CanWrite;

        public SectionStream(Stream stream, long offset, long size = -1) {
            this.Base = stream;
            this.mSize = size;
            this.Offset = offset;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            if (origin == SeekOrigin.Begin) {
                if (offset < 0) {
                    throw new IOException("An attempt was made to move the position before the beginning of the stream.");
                }
                return this.Base.Seek(offset + this.Offset, SeekOrigin.Begin);
            } else if (origin == SeekOrigin.End) {
                if (this.Offset + this.Length + offset < this.Offset) {
                    throw new IOException("An attempt was made to move the position before the beginning of the stream.");
                }
                return this.Base.Seek(this.Offset + this.Length + offset, SeekOrigin.Begin);
            } else {
                if (this.Position + offset < this.Offset) {
                    throw new IOException("An attempt was made to move the position before the beginning of the stream.");
                }
                return this.Base.Seek(offset, origin);
            }
        }

        public override void Close() {
            // do nothing
        }

        public override void Flush() => this.Base.Flush();
        public override void SetLength(long value) => this.mSize = value;
        public override int Read(byte[] buffer, int offset, int count) {
            if (this.Available < count) {
                throw new EndOfStreamException();
            }
            return this.Base.Read(buffer, offset, count);
        }
        public override void Write(byte[] buffer, int offset, int count) {
            this.Base.Write(buffer, offset, count);
            if (this.mSize != -1 && this.Available < count) {
                this.mSize += count - this.Available;
            }
        }
    }
}
