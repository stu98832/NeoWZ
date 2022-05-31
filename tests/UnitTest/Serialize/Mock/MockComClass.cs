using NeoWZ.Serialize.Attributes;

namespace NeoWZ.Serialize.Test.Mock
{
    [ComClass("Mock")]
    public class MockComClass : WzComBase
    {
        public int Value { get; set; }

        public override IComObject Clone() => new MockComClass() { Value = this.Value };
        public override void Deserialize(WzStream stream, ComSerializer serializer) => this.Value = stream.ReadInt32();
        public override void Serialize(WzStream stream, ComSerializer serializer) => stream.WriteInt32(this.Value);
    }
}
