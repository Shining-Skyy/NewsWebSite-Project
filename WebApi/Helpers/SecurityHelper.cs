using System.Security.Cryptography;
using System.Text;

namespace WebApi.Helpers
{
    public class SecurityHelper
    {
        // This method takes a string input and returns its SHA-256 hash as a Base64 encoded string.
        public string Getsha256Hash(string value)
        {
            // Create an instance of the SHA256CryptoServiceProvider to perform the hashing.
            var algoritm = new SHA256CryptoServiceProvider();

            // Convert the input string to a byte array using UTF-8 encoding.
            var byteValue = Encoding.UTF8.GetBytes(value);

            // Compute the hash of the byte array, resulting in another byte array.
            var byteHash = algoritm.ComputeHash(byteValue);

            // Convert the byte array hash to a Base64 string for easier representation.
            return Convert.ToBase64String(byteHash);
        }
    }
}
