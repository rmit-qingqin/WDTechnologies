using System;
using System.Security.Cryptography;
namespace WebServiceAndDatabaseExample
{
    public class PBKDF2
    {
        private int hashBytes;
        private int saltBytes;
        private int iterations;
        //if the salt and iteration are permanently set, then change the encryption method so that can only out put a hashed password without other information
        public PBKDF2(int _hashBytes, int _saltBytes, int _iterations)
        {
            hashBytes = _hashBytes;
            saltBytes = _saltBytes;
            iterations = _iterations;
        }

        public string createHash(string password)
        {
            byte[] salt = new byte[saltBytes];
            Random random = new Random();
            random.NextBytes(salt);
            Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(password, salt, iterations);
            return "sha1:" + Convert.ToString(iterations) + ":" + Convert.ToString(hashBytes) + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash.GetBytes(hashBytes));
        }

        public Boolean verifyHash(string password, string hashString)
        {
           string[] frames = hashString.Split(':');
           string _algorithm = frames[0];
           int _iterations = Convert.ToInt32(frames[1]);
           int _hashBytes = Convert.ToInt32(frames[2]);
           byte[] salt = Convert.FromBase64String(frames[3]);
            string hash = frames[4].Trim();
            string generatedHash = Convert.ToBase64String(new Rfc2898DeriveBytes(password, salt, _iterations).GetBytes(_hashBytes));
            return generatedHash.Equals(hash);
        }
    }
}

// Example usage
/*public class Program
{
    public static void Main()
    {
        PBKDF2 pkbdf2 = new PBKDF2(64, 32, 50000);
        string hash = pkbdf2.createHash("abc123");
        Boolean verify = pkbdf2.verifyHash("abc123", hash);
        Console.WriteLine("hash: " + hash);
        Console.WriteLine("Password matches hash: " + verify);
    }
}*/
