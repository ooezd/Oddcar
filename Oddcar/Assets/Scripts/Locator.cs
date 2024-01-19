using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

    public static Locator Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Register<T>(T service)
    {
        var serviceType = typeof(T);

        if (IsRegistered(serviceType))
        {
            Debug.LogWarning($"{serviceType} is already registered");
        }

        Services[typeof(T)] = service;
    }

    public T Resolve<T>()
    {
        var serviceType = typeof(T);

        if (IsRegistered(serviceType))
        {
            return (T)Services[serviceType];
        }

        Debug.LogError($"{serviceType.Name} hasn't been registered!");
        return default(T);
    }

    public bool IsRegistered(Type t)
    {
        return Services.ContainsKey(t);
    }
}
