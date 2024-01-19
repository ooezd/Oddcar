using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : BaseController
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private Button menuButton;

    private GameManager gameManager;
    private LevelManager levelManager;
    private Timer timer;

    private void Start()
    {
        base.Awake();
        gameManager = Locator.Instance.Resolve<GameManager>();
        levelManager = Locator.Instance.Resolve<LevelManager>();
        levelManager.OnLevelCompleted += OnLevelCompleted;
        timer = gameManager._timer;
        SetBestTimeText();
        menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void OnMenuButtonClick()
    {
        levelManager.UnloadLevel();
        viewManager.DestroyLastView();
        viewManager.LoadView("MainMenuView");
    }

    private void SetBestTimeText()
    {
        float bestTime = levelManager.GetLoadedLevel().data.bestTime;
        bestTimeText.text = FormatTime(bestTime);
    }

    private void OnLevelCompleted(Level level)
    {
        viewManager.LoadView("EndGameMenuView");
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        int milliseconds = (int)((timeInSeconds * 100f) % 100f);
        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
    }

    private void Update()
    {
        if (timer != null)
        {
            timeText.text = FormatTime(timer.currentTime);
        }
    }

    private void OnDestroy()
    {
        menuButton.onClick.RemoveListener(OnMenuButtonClick);
    }
}
