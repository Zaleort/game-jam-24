using System.Collections;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate; // Referencia al objeto que quieres activar
    public float delayInSeconds = 5.0f; // Tiempo de retardo en segundos antes de activar el objeto

    private void Start()
    {
        if (objectToActivate != null)
        {
            // Asegura que el objeto esté inicialmente desactivado
            objectToActivate.SetActive(false);

            // Inicia la coroutine para activar el objeto después del retardo
            StartCoroutine(ActivateObjectAfterDelay());
        }
    }

    IEnumerator ActivateObjectAfterDelay()
    {
        // Espera el tiempo definido en delayInSeconds
        yield return new WaitForSeconds(delayInSeconds);

        // Activa el objeto
        objectToActivate.SetActive(true);
    }
}
