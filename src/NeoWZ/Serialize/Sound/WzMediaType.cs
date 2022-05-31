using NeoWZ.Extensions;

namespace NeoWZ.Serialize.Sound
{
    /// <summary> custom AM_MEDIA_TYPE </summary>
    public class WzMediaType
    {
        public Guid MajorType { get; set; } = Guid.Empty;
        public Guid SubType { get; set; } = Guid.Empty;
        public int SampleSize { get; set; } = 0;
        public bool TemporalCompression { get; set; } = false;
        public bool FixedSizeSamples { get; set; } = false;
        public Guid FormatType { get; set; } = Guid.Empty;
        public int FormatLength { get; set; } = 0;
        public byte[] FormatData { get; set; } = null;

        public WzMediaType Clone() {
            var clone = new WzMediaType();
            clone.MajorType = this.MajorType;
            clone.SubType = this.SubType;
            clone.SampleSize = this.SampleSize;
            clone.TemporalCompression = this.TemporalCompression;
            clone.FixedSizeSamples = this.FixedSizeSamples;
            clone.FormatType = this.FormatType;
            clone.FormatLength = this.FormatLength;
            clone.FormatData = new byte[this.FormatLength];
            this.FormatData.CopyTo(clone.FormatData, 0);
            return clone;
        }
        public void Deserialize(WzStream stream) {
            this.MajorType = new Guid(stream.Read(16));          //MEDIATYPE_Stream
            this.SubType = new Guid(stream.Read(16));            //MEDIASUBTYPE_WAVE
            this.SampleSize = stream.ReadCompressedInt32();
            var flag = stream.ReadCompressedInt32();
            this.TemporalCompression = (flag & 0x2) != 0;
            this.FixedSizeSamples = (flag & 0x1) != 0;
            this.FormatType = new Guid(stream.Read(16));
            this.FormatLength = stream.ReadCompressedInt32();
            this.FormatData = stream.Read(this.FormatLength);
        }

        public void Serialize(WzStream stream) {
            stream.Write(this.MajorType.ToByteArray());
            stream.Write(this.SubType.ToByteArray());
            stream.WriteCompressedInt32(this.SampleSize);
            var flag = (this.TemporalCompression ? 2 : 0) | (this.FixedSizeSamples ? 1 : 0);
            stream.WriteCompressedInt32(flag);
            stream.Write(this.FormatType.ToByteArray());
            stream.WriteCompressedInt32(this.FormatLength);
            stream.Write(this.FormatData);
        }
    }
}
