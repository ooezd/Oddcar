using System;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Locator locatorPrefab;
    [SerializeField] private ViewManager viewManagerPrefab;
    [SerializeField] private PreferencesManager preferencesManagerPrefab;
    [SerializeField] private LevelManager levelManagerPrefab;
    [SerializeField] private CarManager carManagerPrefab;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private InputProvider inputProviderPrefab;

    ViewManager viewManager;
    GameManager gameManager;

    private void Start()
    {
        Application.targetFrameRate = 120;

        LoadApp();
    }

    private void LoadApp()
    {
        var locator = Instantiate(locatorPrefab);

        locator.Register(this);

        viewManager = Instantiate(viewManagerPrefab);
        locator.Register(viewManager);

        locator.Register(Instantiate(levelManagerPrefab));
        locator.Register(Instantiate(preferencesManagerPrefab));
        locator.Register(Instantiate(inputProviderPrefab)); 

        viewManager.LoadView("MainMenuView");

        //var profileManager = new ProfileManager();
        //Locator.Instance.Register(profileManager);

        //var audioManager = Instantiate(audioManagerPrefab).GetComponent<AudioManager>();
        //Locator.Instance.Register(audioManager);

        //var timerManager = Instantiate(new GameObject("TimerManager")).AddComponent<TimerManager>();
        //Locator.Instance.Register(timerManager);

        //viewManager.LoadView("LoaderPresenter");

        //authorizationManager = new AuthorizationManager();
        //Locator.Instance.Register(authorizationManager);
        //authorizationManager.ConnectPlayfab();
        //authorizationManager.OnLoginSuccess += OnLoginSuccess;
        //authorizationManager.OnLoginFail += OnLoginFail;
    }

    public void LoadGame()
    {
        if (!Locator.Instance.IsRegistered(typeof(CarManager)))
            Locator.Instance.Register(Instantiate(carManagerPrefab));

        if (!Locator.Instance.IsRegistered(typeof(GameManager)))
        {
            gameManager = Instantiate(gameManagerPrefab);
            Locator.Instance.Register(gameManager);
        }

        gameManager.Init();
    }
}