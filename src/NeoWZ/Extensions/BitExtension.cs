namespace NeoWZ.Extensions
{
    public static class BitExtension
    {
        /// <summary>
        /// Rotate <see cref="uint"/> bits in left direction
        /// </summary>
        /// <param name="x"> source </param>
        /// <param name="n"> rotate size (bit) </param>
        /// <returns></returns>
        public static uint RotateLeft(this uint x, byte n)
            => x << (n % 32) | x >> (32 - (n % 32));
    }
}
