using System;
using System.Threading;
using Cysharp.Threading.Tasks;

internal sealed class QueueItem<T> : QueueItem
{
    private readonly Func<CancellationToken, UniTask<T>> request;
    private readonly UniTaskCompletionSource<T> completion = new();

    public QueueItem(string tag, Func<CancellationToken, UniTask<T>> request, CancellationToken externalToken)
        : base(tag, externalToken)
    {
        this.request = request;
    }

    public UniTask<T> Task => completion.Task;

    public override async UniTask RunAsync()
    {
        try
        {
            T result = await request(Token);
            completion.TrySetResult(result);
        }
        catch (OperationCanceledException)
        {
            completion.TrySetCanceled();
        }
        catch (Exception e)
        {
            completion.TrySetException(e);
        }
    }

    public override void MarkCanceled()
    {
        completion.TrySetCanceled();
    }
}