using System.Threading.Tasks;
using UnityEngine.Networking;

namespace UnityHttpWrapper
{
    public class HttpClient : IHttpClient
    {
        public async Task<byte[]> SendRequest(string uri)
        {
            var request = new UnityWebRequest(uri);
            await request.SendWebRequest();
            return request.downloadHandler.data;
        }
    }
}