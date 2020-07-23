using UnityEngine;
using gg.core.util;

/// <summary>
/// Generates a mesh based off a MeshDefinition, Color and Material
/// </summary>
public class MeshGenerator : MonoBehaviour
{
    /// <summary>
    /// Contains all spatial properties of a mesh (and uvs)
    /// </summary>
    public MeshDefinition _meshDefinition;

    /// <summary>
    /// Material applied to the generated mesh
    /// </summary>
    public Material _meshMaterial;

    /// <summary>
    /// Color applied to the mesh
    /// </summary>
    public Color _meshColor = Color.yellow;

    /// <summary>
    /// Resolved or generated mesh filter
    /// </summary>
    private MeshFilter _meshFilter;


    /// <summary>
    /// Resolved or generated mesh renderer
    /// </summary>
    private MeshRenderer _meshRenderer;

    /// <summary>
    /// Tries to generate a mesh when awoken
    /// </summary>
    public void Awake()
    {
        // if the user defined a mesh, update or create it 
        if (_meshDefinition != null && _meshDefinition.IsValid())
        {
            CreateMesh();
        }
    }


    /// <summary>
    /// Create a mesh and all the dependencies (ie components needed to display the mesh)
    /// </summary>
    public void CreateMesh()
    {
        // check dependencies, create them if necessary
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        if (_meshRenderer == null)
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (_meshFilter == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if (_meshMaterial != null)
        {
            var polyMaterial = new Material(_meshMaterial);
            polyMaterial.color = _meshColor;
            _meshRenderer.material = polyMaterial;
        }

        var mesh = new Mesh();
        _meshFilter.mesh = mesh;

        UpdateMeshDefinition(mesh, _meshDefinition);
    }


    /// <summary>
    /// Called if a new mesh definition is available
    /// </summary>
    public void UpdateMesh()
    {
        Contract.Requires(_meshFilter != null && _meshRenderer != null);

        var mesh = _meshFilter.mesh;
    
        mesh.Clear();
            
        if (_meshRenderer.material != null)
        {
            _meshRenderer.material.color = _meshColor;
        }

        UpdateMeshDefinition(mesh, _meshDefinition);
    }

    /// <summary>
    /// Set all the properties (verts, uvs, tris) of the mesh and recalculate all relevant settings (normals, bounds, tangents)
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="definition"></param>
    private void UpdateMeshDefinition(Mesh mesh, MeshDefinition definition)
    {
        mesh.vertices = definition.vertices;
        mesh.uv = definition.uv;
        mesh.triangles = definition.triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }
}
