namespace NeoWZ.Serialize.Sound
{
    /// <summary> MediaType for wz. the structure like AM_MEDIA_TYPE structure in DirectSound</summary>
    public struct WzMediaType
    {
        public Guid MajorType { get; set; } = Guid.Empty;
        public Guid SubType { get; set; } = Guid.Empty;
        public int SampleSize { get; set; } = 0;
        public int Flag { get; set; } = 0;
        public bool FixedSizeSamples => (this.Flag & 0x1) != 0;
        public bool TemporalCompression => (this.Flag & 0x2) != 0;
        public Guid FormatType { get; set; } = Guid.Empty;
        public int FormatLength { get; set; } = 0;
        public byte[] FormatData { get; set; } = null;

        public WzMediaType() { }
    }
}
