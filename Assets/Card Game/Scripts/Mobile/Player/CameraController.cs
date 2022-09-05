using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum RotAxis { hor, ver }
    [SerializeField] private RotAxis rotAxis = RotAxis.hor;
    [SerializeField] private Joystick _stickCam;
    [SerializeField] private float _sensitivity = 40f;
    [SerializeField] private float _verticalClamp = 30f;
    private float _verRot;
    [SerializeField] private bool _inverted;

    private void Start()
    {
        _verRot = transform.localEulerAngles.x;
    }

    private void Update()
    {
        if (rotAxis == RotAxis.ver)
        {
            _verRot += _stickCam.Vertical * _sensitivity * Time.deltaTime * (_inverted ? -1 : 1);
            _verRot = Mathf.Clamp(_verRot, -_verticalClamp, _verticalClamp);
            transform.localEulerAngles = new Vector3(_verRot, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else
        {
            float _horRot = _stickCam.Horizontal * _sensitivity * Time.deltaTime * (_inverted ? -1 : 1);
            transform.Rotate(0, _horRot, 0);
        }


    }

}
