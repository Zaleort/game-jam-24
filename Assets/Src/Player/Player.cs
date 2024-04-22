using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Player : MonoBehaviour
{
    private enum OxygenState { HIGH, MID, LOW }

    [System.Serializable]
    public struct OxygenLevelWarning
    {
        public float level;
        public EventReference warningEvent;
    }

    public float oxygen = 100f;
    public float oxygenLossPerSecond = 1.0f;
    public bool oxygenLossActivated = false;

    public GameObject gameOver; // Asigna el primer GameObject en el Inspector
    public GameObject mainGame; // Asigna el segundo GameObject en el Inspector

    [SerializeField] private float highThreshold = 75f;
    [SerializeField] private float midThreshold = 15f;

    [SerializeField]
    public List<OxygenLevelWarning> oxygenWarnings;

    [SerializeField]
    public EventReference oxygenRefillEvent;
    [SerializeField]
    public EventReference deathEvent;

    [SerializeField]
    private EventReference highOxygenEvent;
    [SerializeField]
    private EventReference midOxygenEvent;
    [SerializeField]
    private EventReference lowOxygenEvent;

    private FMOD.Studio.EventInstance currentBreathingInstance;
    private OxygenState currentState = OxygenState.HIGH;
    private bool deathEventPlayed = false;
    private float lastOxygenLevel = 100f;

    void Start()
    {
        SetInitialBreathingEvent();
    }

    void Update()
    {
        ConsumeOxygen();
    }

    private void OnEnable()
    {
        // Reset oxygen level every time this GameObject is re-enabled
        oxygen = 100f;
        lastOxygenLevel = 100f;  // Also reset the last oxygen level trigger
        deathEventPlayed = false;  // Reset the death event flag
        SetInitialBreathingEvent();  // Restart the breathing sound from the appropriate state
    }

    private void OnDestroy()
    {
        if (currentBreathingInstance.isValid())
        {
            currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentBreathingInstance.release();
        }
    }

    public void ActivateOxygenConsumption()
    {
        oxygenLossActivated = true;
    }

    public void ConsumeOxygen()
    {
        if (!oxygenLossActivated)
            return;

        oxygen -= oxygenLossPerSecond * Time.deltaTime;
        oxygen = Mathf.Max(oxygen, 0);
        UpdateBreathingEvent();
        PlayOxygenWarning();
        CheckIfDead();
    }

    public void RefillOxygen(float quantity)
    {
        oxygen += quantity;
        oxygen = Mathf.Min(oxygen, 100);
        RuntimeManager.PlayOneShot(oxygenRefillEvent, transform.position);
    }

    private void CheckIfDead()
    {
        if (oxygen <= 0 && !deathEventPlayed)
        {
            if (currentState == OxygenState.LOW)
            {
                currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            RuntimeManager.PlayOneShot(deathEvent, transform.position);
            deathEventPlayed = true;
            StartCoroutine(DelayedGameOver(5f));  // 2 segundos de retraso, cambia esto según necesites
        }
    }
    void StopAllFMODSounds()
    {
        // Detiene todos los eventos de FMOD que están actualmente reproduciéndose
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/"); // Obtén el bus maestro
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Detiene todos los eventos en el bus maestro con un fade out
    }
    private IEnumerator DelayedGameOver(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        StopAllFMODSounds();
        mainGame.SetActive(false);
        gameOver.SetActive(true);
    }

    private void SetInitialBreathingEvent()
    {
        // Always start with the highest oxygen state when enabled
        currentBreathingInstance = RuntimeManager.CreateInstance(highOxygenEvent);
        currentBreathingInstance.start();
        currentState = OxygenState.HIGH;  // Reset the state to HIGH
    }

    private void UpdateBreathingEvent()
    {
        OxygenState newState = DetermineOxygenState();
        if (newState != currentState)
        {
            currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentBreathingInstance.release();
            switch (newState)
            {
                case OxygenState.HIGH:
                    currentBreathingInstance = RuntimeManager.CreateInstance(highOxygenEvent);
                    break;
                case OxygenState.MID:
                    currentBreathingInstance = RuntimeManager.CreateInstance(midOxygenEvent);
                    break;
                case OxygenState.LOW:
                    currentBreathingInstance = RuntimeManager.CreateInstance(lowOxygenEvent);
                    break;
            }
            currentBreathingInstance.start();
            currentState = newState;
        }
    }

    private void PlayOxygenWarning()
    {
        foreach (var warning in oxygenWarnings)
        {
            if (oxygen <= warning.level && lastOxygenLevel > warning.level)
            {
                RuntimeManager.PlayOneShot(warning.warningEvent, transform.position);
                lastOxygenLevel = warning.level;
                break;
            }
        }
    }

    private OxygenState DetermineOxygenState()
    {
        if (oxygen > highThreshold)
            return OxygenState.HIGH;
        else if (oxygen > midThreshold)
            return OxygenState.MID;
        else
            return OxygenState.LOW;
    }
}
