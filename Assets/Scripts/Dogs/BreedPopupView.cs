using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BreedPopupView : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private float showDuration = 0.2f;

    public event Action onCloseClicked;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnCloseClicked);
        backgroundButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(OnCloseClicked);
        backgroundButton.onClick.RemoveListener(OnCloseClicked);
        panel.DOKill();
    }

    public void Show(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;

        gameObject.SetActive(true);

        panel.DOKill();
        panel.localScale = Vector3.one * 0.8f;
        panel.DOScale(1f, showDuration).SetEase(Ease.OutBack).SetLink(gameObject);
    }

    public void Hide()
    {
        panel.DOKill();
        gameObject.SetActive(false);
    }

    private void OnCloseClicked()
    {
        onCloseClicked?.Invoke();
    }
}