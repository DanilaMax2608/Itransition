using System.Security.Cryptography;

class KeyGenerator
{
    public byte[] GenerateKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[32]; // 256 bits
            rng.GetBytes(key);
            return key;
        }
    }
}
