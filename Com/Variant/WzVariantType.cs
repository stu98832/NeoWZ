namespace NeoMS.Wz.Com.Variant
{
    /// <summary> Variant type for wz, corresponding to VARENUM in wtypes.h</summary>
    public enum WzVariantType : byte
    {
        /// <summary> empty(VT_EMPTY) </summary>
        Empty = 0,
        /// <summary> null(VT_NULL) </summary>
        Null = 1,
        /// <summary> <see cref="short"/>(VT_I2) </summary>
        Short = 2,
        /// <summary> <see cref="int"/>(VT_I4) </summary>
        Int = 3,
        /// <summary> <see cref="float"/>(VT_R4) </summary>
        Float = 4,
        /// <summary> <see cref="double"/>(VT_R8) </summary>
        Double = 5,
        /// <summary> <see cref="string"/>(VT_BSTR) </summary>
        String = 8,
        /// <summary> <see cref="WzSerialize"/> object(VT_DISPATCH), it was saved as IUnknown in original VARIANT structure</summary>
        Dispatch = 9,
        /// <summary> <see cref="bool"/>(VARIANT_BOOL), 0xFFFF=true，0x0000=false </summary>
        Boolean = 11,
        /// <summary> <see cref="uint"/>(VT_UI4) </summary>
        UInt = 19,
        /// <summary> <see cref="long"/>(VT_I8) </summary>
        Long = 20,
    }
}
