using System;
using UnityEngine;

/// <summary>
/// Generates a number of connected quads based on a width and height value. 
/// </summary>
public class MeshSheetGenerator : MonoBehaviour
{
    /// <summary>
    /// Who would have known the number of vertices in a triangle is three...
    /// </summary>
    public const int triangleLength = 3;

    /// <summary>
    /// Number of quads in the direction of dimension 1
    /// </summary>
    public int _width;

    /// <summary>
    /// Number of quads in the direction of dimension 2
    /// </summary>
    public int _height;

    /// <summary>
    /// Center point of the sheet
    /// </summary>
    public Vector3 _offset = Vector3.zero;

    /// <summary>
    /// Direction #1 of the quads making up the meshsheet
    /// </summary>
    public Vector3 _dimension1 = Vector3.right;

    /// <summary>
    /// Direction #2 of the quads making up the meshsheet
    /// </summary>
    public Vector3 _dimension2 = Vector3.up;

    private MeshGenerator _generator;

    public void Start()
    {
        TryCreateSheet(_width, _height, _dimension1, _dimension2, _offset);
    }

    /// <summary>
    /// Call added to be invoked from a UI button
    /// </summary>
    public void TryCreateMeshSheet()
    {
        TryCreateSheet(_width, _height, _dimension1, _dimension2, _offset);        
    }

    public void TryCreateSheet(int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset )
    {
        _generator = GetComponent<MeshGenerator>();

        if (_generator != null && width > 0 && height > 0)
        {
            _width = width;
            _height = height;
            _offset = offset;
            _dimension1 = dimension1;
            _dimension2 = dimension2;

            CreateSheet(width, height, dimension1, dimension2, offset, _generator);
        }
    }

    private static void CreateSheet(int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset , MeshGenerator generator)
    {
        if ((width +1)* (height+1) > UInt16.MaxValue)
        {
            // https://forum.unity.com/threads/65535-vertices-limit.294585/
            // and https://docs.unity3d.com/ScriptReference/Mesh-indexFormat.html
            Debug.LogWarning("Vertices exceed the Unity maximum, you may experience issues with rendering...");
        }

        var vertCount = (width + 1) * (height + 1);
        var triangleCount = width * height * 2 * 3;

        var definition = new MeshDefinition(vertCount, triangleCount);

        CreateVertices(definition, width, height, dimension1, dimension2, offset);
        CreateTriangles(definition, width, height);

        generator._meshDefinition = definition;

        generator.CreateMesh();
    }

    private static void CreateVertices(MeshDefinition definition, int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset)
    {
        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                var index = x + y * (width + 1);

                definition._vertices[index] = dimension1 * x + dimension2 * y + offset;
                definition._uv[index] = new Vector2(dimension1.normalized.magnitude * x, dimension2.normalized.magnitude * y);
            }
        }
    }


    private static void CreateTriangles(MeshDefinition definition, int width, int height )
    {
        var quadPoints1 = new int[] { 0, width + 2, 1 };
        var quadPoints2 = new int[] { 0, width + 1, width + 2 };

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var quad = (x + y * width);
                var offset = quad * 6;

                for (int p = 0; p < triangleLength; p++)
                {
                    var index = offset + p;

                    definition._triangles[index] = (x + y * (width + 1)) + quadPoints1[p];
                    definition._triangles[index + triangleLength] = (x + y * (width + 1)) + quadPoints2[p];
                }
            }
        }
    }
}

