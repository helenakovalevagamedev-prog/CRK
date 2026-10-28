using UnityEngine;

[CreateAssetMenu(fileName = "ClickerConfig", menuName = "Configs/Clicker Config")]
public sealed class ClickerConfig : ScriptableObject
{
    [Header("Currency")]
    [SerializeField, Min(0)] private int startCurrency = 0;
    [SerializeField, Min(1)] private int rewardPerClick = 1;

    [Header("Energy")]
    [SerializeField, Min(1)] private int maxEnergy = 1000;
    [SerializeField, Min(0)] private int startEnergy = 1000;
    [SerializeField, Min(0)] private int clickEnergyCost = 1;
    [SerializeField, Min(0)] private int autoCollectEnergyCost = 1;

    [Header("Recharge")]
    [SerializeField, Min(1)] private int energyRegenAmount = 10;
    [SerializeField, Min(0.1f)] private float energyRegenInterval = 10f;

    [Header("AutoCollect")]
    [SerializeField, Min(0.1f)] private float autoCollectInterval = 3f;

    public int StartCurrency => startCurrency;
    public int RewardPerClick => rewardPerClick;

    public int MaxEnergy => maxEnergy;
    public int StartEnergy => startEnergy;
    public int ClickEnergyCost => clickEnergyCost;
    public int AutoCollectEnergyCost => autoCollectEnergyCost;

    public int EnergyRegenAmount => energyRegenAmount;
    public float EnergyRegenInterval => energyRegenInterval;

    public float AutoCollectInterval => autoCollectInterval;
}