using UnityEngine;

public class Bullet : PoolObject
{
    public int Damage => damage;
    [SerializeField] protected int damage;

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
}
