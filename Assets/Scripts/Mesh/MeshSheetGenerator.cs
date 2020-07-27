using UnityEngine;

public class MeshSheetGenerator : MonoBehaviour
{
    public int _width;
    public int _height;

    public Vector3 _dimension1 = Vector3.right;
    public Vector3 _dimension2 = Vector3.up;

    private MeshGenerator _generator;

    public void Start()
    {
        TryCreateMeshSheet();
    }

    public void TryCreateMeshSheet()
    {
        _generator = GetComponent<MeshGenerator>();

        if (_generator != null && _width > 0 && _height > 0)
        {
            CreateSheet(_width, _height, _generator);
        }
    }

    private void CreateSheet(int width, int height, MeshGenerator generator)
    {
        const int triangleLength = 3;
        var quadPoints1 = new int[] { 0, width + 2, 1 };
        var quadPoints2 = new int[] { 0, width + 1, width + 2 };

        var vertCount = (width + 1) * (height + 1);
        var triangleCount = width * height * 2 * 3;

#if REPORT_MESH_GENERATION
        var debugString = new StringBuilder();

        debugString.AppendLine("VertCount count " + vertCount);
        debugString.AppendLine("Triangle point count " + triangleCount);
#endif
        var definition = new MeshDefinition();

        definition._triangles = new int[triangleCount];
        definition._vertices = new Vector3[vertCount];
        definition._uv = new Vector2[vertCount];

#if REPORT_MESH_GENERATION
        debugString.AppendLine("Vertices:\n");
#endif

        for (int y = 0; y <= height; y++)
        { 
            for (int x = 0; x <= width; x++)
            {
                var index = x + y * (width+1);

                var v = _dimension1 * x + _dimension2 * y;

                definition._vertices[index] = v;
                definition._uv[index] = new Vector2(x, y);

#if REPORT_MESH_GENERATION
                debugString.AppendLine("vertex " + index + " = " + definition._vertices[index]);
#endif
            }
        }

#if REPORT_MESH_GENERATION
        var triangle1Debug = new StringBuilder();
        var triangle2Debug = new StringBuilder();
#endif

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

#if REPORT_MESH_GENERATION
                    triangle1Debug.Append(definition._triangles[index] + ",");
                    triangle2Debug.Append(definition._triangles[index + triangleLength] + ",");
                }

                debugString.AppendLine("triangle #1 of quad " + quad + " == " + triangle1Debug);
                debugString.AppendLine("triangle #2 of quad " + quad + " == " + triangle2Debug);

                triangle1Debug.Clear();
                triangle2Debug.Clear();
                }
            }
        }

        Debug.Log(debugString.ToString());
#else
                }
            }
        }
#endif
    
        generator._meshDefinition = definition;

        generator.CreateMesh();
    }
}

