using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;

    private LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.numCornerVertices = 5;
        _lr.numCapVertices = 5;
        cameraController.onZoomChanged += ChangeLineRendererWidth;
    }
    private void Update()
    {
        #region DEMO FOR LINE DRAWING

        /*
          DRAWING: 
            1) Drawing with 1 finger
            2) Moving with 2 fingers

          ZOOM:
            1) Movement with 1 finger
            2) Zooming with 2 fingers
        */

        bool isDrawingAllowed = GameManager.instance.IsDrawingAllowed;
        int touchesAmount = Input.touchCount;
        if (isDrawingAllowed && touchesAmount != 1 || !isDrawingAllowed)
        {
            return;
        }

        #endregion

        if (Input.GetMouseButton(0))
        {
            Vector2 screenPosition = Input.mousePosition;
            AddNewPosition(screenPosition);
        }
    }

    private void ChangeLineRendererWidth(float orthgraphicSize)
    {
        float startZoom = cameraController.StartZoom;
        float newWdith = orthgraphicSize / startZoom;
        _lr.widthMultiplier = newWdith;
    }
    private void AddNewPosition(Vector3 screenPosition)
    {
        Vector3 worldMousePosition = cameraController.Camera.ScreenToWorldPoint(screenPosition);
        worldMousePosition.z = 0;
        Vector3 lastPosition = Vector3.zero;
        if (_lr.positionCount > 0)
        {
            lastPosition = _lr.GetPosition(_lr.positionCount - 1);
        }
        if (!ApproximatelyEqual(lastPosition, worldMousePosition))
        {
            _lr.SetPosition(++_lr.positionCount - 1, worldMousePosition);
        }
    }
    private bool ApproximatelyEqual(Vector3 left, Vector3 right)
    {
        float threshold = 2f;
        if (Vector3.Distance(left, right) > threshold) 
            return false;
        else
            return true;
    }
}
