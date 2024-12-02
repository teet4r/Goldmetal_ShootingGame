using UnityEngine;

public class SceneSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => _instance;
    private static T _instance;

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
