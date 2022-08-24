using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Joystick _stickMove;
    [SerializeField] private ActionButton _buttonJump;
    [SerializeField] private CharacterController _charCon;
    [SerializeField] private float _maxSpd = 5f;
    [SerializeField] private float _jumpSpd = 20f, _grv = 40f, _bufferJumpMax = 0.1f;
    private Vector3 moveDir;
    private float _bufferJump;

    private void Start()
    {
        if (_charCon == null)
            _charCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveDir = new Vector3(_stickMove.Horizontal, moveDir.y, _stickMove.Vertical);
        moveDir.x *= _maxSpd;
        moveDir.z *= _maxSpd;

        if (_charCon.isGrounded)
        {
            moveDir.y = Mathf.Clamp(moveDir.y, -2f, float.PositiveInfinity);
            if (_buttonJump.Pressed || _bufferJump > 0)
            {
                moveDir.y = _jumpSpd;
            }
        }
        else
        {
            if (_buttonJump.Pressed && _bufferJump == 0)
            {
                _bufferJump = _bufferJumpMax;
            }
            else
                _bufferJump = Mathf.MoveTowards(_bufferJump, 0, Time.deltaTime);
        }

        _buttonJump.Pressed = false;

        moveDir.y -= _grv * Time.deltaTime;

        moveDir = transform.TransformDirection(moveDir);

        _charCon.Move(moveDir * Time.deltaTime);
    }
}
