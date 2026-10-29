using UnityEngine;

public sealed class WeatherForecast
{
    public WeatherForecast(int temperature, string unit, Texture2D icon)
    {
        Temperature = temperature;
        Unit = unit;
        Icon = icon;
    }

    public int Temperature { get; }
    public string Unit { get; }
    public Texture2D Icon { get; }
}