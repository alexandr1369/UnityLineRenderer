using UnityEngine;
using UnityEngine.UI;

public class ToggleZoomButton : InteractableButton
{
    [SerializeField]
    private Text text;

    protected override void Init()
    {
        GameManager.instance.IsDrawingAllowed = false;
    }
    protected override void Perform()
    {
        bool drawingState = GameManager.instance.IsDrawingAllowed = !GameManager.instance.IsDrawingAllowed;
        if (drawingState)
            text.text = "To Zoom";
        else
            text.text = "To Drawing";
    }
}