using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public sealed class FlyingText : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float flyDistance = 250f;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float horizontalSpread = 60f;

    private Sequence sequence;

    public void Play(string text, Vector3 worldPosition, Action onFinished)
    {
        sequence?.Kill();

        var rectTransform = (RectTransform)transform;

        label.text = text;
        canvasGroup.alpha = 1f;

        rectTransform.position = worldPosition;
        rectTransform.anchoredPosition += new Vector2(UnityEngine.Random.Range(-horizontalSpread, horizontalSpread), 0f);

        float targetY = rectTransform.anchoredPosition.y + flyDistance;

        sequence = DOTween.Sequence()
            .Append(rectTransform.DOAnchorPosY(targetY, duration).SetEase(Ease.OutCubic))
            .Join(canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad))
            .OnComplete(() => onFinished?.Invoke())
            .SetLink(gameObject);
    }

    private void OnDestroy()
    {
        sequence?.Kill();
    }
}