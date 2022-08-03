using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateFromPixelMap : EditorWindow
{
    [MenuItem("Rubber Duck/Tool Windows/Create From Pixel Map")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreateFromPixelMap));
    }

    public Texture2D mapImage;
    private Color currentPixelColour;
    public List<Color> tempColorList = new List<Color>();
    public GameObject[] spawnObj;
    public Color[] foundColours;
    public GameObject parentObject;
    bool _useParentPosition;
    bool _zAxis;
    bool _pivotAtCentre;
    List<GameObject> _spawnedObjects = new List<GameObject>();

    private void OnGUI()
    {
        GUILayout.Label("Create a level from a pixel map");

        EditorGUI.BeginChangeCheck();
        mapImage = EditorGUILayout.ObjectField("Map", mapImage, typeof(Texture2D), false) as Texture2D;
        if (EditorGUI.EndChangeCheck())
        {
            if (mapImage != null)
                ScanForColours();
        }

        if (mapImage != null && foundColours.Length > 0)
        {
            for (int i = 0; i < foundColours.Length; i++)
            {
                GUI.backgroundColor = foundColours[i];
                spawnObj[i] = EditorGUILayout.ObjectField("Object for colour", spawnObj[i], typeof(GameObject), false) as GameObject;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Space(16);

            parentObject = EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true) as GameObject;
            if (parentObject != null && !parentObject.scene.IsValid())
            {
                Debug.LogError("ERROR: Parent must be present in the scene");
                parentObject = null;
            }

            EditorGUI.BeginDisabledGroup(parentObject == null);
            _useParentPosition = EditorGUILayout.Toggle("Use Parent Position", _useParentPosition);
            if (parentObject == null)
            {
                _useParentPosition = false;
            }
            EditorGUI.EndDisabledGroup();

            _zAxis = EditorGUILayout.Toggle("Use Z-Axis", _zAxis);

            _pivotAtCentre = EditorGUILayout.Toggle(new GUIContent("Pivot At Centre", "Tick to change pivot point from bottom-left to centre"), _pivotAtCentre);

            //Spawn button
            //create a button and check if it is pressed
            if (GUILayout.Button("Generate"))
            {
                //run the SpawnObject method
                GenerateLevel();
            }
        }
    }

    void GenerateObject(int x, int y)
    {
        //read the pixel colour
        currentPixelColour = mapImage.GetPixel(x, y);
        if (currentPixelColour.a == 0)
        {
            //if no colour, do nothing
            Debug.Log("Skipped empty pixel");
            return;
        }

        for (int i = 0; i < foundColours.Length; i++)
        {
            Debug.Log("Checking match: " + currentPixelColour.ToString() + " against " + foundColours[i].ToString());
            if (foundColours[i] == currentPixelColour && spawnObj[i] != null)
            {
                Debug.Log("Colour Match Found");
                Vector3 pos = new Vector3(
                    x - (_pivotAtCentre ? mapImage.width / 2f : 0), //set X based on the x coordinate and adjust if pivoting from centre
                    _zAxis ? 0 : y - (_pivotAtCentre ? mapImage.height / 2f : 0), //set Y based on whether we use the Z axis, and if not, adjust if pivoting from centre
                    _zAxis ? y - (_pivotAtCentre ? mapImage.height / 2f : 0) : 0) //set Z based on whether we use the Z axis, and if so, adjust if pivoting from centre
                    + (_useParentPosition ? parentObject.transform.position : Vector3.zero); //add the parent's transform position to the final result to account for their location

                GameObject obj = Instantiate(spawnObj[i], pos, Quaternion.identity, parentObject != null ? parentObject.transform : null);
            }
        }
    }

    void GenerateLevel()
    {
        //scan whole texture and get pixel positions
        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                GenerateObject(x, y);
            }
        }
    }

    void ScanForColours()
    {
        tempColorList.Clear();
        //scan whole texture and get pixel positions
        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                SetColourFromPixel(x, y);
            }
        }
        foundColours = new Color[tempColorList.Count];
        foundColours = tempColorList.ToArray();
        spawnObj = new GameObject[foundColours.Length];
    }

    void SetColourFromPixel(int x, int y)
    {
        //read the pixel colour
        currentPixelColour = mapImage.GetPixel(x, y);
        if (currentPixelColour.a == 0)
        {
            //if no colour, do nothing
            Debug.Log("Skipped empty pixel");
            return;
        }
        if (!tempColorList.Contains(currentPixelColour))
        {
            tempColorList.Add(currentPixelColour);
        }
    }


}
