using UnityEngine;

[CreateAssetMenu(fileName = "WeatherConfig", menuName = "Configs/Weather Config")]
public sealed class WeatherConfig : ScriptableObject
{
    [SerializeField] private string forecastUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
    [SerializeField, Min(0.5f)] private float pollInterval = 5f;

    public string ForecastUrl => forecastUrl;
    public float PollInterval => pollInterval;
}