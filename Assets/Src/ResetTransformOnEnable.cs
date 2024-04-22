using UnityEngine;

public class ResetTransformOnEnable : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPosition = Vector3.zero;
    [SerializeField] private Vector3 defaultRotation = Vector3.zero;
    [SerializeField] private Vector3 defaultScale = Vector3.one;

    void OnEnable()
    {
        ResetTransform();
    }

    private void ResetTransform()
    {
        // Restablecer posici�n
        transform.position = defaultPosition;

        // Restablecer rotaci�n
        transform.rotation = Quaternion.Euler(defaultRotation);

        // Restablecer escala
        transform.localScale = defaultScale;
    }
}
