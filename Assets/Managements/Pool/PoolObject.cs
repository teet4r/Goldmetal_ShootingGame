using System.Threading;
using UniRx;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public bool IsPooled => transform.parent == ObjectPoolManager.Instance.transform && !gameObject.activeInHierarchy;
    public new Transform transform => _transform;
    protected Transform _transform;

    protected bool IsTokenCancellable => !_cancellationTokenSource.IsNull() && !_cancellationTokenSource.IsCancellationRequested;
    protected CancellationTokenSource CancellationTokenSource
    {
        get
        {
            if (!IsTokenCancellable)
                _cancellationTokenSource = new CancellationTokenSource();
            return _cancellationTokenSource;
        }
    }
    private CancellationTokenSource _cancellationTokenSource;

    protected virtual void Awake()
    {
        _transform = gameObject.transform;

        ObjectPoolManager.Instance.OnHideOrClear.Subscribe(_ => Return())
            .AddTo(gameObject);
    }

    public virtual void Return()
    {
        if (IsPooled)
            return;

        if (IsTokenCancellable)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
        ObjectPoolManager.Instance.Return(this);
    }
}