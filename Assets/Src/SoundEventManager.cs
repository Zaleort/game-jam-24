using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System; // Make sure you have this for IntPtr
using System.Runtime.InteropServices; // Necessary for handling pointers if needed
using System.Collections.Generic;
using System.Collections;

public class SoundEventManager : MonoBehaviour
{
    [SerializeField] private List<EventReference> eventReferences; // Lista de EventReferences
    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(PlaySequentialEvents());
    }

    private IEnumerator PlaySequentialEvents()
    {
        foreach (var eventRef in eventReferences)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef); // Crea una instancia del evento
            eventInstance.start(); // Comienza a reproducir el evento
            eventInstance.release(); // Libera la instancia una vez que comienza a reproducir

            yield return new WaitUntil(() => {
                PLAYBACK_STATE pbState;
                eventInstance.getPlaybackState(out pbState);
                return pbState == PLAYBACK_STATE.STOPPED; // Espera hasta que el evento se haya detenido
            });
        }
    }
}
