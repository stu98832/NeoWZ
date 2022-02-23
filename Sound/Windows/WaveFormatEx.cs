using System;
using System.IO;
using System.ComponentModel;

namespace NeoMS.Wz.Sound.Windows
{
    /// <summary> </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct WaveFormatEx
    {
        /// <summary> format type </summary>
        public ushort wFormatTag { get; internal set; }

        /// <summary> sample rate </summary>
        public ushort nChannels { get; internal set; }

        /// <summary> for buffer estimation </summary>
        public uint nSamplesPerSec { get; internal set; }

        /// <summary> for buffer estimation </summary>
        public uint nAvgBytesPerSec { get; internal set; }

        /// <summary> block size of data </summary>
        public ushort nBlockAlign { get; internal set; }

        /// <summary> number of bits per sample of mono data </summary>
        public ushort wBitsPerSample { get; internal set; }

        /// <summary> the count in bytes of the size of </summary>
        public ushort cbSize { get; internal set; }
        /* extra information (after cbSize)          */

        /// <summary> 將<see cref="WaveFormatEx"/>結構轉成<see cref="byte"/>陣列 </summary>
        public byte[] ToArray() {
            byte[] arr = new byte[18];
            MemoryStream ms = new MemoryStream(arr);
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.wFormatTag);
            writer.Write(this.nChannels);
            writer.Write(this.nSamplesPerSec);
            writer.Write(this.nAvgBytesPerSec);
            writer.Write(this.nBlockAlign);
            writer.Write(this.wBitsPerSample);
            writer.Write(this.cbSize);
            writer.Flush();
            writer.Close();
            ms.Dispose();

            return arr;
        }

        /// <summary> 由<see cref="byte"/>陣列組成<see cref="WaveFormatEx"/>結構 </summary>
        public static WaveFormatEx FromArray(byte[] datas) {
            if (datas.Length < 18) {
                throw new InvalidCastException("Invalid datas");
            }

            MemoryStream ms = new MemoryStream(datas);
            BinaryReader reader = new BinaryReader(ms);
            WaveFormatEx wfx = new WaveFormatEx();

            wfx.wFormatTag = reader.ReadUInt16();
            wfx.nChannels = reader.ReadUInt16();
            wfx.nSamplesPerSec = reader.ReadUInt32();
            wfx.nAvgBytesPerSec = reader.ReadUInt32();
            wfx.nBlockAlign = reader.ReadUInt16();
            wfx.wBitsPerSample = reader.ReadUInt16();
            wfx.cbSize = reader.ReadUInt16();
            reader.Close();
            ms.Dispose();

            return wfx;
        }

        /// <summary> <see cref="WaveFormatEx"/>結構的大小 </summary>
        public const uint StructureSize = 18;
    }
}
