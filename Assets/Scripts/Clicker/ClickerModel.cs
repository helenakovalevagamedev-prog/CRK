using System;
using UnityEngine;

public sealed class ClickerModel
{
    public ClickerModel(ClickerConfig config)
    {
        MaxEnergy = config.MaxEnergy;
        Currency = config.StartCurrency;
        Energy = Mathf.Min(config.StartEnergy, MaxEnergy);
    }

    public event Action<int> onCurrencyChanged;
    public event Action<int> onEnergyChanged;

    public int Currency { get; private set; }
    public int Energy { get; private set; }
    public int MaxEnergy { get; }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        onCurrencyChanged?.Invoke(Currency);
    }

    public bool TrySpendEnergy(int amount)
    {
        if (Energy < amount)
        {
            return false;
        }

        Energy -= amount;
        onEnergyChanged?.Invoke(Energy);
        return true;
    }

    public void AddEnergy(int amount)
    {
        int newEnergy = Mathf.Min(Energy + amount, MaxEnergy);

        if (newEnergy == Energy)
        {
            return;
        }

        Energy = newEnergy;
        onEnergyChanged?.Invoke(Energy);
    }
}