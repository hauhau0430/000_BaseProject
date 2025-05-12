using System.Collections;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;
    public static T Instance => CreateInstance();

    protected SingletonScriptableObject() { }

    public IEnumerator Load()
    {
        yield return instance;
    }

    private static T CreateInstance()
    {
        if (instance == null)
        {
            string path = "ScriptableObject/" + typeof(T).Name;
            instance = Resources.Load<T>(path);
            
            if (instance == null)
            {
                Debug.Log($"{path} がありません。");
            }
        }

        return instance;
    }
}
