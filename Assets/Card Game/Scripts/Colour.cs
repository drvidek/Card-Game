using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour : MonoBehaviour
{
    [SerializeField] Color color = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color defaultColor = new Color(1f, 1f, 1f, 1f);

    public void RandomColor(bool doAlpha)
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), doAlpha ? Random.Range(0f, 1f) : color.a);
    }

    public void DefaultColor()
    {
        color = new Color(1f, 1f, 1f);
    }
}
