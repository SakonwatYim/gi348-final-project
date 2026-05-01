using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // If no instance yet, claim this one.
        if (Instance == null)
        {
            Instance = this as T;
            return;
        }

        // If another instance already exists and it's not this one, destroy this duplicate.
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}

public class PersistentSingleleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        // Only mark the surviving instance as DontDestroyOnLoad.
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
