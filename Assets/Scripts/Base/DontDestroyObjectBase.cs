using System;
using UnityEngine;

public abstract class DontDestroyObjectBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => CreateInstance();
    
    private static bool created = false;

    protected virtual void Awake()
    {
        CheckInstance();

        if (!created)
        {
            created = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        
        Destroy(this);
        return false;
    }

    protected static T CreateInstance()
    {
        if (instance == null)
        {
            Type type = typeof(T);
            instance = (T)FindAnyObjectByType(type);

            if (instance == null)
            {
                Debug.Log("該当のGameObjectがありません。");
            }
        }

        return instance;
    }
}
