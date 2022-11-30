using System.Threading.Tasks;

namespace UnityHttpWrapper
{
    public interface IHttpClient
    {
        Task<byte[]> SendRequest(string uri);
    }
}