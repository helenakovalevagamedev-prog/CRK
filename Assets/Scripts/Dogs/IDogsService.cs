using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public interface IDogsService
{
    UniTask<IReadOnlyList<Breed>> GetBreedsAsync(CancellationToken token);
    UniTask<Breed> GetBreedAsync(string id, CancellationToken token);
}