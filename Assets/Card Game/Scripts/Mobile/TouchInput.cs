using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    //layer mask to define what is touchable
    public LayerMask touchMask;
    //list of things we have currently touched
    [SerializeField] private List<GameObject> _touchList = new List<GameObject>();
    //array of old touches
    [SerializeField] private GameObject[] _touchOld;
    //to see if we are touching an object in the correct layer mask
    [SerializeField] private RaycastHit _hitInfo;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        #region Developer
        //this is the developer version of the code
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click registered");
            //set a new array to the size of the list length
            _touchOld = new GameObject[_touchList.Count];
            Debug.Log("TouchOld set to length " + _touchList.Count);

            //copy the list to the array
            _touchList.CopyTo(_touchOld);
            Debug.Log("List copied to array");
            //clear the currnet list
            _touchList.Clear();
            Debug.Log("list cleared");
            //create a Ray from mouse position on screen
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ray created");
            //if the ray hits the touch mask
            if (Physics.Raycast(ray, out _hitInfo, touchMask))
            {
                Debug.Log("Raycast found target");

                //the object we hit store a temp
                GameObject touchedObj = _hitInfo.transform.gameObject;
                //add the temp to the list
                _touchList.Add(touchedObj);
                //if mouse pressed
                if (Input.GetMouseButtonDown(0))
                {
                    //send message to the touched object OnTouchDown
                    touchedObj.SendMessage("OnTouchDown", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchDown sent to " + touchedObj.name);
                }
                //if mouse held
                if (Input.GetMouseButton(0))
                {
                    //send message to the touched object OnTouch
                    touchedObj.SendMessage("OnTouchStay", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchStay sent to " + touchedObj.name);
                }
                //if mouse released
                if (Input.GetMouseButtonUp(0))
                {
                    //send message to the touched object OnTouchUp
                    touchedObj.SendMessage("OnTouchUp", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchUp sent to " + touchedObj.name);
                }
            }
            //OnTouchExit
            //check each item in old touches
            foreach (GameObject obj in _touchOld)
            {
                if (!_touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchExit", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchExit sent to " + obj.name);
                }
            }
        }
#endif
        #endregion
        #region User
        //this is the build version of the code (i.e. for mobile)
        if (Input.touchCount > 0)
        {
            //set a new array to the size of the list length
            _touchOld = new GameObject[_touchList.Count];
            //copy the list to the array
            _touchList.CopyTo(_touchOld);
            //clear the currnet list
            _touchList.Clear();
            //check each touch in inputs touch
            foreach (Touch touch in Input.touches)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                //if the ray hits the touch mask
                if (Physics.Raycast(ray, out _hitInfo, touchMask))
                {
                    //the object we hit store a temp
                    GameObject touchedObj = _hitInfo.transform.gameObject;
                    //add the temp to the list
                    _touchList.Add(touchedObj);
                    //if touch pressed
                    if (touch.phase == TouchPhase.Began)
                    {
                        //send message to the touched object OnTouchDown
                        touchedObj.SendMessage("OnTouchDown", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchDown sent to " + touchedObj.name);
                    }
                    //if touch held
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        //send message to the touched object OnTouch
                        touchedObj.SendMessage("OnTouchStay", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchStay sent to " + touchedObj.name);
                    }
                    //if touch released
                    if (touch.phase == TouchPhase.Ended)
                    {
                        //send message to the touched object OnTouchUp
                        touchedObj.SendMessage("OnTouchUp", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchUp sent to " + touchedObj.name);
                    }
                    //if touch cancelled
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        //send message to the touched object OnTouchUp
                        touchedObj.SendMessage("OnTouchExit", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                        Debug.Log("OnTouchExit sent to " + touchedObj.name);
                    }
                }
            }
            //OnTouchExit
            //check each item in old touches
            foreach (GameObject obj in _touchOld)
            {
                if (!_touchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchExit", _hitInfo.point, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("OnTouchExit sent to " + obj.name);
                }
            }
        }
        #endregion
    }
}