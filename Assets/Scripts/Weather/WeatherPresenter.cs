using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public sealed class WeatherPresenter : IInitializable, IDisposable
{
    private readonly ITabsService tabs;
    private readonly IWeatherService weatherService;
    private readonly WeatherView view;
    private readonly WeatherConfig config;

    private CancellationTokenSource pollingCts;

    public WeatherPresenter(ITabsService tabs, IWeatherService weatherService, WeatherView view, WeatherConfig config)
    {
        this.tabs = tabs;
        this.weatherService = weatherService;
        this.view = view;
        this.config = config;
    }

    public void Initialize()
    {
        tabs.onTabOpened += OnTabOpened;
        tabs.onTabClosed += OnTabClosed;

        if (tabs.Current == TabType.Weather)
        {
            StartPolling();
        }
    }

    public void Dispose()
    {
        tabs.onTabOpened -= OnTabOpened;
        tabs.onTabClosed -= OnTabClosed;

        StopPolling();
    }

    private void OnTabOpened(TabType tab)
    {
        if (tab == TabType.Weather)
        {
            StartPolling();
        }
    }

    private void OnTabClosed(TabType tab)
    {
        if (tab == TabType.Weather)
        {
            StopPolling();
        }
    }

    private void StartPolling()
    {
        StopPolling();

        view.ShowLoading();
        pollingCts = new CancellationTokenSource();
        PollAsync(pollingCts.Token).Forget();
    }

    private void StopPolling()
    {
        if (pollingCts == null)
        {
            return;
        }

        pollingCts.Cancel();
        pollingCts.Dispose();
        pollingCts = null;
    }

    private async UniTaskVoid PollAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            RequestForecastAsync(token).Forget();

            bool canceled = await UniTask
                .Delay(TimeSpan.FromSeconds(config.PollInterval), cancellationToken: token)
                .SuppressCancellationThrow();

            if (canceled)
            {
                return;
            }
        }
    }

    private async UniTaskVoid RequestForecastAsync(CancellationToken token)
    {
        try
        {
            WeatherForecast forecast = await weatherService.GetForecastAsync(token);
            view.ShowForecast(forecast.Icon, $"Сегодня - {forecast.Temperature}{forecast.Unit}");
        }
        catch (OperationCanceledException)
        {
            // Moved from a tab
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error of loading forecast: {e.Message}");
            view.ShowError();
        }
    }
}