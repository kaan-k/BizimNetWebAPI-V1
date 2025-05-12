using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace Core.Utilities.Security.Encryption.SHA256
{
    public static class PasswordHasher
    {
        public static string ComputeSHA256Hash(string input)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert byte to hex string
                }

                return builder.ToString();
            }
        }
    }
}
