using System;

using UnityEngine;

/// <summary>
/// Describes the spatial properties of a mesh
/// </summary>
[Serializable]
public class MeshDefinition
{
    public Vector3[] _vertices;
    public Vector2[] _uv;
    public int[] _triangles;

    public bool IsValid() => 
        _vertices != null && _triangles != null && _uv != null &&
        _vertices.Length >= 3 && 
        _triangles.Length >= 3 && _triangles.Length % 3 == 0 &&
        _uv.Length == _vertices.Length;
}

