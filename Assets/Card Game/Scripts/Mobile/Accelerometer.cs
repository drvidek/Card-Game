using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    [SerializeField] private Vector3 _moveDir;

    // Update is called once per frame
    void Update()
    {
        _moveDir = Input.acceleration;
        transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
    }
}
