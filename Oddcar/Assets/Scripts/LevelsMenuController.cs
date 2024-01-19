using System.Linq;
using UnityEngine;

public class LevelsMenuController : BaseController
{
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private LevelButton levelButtonPrefab;
    private LevelManager levelManager;

    protected override void Awake()
    {
        base.Awake();
        levelManager = Locator.Instance.Resolve<LevelManager>();
        InitializeLevelButtons();
    }

    private void InitializeLevelButtons()
    {
        for (int i = 0; i < levelManager.GetLevelCount(); i++)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, buttonsParent.transform);
            levelButton.Init(i, levelManager.unlockedLevels.Contains(i), OnLevelButtonClick);
        }
    }

    private void OnLevelButtonClick(int levelIndex)
    {
        levelManager.LoadLevel(levelIndex);
        viewManager.DestroyLastView();
        viewManager.LoadView("GameMenuView");
        Locator.Instance.Resolve<Launcher>().LoadGame();
    }
}
