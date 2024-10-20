using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class WorldRotation : Editor
{

    public override void OnInspectorGUI()
    {
        Transform targetTransform = (Transform)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("World Rotation", EditorStyles.boldLabel);

        var worldRotation = targetTransform.rotation;

        var newRotation = EditorGUILayout.Vector3Field("Rotation (Euler)", worldRotation.eulerAngles);

        targetTransform.rotation = Quaternion.Euler(newRotation);

        EditorGUILayout.Space();
    }
}
