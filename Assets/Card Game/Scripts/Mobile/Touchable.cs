using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    public Renderer testRend;
    public Color[] testColors = new Color[4];
    private void Start()
    {
        testRend = GetComponent<Renderer>();
    }

    public void OnTouchDown()
    {
        testRend.material.color = testColors[0];
    }

    public void OnTouchStay()
    {
        testRend.material.color = testColors[1];
    }

    public void OnTouchUp()
    {
        testRend.material.color = testColors[2];
    }
    public void OnTouchExit()
    {
        testRend.material.color = testColors[3];
    }
}
