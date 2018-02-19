namespace Wallet.Models.ViewModels
{
    using System;

    public class SendTransactionVm
    {
        public string From { get; set; }

        public string To { get; set; }

        public double Value { get; set; }

        public int Fee { get; set; }

        public string SenderPubKey { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
