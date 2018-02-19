namespace Wallet.Services.Models
{
    using Wallet.Services.Interfaces;
    using System.Text;
    using System.Linq;
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
    using Wallet.Models.ViewModels;

    public class TransactionsService : ITransactionsService
    {
        private readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
        public CreateNewWalletVm RandomPrivateKeyToAddress()
        {
            var keyPair = GenerateRandomKeys();
            BigInteger privateKey = ((ECPrivateKeyParameters)keyPair.Private).D;
            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);//((ECPublicKeyParameters)keyPair.Public).Q;
            string pubKeyCompressed = EncodeECPointHexCompressed(pubKey);
            string address = CalcRipeMD160(pubKeyCompressed);

            CreateNewWalletVm newWallet = new CreateNewWalletVm
            {
                PrivateKey = privateKey.ToString(16),
                PublicKey = pubKeyCompressed,
                Address = address,
                Info = $"\nGenerated random private key:\n{privateKey.ToString(16)}\nExtracted public key:\n{pubKeyCompressed}\nExtracted blockchain address:\n{address}"
            };

            return newWallet;
        }

        public OpenExistingWalletVm ExistingPrivateKeyToAddress(string privKeyHex)
        {
            //todo check for null parameters
            BigInteger privateKey = new BigInteger(privKeyHex, 16);

            ECPoint pubKey = GetPublicKeyFromPrivateKey(privateKey);

            string pubKeyCompressed = EncodeECPointHexCompressed(pubKey);

            string address = CalcRipeMD160(pubKeyCompressed);

            OpenExistingWalletVm model = new OpenExistingWalletVm
            {
                PrivateKey = privateKey.ToString(16),
                PublicKey = pubKeyCompressed,
                Address = address,
                Info = $"\nDecoded existing private key:\n{privateKey.ToString(16)}\nExtracted public key:\n{pubKeyCompressed}\nExtracted blockchain address:\n{address}"
            };

            return model;
        }

        public string CreateAndSignTransaction(string recipientAddress, int value, int fee,
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

            var tranSigned = new
            {
                from = senderAddress,
                to = recipientAddress,
                value = value,
                dateCreated = iso8601datetime,
                senderPubKey = senderPubKeyCompressed,
                senderSignature = new string[]
                {
                tranSignature[0].ToString(16),
                tranSignature[1].ToString(16)
                }
            };

            string signedTranJson = JsonConvert.SerializeObject(tranSigned, Formatting.Indented);
            return signedTranJson;
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

        private AsymmetricCipherKeyPair GenerateRandomKeys(int keySize = 256)
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            KeyGenerationParameters keyGenParam =
                new KeyGenerationParameters(secureRandom, keySize);
            gen.Init(keyGenParam);
            return gen.GenerateKeyPair();
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
