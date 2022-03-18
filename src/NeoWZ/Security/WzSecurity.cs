namespace NeoWZ.Security
{
    /// <summary>
    /// Provide crypto functions for .wz file
    /// </summary>
    public static class WzSecurity
    {
        /// <summary>
        /// Encrypt offset
        /// </summary>
        /// <param name="offset"> Offset </param>
        /// <param name="baseOff"> Base offset of data </param>
        /// <param name="key"> key </param>
        /// <returns></returns>
        public static uint EncryptOffset(uint offset, uint baseOff, uint key) => (offset - baseOff * 2) ^ key;

        /// <summary>
        /// Decrypt offset
        /// </summary>
        /// <param name="encrypted"> Encrypted offset </param>
        /// <param name="baseOff"> Base offset of data </param>
        /// <param name="key"> Key </param>
        /// <returns></returns>
        public static uint DecryptOffset(uint encrypted, uint baseOff, uint key) => (encrypted ^ key) + baseOff * 2;
    }
}
