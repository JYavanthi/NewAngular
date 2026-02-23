using System;
using System.Security.Cryptography;
using System.Text;

namespace sanchar6tBackEnd.Helpers
{
    public static class PhonePeHelper
    {
        public static string GenerateXVerify(string jsonPayload, string merchantKey)
        {
            string base64Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPayload)).TrimEnd('=');

            string dataToHash = base64Payload + "/pg/v1/pay/" + "3013c44a-99b1-4482-88b7-b1387e079b49";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

