using UnityEngine;
using UnityEngine.UI;

public class RaycastTextButton : ToggleButton
{
    [Header("Data")]
    [SerializeField] private Text text;

    private Camera _camera;

    private Vector3 origin, direction;

    protected override void Init()
    {
        base.Init();
        _camera = Camera.main;
    }
    protected override void Perform()
    {
        base.Perform();

        origin = _camera.ScreenToWorldPoint(new Vector3(0, Screen.height));
        direction = new Vector3(origin.x, origin.y, -origin.z).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 10f);
        if (hit)
        {
            text.text = hit.collider.name;
        }
    }
}
