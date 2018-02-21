using System;

namespace Wallet.Models.ViewModels
{
    public class SendTransactionBody
    {
        public string From { get; set; }

        public string To { get; set; }

        public int Value { get; set; }

        public int Fee { get; set; }

        public string DateCreated { get; set; }

        public string SenderPubKey { get; set; }

        public string[] SenderSignature { get; set; }
    }
}
