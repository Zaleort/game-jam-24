using UnityEngine;
using FMODUnity;

public class SoundEventManager : MonoBehaviour
{
    [EventRef]
    public string[] soundEvents; // Aquí puedes asignar los eventos de sonido desde el Inspector

    private FMOD.Studio.EventInstance[] eventInstances;
    private int currentEventIndex = 0;

    void Start()
    {
        eventInstances = new FMOD.Studio.EventInstance[soundEvents.Length];
        for (int i = 0; i < soundEvents.Length; i++)
        {
            eventInstances[i] = FMODUnity.RuntimeManager.CreateInstance(soundEvents[i]);
        }
        PlayNextSound();
    }

    void PlayNextSound()
    {
        if (currentEventIndex < soundEvents.Length)
        {
            eventInstances[currentEventIndex].start();
            currentEventIndex++;
        }
    }
}