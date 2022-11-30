using System.Threading.Tasks;
using UnityEngine;
using UnityHttpWrapper;

public class PicsumService : MonoBehaviour
{
    private IHttpClient _client;

    public void Resolve(IHttpClient client)
    {
        _client = client;
    }

    public async Task<Texture2D> LoadRandomTexture(int width, int height)
    {
        string uri = $"https://picsum.photos/{width}/{height}";
        Task<byte[]> request = _client.SendRequest(uri);
        
        await request;
        
        Debug.Log(request.Result.Length);
        var texture = new Texture2D(width, height);
        texture.LoadImage(request.Result);
        return texture;
    }
}