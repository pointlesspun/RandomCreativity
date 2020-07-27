using UnityEngine;
using UnityEditor;

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
        // Need a delay call because of Unity-implementation reasons 
#if UNITY_EDITOR
        EditorApplication.delayCall += UpdateOrCreateMesh;
#else
        UpdateOrCreateMesh();
#endif
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

            // check if in editor mode or in game mode, we need different materials (and meshes) otherwise we get warnings
            // from the editor
            if (Application.isPlaying)
            {
                _meshRenderer.material = polyMaterial;
            }
            else
            {
                _meshRenderer.sharedMaterial = polyMaterial;
            }
        }

        var mesh = new Mesh();
        
        if (Application.isPlaying)
        {     
            _meshFilter.mesh = mesh;
        }
        else
        {
            _meshFilter.sharedMesh = mesh;
        }

        UpdateMeshDefinition(mesh, _meshDefinition);
    }

    /// <summary>
    /// Called if a new mesh definition is available
    /// </summary>
    public void UpdateMesh()
    {
        Contract.Requires(_meshFilter != null && _meshRenderer != null);

        var mesh = Application.isPlaying ? _meshFilter.mesh : _meshFilter.sharedMesh;
    
        mesh.Clear();

        // check if in editor mode or in game mode, we need to assign different materials and meshes otherwise we get warnings
        // from the editor
        if (Application.isPlaying)
        {
            UpdateMaterial(_meshRenderer, _meshMaterial, _meshColor);
        }
        else
        {
           UpdateSharedMaterial(_meshRenderer, _meshMaterial, _meshColor);
        }

        UpdateMeshDefinition(mesh, _meshDefinition);
    }

    /// <summary>
    /// Callback from the editor something has changed
    /// </summary>
    public void OnValidate()
    {
        // Need a delay call because of Unity-implementation reasons 
#if UNITY_EDITOR
        EditorApplication.delayCall += UpdateOrCreateMesh;
#endif
    }

    public void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall -= UpdateOrCreateMesh;
#endif
    }

    private void UpdateOrCreateMesh()
    {
        // if the user defined a mesh, update or create it 
        if (_meshDefinition != null && _meshDefinition.IsValid())
        {
            if (_meshFilter == null || _meshRenderer == null)
            {
                CreateMesh();
            }
            else
            {
                UpdateMesh();
            }
        }
    }    

    private void UpdateMaterial(MeshRenderer meshRenderer, Material meshMaterial, Color meshColor)
    {
        // is mesh material different from the current material and is it defined? 
        if (meshRenderer.material != meshMaterial && meshMaterial != null)
        {
            var polyMaterial = new Material(meshMaterial);
            polyMaterial.color = meshColor;
            meshRenderer.material = polyMaterial;
        }
        // material defined and mesh material is different
        else if (meshRenderer.material != meshMaterial && meshMaterial == null)
        {
            meshRenderer.material = null;
        }
        else if (meshRenderer.material == meshMaterial && meshMaterial != null)
        {
            meshRenderer.material.color = meshColor;
        }
    }

    private void UpdateSharedMaterial(MeshRenderer meshRenderer, Material meshMaterial, Color meshColor)
    {
        // is mesh material different from the current material and is it defined? 
        if (meshRenderer.sharedMaterial != meshMaterial && meshMaterial != null)
        {
            var polyMaterial = new Material(meshMaterial);
            polyMaterial.color = meshColor;
            meshRenderer.sharedMaterial = polyMaterial;
        }
        // material defined and mesh material is different
        else if (meshRenderer.sharedMaterial != meshMaterial && meshMaterial == null)
        {
            meshRenderer.sharedMaterial = null;
        }
        else if (meshRenderer.sharedMaterial == meshMaterial && meshMaterial != null)
        {
            meshRenderer.sharedMaterial.color = meshColor;
        }
    }

    /// <summary>
    /// Set all the properties (verts, uvs, tris) of the mesh and recalculate all relevant settings (normals, bounds, tangents)
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="definition"></param>
    private void UpdateMeshDefinition(Mesh mesh, MeshDefinition definition)
    {
        mesh.vertices = definition._vertices;
        mesh.uv = definition._uv;
        mesh.triangles = definition._triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }
}
