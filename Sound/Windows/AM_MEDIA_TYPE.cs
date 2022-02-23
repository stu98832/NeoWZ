using System;

namespace NeoMS.Wz.Sound.Windows
{
    /// <summary> AM_MEDIA_TYPE(DirectX8 Sound) </summary>
    public class AM_MEDIA_TYPE
    {
        /// <summary> </summary>
        public Guid MajorType { get; internal set; }
        /// <summary> </summary>
        public Guid SubType { get; internal set; }
        /// <summary> </summary>
        public uint SampleSize { get; internal set; }
        /// <summary> </summary>
        public bool FixedSizeSamples { get; set; }
        /// <summary> </summary>
        public bool TemporalCompression { get; set; }
        /// <summary> </summary>
        public Guid FormatType { get; internal set; }
        /// <summary> </summary>
        public uint FormatLength { get; internal set; }
        /// <summary> </summary>
        public byte[] FormatData { get; internal set; }
    }
}
