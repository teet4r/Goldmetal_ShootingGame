using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Start()
    {
        _Bootstrap().Forget();
    }

    private async UniTask _Bootstrap()
    {
        await UniTask.WaitUntil(() =>
            ObjectPoolManager.Instance.IsLoaded && SceneManager.Instance.IsLoaded
        );

        SceneManager.Instance.LoadSceneAsync(SceneName.Ingame).Forget();
    }
}
