using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityHttpWrapper
{
    public class HttpClient : MonoBehaviour, IHttpClient
    {
        public async Task<byte[]> SendRequest(string uri)
        {
            var request = new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
            await request.SendWebRequest();
            return request.downloadHandler.data;
        }
    }
}