using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApiRest.Helper
{
    public class HashHelper
    {
        public static HashedPassword Hash(string password)
        {
            byte[] token = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
            }

            //derive a 256-bit subkey (use HMACShA with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: token,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return new HashedPassword() { Password = hashed, Token = Convert.ToBase64String(token) };
        }

        public static bool CheckHash(string attemptePassword, string hash, string token)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: attemptePassword,
                salt: Convert.FromBase64String(token),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hash == hashed;
        }

        public static byte[] GetHash(string password, string token)
        {
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(string.Concat(token, password));
            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);
            return hashedBytes;
        }
    }

    public class HashedPassword
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
