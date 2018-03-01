namespace Miner
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Hash
    {
        public static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed sHA256 = new SHA256Managed();

            byte[] hash = sHA256.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }

            return hashString;
        }
    }
}