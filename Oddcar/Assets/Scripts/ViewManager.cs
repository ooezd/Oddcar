using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private GameObject[] viewPrefabs;
    private Dictionary<string, GameObject> _views = new();
    private Stack<GameObject> activeViews = new();

    public static ViewManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        PrepareViews();
    }

    void PrepareViews()
    {
        foreach (var viewPrefab in viewPrefabs)
        {
            _views.Add(viewPrefab.name, viewPrefab);
        }
    }
    public void LoadView(string viewName)
    {
        if (!_views.ContainsKey(viewName))
        {
            Debug.LogWarning($"{viewName} couldn't found in views list.");
        }
        if (activeViews.Count > 0)
            Destroy(activeViews.Pop());

        activeViews.Push(Instantiate(_views[viewName]));
    }

    public void DestroyLastView()
    {
        Debug.Log("Destroying view...");
        if (activeViews.Count > 0)
        {
            Destroy(activeViews.Pop());
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneAsync(string sceneName, Action<AsyncOperation> onComplete)
    {
        SceneManager.LoadSceneAsync(sceneName).completed += onComplete;
    }
}