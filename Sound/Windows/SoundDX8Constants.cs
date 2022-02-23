using System;

namespace NeoMS.Wz.Sound.Windows
{
    /// <summary> 與DirectSound8相關，定義在DirectShow中的常數 </summary>
    public class SoundDX8Constants
    {
        /// <summary> </summary>
        public readonly static Guid TIME_FORMAT_NONE = new Guid("{00000000-0000-0000-0000-000000000000}");

        /// <summary> </summary>
        public readonly static Guid MEDIATYPE_Stream = new Guid("{E436EB83-524F-11CE-9F53-0020AF0BA770}");

        /// <summary> </summary>
        public readonly static Guid MEDIASUBTYPE_WAVE = new Guid("{E436EB8B-524F-11CE-9F53-0020AF0BA770}");

        /// <summary> </summary>
        public readonly static Guid MEDIASUBTYPE_MPEG1Audio = new Guid("{E436EB87-524F-11CE-9F53-0020AF0BA770}");

        /// <summary> </summary>
        public readonly static Guid WMFORMAT_WaveFormatEx = new Guid("{05589F81-C356-11CE-BF01-00AA0055595A}");

        /// <summary> </summary>
        public const ushort WAVE_FORMAT_PCM = 0x0001;

        /// <summary> </summary>
        public const ushort WAVE_FORMAT_MPEGLAYER3 = 0x0055;

        /// <summary> </summary>
        public const ushort MPEGLAYER3_ID_MPEG = 0x0001;

        /// <summary> </summary>
        public const ushort MPEGLAYER3_FLAG_PADDING_ISO = 0x0000;

        /// <summary> </summary>
        public const ushort MPEGLAYER3_FLAG_PADDING_ON = 0x0001;

        /// <summary> </summary>
        public const ushort MPEGLAYER3_FLAG_PADDING_OFF = 0x0002;
    }
}
