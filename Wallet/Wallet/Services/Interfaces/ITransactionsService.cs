namespace Wallet.Services.Interfaces
{
    using Wallet.Models.ViewModels;

    public interface ITransactionsService
    {
        CreateNewWalletVm RandomPrivateKeyToAddress();

        OpenExistingWalletVm ExistingPrivateKeyToAddress(string privKeyHex);
    }
}
