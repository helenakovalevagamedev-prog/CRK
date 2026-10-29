using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class WeatherService : IWeatherService
{
    private const string RequestTag = "weather";

    private readonly IRequestQueue queue;
    private readonly IHttpClient http;
    private readonly WeatherConfig config;
    private readonly Dictionary<string, Texture2D> iconCache = new();

    public WeatherService(IRequestQueue queue, IHttpClient http, WeatherConfig config)
    {
        this.queue = queue;
        this.http = http;
        this.config = config;
    }

    public async UniTask<WeatherForecast> GetForecastAsync(CancellationToken token)
    {
        Debug.Log($"[{Time.time:F1}] Погода: запрос в очередь");
        string json = await queue.Enqueue(RequestTag, ct => http.GetTextAsync(config.ForecastUrl, ct), token);

        var response = JsonUtility.FromJson<WeatherResponse>(json);
        var periods = response?.properties?.periods;

        if (periods == null || periods.Length == 0)
        {
            throw new Exception("There is no forecast in the response");
        }

        WeatherPeriod today = periods[0];
        Texture2D icon = await GetIconAsync(today.icon, token);

        return new WeatherForecast(today.temperature, today.temperatureUnit, icon);
    }

    private async UniTask<Texture2D> GetIconAsync(string url, CancellationToken token)
    {
        if (iconCache.TryGetValue(url, out Texture2D cached))
        {
            return cached;
        }

        Texture2D icon = await queue.Enqueue(RequestTag, ct => http.GetTextureAsync(url, ct), token);
        iconCache[url] = icon;
        return icon;
    }
}