using UnityEditor;
using UnityEngine;

/// <summary>
/// Reproduces a bug in unity 2020.1.0f1.4196 Personal
/// </summary>
public class Repro_1 : MonoBehaviour
{
    public int _testField = 0;

    public void OnValidate()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += DummyCall;
#endif
    }

    public void DummyCall()
    {
        /* 
         * Having this line in here causes the error message:
         * MissingReferenceException: The object of type 'Repro_1' has been destroyed but you are still trying to access it.
         * Your script should either check if it is null or you should not destroy the object.
         * UnityEngine.Component.GetComponent[T] () (at <4cc8ec075538416496e5db5d391208ac>:0)
         * Repro_1.DummyCall () (at Assets/Scripts/Misc/Repro_1.cs:17)
         * UnityEditor.EditorApplication.Internal_CallDelayFunctions () (at <b17f35b08b864a3ca09a7032b437596e>:0)
         * 
         * Version: 2020.1.0f1.4196 Personal
         * Revision: 2020.1/staging 2ab9c4179772
         * Built: Wed, 15 Jul 2020 21:28:18 GMT
         */
        var nullComponent = GetComponent<MeshFilter>();
        _testField++;
    }
}
