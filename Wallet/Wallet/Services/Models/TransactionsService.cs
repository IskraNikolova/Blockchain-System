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
