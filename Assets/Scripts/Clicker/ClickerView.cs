using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ClickerView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button collectButton;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text energyText;

    [Header("Animation")]
    [SerializeField] private float punchStrength = 0.15f;
    [SerializeField] private float punchDuration = 0.25f;
    [SerializeField] private int punchVibrato = 8;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectClip;
    [SerializeField, Range(0f, 0.3f)] private float pitchVariation = 0.08f;

    private Transform buttonTransform;
    private Vector3 buttonBaseScale;

    public event Action onCollectClicked;

    private void Awake()
    {
        buttonTransform = collectButton.transform;
        buttonBaseScale = buttonTransform.localScale;

        collectButton.onClick.AddListener(OnCollectButtonClicked);
    }

    private void OnDestroy()
    {
        collectButton.onClick.RemoveListener(OnCollectButtonClicked);
        buttonTransform.DOKill();
    }

    public void SetCurrency(int value)
    {
        currencyText.text = $"Coins: {value}";
    }

    public void SetEnergy(int value, int max)
    {
        energyText.text = $"Energy: {value} / {max}";
    }

    public void PlayCollectEffect()
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        PlayButtonPunch();
        PlaySound();
    }

    private void PlayButtonPunch()
    {
        buttonTransform.DOKill();
        buttonTransform.localScale = buttonBaseScale;

        buttonTransform
            .DOPunchScale(Vector3.one * punchStrength, punchDuration, punchVibrato, 0.5f)
            .SetLink(gameObject);
    }

    private void PlaySound()
    {
        audioSource.pitch = 1f + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(collectClip);
    }

    private void OnCollectButtonClicked()
    {
        onCollectClicked?.Invoke();
    }
}