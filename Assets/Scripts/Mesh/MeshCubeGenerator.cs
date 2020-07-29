using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Generates a number of connected quads based on a width and height value. 
/// xxx note this version does not work well with uvs because of the need to duplicate verts
/// </summary>
public class MeshCubeGenerator : MonoBehaviour
{
    public const int verticesPerCube = 6 * 4;

    /// <summary>
    /// Who would have known the number of vertices in a triangle is three...
    /// </summary>
    public const int triangleLength = 3;

    /// <summary>
    /// Number of quads in the x direction 
    /// </summary>
    public int _width;

    /// <summary>
    /// Number of quads in the y direction 
    /// </summary>
    public int _height;

    /// <summary>
    /// Number of quads in the z direction 
    /// </summary>
    public int _depth;

    /// <summary>
    /// Center point of the sheet
    /// </summary>
    public Vector3 _offset = Vector3.zero;

    private MeshGenerator _generator;

    public void Start()
    {
        TryCreateCubes(_width, _height, _depth, _offset);
    }

    /// <summary>
    /// Call added to be invoked from a UI button
    /// </summary>
    public void TryCreateCubes()
    {
        TryCreateCubes(_width, _height, _depth, _offset);
    }

    public void TryCreateCubes(int width, int height, int depth, Vector3 offset)
    {
        _generator = GetComponent<MeshGenerator>();

        if (_generator != null && width > 0 && height > 0 && depth > 0 )
        {
            _width = width;
            _height = height;
            _offset = offset;

            CreateCubes(width, height, depth, offset, _generator);
        }
    }

    private static void CreateCubes(int width, int height, int depth, Vector3 offset, MeshGenerator generator)
    {
        var vertexCount = width * height * depth * verticesPerCube;

        if (vertexCount > UInt16.MaxValue)
        {
            // https://forum.unity.com/threads/65535-vertices-limit.294585/
            // and https://docs.unity3d.com/ScriptReference/Mesh-indexFormat.html
            Debug.LogWarning("Vertices exceed the Unity maximum, you may experience issues with rendering...");
        }

        var triangleCount = width * height * depth * 6 * 6;
        var definition = new MeshDefinition(vertexCount, triangleCount);

        CreateVertices(definition, width, height, depth, offset);
        CreateTriangles(definition, new Vector3Int(width, height, depth));

        generator._meshDefinition = definition;


        generator.CreateMesh();
    }

    private static void CreateVertices(MeshDefinition definition, int width, int height, int depth, in Vector3 offset)
    {
        var uvs = new Vector2[]
        {
            // bottom
            new Vector2(0, 0.75f),
            new Vector2(0.25f, 0.75f),
            new Vector2(0.0f, 1f),
            new Vector2(0.25f, 1f),
            
            // back
            new Vector2(0.5f, 1f),
            new Vector2(0.25f, 1f),
            new Vector2(0.5f, 0.75f),
            new Vector2(0.25f, 0.75f),

            // right
            new Vector2(0.75f, 0.75f),
            new Vector2(0.75f, 1f),
            new Vector2(0.5f, 0.75f),
            new Vector2(0.5f, 1f),

            // front
            new Vector2(0.0f, 0.5f),
            new Vector2(0.25f, 0.5f),
            new Vector2(0.0f, 0.75f),
            new Vector2(0.25f, 0.75f),

            // left
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.75f),
            new Vector2(0.25f, 0.5f),
            new Vector2(0.25f, 0.75f),

            // top
            new Vector2(0.5f, 0.5f),
            new Vector2(0.75f, 0.5f),
            new Vector2(0.5f, 0.75f),
            new Vector2(0.75f, 0.75f),
        };

        var uvIndex = 0;

        var cubeSides = new Vector3[][]
        {
            // bottom
            new Vector3[] { new Vector3(0,0,1), new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0)},

            // back
            new Vector3[] { new Vector3(0,1,1), new Vector3(1, 1, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 1)},

            // right
            new Vector3[] { new Vector3(1,0,1), new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector3(1, 1, 0)},

            // front
           new Vector3[] { new Vector3(0,0,0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) },

            // left
           new Vector3[] { new Vector3(0,0,0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 1) },

            // top
           new Vector3[] { new Vector3(0,1,0), new Vector3(1, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1) },

        };

        for (int z = 0; z < depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var positionOffset = new Vector3(x, y, z);

                    var baseIndex = (x + (y * width) +  (z * width * height)) * verticesPerCube;

                    foreach ( var side in cubeSides)
                    {
                        for (var i = 0; i < side.Length; i++)
                        {
                            definition._vertices[baseIndex] = positionOffset + side[i] + offset;
                            definition._uv[baseIndex] = uvs[uvIndex];

                            uvIndex = (uvIndex + 1) % uvs.Length;
                            baseIndex++;
                        }
                    }
                }
            }
        }
    }

    private static void CreateTriangles(MeshDefinition definition, in Vector3Int dimensions)
    {
        var cubeTriangles = new int[] { 0, 3, 1, 0, 2, 3 };

        var cubeLocation = Vector3Int.zero;        

        for (int z = 0; z < dimensions.z; z++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int x = 0; x < dimensions.x; x++)
                {
                    cubeLocation.x = x;
                    cubeLocation.y = y;
                    cubeLocation.z = z;

                    CreateQuad(definition, cubeLocation, dimensions, cubeTriangles);
                }
            }
        }
    }

    private static void CreateQuad(MeshDefinition definition, 
                                    in Vector3Int cubeLocation, 
                                    in Vector3Int dimensions, 
                                    int[] cubeTriangles)
    {
        var cubeIndex = (cubeLocation.x
                             + cubeLocation.y * dimensions.x
                             + cubeLocation.z * dimensions.x * dimensions.y);

        var triangleIndex = 6 * 6 * cubeIndex;
        var vertexIndex = 4 * 6 * cubeIndex;

        // create each side of the cube
        for (int j = 0; j < 6; j++)
        {

            for (int i = 0; i < cubeTriangles.Length; i++)
            {
                definition._triangles[triangleIndex] = vertexIndex + 4 * j + cubeTriangles[i];
                triangleIndex++;
            }
        }
    }
}

