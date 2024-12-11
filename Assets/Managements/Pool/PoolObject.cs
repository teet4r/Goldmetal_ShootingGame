using System;
using System.Threading;
using UniRx;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public new Transform transform => _transform;
    private Transform _transform;

    private bool IsTokenCancellable => !_returnCancellationToken.IsNull() && !_returnCancellationToken.IsCancellationRequested;
    protected CancellationTokenSource returnCancellationTokenSource
    {
        get
        {
            if (!IsTokenCancellable)
                _returnCancellationToken = new CancellationTokenSource();
            return _returnCancellationToken;
        }
    }
    private CancellationTokenSource _returnCancellationToken;

    protected virtual void Awake()
    {
        _transform = gameObject.transform;

        ObjectPoolManager.Instance.OnHideOrClear.Subscribe(_ => Return())
            .AddTo(gameObject);
    }

    public virtual void Return()
    {
        if (!gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
        if (IsTokenCancellable)
        {
            _returnCancellationToken.Cancel();
            _returnCancellationToken.Dispose();
            _returnCancellationToken = null;
        }
        ObjectPoolManager.Instance.Return(this);
    }
}