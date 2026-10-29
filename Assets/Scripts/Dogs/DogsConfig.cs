using UnityEngine;

[CreateAssetMenu(fileName = "DogsConfig", menuName = "Configs/Dogs Config")]
public sealed class DogsConfig : ScriptableObject
{
    [SerializeField] private string breedsUrl = "https://dogapi.dog/api/v2/breeds";
    [SerializeField, Min(1)] private int breedsCount = 10;

    public string BreedsUrl => breedsUrl;
    public int BreedsCount => breedsCount;
}