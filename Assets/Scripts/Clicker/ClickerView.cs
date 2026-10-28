using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ClickerView : MonoBehaviour
{
    [SerializeField] private Button collectButton;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text energyText;

    public event Action onCollectClicked;

    private void Awake()
    {
        collectButton.onClick.AddListener(OnCollectButtonClicked);
    }

    private void OnDestroy()
    {
        collectButton.onClick.RemoveListener(OnCollectButtonClicked);
    }

    public void SetCurrency(int value)
    {
        currencyText.text = $"Currency: {value}";
    }

    public void SetEnergy(int value, int max)
    {
        energyText.text = $"Energy: {value} / {max}";
    }

    public void PlayCollectEffect()
    {
        // TODO анимация, звук, частицы и +1
    }

    private void OnCollectButtonClicked()
    {
        onCollectClicked?.Invoke();
    }
}