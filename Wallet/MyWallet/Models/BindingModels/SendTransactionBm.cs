namespace Wallet.Models.BindingModels
{
    public class SendTransactionBm
    {
        public string Sender { get; set; }

        public string Recipient { get; set; }

        public double Value { get; set; }
    }
}
