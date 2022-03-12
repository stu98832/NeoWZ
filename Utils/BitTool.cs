namespace NeoMS.Wz.Utils
{
    public class BitTool
    {
        private static ulong RotateInner(ulong x, byte n, byte bits, bool left)
            => left
                ? x << n % bits | x >> bits - n % bits
                : x >> n % bits | x << bits - n % bits;

        public static byte RotateL(byte x, byte n) => (byte)RotateInner(x, n, 8, true);
        public static ushort RotateL(ushort x, byte n) => (ushort)RotateInner(x, n, 16, true);
        public static uint RotateL(uint x, byte n) => (uint)RotateInner(x, n, 32, true);
        public static ulong RotateL(ulong x, byte n) => RotateInner(x, n, 64, true);

        public static byte RotateR(byte x, byte n) => (byte)RotateInner(x, n, 8, false);
        public static ushort RotateR(ushort x, byte n) => (ushort)RotateInner(x, n, 16, false);
        public static uint RotateR(uint x, byte n) => (uint)RotateInner(x, n, 32, false);
        public static ulong RotateR(ulong x, byte n) => RotateInner(x, n, 64, false);
    }
}
