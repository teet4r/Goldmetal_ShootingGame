using UnityEngine;

public class Bullet : PoolObject
{
    public int Damage => damage;
    [SerializeField] protected int damage;
    [Min(1)][SerializeField] protected float bulletSpeed;

    protected Rigidbody2D rigid;

    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out rigid);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.BulletBorder))
            Return();
    }

    public virtual void Fire(Vector2 position)
    {
        transform.position = position;
        rigid.linearVelocity = bulletSpeed * Vector2.up;
    }
}
