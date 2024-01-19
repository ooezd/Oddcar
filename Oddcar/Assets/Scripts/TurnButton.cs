using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isRightButton;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRightButton)
            InputProvider.ToggleTurnRight(true);
        else
            InputProvider.ToggleTurnLeft(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isRightButton)
            InputProvider.ToggleTurnRight(false);
        else
            InputProvider.ToggleTurnLeft(false);
    }
}
