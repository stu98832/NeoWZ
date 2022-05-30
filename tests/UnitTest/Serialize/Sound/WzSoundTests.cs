using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace NeoWZ.Serialize.Sound.Tests
{
    [TestClass]
    public class WzSoundTests
    {
        [TestMethod]
        public void SerializeDeserializeTest() {
            var memory = new MemoryStream();
            var sound = new WzSound() {
                Unknown = 31,
                Duration = 1000,
                SoundData = new byte[32],
                MediaType = new WzMediaType() {
                    TemporalCompression = true,
                    FixedSizeSamples = true,
                    FormatLength = 16,
                    FormatData = new byte[16],
                    FormatType = Guid.NewGuid(),
                    MajorType = Guid.NewGuid(),
                    SubType = Guid.NewGuid(),
                    SampleSize = 321,
                }
            };
            ComSerializer.Default.Serialize(memory, sound);
            memory.Seek(0, SeekOrigin.Begin);

            var anotherSound = ComSerializer.Default.Deserialize<WzSound>(memory);
            Assert.AreEqual(sound.Unknown, anotherSound.Unknown);
            Assert.AreEqual(sound.Duration, anotherSound.Duration);
            CollectionAssert.AreEqual(sound.SoundData, anotherSound.SoundData);
            Assert.AreEqual(sound.MediaType.FormatLength, anotherSound.MediaType.FormatLength);
            CollectionAssert.AreEqual(sound.MediaType.FormatData, anotherSound.MediaType.FormatData);
            Assert.AreEqual(sound.MediaType.FormatType, anotherSound.MediaType.FormatType);
            Assert.AreEqual(sound.MediaType.MajorType, anotherSound.MediaType.MajorType);
            Assert.AreEqual(sound.MediaType.SubType, anotherSound.MediaType.SubType);
            Assert.AreEqual(sound.MediaType.SampleSize, anotherSound.MediaType.SampleSize);
            Assert.AreEqual(sound.MediaType.TemporalCompression, anotherSound.MediaType.TemporalCompression);
            Assert.AreEqual(sound.MediaType.FixedSizeSamples, anotherSound.MediaType.FixedSizeSamples);
        }
    }
}