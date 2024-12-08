using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : PoolObject
{
    [Header("References")]
    [SerializeField] private Sprite _originSprite;
    [SerializeField] private Sprite _hitSprite;

    [Header("Variables")]
    [SerializeField] private int _maxHp;
    [SerializeField] private float _speed;
    [Min(1)][SerializeField] private int _delayMsPerShot;

    public bool IsDead => _hp <= 0;
    protected virtual EnemyBullet bullet => ObjectPoolManager.Instance.Get<EnemyBulletA>();

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigid;
    private int _hp;

    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out _spriteRenderer);
        TryGetComponent(out _rigid);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.BulletBorder))
            Return();
    }

    public virtual void Bind(Vector2 position)
    {
        transform.position = position;
        _hp = _maxHp;
        _rigid.linearVelocity = _speed * Vector2.down;
        _spriteRenderer.sprite = _originSprite;

        _Attack().Forget();
    }

    private async UniTask _Attack()
    {
        while (!IsDead)
        {
            await UniTask.Delay(_delayMsPerShot, cancellationToken: CancellationTokenSource.Token);

            bullet.Fire(transform.position);
        }
    }

    public void GetDamage(int damage)
    {
        if (IsDead)
            return;

        _hp -= damage;
        _spriteRenderer.sprite = _hitSprite;
        _ReturnSprite().Forget();

        if (IsDead)
            Return();
    }

    private async UniTask _ReturnSprite()
    {
        await UniTask.Delay(100, cancellationToken: CancellationTokenSource.Token);
        _spriteRenderer.sprite = _originSprite;
    }
}
