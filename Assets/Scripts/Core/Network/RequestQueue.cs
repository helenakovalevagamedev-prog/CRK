using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class RequestQueue : IRequestQueue, IDisposable
{
    private readonly LinkedList<QueueItem> pending = new();
    private QueueItem current;
    private bool isProcessing;

    public UniTask<T> Enqueue<T>(string tag, Func<CancellationToken, UniTask<T>> request, CancellationToken externalToken = default)
    {
        var item = new QueueItem<T>(tag, request, externalToken);

        item.OnCanceled(() =>
        {
            pending.Remove(item);
            item.MarkCanceled();
        });

        pending.AddLast(item);

        if (!isProcessing)
        {
            ProcessAsync().Forget();
        }

        return item.Task;
    }

    public void Cancel(string tag)
    {
        var toCancel = new List<QueueItem>();

        foreach (var item in pending)
        {
            if (item.Tag == tag)
            {
                toCancel.Add(item);
            }
        }

        if (current != null && current.Tag == tag)
        {
            toCancel.Add(current);
        }

        foreach (var item in toCancel)
        {
            item.Cancel();
        }
    }

    public void Dispose()
    {
        var all = new List<QueueItem>(pending);

        if (current != null)
        {
            all.Add(current);
        }

        foreach (var item in all)
        {
            item.Cancel();
        }
    }

    private async UniTaskVoid ProcessAsync()
    {
        isProcessing = true;

        while (pending.Count > 0)
        {
            current = pending.First.Value;
            pending.RemoveFirst();

            if (!current.IsCanceled)
            {
                try
                {
                    await current.RunAsync();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            var finished = current;
            current = null;
            finished.Dispose();
        }

        isProcessing = false;
    }
}