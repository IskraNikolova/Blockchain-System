namespace MyFaucet.Services.Interfaces
{
    using MyFaucet.Models;

    public interface ITransactionsService
    {
        SendTransactionBody CreateAndSignTransaction(string recipientAddress, int value, int fee,
                              string iso8601datetime, string senderPrivKeyHex);
    }
}
