using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Joystick stick;
    [SerializeField] private float _maxSpd = 5f;
   
    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(stick.Horizontal, 0, stick.Vertical);
        moveDir *= _maxSpd * Time.deltaTime;

        transform.position += moveDir;
    }
}
