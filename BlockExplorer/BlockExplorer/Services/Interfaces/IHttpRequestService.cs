namespace BlockExplorer.Services.Interfaces
{
    public interface IHttpRequestService
    {
        T Get<T>(string resUrl);
    }
}