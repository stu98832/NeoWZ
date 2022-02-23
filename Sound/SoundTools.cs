using NeoMS.Wz.Sound.Windows;
using System;
using System.IO;
using System.Text;

namespace NeoMS.Wz.Sound
{
    /// <summary> </summary>
    public class SoundTools
    {
        /// <summary> </summary>
        internal static void LoadWAV(WzSound sound, BinaryReader reader) {
            WaveFormatEx wf = new WaveFormatEx();

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            uint cbData = reader.ReadUInt32();

            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "WAVE")
                throw new Exception("非WAVE格式");

            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) == "fmt ") {
                int cbBlockSize = reader.ReadInt32();

                wf.wFormatTag = reader.ReadUInt16();
                wf.nChannels = reader.ReadUInt16();
                wf.nSamplesPerSec = reader.ReadUInt32();
                wf.nAvgBytesPerSec = reader.ReadUInt32();
                wf.nBlockAlign = reader.ReadUInt16();
                wf.wBitsPerSample = reader.ReadUInt16();
                wf.cbSize = 0;
            }
            if (Encoding.ASCII.GetString(reader.ReadBytes(4)) == "data") {
                int cbBlockSize = reader.ReadInt32();

                sound.Duration = cbBlockSize / wf.wBitsPerSample * 1000 / wf.wBitsPerSample;
                sound.DataSize = cbBlockSize;
                sound.SoundData = reader.ReadBytes(cbBlockSize);
            }

            WzMediaType wzmt = new WzMediaType();
            wzmt.MajorType = SoundDX8Constants.MEDIATYPE_Stream;
            wzmt.SubType = SoundDX8Constants.MEDIASUBTYPE_WAVE;
            wzmt.TemporalCompression = false;
            wzmt.FixedSizeSamples = true;
            wzmt.FormatType = SoundDX8Constants.WMFORMAT_WaveFormatEx;
            wzmt.FormatLength = WaveFormatEx.StructureSize;
            wzmt.FormatData = wf.ToArray();
            sound.MediaType = wzmt;
        }

        /// <summary> </summary>
        internal static void LoadMP3_ID3(WzSound sound, BinaryReader reader) {
            reader.BaseStream.Seek(3, SeekOrigin.Begin);

            ushort wTagVersion = reader.ReadUInt16();
            byte bFlag = reader.ReadByte();
            byte[] aHeaderSize = reader.ReadBytes(4);
            int cbHeader = ((aHeaderSize[0] & 0x7F) << 21) + ((aHeaderSize[1] & 0x7F) << 14) + ((aHeaderSize[2] & 0x7F) << 7) + (aHeaderSize[3] & 0x7F);

            reader.BaseStream.Seek(cbHeader, SeekOrigin.Current);

            LoadMP3_ReadFrameData(sound, reader, (int)(reader.BaseStream.Length - reader.BaseStream.Position));
        }

        /// <summary> </summary>
        internal static void LoadMP3_TAG(WzSound sound, BinaryReader reader) {
            reader.BaseStream.Seek(-128, SeekOrigin.End);
            // Header : 
            // pos ,size: name
            // -128,  3 : tagFormat
            // -125, 30 : sName
            // - 95, 30 : sArtist
            // - 65, 30 : sAlbum
            // - 35,  4 : sYear
            // - 31, 30 : Comment
            // - 01,  1 : bGenre
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            LoadMP3_ReadFrameData(sound, reader, (int)(reader.BaseStream.Length - 128));
        }

        /// <summary> </summary>
        internal static void LoadMP3_ReadFrameData(WzSound sound, BinaryReader reader, int cbData) {
            uint dwFrameFormat = (uint)(reader.ReadByte() << 24 | reader.ReadByte() << 16 | reader.ReadByte() << 08 | reader.ReadByte());

            reader.BaseStream.Seek(-4, SeekOrigin.Current);

            WaveFormatEx wf = new WaveFormatEx();

            if ((dwFrameFormat >> 21) != 0x7FF)
                throw new Exception();

            while ((dwFrameFormat >> 21) == 0x7FF) {
                // ---vv--- (0 = mpeg2.5, 1 = reserved, 2 = mpeg2, 3 = mpeg1)
                int version = (int)(dwFrameFormat >> 19) & 3;
                // -----ll- (0 = reserved, 1 = layer3, 2 = layer2, 3 = layer1)
                int layer = 3 - (int)((dwFrameFormat >> 17) & 3);
                int crc = (int)(dwFrameFormat >> 16) & 1;
                int bitrate = (int)(dwFrameFormat >> 12) & 15;
                int frequency = (int)(dwFrameFormat >> 10) & 3;
                int padding = (int)(dwFrameFormat >> 9) & 1;
                int private_bit = (int)(dwFrameFormat >> 8) & 1;
                int channel = 1 + (int)(dwFrameFormat >> 6) & 3;
                int mode_extension = (int)(dwFrameFormat >> 4) & 3;
                int copyright = (int)(dwFrameFormat >> 3) & 1;
                int original = (int)(dwFrameFormat >> 2) & 1;
                int emphasis = (int)(dwFrameFormat) & 3;

                int bitrate_ = MpegBitRate[version == 3 ? 0 : 1, layer, bitrate];
                int length = (int)((cbData / bitrate_) * 8.0);

                if (version == 1 || layer == 3 || bitrate == 0 || frequency == 3)
                    break;

                wf.wFormatTag = SoundDX8Constants.WAVE_FORMAT_MPEGLAYER3;
                wf.nChannels = (ushort)channel;
                wf.nSamplesPerSec = MpegSamplesPerSec[version, frequency];
                wf.nAvgBytesPerSec = (uint)(cbData / (length / 1000.0));
                wf.nBlockAlign = 1;
                wf.wBitsPerSample = 0;
                wf.cbSize = 12;

                // 目前不處理 extra 的部份

                sound.Duration = length;
                break;
            }

            MPEGLayer3WaveFormat mpegwf = new MPEGLayer3WaveFormat();
            mpegwf.wfx = wf;
            mpegwf.wID = SoundDX8Constants.MPEGLAYER3_ID_MPEG;
            mpegwf.fdwFlags = SoundDX8Constants.MPEGLAYER3_FLAG_PADDING_OFF; // (default)
            mpegwf.nBlockSize = 522; //144 * 4 (default)
            mpegwf.nFramesPerBlock = 1; // (default)
            mpegwf.nCodecDelay = 0;

            WzMediaType wzmt = new WzMediaType();
            wzmt.MajorType = SoundDX8Constants.MEDIATYPE_Stream;
            wzmt.SubType = SoundDX8Constants.MEDIASUBTYPE_WAVE;
            wzmt.TemporalCompression = false;
            wzmt.FixedSizeSamples = true;
            wzmt.FormatType = SoundDX8Constants.WMFORMAT_WaveFormatEx;
            wzmt.FormatLength = MPEGLayer3WaveFormat.StructureSize;
            wzmt.FormatData = mpegwf.ToArray();

            sound.MediaType = wzmt;
            sound.DataSize = cbData;
            sound.SoundData = reader.ReadBytes(cbData);
        }

        private static readonly ushort[,,] MpegBitRate = //bitrates
	    {
		    //mpeg1
		    {
                { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 0, }, // layer1
		        { 0, 32, 48, 56,  64,  80,  96, 112, 128, 160, 192, 224, 256, 320, 384, 0, }, // layer2
		        { 0, 32, 40, 48,  56,  64,  80,  96, 112, 128, 160, 192, 224, 256, 320, 0, }, // layer3
            },
		    //mpeg2 & 2.5
		    {
                { 0, 32, 48, 56,  64,  80,  96, 112, 128, 144, 160, 176, 192, 224, 256, 0, }, // layer1
		        { 0,  8, 16, 24,  32,  40,  48,  56,  64,  80,  96, 112, 128, 144, 160, 0, }, // layer2
		        { 0,  8, 16, 24,  32,  40,  48,  56,  64,  80,  96, 112, 128, 144, 160, 0, }, // layer3
            },
        };

        private static readonly uint[,] MpegSamplesPerSec =
        {
            { 11025, 12000,  8000, 0, }, // mpeg2.5
		    {     0,     0,     0, 0, }, // reserved
		    { 22050, 24000, 16000, 0, }, // mpeg2
		    { 44100, 48000, 32000, 0, }, // mpeg1
	    };

        private static readonly int[,] MpegSamplesPerFrame =
        {
		    //mpeg1
		    {
                 384, // layer1
		        1152, // layer2
		        1152, // layer3
		    },
		    //mpeg2 & 2.5
		    {
                 384, // layer1
		        1152, // layer2
		         576, // layer3
		    },
        };

        private static readonly int[,] MpegFactor =
        {
		    //mpeg1
		    {
                 12, // layer1
		    	144, // layer2
		    	144, // layer3
		    },
		    //mpeg2 & 2.5
		    {
                 12, // layer1
		    	144, // layer2
		    	 72, // layer3
		    },
        };

        private static readonly int[] MpegSolt =
        {
            4, // layer1
		    1, // layer2
		    1, // layer3
	    };
    }
}
