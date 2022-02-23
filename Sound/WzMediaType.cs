using System;
using NeoMS.Wz.Com;
using NeoMS.Wz.Sound.Windows;

namespace NeoMS.Wz.Sound
{
    /// <summary> MediaType for wz. the structure like AM_MEDIA_TYPE structure in DirectSound</summary>
    public class WzMediaType : AM_MEDIA_TYPE
    {
        /// <summary> Default <see cref="WzMediaType"/> setting for <see cref="WzSound"/>(MP3) </summary>
        public readonly static WzMediaType Default = new WzMediaType() {
            MajorType = SoundDX8Constants.MEDIATYPE_Stream,
            SubType = SoundDX8Constants.MEDIASUBTYPE_WAVE,
            SampleSize = 0,
            FixedSizeSamples = true,
            TemporalCompression = false,
            FormatType = SoundDX8Constants.WMFORMAT_WaveFormatEx,
            FormatLength = 30, // WaveFormat(18) + MPGELayer3(12)
            FormatData = new byte[] {
                0x55, 0x00,             // wFormat         : WAVE_FORMAT_MPGELAYER3
                0x02, 0x00,             // nChannels       : 2
                0xE4, 0x57, 0x00, 0x00, // nSamplesPerSec  : 22500
                0x10, 0x27, 0x00, 0x00, // nAvgBytesPerSec : 10000
                0x01, 0x00,             // nBlockAlign     : 1
                0x00, 0x00,             // wBitsPerSample  : 0
                0x0C, 0x00,             // cbSize          : 12
                0x01, 0x00,             // wID             : MPEGLAYER3_ID_MPEG
                0x02, 0x00, 0x00, 0x00, // fdwFlags        : MPEGLAYER3_FLAG_PADDING_OFF
                0x0A, 0x02,             // nBlockSize      : 522
                0x01, 0x00,             // nFramesPerBlock : 1
                0x00, 0x00,             // nCodecDelay     : 0
            }
        };

        /// <summary> </summary>
        public void Read(IWzFileStream stream) {
            this.MajorType = new Guid(stream.Read(16));          //MEDIATYPE_Stream
            this.SubType = new Guid(stream.Read(16));            //MEDIASUBTYPE_WAVE
            this.SampleSize = (uint)stream.ReadInt32(true);
            uint flag = (uint)stream.ReadInt32(true);
            this.FixedSizeSamples = (flag & 0x1) != 0;
            this.TemporalCompression = (flag & 0x2) != 0;
            this.FormatType = new Guid(stream.Read(16));         //WMFORMAT_WaveFormatEx

            if (this.FormatType == SoundDX8Constants.WMFORMAT_WaveFormatEx) {
                this.FormatLength = (uint)stream.ReadInt32(true);
                this.FormatData = stream.Read((int)this.FormatLength);
            }
        }

        /// <summary> </summary>
        public void Write(IWzFileStream stream) {
            stream.Write(this.MajorType.ToByteArray());
            stream.Write(this.SubType.ToByteArray());
            stream.WriteInt32((int)this.SampleSize, true);
            stream.WriteInt32((this.FixedSizeSamples ? 1 : 0) | (this.TemporalCompression ? 2 : 0));
            stream.Write(this.FormatType.ToByteArray());

            if (this.FormatType == SoundDX8Constants.WMFORMAT_WaveFormatEx) {
                stream.WriteInt32((int)this.FormatLength, true);
                stream.Write(this.FormatData);
            }
        }
    }
}
