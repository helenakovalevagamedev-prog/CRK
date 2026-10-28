using System;
using UnityEngine;
using Zenject;

public sealed class AutoCollector : ITickable
{
    private readonly ClickerConfig config;

    private float elapsed;

    public AutoCollector(ClickerConfig config)
    {
        this.config = config;
    }

    public event Action onTriggered;

    public void Tick()
    {
        elapsed += Time.deltaTime;

        while (elapsed >= config.AutoCollectInterval)
        {
            elapsed -= config.AutoCollectInterval;
            onTriggered?.Invoke();
        }
    }
}