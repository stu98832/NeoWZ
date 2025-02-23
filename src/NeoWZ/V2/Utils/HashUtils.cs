namespace NeoWZ.V2.Utils;

public class HashUtils {
    public static int StringHash(string str) {
        int hash = 0;
        foreach (var ch in str) {
            hash = (hash << 5) + (byte)ch + 1;
        }
        return hash;
    }

    public static byte XorHash(int data) {
        byte[] bytes = BitConverter.GetBytes(data);
        byte hash = 0xFF;
        for (int i = 0; i < 4; i++) {
            hash ^= bytes[i];
        }
        return hash;
    }
}