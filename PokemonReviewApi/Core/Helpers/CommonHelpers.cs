using System.Security.Cryptography;
using System.Text;

namespace PokemonReviewApi.Core.Helpers
{
    public class CommonHelpers
    {
        public static string Sha256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }

        public static string GenerateSecureRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringBuilder = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[sizeof(uint)];

                while (stringBuilder.Length < length)
                {
                    rng.GetBytes(byteBuffer);
                    uint num = BitConverter.ToUInt32(byteBuffer, 0);
                    stringBuilder.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
