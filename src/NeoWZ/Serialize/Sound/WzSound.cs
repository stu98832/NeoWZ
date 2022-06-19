using NeoWZ.Extensions;
using NeoWZ.Com;

namespace NeoWZ.Serialize.Sound
{
    [ComClass("Sound_DX8")]
    public class WzSound : WzComBase
    {
        public int Duration { get; set; } = 0;
        public int Unknown { get; set; } = 2; // 0 when WzMediaType is null
        public WzMediaType MediaType { get; set; } = new WzMediaType();
        public byte[] SoundData { get; set; } = null;

        public override WzComBase Clone() {
            var sound = new WzSound() {
                Name = this.Name,
                Duration = this.Duration,
                Unknown = this.Unknown,
                MediaType = this.MediaType.Clone(),
                SoundData = new byte[this.SoundData.Length],
            };

            this.SoundData.CopyTo(sound.SoundData, 0);

            return sound;
        }

        public override void Dispose() {
            this.SoundData = null;
            base.Dispose();
        }

        public override void Serialize(WzStream stream, ComSerializer serializer) {
            stream.WriteByte(0);
            stream.WriteCompressedInt32(this.SoundData.Length);
            stream.WriteCompressedInt32(this.Duration);
            stream.WriteCompressedInt32(this.Unknown);
            this.MediaType.Serialize(stream);
            stream.Write(this.SoundData);
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            stream.ReadByte(); // non-zero throw error in PCOM
            var dataSize = stream.ReadCompressedInt32();
            this.Duration = stream.ReadCompressedInt32();
            this.Unknown = stream.ReadCompressedInt32();
            this.MediaType.Deserialize(stream);
            this.SoundData = stream.Read(dataSize);
        }
    }
}
