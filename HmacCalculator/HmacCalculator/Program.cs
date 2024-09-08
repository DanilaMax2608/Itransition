using System;
using System.Security.Cryptography;
using System.Text;

class HmacCalculator
{
    public static string CalculateHmac(string keyHex, string message)
    {
        byte[] key = HexStringToByteArray(keyHex);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hmac = new HMACSHA256(key))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }
    }

    private static byte[] HexStringToByteArray(string hex)
    {
        int length = hex.Length;
        byte[] bytes = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}

class Program
{
    static void Main()
    {
        string hmacKey = " "; //Write HMAC key
        string computerMove = " "; //Write the movement of the computer
        string hmacResult = HmacCalculator.CalculateHmac(hmacKey, computerMove);
        Console.WriteLine($"Calculated HMAC: {hmacResult}");
        Console.ReadKey();
    }
}
