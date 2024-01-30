using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Utility class for common operations
    public static class Utils
    {
        // Delimiter for separating segments in the hashed string
        private const char _segmentDelimiter = ':';

        // Hash a secret using PBKDF2
        public static string HashSecret(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;   
            var keySize = 32;
            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            // Compute the hash using PBKDF2
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            // Combine segments into a string
            return string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }

        // Verify a hashed string against an input
        public static bool VerifyHash(string input, string hashString)
        {
            // Split the hashed string into segments
            string[] segments = hashString.Split(_segmentDelimiter);
            // Extract hash, salt, iterations, and algorithm from segments
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);
            // Compute the hash of the input using the same parameters
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            // Compare the computed hash with the stored hash using constant time equality
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        // Get the application directory path and create if it doesn't exist
        public static string GetAppDirectoryPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BisleriumCafe_JSON");
            // Create the directory if it doesn't exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        // Get the file path for storing user information
        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        // Get the file path for storing coffee information
        public static string GetCoffeesFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "_coffees.json");
        }

        // Get the file path for storing add-ins list information
        public static string GetAddinsListFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "_addInsList.json");
        }

        // Get the file path for storing orders list information
        public static string GetOrdersListFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "_ordersList.json");
        }

        // Get the file path for storing customer list information
        public static string GetCustomersListFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "_customerList.json");
        }
    }
}
