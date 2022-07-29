using UnityEngine;
using UnityEditor;

namespace Variable
{
    [CustomEditor(typeof(BaseVariable), true)]
    [CanEditMultipleObjects]
    public class VariableEditor : Editor
    {
        #region Variables
        private SerializedProperty initialValue;
        private SerializedProperty runtimeValue;
        private SerializedProperty runtimeMode;
        private SerializedProperty persistenceMode;

        #endregion

        private void OnEnable()
        {
            initialValue = serializedObject.FindProperty("initialValue");
            runtimeValue = serializedObject.FindProperty("runtimeValue");
            runtimeMode = serializedObject.FindProperty("runtimeMode");
            persistenceMode = serializedObject.FindProperty("persistenceMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(initialValue);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(runtimeValue);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(runtimeMode);
            EditorGUILayout.PropertyField(persistenceMode);

            EditorGUI.BeginDisabledGroup(persistenceMode.boolValue == true);
            if (GUILayout.Button("Save To Initial Value"))
            {
                (target as BaseVariable).SaveToInitialValue();
            }
            EditorGUILayout.PropertyField(runtimeValue);
            EditorGUI.EndDisabledGroup();
            if (target)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

}