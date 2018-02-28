namespace MyFaucet.Services.Interfaces
{
    using MyFaucet.Models;

    public interface IHttpRequestService
    {
        ResponseModel Post(string resURL, SendTransactionBody data);
    }
}