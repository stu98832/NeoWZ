namespace NeoMS.Wz.Utils
{
    public class BitTool
    {
        public static uint RotateL(uint x, byte n) {
            return ((x << n) | ((x) >> (32 - n)));
        }
        public static uint RotateR(uint x, byte n) {
            return ((x >> n) | ((x) << (32 - n)));
        }
    }
}
