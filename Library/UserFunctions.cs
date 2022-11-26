using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace EcommerceMVC.Library
{
    public static class UserFunctions
    {
        public static string PasswordHasher(string password)
        {
            byte[] salt = Encoding.Unicode.GetBytes("NZsP6NnmfBuYeJrrAKNuVQ==");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
