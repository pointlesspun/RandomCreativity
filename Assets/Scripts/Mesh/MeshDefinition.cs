using System;
using UnityEngine;

/// <summary>
/// Describes the spatial properties of a mesh
/// </summary>
[Serializable]
public class MeshDefinition
{
    public Vector3[] vertices;
    public Vector2[] uv;
    public int[] triangles;

    public bool IsValid() => vertices.Length >= 3 && triangles.Length >= 3 && uv.Length >= 3;
}

