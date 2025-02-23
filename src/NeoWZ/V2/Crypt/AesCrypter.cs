using System.Security.Cryptography;

namespace NeoWZ.V2.Crypt;

public class AesCrypter
{
    private ICryptoTransform crypter;
    private byte[] key;

    public AesCrypter(byte[] key, byte[] iv) {
        InitCrypter(key, iv);
    }

    private void InitCrypter(byte[] aesKey, byte[] iv) {
        if (iv.Length != 4) {
            throw new ArgumentException("IV should be 4 bytes");
        }

        // init AES
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Key = aesKey;
        this.crypter = aes.CreateEncryptor();

        // init key
        byte[] key = new byte[16];
        for (int i = 0; i < 4; ++i) {
            iv.CopyTo(key, 4 * i);
        }
        this.key = key;
    }

    private void ExpendKey(int length) {
        if (length <= this.key.Length) {
            return;
        }

        int oldLength = this.key.Length;
        int newLength = (int)Math.Ceiling(length / 16.0) * 16;
        byte[] block = new byte[16];
        byte[] newKey = new byte[newLength];

        this.key.CopyTo(newKey, 0);
        Array.Copy(this.key, this.key.Length - 16, block, 0, 16);

        for (var i = oldLength; i < newLength; i += 16) {
            block = this.crypter.TransformFinalBlock(block, 0, 16);
            Array.Copy(block, 0, newKey, i, 16);
        }
        this.key = newKey;
    }

    public void Transform(byte[] data) {
        if (this.key.Length < data.Length) {
            this.ExpendKey(data.Length);
        }
        for (var i = 0; i < data.Length; ++i) {
            data[i] ^= this.key[i];
        }
    }
}