using System.Threading;
using Cysharp.Threading.Tasks;

public interface IWeatherService
{
    UniTask<WeatherForecast> GetForecastAsync(CancellationToken token);
}