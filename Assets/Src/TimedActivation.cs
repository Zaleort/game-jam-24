using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedActivation : MonoBehaviour
{
    // Definir el enum para las acciones de activación/desactivación
    public enum ActionType
    {
        ACTIVAR,
        DESACTIVAR
    }

    [System.Serializable]
    public class ActivationEntry
    {
        public GameObject gameObject;
        public float delayInSeconds;
        public ActionType action;  // Usar ActionType en lugar de bool
    }

    public List<ActivationEntry> activations = new List<ActivationEntry>();

    // Start is called before the first frame update
    void Start()
    {
        // Programar la activación/desactivación para cada entrada
        foreach (var entry in activations)
        {
            StartCoroutine(ActivateDeactivate(entry));
        }
    }

    IEnumerator ActivateDeactivate(ActivationEntry entry)
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(entry.delayInSeconds);

        // Activar o desactivar el objeto dependiendo del enum
        if (entry.gameObject != null)
        {
            entry.gameObject.SetActive(entry.action == ActionType.ACTIVAR);
        }
    }
}
