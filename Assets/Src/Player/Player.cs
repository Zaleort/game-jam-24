using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Player : MonoBehaviour
{
    private enum OxygenState { HIGH, MID, LOW }

    // Define a structure to hold oxygen level thresholds and corresponding events
    [System.Serializable]
    public struct OxygenLevelWarning
    {
        public float level;
        public EventReference warningEvent;
    }

    public float oxygen = 100f;
    public float oxygenLossPerSecond = 1.0f;
    public bool oxygenLossActivated = false;

    // Dynamic threshold values for HIGH, MID, and LOW states
    [SerializeField] private float highThreshold = 75f;
    [SerializeField] private float midThreshold = 15f;

    [SerializeField]
    public List<OxygenLevelWarning> oxygenWarnings; // List of variable oxygen warnings

    [SerializeField]
    public EventReference oxygenRefillEvent;
    [SerializeField]
    public EventReference deathEvent;

    [SerializeField]
    private EventReference highOxygenEvent; // FMOD event for HIGH oxygen level
    [SerializeField]
    private EventReference midOxygenEvent;  // FMOD event for MID oxygen level
    [SerializeField]
    private EventReference lowOxygenEvent;  // FMOD event for LOW oxygen level

    private FMOD.Studio.EventInstance currentBreathingInstance;
    private OxygenState currentState = OxygenState.HIGH;
    private bool deathEventPlayed = false;
    private float lastOxygenLevel = 100f; // Track the last oxygen level that triggered a warning

    void Start()
    {
        SetInitialBreathingEvent();
    }

    void Update()
    {
        ConsumeOxygen();
    }

    private void OnDestroy()
    {
        currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        currentBreathingInstance.release();
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
            // Immediately stop the low oxygen sound if it is playing
            if (currentState == OxygenState.LOW)
            {
                currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            RuntimeManager.PlayOneShot(deathEvent, transform.position);
            deathEventPlayed = true;
            GameController.Instance.GameOver();
        }
    }

    private void SetInitialBreathingEvent()
    {
        currentBreathingInstance = RuntimeManager.CreateInstance(highOxygenEvent);
        currentBreathingInstance.start();
    }

    private void UpdateBreathingEvent()
    {
        OxygenState newState = DetermineOxygenState();
        if (newState != currentState)
        {
            // Stop the current event
            currentBreathingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentBreathingInstance.release();

            // Start the new appropriate event based on the current oxygen state
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
            currentState = newState;  // Update current state
        }
    }

    private void PlayOxygenWarning()
    {
        // Check against each defined warning level
        foreach (var warning in oxygenWarnings)
        {
            if (oxygen <= warning.level && lastOxygenLevel > warning.level)
            {
                RuntimeManager.PlayOneShot(warning.warningEvent, transform.position);
                lastOxygenLevel = warning.level; // Update last triggered level
                break; // Only play the first matching warning
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
