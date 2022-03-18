namespace NeoWZ.Serialize.Canvas
{
    /// <summary> Format of bitmap in <see cref="WzCanvas"/> </summary>
    public enum WzCanvasFormat : int
    {
        /// <summary> BBBBGGGG RRRRAAAA </summary>
        B4G4R4A4 = 0x001,
        /// <summary> BBBBBBBB GGGGGGGG RRRRRRRR AAAAAAAA</summary>
        B8G8R8A8 = 0x002,
        /// <summary> BBBBBGGG GGGRRRRR </summary>
        B5G6R5 = 0x201,
        /// <summary> DXT3 Compress </summary>
        DDS_DXT3 = 0x402,
        /// <summary> DXT5 Compress </summary>
        DDS_DXT5 = 0x802,
    }
}
