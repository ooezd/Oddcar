using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected ViewManager viewManager;

    protected virtual void Awake()
    {
        viewManager = Locator.Instance.Resolve<ViewManager>();
    }
}