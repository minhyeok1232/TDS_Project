using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }
    
    // SingleTon이 만들어졌는지에 대한 여부
    public static bool IsInitialized => _instance != null;
    
    public static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
            }
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Helpers.LogWarning($"{typeof(T).Name} 중복 인스턴스 발견");
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake() { }
}
