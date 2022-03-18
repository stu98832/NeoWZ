namespace NeoWZ.Extensions
{
    public static class WzStreamExtension
    {
        public static int ReadCompressedInt32(this WzStream stream) {
            int val = stream.ReadSByte();
            return val != -128 ? val : stream.ReadInt32();
        }

        public static long ReadCompressedInt64(this WzStream stream) {
            long val = stream.ReadSByte();
            return val != -128 ? val : stream.ReadInt64();
        }

        public static float ReadCompressedFloat(this WzStream stream) {
            float val = stream.ReadSByte();
            return val != -128 ? val : stream.ReadFloat();
        }

        public static void WriteCompressedInt32(this WzStream stream, int val) {
            var compressable = (-127 <= val && val <= 127);
            stream.WriteSByte(compressable ? (sbyte)val : (sbyte)-128);
            if (!compressable) {
                stream.WriteUInt32((uint)val);
            }
        }

        public static void WriteCompressedInt64(this WzStream stream, long val) {
            var compressable = (-127 <= val && val <= 127);
            stream.WriteSByte(compressable ? (sbyte)val : (sbyte)-128);
            if (!compressable) {
                stream.WriteUInt64((ulong)val);
            }
        }

        public static void WriteCompressedFloat(this WzStream stream, float val) {
            var compressable = (val == 0.0f);
            stream.WriteSByte(compressable ? (sbyte)0 : (sbyte)-128);
            if (!compressable) {
                stream.WriteFloat(val);
            }
        }
    }
}
