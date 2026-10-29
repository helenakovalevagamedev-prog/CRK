using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BreedItemView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text label;
    [SerializeField] private GameObject loader;

    private int index;

    public event Action<int> onClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    public void Setup(int index, string text)
    {
        this.index = index;
        label.text = text;
        SetLoading(false);
    }

    public void SetLoading(bool isLoading)
    {
        loader.SetActive(isLoading);
    }

    private void OnButtonClicked()
    {
        onClicked?.Invoke(index);
    }
}