using System;
using System.Security.Cryptography;
using System.Text;

class HmacCalculator
{
    public string CalculateHmac(string message, byte[] key)
    {
        using (var hmac = new HMACSHA256(key))
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
