using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    public float perspectiveZoomSpd = .5f;
    public float orthoZoomSpd = .5f;
    private float _lastTouchDist;

    private Camera _cam;

    private void Start()
    {
        _cam ??= GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouhcDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagDiff = prevTouhcDeltaMag - touchDeltaMag;

            if (_cam.orthographic)
            {
                _cam.orthographicSize += deltaMagDiff * orthoZoomSpd;
                _cam.orthographicSize = Mathf.Max(_cam.orthographicSize, 0.1f);
            }
            else
            {
                _cam.fieldOfView += deltaMagDiff * perspectiveZoomSpd;
                _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView,0.1f,179.9f);
            }
        }
    }

}
