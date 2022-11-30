using System;
using System.Threading.Tasks;
using GameCore.Services;
using UnityEngine;
using UnityHttpWrapper;

namespace ImageLoading
{
    public class PicsumService : MonoBehaviour, ILoadImage
    {
        private IHttpClient _client;

        public void Resolve(IHttpClient client)
        {
            _client = client;
        }

        public async Task<Texture2D> LoadRandomTexture(int width, int height)
        {
            var uri = $"https://picsum.photos/{width}/{height}";
            Task<byte[]> request = _client.SendRequest(uri);

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
            return texture;
        }
    }
}