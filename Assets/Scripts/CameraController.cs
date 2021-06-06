using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private bool isActive = false;
    private GridData gridData;
    private Camera currentCamera;
    public Camera Camera { get => currentCamera; private set => currentCamera = value; }

    private Vector3 _originPosition;
    private Vector3 _difference;

    #region Zooming

    [Range(0, 15f)]
    [SerializeField]
    private float maxZoom = 15f;
    private float minZoom;
    private float startZoom;
    public float StartZoom { get => startZoom; private set => startZoom = value; }

    private float _zoomSpeed;
    private float _lastDeltaDistance;
    private float _maxZoomSwipesDistance = Screen.width * 2 / 3;

    public delegate void OnZoomChanged(float orthographicSize);
    public OnZoomChanged onZoomChanged;

    #endregion

    private IEnumerator Start()
    {
        // load current init after location loader init
        yield return null;
        isActive = true;

        Camera = GetComponent<Camera>();
        StartZoom = minZoom = currentCamera.orthographicSize;
        _zoomSpeed = 10f;

        gridData = GameManager.instance.LocationLoader.GridData;
        Moving();
    }
    private void LateUpdate()
    {
        if (!isActive || GameManager.instance.GameState == GameState.PAUSED) return;

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
        if (isDrawingAllowed)
        {
            if(touchesAmount >= 2)
            {
                Moving();
            }
        }
        else
        {
            if(touchesAmount == 1)
            {
                Moving();
            }
            else if(touchesAmount >= 2)
            {
                Zooming();
            }
        }

        #endregion
    }

    private bool Zooming()
    {
        bool zoomState = Input.touchCount == 2;
        if (Input.touchSupported)
        {
            // check for zooming
            if (zoomState)
            {
                Touch touch1, touch2;
                Vector2 oldTouchPosition1, oldTouchPosition2;
                float currentTouchesDistance, oldTouchesDistance, deltaDistance;
                float deltaTouchDistance, newOrthographicSize;

                // get current touches
                touch1 = Input.GetTouch(0);
                touch2 = Input.GetTouch(1);

                // get touches's positions from previous frame
                oldTouchPosition1 = touch1.position - touch1.deltaPosition;
                oldTouchPosition2 = touch2.position - touch2.deltaPosition;

                // get distance between old and current touches
                oldTouchesDistance = Vector2.Distance(oldTouchPosition1, oldTouchPosition2);
                currentTouchesDistance = Vector2.Distance(touch1.position, touch2.position);

                // zoom offset value between old and current touches
                _lastDeltaDistance = deltaDistance = (currentTouchesDistance - oldTouchesDistance) / _maxZoomSwipesDistance;

                // zooming 
                deltaTouchDistance = deltaDistance * _zoomSpeed;
                newOrthographicSize = Mathf.Clamp(currentCamera.orthographicSize - deltaTouchDistance, minZoom - maxZoom, minZoom);
                currentCamera.orthographicSize = newOrthographicSize;
                onZoomChanged(newOrthographicSize);
            }
            else
            {
                _lastDeltaDistance = 0;
            }
        }
        else
        {
            currentCamera.orthographicSize -= Input.mouseScrollDelta.y;
            currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, minZoom - maxZoom, minZoom);

            // DEMO
            //DEMO();
        }

        return zoomState;
    }
    private void Moving()
    {
        // move camera by mouse or touches
        if (Input.GetMouseButtonDown(0))
        {
            _originPosition = GetMousePosition();
        }
        if (Input.GetMouseButton(0))
        {
            _difference = GetMousePosition() - transform.position;
            transform.position = _originPosition - _difference;
        }

        // clamp camera position to loaded location
        float cameraSize = currentCamera.orthographicSize;
        float minX, minY, maxX, maxY;
        minX = (float)Screen.width / Screen.height * cameraSize;
        maxX = gridData.Width - minX;
        maxY = -cameraSize;
        minY = -gridData.Height - maxY;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
    private Vector3 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // DEMO FOR PC (REMOVE)
    //private void DEMO()
    //{
    //    onZoomChanged(currentCamera.orthographicSize);
    //}
}
