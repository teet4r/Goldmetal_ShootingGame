using UnityEngine;

public class EnemyBullet : Bullet
{
    public override void Fire(Vector2 position)
    {
        rigid.position = position;
        rigid.linearVelocity = bulletSpeed * Vector2.down;
    }
}
