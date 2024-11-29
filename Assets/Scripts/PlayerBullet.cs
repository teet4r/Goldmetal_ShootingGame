using UnityEngine;

public class PlayerBullet : Bullet
{
    public virtual void Fire(float speed)
    {
        rigid.AddForce(speed * Vector2.up, ForceMode2D.Impulse);
    }
}
