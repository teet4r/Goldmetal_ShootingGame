using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Player : PoolObject
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

    private void Start()
    {
        _curAttackPattern = _NormalAttack;

        _Attack().Forget();
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
            _curAttackPattern = _AdvancedAttack;
    }

    private async UniTask _Attack()
    {
        while (true)
        {
            _curAttackPattern();

            await UniTask.Delay(_delayMsPerShot, cancellationToken: CancellationTokenSource.Token);
        }
    }

    private void _NormalAttack()
    {
        var bullet = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        bullet.transform.position = transform.position;
        bullet.Fire(10f);
    }

    private void _AdvancedAttack()
    {
        var pos = transform.position;
        var bullet1 = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        pos.x += 0.25f;
        bullet1.transform.position = pos;
        bullet1.Fire(10f);
        
        pos = transform.position;
        var bullet2 = ObjectPoolManager.Instance.Get<PlayerBulletA>();
        pos.x -= 0.25f;
        bullet2.transform.position = pos;
        bullet2.Fire(10f);
    }
}