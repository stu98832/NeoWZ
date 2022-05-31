namespace NeoWZ.Serialize.Property
{
    /// <summary> Invalid variant which only thrown when not found. </summary>
    public class WzInvalidVariant : WzVariant
    {
        public WzInvalidVariant() : base(null) {
        }

        public override WzVariant Clone() {
            throw new NotImplementedException();
        }

        public override bool Equals(WzVariant obj) => true;
    }
}
