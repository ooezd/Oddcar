using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public static bool isTurnRightButtonPressed {  get; private set; }
    public static bool isTurnLeftButtonPressed { get; private set; }
    public static bool isScreenTouched {  get; private set; }
    public static void ToggleTurnRight(bool toggle)
    {
        isTurnRightButtonPressed = toggle;
    }
    public static void ToggleTurnLeft(bool toggle)
    {
        isTurnLeftButtonPressed = toggle;
    }

    private void Update()
    {
#if MOBILE_PLATFORM && !UNITY_EDITOR
        isScreenTouched = Input.touchCount > 0;
#else
        isScreenTouched = isTurnRightButtonPressed || isTurnLeftButtonPressed;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ToggleTurnLeft(true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToggleTurnRight(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ToggleTurnLeft(false);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ToggleTurnRight(false);
        }
#endif
    }
}
