using Cysharp.Threading.Tasks;
using UnityEngine;

public class Ingame : SceneSingletonBehaviour<Ingame>
{
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        _SpawnEnemies().Forget();
    }

    private async UniTask _SpawnEnemies()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(Random.Range(250, 2000), cancellationToken: destroyCancellationToken);


        }
    }
}
