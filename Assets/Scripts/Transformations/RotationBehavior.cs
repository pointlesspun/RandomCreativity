using UnityEngine;

/// <summary>
/// Rotates an object around an axis by a degree per second
/// </summary>
public class RotationBehavior : MonoBehaviour
{
    public float _anglesPerSecond = 1;

    public Vector3 _axis = Vector3.up;

    public void Update()
    {
        gameObject.transform.rotation *= Quaternion.AngleAxis(_anglesPerSecond * Time.deltaTime, _axis);
    }
}

