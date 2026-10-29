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

    private IReadOnlyList<Breed> breeds = Array.Empty<Breed>();
    private CancellationTokenSource listCts;

    public DogsPresenter(ITabsService tabs, IDogsService dogsService, DogsView view)
    {
        this.tabs = tabs;
        this.dogsService = dogsService;
        this.view = view;
    }

    public void Initialize()
    {
        tabs.onTabOpened += OnTabOpened;
        tabs.onTabClosed += OnTabClosed;

        if (tabs.Current == TabType.Dogs)
        {
            LoadBreedsAsync().Forget();
        }
    }

    public void Dispose()
    {
        tabs.onTabOpened -= OnTabOpened;
        tabs.onTabClosed -= OnTabClosed;

        CancelList();
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
        }
    }

    private async UniTaskVoid LoadBreedsAsync()
    {
        CancelList();

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
            // Ушли с вкладки — ничего не делаем
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
}