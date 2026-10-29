using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class ClickerView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button collectButton;
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text energyText;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectClip;
    [SerializeField, Range(0f, 0.3f)] private float pitchVariation = 0.08f;
    
    [Header("Particle")]
    [SerializeField] private ParticleSystem collectParticles;
    [SerializeField, Min(1)] private int particleCount = 15;

    private Transform buttonTransform;
    private Vector3 buttonBaseScale;

    public event Action onCollectClicked;
    
    private FlyingTextPool flyingTextPool;
    private ClickerAnimationConfig animationConfig;

    [Inject]
    public void Construct(FlyingTextPool flyingTextPool,ClickerAnimationConfig animationConfig)
    {
        this.flyingTextPool = flyingTextPool;
        this.animationConfig = animationConfig;
    }

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

    public void PlayCollectEffect(int reward)
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        PlayButtonPunch();
        PlaySound();
        PlayParticles();
        PlayFlyingText(reward);
    }

    private void PlayFlyingText(int reward)
    {
        FlyingText item = flyingTextPool.Spawn();
        item.Play($"+{reward}", buttonTransform.position, () => flyingTextPool.Despawn(item));
    }

    private void PlayParticles()
    {
        collectParticles.transform.position = buttonTransform.position;
        collectParticles.Emit(particleCount);
    }

    private void PlayButtonPunch()
    {
        buttonTransform.DOKill();
        buttonTransform.localScale = buttonBaseScale;

        buttonTransform
            .DOPunchScale(
                Vector3.one * animationConfig.PunchStrength,
                animationConfig.PunchDuration,
                animationConfig.PunchVibrato,
                animationConfig.PunchElasticity)
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