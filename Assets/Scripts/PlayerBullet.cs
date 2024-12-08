using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Enemy))
        {
            if (!collision.TryGetComponent(out Enemy enemy))
                return;

            if (enemy.IsDead)
                return;
            enemy.GetDamage(Damage);
            Return();
        }
    }
}
