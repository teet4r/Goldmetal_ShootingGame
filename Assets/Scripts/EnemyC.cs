using UnityEngine;

public class EnemyC : Enemy
{
    protected override EnemyBullet bullet => ObjectPoolManager.Instance.Get<EnemyBulletB>();


}
