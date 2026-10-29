using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class UnityHttpClient : IHttpClient
{
    private const string UserAgent = "UnityTestTask/1.0";

    public async UniTask<string> GetTextAsync(string url, CancellationToken token)
    {
        using var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("User-Agent", UserAgent);

        await request.SendWebRequest().WithCancellation(token);

        return request.downloadHandler.text;
    }

    public async UniTask<Texture2D> GetTextureAsync(string url, CancellationToken token)
    {
        using var request = UnityWebRequestTexture.GetTexture(url);
        request.SetRequestHeader("User-Agent", UserAgent);

        await request.SendWebRequest().WithCancellation(token);

        return DownloadHandlerTexture.GetContent(request);
    }
}