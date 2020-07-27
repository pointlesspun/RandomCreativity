#define REPORT_MESH_GENERATION

#if REPORT_MESH_GENERATION
using System.Text;
#endif

using UnityEngine;


public class MeshSheetGenerator : MonoBehaviour
{
    public const int triangleLength = 3;

    public int _width;
    public int _height;

    public Vector3 _offset = Vector3.zero;

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
            CreateSheet(_width, _height, _dimension1, _dimension2, _offset, _generator);
        }
    }

    public void TryCreateSheet(int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset )
    {
        _generator = GetComponent<MeshGenerator>();

        if (_generator != null && width > 0 && height > 0)
        {
            CreateSheet(width, height, dimension1, dimension2, offset, _generator);
        }
    }

    private static void CreateSheet(int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset , MeshGenerator generator)
    {
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
        CreateVertices(definition, width, height, dimension1, dimension2, offset, debugString);
        CreateTriangles(definition, width, height, debugString);
#else
        CreateVertices(definition, width, height, dimension1, dimension2, offset);
        CreateTriangles(definition, width, height);
#endif
        generator._meshDefinition = definition;

        generator.CreateMesh();
    }

    private static void CreateVertices(MeshDefinition definition, int width, int height, Vector3 dimension1, Vector3 dimension2, Vector3 offset, StringBuilder report = null)
    {
#if REPORT_MESH_GENERATION
        report.AppendLine("Vertices:\n");
#endif

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                var index = x + y * (width + 1);

                definition._vertices[index] = dimension1 * x + dimension2 * y + offset;
                definition._uv[index] = new Vector2(dimension1.normalized.magnitude * x, dimension2.normalized.magnitude * y);

#if REPORT_MESH_GENERATION
                report.AppendLine("vertex " + index + " = " + definition._vertices[index]);
                report.AppendLine("uv " + index + " = " + definition._uv[index]);
#endif
            }
        }
    }


    private static void CreateTriangles(MeshDefinition definition, int width, int height, StringBuilder report = null )
    {
        var quadPoints1 = new int[] { 0, width + 2, 1 };
        var quadPoints2 = new int[] { 0, width + 1, width + 2 };

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

                report.AppendLine("triangle #1 of quad " + quad + " == " + triangle1Debug);
                report.AppendLine("triangle #2 of quad " + quad + " == " + triangle2Debug);

                triangle1Debug.Clear();
                triangle2Debug.Clear();

            }
        }

        Debug.Log(report.ToString());
#else
                }
            }
        }
#endif

    }
}

