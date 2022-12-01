using System;
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

            if (request.result == UnityWebRequest.Result.Success)
                return request.downloadHandler.data;

            return Array.Empty<byte>();
        }
    }
}