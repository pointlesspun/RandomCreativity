
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshSheetGenerator))]
[CanEditMultipleObjects]
public class MeshSheetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create sheet"))
        {
            var generator = target as MeshSheetGenerator;

            generator.TryCreateMeshSheet();
        }
    }
}

