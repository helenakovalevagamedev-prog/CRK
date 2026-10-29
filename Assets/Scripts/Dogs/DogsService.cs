using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class DogsService : IDogsService
{
    private const string RequestTag = "dogs";

    private readonly IRequestQueue queue;
    private readonly IHttpClient http;
    private readonly DogsConfig config;

    public DogsService(IRequestQueue queue, IHttpClient http, DogsConfig config)
    {
        this.queue = queue;
        this.http = http;
        this.config = config;
    }

    public async UniTask<IReadOnlyList<Breed>> GetBreedsAsync(CancellationToken token)
    {
        Debug.Log("Породы: запрос в очередь");
        string json = await queue.Enqueue(RequestTag, ct => http.GetTextAsync(config.BreedsUrl, ct), token);

        var response = JsonUtility.FromJson<BreedListResponse>(json);

        if (response?.data == null)
        {
            throw new Exception("There is no breeds in the response");
        }

        int count = Math.Min(config.BreedsCount, response.data.Length);
        var breeds = new List<Breed>(count);

        for (int i = 0; i < count; i++)
        {
            breeds.Add(ToBreed(response.data[i]));
        }

        return breeds;
    }
    
    public async UniTask<Breed> GetBreedAsync(string id, CancellationToken token)
    {
        string url = $"{config.BreedsUrl}/{id}";
        string json = await queue.Enqueue(RequestTag, ct => http.GetTextAsync(url, ct), token);

        var response = JsonUtility.FromJson<BreedDetailsResponse>(json);

        if (response?.data == null)
        {
            throw new Exception($"There is no breed data in the response {id}");
        }

        return ToBreed(response.data);
    }

    private static Breed ToBreed(BreedData data)
    {
        return new Breed(data.id, data.attributes?.name, data.attributes?.description);
    }
}