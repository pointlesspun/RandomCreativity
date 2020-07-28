using TMPro;
using UnityEngine;

/// <summary>
/// Interaction between the UI and the MeshSheetGenerator demo.
/// </summary>
public class MeshSheetUI : MonoBehaviour
{
    public GameObject _widthTextfieldObject;
    public GameObject _heightTextfieldObject;

    private MeshSheetGenerator _generator;
    private TMP_InputField _widthField;
    private TMP_InputField _heightField;


    public void Start()
    {
        var generatorObject = GameObject.FindGameObjectWithTag(Tags.MeshSheetGeneratorTag);

        if (!generatorObject)
        {
            Debug.LogWarning("MeshSheetUI component belonging to " + gameObject.name
                                + " is looking for an object with a tag " + Tags.MeshSheetGeneratorTag + ", but none was found. Mesh UI will not work");
        }
        else
        {
            _generator = generatorObject.GetComponent<MeshSheetGenerator>();

            if (!_generator)
            {
                Debug.LogWarning("MeshSheetUI component belonging to " + gameObject.name
                                    + " is looking for a component of the type " + typeof(MeshSheetGenerator).Name + ", but none was found on object " 
                                    + _generator.name + ". Mesh UI will not work");

            }
        }

        _widthField = _widthTextfieldObject ? _widthTextfieldObject.GetComponent<TMP_InputField>() : null;
        _heightField = _heightTextfieldObject ? _heightTextfieldObject.GetComponent<TMP_InputField>() : null;

    }

    public void GenerateMesh()
    {
        if (!_generator)
        {
            Debug.LogWarning("MeshSheetUI has no MeshSheetGenerator, no mesh will be generated");
        }
        else
        {
            var width = _widthField != null && !string.IsNullOrEmpty(_widthField.text) ? int.Parse(_widthField.text) : 1;
            var height = _heightField != null && !string.IsNullOrEmpty(_heightField.text) ? int.Parse(_heightField.text) : 1;

            var clampedWidth = Mathf.Clamp(width, 1, 100);
            var clampedHeight = Mathf.Clamp(height, 1, 100);


            var offsetX = clampedWidth / 2;
            var offsetZ = clampedHeight / 2;

            _widthField.text = clampedWidth.ToString();
            _heightField.text = clampedHeight.ToString();

            _generator.TryCreateSheet(clampedWidth, clampedHeight, Vector3.right, Vector3.forward, new Vector3(-offsetX, 0, -offsetZ));
        }

    }
}
