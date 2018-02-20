namespace Wallet.Models.ViewModels
{
    using System;

    public class SignTransactionVm
    {
        public string From { get; set; }

        public string To { get; set; }

        public int Value { get; set; }

        public int Fee { get; set; }

        public DateTime DateCreated { get; set; }

        public string SenderPubKey { get; set; }

        public string Info { get; set; }

        public string Response { get; set; }
    }
}
