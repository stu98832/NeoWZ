namespace NeoWZ.V2.IO;

public class PartialStream : Stream
{
    private readonly Stream baseStream;
    private readonly long offset;
    private readonly long length;

    public override bool CanRead => this.baseStream.CanRead;

    public override bool CanSeek => this.baseStream.CanSeek;

    public override bool CanWrite => false;

    public override long Length => this.length;

    private long position = 0;

    public override long Position {
        get => position;
        set {
            if (position < 0 || position > this.length) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            position = value;
        }
    }

    public PartialStream(Stream baseStream, long offset, long length) {
        if (baseStream == null) {
            throw new ArgumentNullException(nameof(baseStream));
        }
        if (offset < 0 || offset > baseStream.Length) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        if (length < 0 || offset + length > baseStream.Length) {
            throw new ArgumentOutOfRangeException(nameof(length));
        }
        this.baseStream = baseStream;
        this.offset = offset;
        this.length = length;
    }

    public override void Flush() {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count) {
        // move base stream to correct position as needed
        if (this.baseStream.Position != this.offset + this.Position) {
            this.baseStream.Position = this.offset + this.Position;
        }
        if (this.Position >= this.length) {
            return 0;
        }
        if (this.Position + count > this.length) {
            count = (int)(this.length - this.Position);
        }
        int read = this.baseStream.Read(buffer, offset, count);
        this.Position += read;
        return read;
    }

    public override long Seek(long offset, SeekOrigin origin) {
        long pos = origin switch {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => this.Position + offset,
            SeekOrigin.End => this.length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };
        if (pos < 0 || pos > this.length) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        this.Position = pos;
        return pos;
    }

    public override void SetLength(long value) {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count) {
        throw new NotImplementedException();
    }
}