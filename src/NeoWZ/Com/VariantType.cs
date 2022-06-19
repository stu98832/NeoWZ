namespace NeoWZ.Com
{
    /// <summary> See <see href="https://docs.microsoft.com/windows/win32/api/wtypes/ne-wtypes-varenum"/></summary>
    public enum VariantType : ushort
    {
        Empty = 0x00,
        Null = 0x01,
        Int16 = 0x02,
        Int32 = 0x03,
        Float = 0x04,
        Double = 0x05,
        String = 0x08,
        Dispatch = 0x09,
        Boolean = 0x0b,
        Unknown = 0x0d,
        UInt16 = 0x12,
        UInt32 = 0x13,
        Int64 = 0x14,
        UInt64 = 0x15,
    }
}
