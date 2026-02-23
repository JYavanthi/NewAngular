/*namespace sanchar6tBackEnd.Helpers
{
    public class OTPGenerator
    {
    }
}
*/
using System.Security.Cryptography;

public class OTPGenerator
{
    public static string GenerateOTP(int length = 6)
    {
        const string chars = "0123456789";
        var otp = new char[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            var data = new byte[length];
            rng.GetBytes(data);
            for (int i = 0; i < length; i++)
                otp[i] = chars[data[i] % chars.Length];
        }
        return new string(otp);
    }
}
