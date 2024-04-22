using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;

public class SoundEventManager : MonoBehaviour
{
    [SerializeField] private List<EventReference> eventReferences; // Lista de EventReferences
    [SerializeField] private float delayBeforeStart = 0.0f; // Delay antes de empezar cada evento
    [SerializeField] private GameObject objectToActivate; // Objeto a activar al terminar todos los eventos
    [SerializeField] private GameObject objectToDeactivate; // Objeto a desactivar al terminar todos los eventos

    private EventInstance currentEventInstance; // Instancia actual del evento de sonido
    private int currentIndex = 0;
    private bool eventsCompleted = false;

    private void Start()
    {
        StartCoroutine(PlaySequentialEvents());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !eventsCompleted)
        {
            currentEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); // Detiene el evento de sonido inmediatamente
            currentEventInstance.release(); // Libera la instancia
            StopAllCoroutines(); // Detiene la corutina de eventos
            HandleCompletion(); // Maneja la finalización como si todos los eventos hubieran terminado
        }
    }

    private IEnumerator PlaySequentialEvents()
    {
        yield return new WaitForSeconds(delayBeforeStart); // Espera inicial según el delay configurado

        foreach (var eventRef in eventReferences)
        {
            currentEventInstance = RuntimeManager.CreateInstance(eventRef); // Crea y guarda la instancia del evento actual
            currentEventInstance.start();
            yield return new WaitUntil(() => {
                PLAYBACK_STATE pbState;
                currentEventInstance.getPlaybackState(out pbState);
                return pbState == PLAYBACK_STATE.STOPPED;
            });
            currentEventInstance.release(); // Libera la instancia después de que se detenga
        }

        HandleCompletion(); // Maneja las acciones al finalizar todos los eventos
    }

    private void HandleCompletion()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(true); // Activa el GameObject configurado

        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(false); // Desactiva el GameObject configurado

        eventsCompleted = true; // Marca que todos los eventos han sido completados
    }
}
