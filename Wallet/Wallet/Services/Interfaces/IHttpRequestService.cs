using System.Threading.Tasks;

namespace Wallet.Services.Interfaces
{
    public interface IHttpRequestService
    {
        T Get<T>(string resUrl);

        T Pots<T>(string resURL, object data);
    }
}