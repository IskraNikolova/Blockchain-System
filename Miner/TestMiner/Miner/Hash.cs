namespace Miner
{
    using System.Security.Cryptography;
    using System.Text;

    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Crypto.Digests;
    using Newtonsoft.Json;
    using Org.BouncyCastle.Asn1.Ocsp;
    using Org.BouncyCastle.Crypto.Signers;

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

        private byte[] CalcSHA256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return result;
        }
    }
}