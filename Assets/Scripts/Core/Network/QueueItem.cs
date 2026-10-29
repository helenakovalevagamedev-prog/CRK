using System;
using System.Threading;
using Cysharp.Threading.Tasks;

internal abstract class QueueItem : IDisposable
{
    private readonly CancellationTokenSource cts;
    private CancellationTokenRegistration registration;

    protected QueueItem(string tag, CancellationToken externalToken)
    {
        Tag = tag;
        cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
    }

    public string Tag { get; }
    public bool IsCanceled => cts.IsCancellationRequested;

    protected CancellationToken Token => cts.Token;

    public void Cancel()
    {
        cts.Cancel();
    }

    public void OnCanceled(Action callback)
    {
        registration = cts.Token.Register(callback);
    }

    public abstract UniTask RunAsync();
    public abstract void MarkCanceled();

    public void Dispose()
    {
        registration.Dispose();
        cts.Dispose();
    }
}