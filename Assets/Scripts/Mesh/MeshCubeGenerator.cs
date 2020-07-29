using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Generates a number of connected quads based on a width and height value. 
/// xxx note this version does not work well with uvs because of the need to duplicate verts
/// </summary>
public class MeshCubeGenerator : MonoBehaviour
{
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

    private StringBuilder _debugReport = new StringBuilder();

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
        if ((width + 1) * (height + 1) * (depth +1) > UInt16.MaxValue)
        {
            // https://forum.unity.com/threads/65535-vertices-limit.294585/
            // and https://docs.unity3d.com/ScriptReference/Mesh-indexFormat.html
            Debug.LogWarning("Vertices exceed the Unity maximum, you may experience issues with rendering...");
        }

        var vertCount = (width + 1) * (height + 1) * (depth+1);
        var triangleCount = width * height * depth * 2 * 3 * 6;
        var definition = new MeshDefinition(vertCount, triangleCount);

        CreateVertices(definition, width, height, depth, offset);
        CreateTriangles(definition, new Vector3Int(width, height, depth));

        generator._meshDefinition = definition;

        generator.CreateMesh();
    }

    private static void CreateVertices(MeshDefinition definition, int width, int height, int depth, in Vector3 offset)
    {
        var uvs = new Vector2[]
        {
            new Vector2(0, 0.5f),
            new Vector2(0.25f, 0.5f),
            new Vector2(0.0f, 0.75f),
            new Vector2(0.25f, 0.75f),
            new Vector2(0.0f, 0.5f),
            new Vector2(0.25f, 0.5f),
            new Vector2(0.0f, 0.75f),
            new Vector2(0.25f, 0.75f),
        };

        var uvIndex = 0;

        for (int z = 0; z <= depth; z++)
        {
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    var index = x + y * (width + 1) + z *((width+1) * (height+1));

                    definition._vertices[index] = new Vector3(x, y, z) + offset;
                    definition._uv[index] = uvs[uvIndex];

                    uvIndex = (uvIndex + 1) % uvs.Length;
                }
            }
        }
    }

    private static void CreateTriangles(MeshDefinition definition, in Vector3Int dimensions)
    {
        var w1  = dimensions.x + 1;
        var h1  = dimensions.y + 1;
        var wh1 = w1 * h1;

        // relative vertex points
        var v1 = 0;
        var v2 = 1;
        var v3 = wh1;
        var v4 = wh1 + 1;
        var v5 = w1;
        var v6 = 1 + w1;
        var v7 = w1 + wh1;
        var v8 = 1 + w1 + wh1;
        
        var cubeTriangles = new int[][] {
            
            // bottom 1
            new int[] { v2, v4, v1 },
            // bottom 2
            new int[] { v4, v3, v1 },
            
            // backwards
            new int[] { v4, v8, v3 },
            // bottom 2
            new int[] { v8, v7, v3},
            
            // right
            new int[] { v6, v4, v2 },
            // bottom 2
            new int[] { v6, v8, v4},
            
            // front 1
            new int[] { v1, v6, v2 },
            // front 2
            new int[] { v1, v5, v6 },
           
            // left 1
            new int[] { v3, v5, v1 },
            // left 2
             new int[] { v3, v7, v5 },
                         
            // top 1
            new int[] { v5, v8, v6 },
            // top 2
            new int[] { v5, v7, v8 },
        };

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
                                    int[][] cubeTriangles)
    {
        var triangleOffset = 6 * 6 *
                            (cubeLocation.x
                             + cubeLocation.y * dimensions.x
                             + cubeLocation.z * dimensions.x * dimensions.y);

        for (int i = 0; i < cubeTriangles.Length; i++)
        {
            var index = triangleOffset + i * triangleLength;
            var triangleIndices = cubeTriangles[i];

            for (int j = 0; j < triangleIndices.Length; j++)
            {
                var vertexIndex = cubeLocation.x
                                + (cubeLocation.y * (dimensions.x + 1))
                                + (cubeLocation.z * (dimensions.x + 1) * (dimensions.y + 1));

                definition._triangles[index+j] = vertexIndex + triangleIndices[j];
            }
        }
    }
}

