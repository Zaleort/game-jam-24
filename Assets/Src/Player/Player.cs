using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Player : MonoBehaviour
{
    public static int START_OXYGEN_LEVEL = 0;
    public static int HIGH_OXYGEN_LEVEL = 1;
    public static int HALF_OXYGEN_LEVEL = 2;
    public static int LOW_OXYGEN_LEVEL = 3;

    public float oxygen = 100f;
    public float oxygenLossPerSecond = 1.0f;
    public bool oxygenLossActivated = false;

    [SerializeField]
    public EventReference oxygenRefillEvent;

    [SerializeField]
    public EventReference deathEvent;
    private bool deathEventPlayed = false;

    [SerializeField]
    public EventReference breathingEvent;
    private FMOD.Studio.EventInstance breathingInstance;

    [SerializeField]
    public EventReference rapidBreathingEvent;
    private FMOD.Studio.EventInstance rapidBreathingInstance;

    public int actualOxygenLevelWarning = START_OXYGEN_LEVEL;

    [SerializeField]
    public EventReference highOxygenWarning;

    [SerializeField]
    public EventReference halfOxygenWarning;

    [SerializeField]
    public EventReference lowOxygenWarning;

    void Start()
    {
        breathingInstance = RuntimeManager.CreateInstance(breathingEvent);
        rapidBreathingInstance = RuntimeManager.CreateInstance(rapidBreathingEvent);
        Debug.Log("Breathing and Rapid Breathing Instances created.");
    }

    void Update()
    {
        ConsumeOxygen();
    }

    private void OnDestroy()
    {
        breathingInstance.release();
        rapidBreathingInstance.release();
        Debug.Log("Breathing Instances released.");
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
        ManageBreathingEvents();
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
            RuntimeManager.PlayOneShot(deathEvent, transform.position);
            deathEventPlayed = true;
            GameController.Instance.GameOver();
        }
    }

    private void ManageBreathingEvents()
    {
        if (oxygen > 20)
        {
            if (!breathingInstance.isValid())
            {
                breathingInstance = RuntimeManager.CreateInstance(breathingEvent);
            }
            rapidBreathingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            breathingInstance.start();
        }
        else
        {
            if (!rapidBreathingInstance.isValid())
            {
                rapidBreathingInstance = RuntimeManager.CreateInstance(rapidBreathingEvent);
            }
            breathingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            rapidBreathingInstance.start();
        }
    }

    private void PlayOxygenWarning()
    {
        if (oxygen <= 80 && oxygen > 50 && actualOxygenLevelWarning != HIGH_OXYGEN_LEVEL)
        {
            actualOxygenLevelWarning = HIGH_OXYGEN_LEVEL;
            RuntimeManager.PlayOneShot(highOxygenWarning, transform.position);
        }

        if (oxygen <= 50 && oxygen > 20 && actualOxygenLevelWarning != HALF_OXYGEN_LEVEL)
        {
            actualOxygenLevelWarning = HALF_OXYGEN_LEVEL;
            RuntimeManager.PlayOneShot(halfOxygenWarning, transform.position);
        }

        if (oxygen <= 20 && actualOxygenLevelWarning != LOW_OXYGEN_LEVEL)
        {
            actualOxygenLevelWarning = LOW_OXYGEN_LEVEL;
            RuntimeManager.PlayOneShot(lowOxygenWarning, transform.position);
        }
    }
}
