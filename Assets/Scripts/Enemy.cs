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
        else if (collision.CompareTag(Tag.PlayerBullet))
        {
            if (!collision.TryGetComponent(out PlayerBullet playerBullet))
                return;
            GetDamage(playerBullet.Damage);
            playerBullet.Return();
        }
    }

    public void Bind()
    {
        _hp = _maxHp;
        _rigid.linearVelocity = _speed * Vector2.down;
        _spriteRenderer.sprite = _originSprite;
    }

    public void GetDamage(int damage)
    {
        _hp -= damage;
        _spriteRenderer.sprite = _hitSprite;
        _ReturnSprite().Forget();

        if (_hp <= 0)
            Return();
    }

    private async UniTask _ReturnSprite()
    {
        await UniTask.Delay(100, cancellationToken: CancellationTokenSource.Token);
        _spriteRenderer.sprite = _originSprite;
    }
}
