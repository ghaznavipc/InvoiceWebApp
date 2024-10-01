using System.Security.Cryptography;

namespace Common.Utilities;

public static class SecurityHelper
{
    public static string GetSha256Hash(string input)
    {
        //using (var sha256 = new SHA256CryptoServiceProvider())
        using (var sha256 = SHA256.Create())
        {
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = sha256.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
            //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
        }
    }

    public static string GetVerifyCode()
    {
        int _min = 1000;
        int _max = 9999;
        Random _rdm = new Random();
        return _rdm.Next(_min, _max).ToString();
    }
    public static string GenerateCode4()
    {
        int min = 1000, max = 9999;
        Random r = new Random();
        return r.Next(min, max).ToString();
    }
}
