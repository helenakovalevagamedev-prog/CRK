using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public sealed class FlyingText : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private CanvasGroup canvasGroup;

    private ClickerAnimationConfig config;
    private Sequence sequence;

    [Inject]
    public void Construct(ClickerAnimationConfig config)
    {
        this.config = config;
    }

    public void Play(string text, Vector3 worldPosition, Action onFinished)
    {
        sequence?.Kill();

        var rectTransform = (RectTransform)transform;

        label.text = text;
        canvasGroup.alpha = 1f;

        float spread = config.FlyHorizontalSpread;
        rectTransform.position = worldPosition;
        rectTransform.anchoredPosition += new Vector2(UnityEngine.Random.Range(-spread, spread), 0f);

        float targetY = rectTransform.anchoredPosition.y + config.FlyDistance;

        sequence = DOTween.Sequence()
            .Append(rectTransform.DOAnchorPosY(targetY, config.FlyDuration).SetEase(config.FlyMoveEase))
            .Join(canvasGroup.DOFade(0f, config.FlyDuration).SetEase(config.FlyFadeEase))
            .OnComplete(() => onFinished?.Invoke())
            .SetLink(gameObject);
    }

    private void OnDestroy()
    {
        sequence?.Kill();
    }
}