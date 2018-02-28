using System.Threading.Tasks;
using Wallet.Models.ViewModels;

namespace Wallet.Services.Interfaces
{
    public interface IHttpRequestService
    {
        T Get<T>(string resUrl);

        ResponseSentTransactionVm Post(string resURL, SendTransactionBody data);
    }
}