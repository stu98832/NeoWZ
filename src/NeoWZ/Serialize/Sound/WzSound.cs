using NeoWZ.Extensions;
using NeoWZ.Serialize.Attributes;

namespace NeoWZ.Serialize.Sound
{
    [ComClass("Sound_DX8")]
    public class WzSound : WzComBase
    {
        public byte Unknow1_Byte { get; set; } = 0; // 00
        public int Duration { get; set; } = 0;
        public byte Unknow3_Byte { get; set; } = 2; // 02
        public WzMediaType MediaType { get; set; } = new WzMediaType();
        public byte[] SoundData { get; set; } = null;

        public override WzComBase Clone() {
            var mediaType = this.MediaType;
            mediaType.FormatData = new byte[this.MediaType.FormatLength];
            this.MediaType.FormatData.CopyTo(this.MediaType.FormatData, 0);

            var sound = new WzSound() {
                Name = this.Name,
                Unknow1_Byte = this.Unknow1_Byte,
                Duration = this.Duration,
                Unknow3_Byte = this.Unknow3_Byte,
                MediaType = mediaType,
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
            stream.WriteByte(this.Unknow1_Byte);
            stream.WriteCompressedInt32(this.SoundData.Length);
            stream.WriteCompressedInt32(this.Duration);
            stream.WriteByte(this.Unknow3_Byte);

            // media type
            stream.Write(this.MediaType.MajorType.ToByteArray());
            stream.Write(this.MediaType.SubType.ToByteArray());
            stream.WriteCompressedInt32(this.MediaType.SampleSize);
            stream.WriteCompressedInt32(this.MediaType.Flag);
            stream.Write(this.MediaType.FormatType.ToByteArray());
            stream.WriteCompressedInt32(this.MediaType.FormatLength);
            stream.Write(this.MediaType.FormatData);

            stream.Write(this.SoundData);
        }

        public override void Deserialize(WzStream stream, ComSerializer serializer) {
            this.Unknow1_Byte = (byte)stream.ReadByte();
            var dataSize = stream.ReadCompressedInt32();
            this.Duration = stream.ReadCompressedInt32();
            this.Unknow3_Byte = (byte)stream.ReadByte();

            // media type
            var mediaType = new WzMediaType();
            mediaType.MajorType = new Guid(stream.Read(16));          //MEDIATYPE_Stream
            mediaType.SubType = new Guid(stream.Read(16));            //MEDIASUBTYPE_WAVE
            mediaType.SampleSize = stream.ReadCompressedInt32();
            mediaType.Flag = stream.ReadCompressedInt32();
            mediaType.FormatType = new Guid(stream.Read(16));
            mediaType.FormatLength = stream.ReadCompressedInt32();
            mediaType.FormatData = stream.Read(mediaType.FormatLength);
            this.MediaType = mediaType;

            // data
            this.SoundData = stream.Read(dataSize);
        }
    }
}
