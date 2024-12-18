using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Player : PoolObject, IAttackable
{
    private const string _HORIZONTAL = "Horizontal";
    private const string _VERTICAL = "Vertical";

    [Header("References")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private Animator _animator;

    [Header("Variables")]
    [SerializeField] private float _speed;
    [Min(1)][SerializeField] private int _delayMsPerShot;

    private readonly int _inputHash = Animator.StringToHash("Input");
    private Vector2 _dir = new();
    private Action _curAttackPattern;

    public bool IsAttacking => _isAttacking;
    private bool _isAttacking;

    private void Start() // �ӽ� Bind
    {
        _isAttacking = false;
        _curAttackPattern = _Power2Attack;

        StartAttack();
    }

    private void FixedUpdate()
    {
        _dir.x = Input.GetAxisRaw(_HORIZONTAL);
        _dir.y = Input.GetAxisRaw(_VERTICAL);

        _rigid.position += _speed * Time.fixedDeltaTime * _dir.normalized;

        _animator.SetInteger(_inputHash, (int)_dir.x);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _curAttackPattern = _Power3Attack;
            _delayMsPerShot /= 2;
        }
    }

    public void StartAttack()
    {
        if (IsAttacking)
            return;

        _isAttacking = true;
        _AttackRoutine().Forget();
    }

    public void StopAttack()
    {
        if (!IsAttacking)
            return;

        _isAttacking = false;
    }

    private async UniTask _AttackRoutine()
    {
        while (IsAttacking)
        {
            _curAttackPattern();

            await UniTask.Delay(_delayMsPerShot, cancellationToken: returnCancellationTokenSource.Token);
        }
    }

    private void _Power1Attack()
    {
        var bullet = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        bullet.Fire(transform.position);
    }

    private void _Power2Attack()
    {
        var pos = transform.position;
        var BulletA1 = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        pos.x += 0.3f;
        BulletA1.Fire(pos);
        
        pos = transform.position;
        var BulletA2 = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        pos.x -= 0.3f;
        BulletA2.Fire(pos);
    }

    private void _Power3Attack()
    {
        _Power2Attack();
        
        var BulletB = ObjectPoolManager.Instance.Get<PlayerBulletB>();
        BulletB.Fire(transform.position);
    }
}