
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshCubeGenerator))]
[CanEditMultipleObjects]
public class MeshCubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create cubes"))
        {
            var generator = target as MeshCubeGenerator;

            generator.TryCreateCubes();
        }
    }
}

