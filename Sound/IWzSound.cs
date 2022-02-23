using NeoMS.Wz.Com;

namespace NeoMS.Wz.Sound
{
    /// <summary> Wizet Sound Interface </summary>
    public interface IWzSound : IWzSerialize
    {
        /// <summary> Get data size of this sound </summary>
        int DataSize { get; }

        /// <summary> Get sound duration </summary>
        int Duration { get; }

        /// <summary> Get media type of this sound </summary>
        WzMediaType MediaType { get; }

        /// <summary> Get sound data </summary>
        byte[] SoundData { get; }
    }
}
