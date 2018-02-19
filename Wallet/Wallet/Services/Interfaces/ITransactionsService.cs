namespace Wallet.Services.Interfaces
{
    using Wallet.Models.ViewModels;

    public interface ITransactionsService
    {
        CreateNewWalletVm RandomPrivateKeyToAddress();

        OpenExistingWalletVm ExistingPrivateKeyToAddress(string privKeyHex);

        string CreateAndSignTransaction(string recipientAddress, int value, int fee,
                              string iso8601datetime, string senderPrivKeyHex);
    }
}
