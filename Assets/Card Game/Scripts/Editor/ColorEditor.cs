using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Colour), true)]
[CanEditMultipleObjects]
public class ColorEditor : Editor
{
    private SerializedProperty color;
    public bool toggle;

    private void OnEnable()
    {
        color = serializedObject.FindProperty("color");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(color);

        GUILayout.BeginHorizontal();
        
        toggle = GUILayout.Toggle(toggle, "Randomise Alpha");


        if (GUILayout.Button("Randomise"))
        {
            (target as Colour).RandomColor(toggle);
        }
        if (GUILayout.Button("Reset"))
        {
            (target as Colour).DefaultColor();
        }

        GUILayout.EndHorizontal();

        if (target)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

}
