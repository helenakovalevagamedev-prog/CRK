using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class DogsPresenter : IInitializable, IDisposable
{
    private readonly ITabsService tabs;
    private readonly IDogsService dogsService;
    private readonly DogsView view;
    private readonly BreedPopupView popup;

    private IReadOnlyList<Breed> breeds = Array.Empty<Breed>();
    private CancellationTokenSource listCts;
    private CancellationTokenSource detailsCts;
    private int loadingIndex = -1;

    public DogsPresenter(ITabsService tabs, IDogsService dogsService, DogsView view, BreedPopupView popup)
    {
        this.tabs = tabs;
        this.dogsService = dogsService;
        this.view = view;
        this.popup = popup;
    }

    public void Initialize()
    {
        tabs.onTabOpened += OnTabOpened;
        tabs.onTabClosed += OnTabClosed;
        view.onBreedClicked += OnBreedClicked;
        popup.onCloseClicked += popup.Hide;

        popup.Hide();

        if (tabs.Current == TabType.Dogs)
        {
            LoadBreedsAsync().Forget();
        }
    }

    public void Dispose()
    {
        tabs.onTabOpened -= OnTabOpened;
        tabs.onTabClosed -= OnTabClosed;
        view.onBreedClicked -= OnBreedClicked;
        popup.onCloseClicked -= popup.Hide;

        CancelList();
        CancelDetails();
    }

    private void OnTabOpened(TabType tab)
    {
        if (tab == TabType.Dogs)
        {
            LoadBreedsAsync().Forget();
        }
    }

    private void OnTabClosed(TabType tab)
    {
        if (tab == TabType.Dogs)
        {
            CancelList();
            CancelDetails();
            popup.Hide();
        }
    }

    private async UniTaskVoid LoadBreedsAsync()
    {
        CancelList();
        CancelDetails();

        listCts = new CancellationTokenSource();
        CancellationToken token = listCts.Token;

        breeds = Array.Empty<Breed>();
        view.ShowLoading();

        try
        {
            breeds = await dogsService.GetBreedsAsync(token);
            view.ShowBreeds(breeds);
        }
        catch (OperationCanceledException)
        {
            // moved from the tab
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Ошибка загрузки пород: {e.Message}");
            view.ShowError("Can't load breeds");
        }
    }

    private void CancelList()
    {
        if (listCts == null)
        {
            return;
        }

        listCts.Cancel();
        listCts.Dispose();
        listCts = null;
    }

    private void OnBreedClicked(int index)
    {
        if (index == loadingIndex)
        {
            return;
        }

        CancelDetails();
        LoadDetailsAsync(index).Forget();
    }

    private async UniTaskVoid LoadDetailsAsync(int index)
    {
        Breed breed = breeds[index];

        detailsCts = new CancellationTokenSource();
        CancellationToken token = detailsCts.Token;

        loadingIndex = index;
        view.SetBreedLoading(index, true);

        try
        {
            Breed details = await dogsService.GetBreedAsync(breed.Id, token);
            FinishDetails();
            popup.Show(details.Name, details.Description);
        }
        catch (OperationCanceledException)
        {
            // Click on another breed or moved from tab
        }
        catch (Exception e)
        {
            FinishDetails();
            Debug.LogWarning($"Ошибка загрузки породы: {e.Message}");
            popup.Show(breed.Name, "Can't load description");
        }
    }

    private void CancelDetails()
    {
        if (detailsCts == null)
        {
            return;
        }

        detailsCts.Cancel();
        FinishDetails();
    }

    private void FinishDetails()
    {
        if (loadingIndex >= 0)
        {
            view.SetBreedLoading(loadingIndex, false);
            loadingIndex = -1;
        }

        detailsCts?.Dispose();
        detailsCts = null;
    }
}