using UnityEngine;
using System.Collections;
using FMODUnity;  // Incluye este namespace para utilizar FMOD

public class CreditosSalir : MonoBehaviour
{
    [SerializeField]
    private string fmodEventPath = "event:/YourEventPath";  // Cambia esto por el path de tu evento en FMOD
    [SerializeField]
    private float initialDelaySeconds = 2f;  // Delay inicial antes de reproducir el sonido, configurable desde el Inspector
    [SerializeField]
    private float timeoutSeconds = 10f;  // Duración en segundos después de la cual el juego se cerrará

    private FMOD.Studio.EventInstance fmodEventInstance;

    void Start()
    {
        // Comienza una coroutine que manejará el delay inicial y la reproducción del evento de FMOD
        StartCoroutine(StartSoundAfterDelay(initialDelaySeconds));
        // Comienza una coroutine para contar el tiempo antes de cerrar el juego
        StartCoroutine(ExitAfterTime(timeoutSeconds + initialDelaySeconds));
    }

    void Update()
    {
        // Comprueba si el usuario presiona la tecla Espacio o Esc
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    private IEnumerator StartSoundAfterDelay(float delay)
    {
        // Espera el tiempo de delay configurado antes de iniciar el sonido
        yield return new WaitForSeconds(delay);

        // Crea una instancia del evento de FMOD y lo reproduce
        fmodEventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        fmodEventInstance.start();
    }

    private IEnumerator ExitAfterTime(float seconds)
    {
        // Espera la cantidad de segundos especificados antes de cerrar el juego
        yield return new WaitForSeconds(seconds);
        ExitGame();
    }

    private void ExitGame()
    {
        // Detiene el evento de FMOD
        fmodEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodEventInstance.release();

        // Cierra la aplicación
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
