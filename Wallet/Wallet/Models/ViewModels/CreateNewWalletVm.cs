namespace Wallet.Models.ViewModels
{
    using System.ComponentModel;

    public class CreateNewWalletVm
    {
        [DisplayName("Generated random private key:")]
        public string PrivateKey { get; set; }

        [DisplayName("Extracted public key:")]
        public string PublicKey { get; set; }

        [DisplayName("Extracted blockcain address")]
        public string Address { get; set; }

        public string Info { get; set; }
    }
}
