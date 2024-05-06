using UnityEngine;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

public class SoundLooper : MonoBehaviour
{
    public EventReference[] soundEvents; // Array de EventReferences de FMOD
    public float delayBetweenSounds = 0.1f; // Retardo adicional entre sonidos
    public float initialDelay = 5.0f; // Retardo inicial antes de comenzar el bucle de sonidos

    private void Start()
    {
        StartCoroutine(PlaySoundsInLoop());
    }

    IEnumerator PlaySoundsInLoop()
    {
        // Espera un tiempo inicial antes de comenzar a reproducir los sonidos
        yield return new WaitForSeconds(initialDelay);

        foreach (EventReference soundEvent in soundEvents)
        {
            EventInstance soundInstance = RuntimeManager.CreateInstance(soundEvent);
            soundInstance.start();

            // Espera hasta que el sonido termine
            PLAYBACK_STATE playbackState;
            do
            {
                soundInstance.getPlaybackState(out playbackState);
                yield return null;
            } while (playbackState != PLAYBACK_STATE.STOPPED);

            // Libera los recursos de FMOD
            soundInstance.release();

            // Espera el tiempo adicional entre sonidos si es necesario
            yield return new WaitForSeconds(delayBetweenSounds);
        }

        // Repite todos los sonidos
        StartCoroutine(PlaySoundsInLoop());
    }
}
