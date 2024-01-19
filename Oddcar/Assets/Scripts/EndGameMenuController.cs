using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenuController : BaseController
{
    [SerializeField] private TextMeshProUGUI finishText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI newRecordText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button menuButton;

    private LevelManager levelManager;
    private GameManager gameManager;
    private CarManager carManager;

    protected override void Awake()
    {
        base.Awake();
        levelManager = Locator.Instance.Resolve<LevelManager>();
        gameManager = Locator.Instance.Resolve<GameManager>();
        carManager = Locator.Instance.Resolve<CarManager>();
    }

    private void Start()
    {
        LevelData levelData = levelManager.GetLoadedLevel().data;
        float finishTime = gameManager._timer.currentTime;
        finishText.text = $"LEVEL {levelData.levelIndex + 1} FINISHED!";
        timeText.text = "Finish Time: " + FormatTime(finishTime);
        bestTimeText.text = "Best Time: " + FormatTime(levelData.bestTime);
        newRecordText.text = finishTime <= levelData.bestTime ? "NEW RECORD!" : string.Empty;

        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void OnMenuButtonClick()
    {
        levelManager.UnloadLevel();
        viewManager.LoadView("MainMenuView");
    }

    private void OnNextLevelButtonClick()
    {
        levelManager.UnloadLevel();

        bool isLoaded = levelManager.LoadNextLevelIfExists();
        if (!isLoaded)
        {
            viewManager.LoadView("MainMenuView");
            return;
        }

        viewManager.DestroyLastView();
        viewManager.LoadView("GameMenuView");

        Locator.Instance.Resolve<Launcher>().LoadGame();
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60f);
        int seconds = (int)(timeInSeconds % 60f);
        int milliseconds = (int)((timeInSeconds * 100f) % 100f);
        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
    }
}