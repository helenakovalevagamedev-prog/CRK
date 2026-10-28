using UnityEngine;
using Zenject;

public sealed class EnergyRegenerator : ITickable
{
    private readonly ClickerModel model;
    private readonly ClickerConfig config;

    private float elapsed;

    public EnergyRegenerator(ClickerModel model, ClickerConfig config)
    {
        this.model = model;
        this.config = config;
    }

    public void Tick()
    {
        elapsed += Time.deltaTime;

        while (elapsed >= config.EnergyRegenInterval)
        {
            elapsed -= config.EnergyRegenInterval;
            model.AddEnergy(config.EnergyRegenAmount);
        }
    }
}