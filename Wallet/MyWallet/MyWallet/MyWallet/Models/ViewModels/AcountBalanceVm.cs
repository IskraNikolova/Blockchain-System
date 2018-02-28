using MyWallet.Models.ViewModels;

namespace Wallet.Models.ViewModels
{
    public class AcountBalanceVm
    {
        public string Address { get; set; }

        public int Confirmations { get; set; }

        public BalanceD ConfirmedBalance { get; set; }

        public BalanceD LastMinedBalance { get; set; }

        public BalanceD PendingBalance { get; set; }
    }
}
