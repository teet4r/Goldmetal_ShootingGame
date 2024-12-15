using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : PoolObject, IDamageable, IAttackable
{
    [Header("References")]
    [SerializeField] private Sprite _originSprite;
    [SerializeField] private Sprite _hitSprite;

    [Header("Variables")]
    [SerializeField] private int _maxHp;
    [SerializeField] private float _speed;
    [Min(1)][SerializeField] private int _delayMsPerShot;

    public bool IsDead => CurHp <= 0;
    public int MaxHp => _maxHp;
    public int CurHp => _curHp;
    public bool IsAttacking => _isAttacking;

    private int _curHp;
    private bool _isAttacking;

    protected virtual EnemyBullet bullet => ObjectPoolManager.Instance.Get<EnemyBulletA>();

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigid;

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

    public void Bind(Vector2 position)
    {
        transform.position = position;
        _curHp = _maxHp;
        _isAttacking = false;
        _rigid.linearVelocity = _speed * Vector2.down;
        _spriteRenderer.sprite = _originSprite;
    }

    public void StartAttack()
    {
        if (IsDead || IsAttacking)
            return;

        _isAttacking = true;
        _AttackRoutine().Forget();
    }

    public void StopAttack()
    {
        if (IsDead || !IsAttacking)
            return;

        _isAttacking = false;
    }

    private async UniTask _AttackRoutine()
    {
        while (!returnCancellationTokenSource.IsCancellationRequested && !IsDead && IsAttacking)
        {
            await UniTask.Delay(_delayMsPerShot, cancellationToken: returnCancellationTokenSource.Token);

            bullet.Fire(transform.position);
        }
    }

    public virtual void GetDamage(int damage)
    {
        if (IsDead)
            return;

        _curHp -= damage;
        _spriteRenderer.sprite = _hitSprite;
        _ReturnSprite().Forget();

        if (IsDead)
            Return();
    }

    private async UniTask _ReturnSprite()
    {
        await UniTask.Delay(100, cancellationToken: returnCancellationTokenSource.Token);
        _spriteRenderer.sprite = _originSprite;
    }
}
