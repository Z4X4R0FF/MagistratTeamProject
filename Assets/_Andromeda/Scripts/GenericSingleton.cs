using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectsOfType(typeof(T)) is T[] instanceCandidates)
                {
                    if (instanceCandidates.Length > 0)
                        _instance = instanceCandidates[0];
                    if (instanceCandidates.Length > 1)
                    {
                        Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                    }
                }
                if (_instance == null)
                {
                    var obj = new GameObject
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}

public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }
	
    public virtual void Awake ()
    {
        if (Instance == null) {
            Instance = this as T;
            DontDestroyOnLoad (this);
        } else {
            Destroy (gameObject);
        }
    }
}