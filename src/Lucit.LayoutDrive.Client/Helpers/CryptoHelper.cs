using System.Security.Cryptography;
using System.Text;

namespace Lucit.LayoutDrive.Client.Helpers
{
    public static class CryptoHelper
    {
        public static string CreateMd5(byte[] inputBytes)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();

                foreach (var @byte in hashBytes)
                {
                    sb.Append(@byte.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}