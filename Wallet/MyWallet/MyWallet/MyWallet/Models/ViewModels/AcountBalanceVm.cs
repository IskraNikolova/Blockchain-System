using MyWallet.Models.ViewModels;

namespace Wallet.Models.ViewModels
{
    public class AcountBalanceVm
    {
        public string Address { get; set; }

        public int Confirmations { get; set; }

        public Balance ConfirmedBalance { get; set; }

        public Balance LastMinedBalance { get; set; }

        public Balance PendingBalance { get; set; }
    }
}
