using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reparable : MonoBehaviour
{
    // Variable booleana para controlar si se puede reparar el objeto
    public bool canRepare = false;

    // Variable para controlar el nivel de reparaci�n
    public int reparedLevel = 0;

    // Valor p�blico que se puede modificar desde el inspector para aumentar el nivel de reparaci�n
    public int repairIncrement = 10;

    // Variable para indicar que el objeto est� completamente reparado
    public bool isRepaired = false;

    // Funci�n que se llama cuando otro collider entra en este trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto que entra tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            canRepare = true; // Permitir reparaci�n
        }
    }

    // Funci�n que se llama cuando otro collider sale de este trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Comprobar si el objeto que sale tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            canRepare = false; // No permitir reparaci�n
        }
    }

    // Funci�n Update se llama en cada frame
    private void Update()
    {
        // Comprobar si se puede reparar y si el usuario presiona la tecla espacio
        if (canRepare && Input.GetKeyDown(KeyCode.Space))
        {
            RepararObjeto(); // Llamar a la funci�n de reparaci�n
        }
    }

    // Funci�n para reparar el objeto
    void RepararObjeto()
    {
        // Aumentar el nivel de reparaci�n
        reparedLevel += repairIncrement;

        // Comprobar si el nivel de reparaci�n ha alcanzado el m�ximo
        if (reparedLevel >= 100)
        {
            reparedLevel = 100; // Asegurar que no supere 100
            isRepaired = true; // Marcar como reparado
            Debug.Log("Objeto reparado completamente!"); // Mensaje de depuraci�n
        }
    }
}
