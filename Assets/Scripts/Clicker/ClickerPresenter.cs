using System;
using Zenject;

public sealed class ClickerPresenter : IInitializable, IDisposable
{
    private readonly ClickerModel model;
    private readonly ClickerView view;
    private readonly ClickerConfig config;
    private readonly AutoCollector autoCollector;

    public ClickerPresenter(ClickerModel model, ClickerView view, ClickerConfig config, AutoCollector autoCollector)
    {
        this.model = model;
        this.view = view;
        this.config = config;
        this.autoCollector = autoCollector;
    }

    public void Initialize()
    {
        view.onCollectClicked += OnCollectClicked;
        autoCollector.onTriggered += OnAutoCollectTriggered;
        model.onCurrencyChanged += OnCurrencyChanged;
        model.onEnergyChanged += OnEnergyChanged;

        OnCurrencyChanged(model.Currency);
        OnEnergyChanged(model.Energy);
    }

    public void Dispose()
    {
        view.onCollectClicked -= OnCollectClicked;
        autoCollector.onTriggered -= OnAutoCollectTriggered;
        model.onCurrencyChanged -= OnCurrencyChanged;
        model.onEnergyChanged -= OnEnergyChanged;
    }

    private void OnCollectClicked()
    {
        TryCollect(config.ClickEnergyCost);
    }
    
    private void OnAutoCollectTriggered()
    {
        TryCollect(config.AutoCollectEnergyCost);
    }

    private void TryCollect(int energyCost)
    {
        if (!model.TrySpendEnergy(energyCost))
        {
            return;
        }

        model.AddCurrency(config.RewardPerClick);
        view.PlayCollectEffect();
    }

    private void OnCurrencyChanged(int value)
    {
        view.SetCurrency(value);
    }

    private void OnEnergyChanged(int value)
    {
        view.SetEnergy(value, model.MaxEnergy);
    }
}