using System;
using System.Security.Cryptography;
using System.Text;


namespace Abl.Security
{
    public static class MD5Utils
    {
        public static string GetMd5Hash(string src)
        {
            using (var md5 = MD5.Create())
            {
                return GetMd5Hash(md5, src);
            }
        }


        public static string GetMd5Hash(MD5 md5Hash, string src)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(src));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        // Verify a hash against a string. 
        public static bool VerifyMd5Hash(MD5 md5Hash, string src, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, src);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}