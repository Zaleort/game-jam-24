using FMODUnity;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject mainGame; // Asigna el primer GameObject en el Inspector
    public GameObject creditosSalir; // Asigna el segundo GameObject en el Inspector
    public GameObject gameOver; // Asigna el segundo GameObject en el Inspector


    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            StopAllFMODSounds(); // Detiene todos los sonidos de FMOD
            mainGame.SetActive(true);
            gameOver.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllFMODSounds(); // Detiene todos los sonidos de FMOD
            creditosSalir.SetActive(true);
            gameOver.SetActive(false);
        }
    }

    void StopAllFMODSounds()
    {
        // Detiene todos los eventos de FMOD que están actualmente reproduciéndose
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/"); // Obtén el bus maestro
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Detiene todos los eventos en el bus maestro con un fade out
    }
}
