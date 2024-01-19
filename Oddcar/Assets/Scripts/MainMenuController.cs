using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseController
{
    [SerializeField] private Button levelsButton;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnLevelsButtonClick()
    {
        viewManager.LoadView("LevelsView");
    }
}
