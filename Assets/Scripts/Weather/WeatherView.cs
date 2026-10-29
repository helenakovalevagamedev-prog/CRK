using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class WeatherView : MonoBehaviour
{
    [SerializeField] private RawImage icon;
    [SerializeField] private TMP_Text label;

    public void ShowLoading()
    {
        icon.enabled = false;
        label.text = "Loading...";
    }

    public void ShowForecast(Texture2D texture, string text)
    {
        icon.texture = texture;
        icon.enabled = texture != null;
        label.text = text;
    }

    public void ShowError()
    {
        icon.enabled = false;
        label.text = "Can't load a forecast";
    }
}