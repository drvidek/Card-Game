using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool _pressed;
    public bool Pressed
    {
        get => _pressed;
        set { _pressed = false; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }
}
