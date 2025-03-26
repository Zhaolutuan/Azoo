using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
        protected static T _instance;
        public static bool HasInstance => _instance != null;

        public static T Instance
        {
                get
                {
                        if (_instance == null)
                        {
                                _instance = FindFirstObjectByType<T>();
                                if (_instance == null)
                                {
                                        GameObject obj = new();
                                        obj.name = typeof(T).Name + "_AutoCreated";
                                        Debug.LogWarning("An instance of " + typeof(T) +
                                                " is needed in the scene, so '" + obj.name + "' was created with DontDestroyOnLoad.");
                                        _instance = obj.AddComponent<T>();
                                }
                        }
                        return _instance;
                }
        }

        protected virtual void Awake()
        {
                if (!Application.isPlaying)
                {
                        return;
                }

                _instance = this as T;
        }
}