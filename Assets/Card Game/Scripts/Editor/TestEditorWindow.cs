using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestEditorWindow : EditorWindow
{
    //this is the menu, sub-menu, and window option
    [MenuItem("Rubber Duck/Tool Windows/Test Window")]
    //this public static method is what creates the window
    public static void ShowWindow()
    {
        //GetWindow comes from EditorWindow - we don't have to call it when we are inheriting from EditorWindow
        GetWindow(typeof(TestEditorWindow));
    }

    string _objectName = "New Object";
    string _objectTag = "Untagged";
    GameObject _objectToSpawn;
    GameObject _parentObject;
    float _objectScale = 1f;
    Vector3 _spawnPosition;
    bool _localPosition;
    bool _randomInRadius;
    float _spawnRadius;
    bool _spawnMultiple;
    int _spawnCount = 1;
    List<GameObject> _objectList = new List<GameObject>();

    private void OnGUI()
    {
        //the display of the actual window

        //a label at the top of the window
        GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);

        //Begin a check for any changes
        EditorGUI.BeginChangeCheck();
        //create an Object field
        _objectToSpawn = EditorGUILayout.ObjectField("Prefab To Spawn", _objectToSpawn, typeof(GameObject), false) as GameObject;
        //if the field has changed
        if (EditorGUI.EndChangeCheck())
        {
            //if there is an object in the field
            if (_objectToSpawn != null)
                //update the tag
                _objectTag = _objectToSpawn.tag;
        }

        //if there is a prefab in the Object field
        if (_objectToSpawn != null)
        {
            //create a text field to update object name
            _objectName = EditorGUILayout.TextField("Object Name", _objectName);

            //create a tag field to update object tag
            _objectTag = EditorGUILayout.TagField("Object Tag", _objectTag);

            EditorGUILayout.BeginHorizontal();
            _spawnMultiple = EditorGUILayout.Toggle("Spawn Multiple", _spawnMultiple);
            _spawnCount = _spawnMultiple ? EditorGUILayout.IntField("Count", _spawnCount) : 1;
            EditorGUILayout.EndHorizontal();

            _parentObject = EditorGUILayout.ObjectField("Object Parent", _parentObject, typeof(GameObject), true) as GameObject;
            if (_parentObject != null && !_parentObject.scene.IsValid())
            {
                Debug.LogError("ERROR: Parent must be present in scene");
                _parentObject = null;
            }

            //create a vector3 field to update the position
            _spawnPosition = EditorGUILayout.Vector3Field("Position To Spawn", _spawnPosition);

            //if there is a parent object
            if (_parentObject != null)
            {
                //allow toggle of whether to use local position
                _localPosition = EditorGUILayout.Toggle("Use Local Position", _localPosition);
            }
            else
            {
                _localPosition = false;
            }

            //Random in Radius group
            EditorGUILayout.BeginHorizontal();
            _randomInRadius = EditorGUILayout.Toggle("Spawn In Random Radius", _randomInRadius);
            _spawnRadius = _randomInRadius ? EditorGUILayout.FloatField("Radius", _spawnRadius) : 0;
            EditorGUILayout.EndHorizontal();

            //create a slider to update the scale
            _objectScale = EditorGUILayout.Slider("Object Scale", _objectScale, 0.25f, 10f);

            //Reset group
            EditorGUILayout.BeginHorizontal();
            //create a button and check if it is pressed
            if (GUILayout.Button(new GUIContent("Reset position", "Deselect Vector3 fields before clicking me")))
            {
                //reset the vector3 to zero
                _spawnPosition = Vector3.zero;
            }
            //create a button and check if it is pressed
            if (GUILayout.Button("Reset scale"))
            {
                //reset the scale to 1
                _objectScale = 1f;
            }
            EditorGUILayout.EndHorizontal();
            
            //Spawn button
            //create a button and check if it is pressed
            if (GUILayout.Button("Spawn"))
            {
                //run the SpawnObject method
                SpawnObject();
            }

            GUILayout.Space(10);

            //Undo group
            EditorGUI.BeginDisabledGroup(_objectList.Count == 0);
            EditorGUILayout.BeginHorizontal();
            //create a button and check if it is pressed
            if (GUILayout.Button("Undo last"))
            {
                //run the SpawnObject method
                DeleteLastObject();
            }
            if (GUILayout.Button("Delete All"))
            {
                //run the SpawnObject method
                DeleteAllObjects();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

        }
    }

    private void SpawnObject()
    {
        //if the window has an empty name field
        if (_objectName == string.Empty)
        {
            //log an error and exit the method
            Debug.LogError("ERROR: Please enter a name for the object");
            return;
        }

        for (int i = 0; i < _spawnCount; i++)
        {
            //instantiate the object
            GameObject spawnObject = Instantiate(_objectToSpawn, _parentObject != null ? _parentObject.transform : null);
            //create a Vector3 based on the random radius
            Vector3 finalSpawnPos = _randomInRadius ? new Vector3(_spawnPosition.x + Random.Range(-_spawnRadius, _spawnRadius), _spawnPosition.y + Random.Range(-_spawnRadius, _spawnRadius), _spawnPosition.z + Random.Range(-_spawnRadius, _spawnRadius))
                                                    : _spawnPosition;
            //set the position using the local or global position
            spawnObject.transform.position = _localPosition ? _parentObject.transform.position + finalSpawnPos : finalSpawnPos;
            //set the name, numbering if multiple spawns
            spawnObject.name = _spawnCount > 1 ? _objectName + i.ToString() : _objectName;
            //set the tag and scale
            spawnObject.tag = _objectTag;
            spawnObject.transform.localScale = Vector3.one * _objectScale;
            //add the object to the spawn list
            _objectList.Add(spawnObject);
        }
    }

    private void DeleteLastObject()
    {
        //check for any objects which no longer exist in the scene
        for (int i = 0; i < _objectList.Count; i++)
        {
            if (_objectList[i] == null)
            {
                _objectList.Remove(_objectList[i]);
                i--;
            }
        }

        if (_objectList.Count == 0)
        {
            Debug.LogError("ERROR: Nothing to delete");
            return;
        }

        DestroyImmediate(_objectList[_objectList.Count - 1]);
        _objectList.Remove(_objectList[_objectList.Count - 1]);
    }

    private void DeleteAllObjects()
    {
        if (_objectList.Count == 0)
        {
            Debug.LogError("ERROR: Nothing to delete");
            return;
        }

        //check for any objects which no longer exist in the scene
        for (int i = 0; i < _objectList.Count; i++)
        {
            if (_objectList[i] != null)
                DestroyImmediate(_objectList[i]);
            _objectList.Remove(_objectList[i]);
            i--;
        }
    }
}
