using System;
using System.Threading.Tasks;
using GameCore.Services;
using UnityEngine;
using UnityHttpWrapper;

namespace ImageLoading
{
    public class PicsumService : MonoBehaviour, ILoadImage
    {
        private const string Url = "https://picsum.photos";

        private IHttpClient _client;

        public void Resolve(IHttpClient client)
        {
            _client = client;
        }

        private void Awake()
        {
            _client = GetComponent<IHttpClient>();
        }

        public async Task<Texture2D> LoadRandomTexture(int width, int height)
        {
            var url = $"{Url}/{width}/{height}";
            Task<byte[]> request = _client.SendRequest(url);

            try
            {
                await request;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (request.Result.Length == 0)
            {
                Debug.LogError("Network error!");
                return GenerateFailedTexture(width, height);
            }

            var texture = new Texture2D(width, height);
            texture.LoadImage(request.Result);
            return texture;
        }

        public async void LoadRandomTexture(int width, int height, Action<Texture2D> onCompleted)
        {
            var url = $"{Url}/{width}/{height}";
            Task<byte[]> request = _client.SendRequest(url);

            try
            {
                await request;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var texture = new Texture2D(width, height);
            texture.LoadImage(request.Result);
            onCompleted.Invoke(texture);
        }

        private static Texture2D GenerateFailedTexture(int width, int height)
        {
           return new Texture2D(width, height);
        }
    }
}