namespace MyFaucet.Services.Models
{
    using System.Text;
    using System.Linq;
    using Org.BouncyCastle.Asn1.X9;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Crypto.Digests;
    using Newtonsoft.Json;
    using Org.BouncyCastle.Crypto.Signers;
    using MyFaucet.Services.Interfaces;
    using MyFaucet.Models;

    public class TransactionsService : ITransactionsService
    {
        private readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
       

        public SendTransactionBody CreateAndSignTransaction(string recipientAddress, int value, int fee,
                              string iso8601datetime, string senderPrivKeyHex)
        {
            BigInteger privateKey = new BigInteger(senderPrivKeyHex, 16);

            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);
            string senderPubKeyCompressed = EncodeECPointHexCompressed(pubKey);
            string senderAddress = CalcRipeMD160(senderPubKeyCompressed);

            var tran = new
            {
                from = senderAddress,
                to = recipientAddress,
                senderPubKey = senderPubKeyCompressed,
                value = value,
                fee = fee,
                dateCreated = iso8601datetime
            };

            string tranJson = JsonConvert.SerializeObject(tran);
            byte[] tranHash = CalcSHA256(tranJson);
            BigInteger[] tranSignature = SignData(privateKey, tranHash);

            SendTransactionBody tranSigned = new SendTransactionBody
            {
                From = senderAddress,
                To = recipientAddress,
                Value = value,
                Fee = fee,
                DateCreated = iso8601datetime,
                SenderPubKey = senderPubKeyCompressed,
                SenderSignature = new string[]
                {
                tranSignature[0].ToString(16),
                tranSignature[1].ToString(16)
                }
            };

            //string signedTranJson = JsonConvert.SerializeObject(tranSigned, Formatting.Indented);
            return tranSigned;
        }

        /// <summary>
        /// Calculates deterministic ECDSA signature (with HMAC-SHA256), based on secp256k1 and RFC-6979.
        /// </summary>
        private BigInteger[] SignData(BigInteger privateKey, byte[] data)
        {
            ECDomainParameters ecSpec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            ECPrivateKeyParameters keyParameters = new ECPrivateKeyParameters(privateKey, ecSpec);
            IDsaKCalculator kCalculator = new HMacDsaKCalculator(new Sha256Digest());
            ECDsaSigner signer = new ECDsaSigner(kCalculator);
            signer.Init(true, keyParameters);
            BigInteger[] signature = signer.GenerateSignature(data);
            return signature;
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

        private  ECPoint GetPublicKeyFromPrivateKey(BigInteger privKey)
        {
            ECPoint pubKey = curve.G.Multiply(privKey).Normalize();
            return pubKey;
        }


        private string EncodeECPointHexCompressed(ECPoint point)
        {
            BigInteger x = point.XCoord.ToBigInteger();
            return x.ToString(16) + System.Convert.ToInt32(!x.TestBit(0));
        }

        private string CalcRipeMD160(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            RipeMD160Digest digest = new RipeMD160Digest();
            digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return BytesToHex(result);
        }

        private string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }
    }
}
