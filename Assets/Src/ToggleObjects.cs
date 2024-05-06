using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject object1; // Primer objeto para alternar
    public GameObject object2; // Segundo objeto para alternar

    private void OnDisable()
    {
        if (object1 != null && object2 != null)
        {
            // Si object1 está activado, lo desactiva y activa object2, y viceversa
            bool isObject1Active = object1.activeSelf;
            object1.SetActive(!isObject1Active);
            object2.SetActive(isObject1Active);
        }
    }
}
