using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public interface IRequestQueue
{
    public UniTask<T> Enqueue<T>(string tag, Func<CancellationToken, UniTask<T>> request, CancellationToken externalToken = default);
    public void Cancel(string tag);
}