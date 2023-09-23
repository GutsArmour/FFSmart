using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace FFsmart.Services
{
    public class PasscodeService
    {
        private int saltSize = 16;
        private int hashSize = 20;
        private int iterations = 100000;

        // https://stackoverflow.com/questions/3253157/generation-9-digits-random-number-including-leading-zero
        public string GeneratePasscode()
        {
            int length = 9;
            Random range = new Random();

            char[] digits = new char[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = (char)(range.Next(10) + '0');
            }
            return new string(digits);
        }

        // https://stackoverflow.com/questions/4181198/how-to-hash-a-password
        public string HashPasscode(string passcode)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Create salt
                byte[] salt;
                rng.GetBytes(salt = new byte[saltSize]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(passcode, salt, iterations))
                {
                    // Create hash
                    byte[] hash = pbkdf2.GetBytes(hashSize);

                    // Combine salt and hash
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, saltSize);
                    Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public bool VerifyHashed(string passcode, string hashedCode)
        {
            var hashBytes = Convert.FromBase64String(hashedCode);
            
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(passcode, salt, iterations))
            {
                var hash = pbkdf2.GetBytes(hashSize);

                for (int i=0; i<hashSize; i++)
                {
                    if (hashBytes[i+saltSize] != hash[i]) { return false; }
                }

                return true;
            }
        }
    }
}
