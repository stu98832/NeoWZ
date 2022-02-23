using System;
using System.IO;
using System.ComponentModel;

namespace NeoMS.Wz.Sound.Windows
{
    /// <summary> </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct MPEGLayer3WaveFormat
    {
        /// <summary> </summary>
        public WaveFormatEx wfx { get; internal set; }

        /// <summary> </summary>
        public ushort wID { get; internal set; } // MPEGLAYER3_ID_MPEG

        /// <summary> </summary>
        public uint fdwFlags { get; internal set; }

        /// <summary> </summary>
        public ushort nBlockSize { get; internal set; }

        /// <summary> </summary>
        public ushort nFramesPerBlock { get; internal set; }

        /// <summary> </summary>
        public ushort nCodecDelay { get; internal set; }

        /// <summary> 將<see cref="MPEGLayer3WaveFormat"/>結構轉成<see cref="byte"/>陣列 </summary>
        public byte[] ToArray() {
            byte[] arr = new byte[30];
            MemoryStream ms = new MemoryStream(arr);
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.wfx.ToArray());
            writer.Write(this.wID);
            writer.Write(this.fdwFlags);
            writer.Write(this.nBlockSize);
            writer.Write(this.nFramesPerBlock);
            writer.Write(this.nCodecDelay);
            writer.Flush();
            writer.Close();
            ms.Dispose();

            return arr;
        }

        /// <summary> 由<see cref="byte"/>陣列組成<see cref="MPEGLayer3WaveFormat"/>結構 </summary>
        public static MPEGLayer3WaveFormat FromArray(byte[] datas) {
            if (datas.Length < 30) {
                throw new InvalidCastException("Invalid datas");
            }

            MemoryStream ms = new MemoryStream(datas);
            BinaryReader reader = new BinaryReader(ms);
            MPEGLayer3WaveFormat ml3wf = new MPEGLayer3WaveFormat();

            ml3wf.wfx = WaveFormatEx.FromArray(reader.ReadBytes((int)WaveFormatEx.StructureSize));
            ml3wf.wID = reader.ReadUInt16();
            ml3wf.fdwFlags = reader.ReadUInt32();
            ml3wf.nBlockSize = reader.ReadUInt16();
            ml3wf.nFramesPerBlock = reader.ReadUInt16();
            ml3wf.nCodecDelay = reader.ReadUInt16();
            reader.Close();
            ms.Dispose();

            return ml3wf;
        }

        /// <summary> <see cref="MPEGLayer3WaveFormat"/>結構的大小 </summary>
        public const uint StructureSize = WaveFormatEx.StructureSize + 12;
    }
}
