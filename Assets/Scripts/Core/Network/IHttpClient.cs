using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IHttpClient
{
    UniTask<string> GetTextAsync(string url, CancellationToken token);
    UniTask<Texture2D> GetTextureAsync(string url, CancellationToken token);
}
