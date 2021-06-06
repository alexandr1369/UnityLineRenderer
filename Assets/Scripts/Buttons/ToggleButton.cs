using UnityEngine;

public class ToggleButton : InteractableButton
{
    [SerializeField]
    private GameObject panel; // panel to toggle
    [SerializeField]
    private bool hasPauseToggle = false; // has permission to toggle game pause

    protected override void Perform()
    {
        // toggle game state
        if (hasPauseToggle)
            GameManager.instance.ToggleGameState();

        // toggle panel
        panel.SetActive(!panel.activeSelf);
    }
}
