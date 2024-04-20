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

    [FMODUnity.EventRef]
    public string oxygenRefillEvent;

    [FMODUnity.EventRef]
    public string deathEvent;

    [FMODUnity.EventRef]
    public string breathingEvent;
    private FMOD.Studio.EventInstance breathingInstance;

    [FMODUnity.EventRef]
    public string rapidBreathingEvent;
    private FMOD.Studio.EventInstance rapidBreathingInstance;

    public int actualOxygenLevelWarning = START_OXYGEN_LEVEL;

    [FMODUnity.EventRef]
    public string highOxygenWarning;

    [FMODUnity.EventRef]
    public string halfOxygenWarning;

    [FMODUnity.EventRef]
    public string lowOxygenWarning;

  // Start is called before the first frame update
  void Start()
    {
      breathingInstance = RuntimeManager.CreateInstance(breathingEvent);
      rapidBreathingInstance = RuntimeManager.CreateInstance(rapidBreathingEvent);
    }

    // Update is called once per frame
    void Update()
    {
        ConsumeOxygen();
    }

    private void OnDestroy()
    {
      breathingInstance.release();
      rapidBreathingInstance.release();
    }

  public void ActivateOxygenConsumption()
    {
      oxygenLossActivated = true;
    }

    public void ConsumeOxygen()
    {
      if (oxygenLossActivated != true)
      {
        return;
      }

      oxygen -= oxygenLossPerSecond * Time.deltaTime;
      ManageBreathingEvents();
      PlayOxygenWarning();

      CheckIfDead();
    }

    public void RefillOxygen(float quantity)
    {
      oxygen += quantity;
      if (oxygen > 100)
      {
        oxygen = 100;
      }

      RuntimeManager.PlayOneShot(oxygenRefillEvent, transform.position);
    }

    private void CheckIfDead()
    {
      if (oxygen <= 0)
      {
        RuntimeManager.PlayOneShot(deathEvent, transform.position);
        GameController.Instance.GameOver();
      } 
    }

    private void ManageBreathingEvents()
    {
      if (oxygen >= 20 && !breathingInstance.isValid()) {
        rapidBreathingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        breathingInstance.start();
      } else if (oxygen < 20 && !rapidBreathingInstance.isValid()) {
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
