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

            Enemy enemy = null;
            switch (Random.Range(0, 3))
            {
                case 0:
                    enemy = ObjectPoolManager.Instance.Get<EnemyA>();
                    break;
                case 1:
                    enemy = ObjectPoolManager.Instance.Get<EnemyB>();
                    break;
                case 2:
                    enemy = ObjectPoolManager.Instance.Get<EnemyC>();
                    break;
            }
            enemy.Bind(_spawnPoints[Random.Range(0, _spawnPoints.Length)].position);
        }
    }
}
