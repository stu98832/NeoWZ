namespace NeoWZ.Serialize.Factory
{
    public interface IComSerializableFactory
    {
        IComSerializable CreateByName(string name); 
    }
}
