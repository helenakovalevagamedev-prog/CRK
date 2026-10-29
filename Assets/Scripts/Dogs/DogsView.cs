using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public sealed class DogsView : MonoBehaviour
{
    [SerializeField] private GameObject listLoader;
    [SerializeField] private TMP_Text statusText;

    private readonly List<BreedItemView> items = new();
    private BreedItemPool itemPool;

    public event Action<int> onBreedClicked;

    [Inject]
    public void Construct(BreedItemPool itemPool)
    {
        this.itemPool = itemPool;
    }

    public void ShowLoading()
    {
        ClearList();
        statusText.gameObject.SetActive(false);
        listLoader.SetActive(true);
    }

    public void ShowBreeds(IReadOnlyList<Breed> breeds)
    {
        ClearList();
        listLoader.SetActive(false);
        statusText.gameObject.SetActive(false);

        for (int i = 0; i < breeds.Count; i++)
        {
            BreedItemView item = itemPool.Spawn();
            item.transform.SetAsLastSibling();
            item.Setup(i, $"{i + 1} - {breeds[i].Name}");
            item.onClicked += OnItemClicked;
            items.Add(item);
        }
    }

    public void ShowError(string message)
    {
        ClearList();
        listLoader.SetActive(false);
        statusText.text = message;
        statusText.gameObject.SetActive(true);
    }

    public void SetBreedLoading(int index, bool isLoading)
    {
        if (index >= 0 && index < items.Count)
        {
            items[index].SetLoading(isLoading);
        }
    }

    private void ClearList()
    {
        foreach (BreedItemView item in items)
        {
            item.onClicked -= OnItemClicked;
            itemPool.Despawn(item);
        }

        items.Clear();
    }

    private void OnItemClicked(int index)
    {
        onBreedClicked?.Invoke(index);
    }
}