using UnityEngine;
using UnityEngine.UI;

public class ToggleGameModeButton : InteractableButton
{
    private Image _btnImage;

    public GameDifficulty gameDifficulty = GameDifficulty.NORMAL;

    protected override void Init()
    {
        _btnImage = GetComponent<Image>();
        if (GameManager.instance.GameDifficulty == gameDifficulty)
        {
            Color32 color = new Color32(0, 0, 0, 128);
            _btnImage.color = color;

            isInteractable = false;
            isAnimated = false;
        }
    }
    protected override void Perform()
    {
        GameManager.instance.ChangeGameDifficulty(gameDifficulty);
    }
}
