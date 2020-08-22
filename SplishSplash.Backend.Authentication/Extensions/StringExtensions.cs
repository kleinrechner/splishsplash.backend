using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.Authentication.Extensions
{
    public static class StringExtensions
    {
        public static string GetMD5Hash(this string input)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
