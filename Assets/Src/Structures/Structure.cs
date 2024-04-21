using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Structure : MonoBehaviour
{
    public float damage = 100f;
    public float activationDistance = 5.0f;
    public float difficultyMultiplier = 1.0f;

    [FMODUnity.EventRef]
    public string repairedEvent;

    [FMODUnity.EventRef]
    public string repairingEvent;
    public EventInstance repairingInstance;
    public bool beingRepaired = false;

    private float[] repairThreshold = new float[2];
    private float soundDuration;
    private float startTime;
    private float tryCooldown = 500f;

    [FMODUnity.EventRef]
    public string failEvent;

    void Start()
    {
      repairingInstance = RuntimeManager.CreateInstance(repairingEvent);
    }

    void Update()
    {
      if (CanBeRepaired())
      {
        ListenForRepair();
      }

      ListenKeyToRepair();
    }

    public void Repair()
    {
        damage -= 100;

        if (isRepaired() == true)
        {
          RuntimeManager.PlayOneShot(repairedEvent, transform.position);
          GameController.Instance.StructureIsRepaired();
        }
  }

    private bool isRepaired()
    {
        return damage <= 0;
    }

    private bool CanBeRepaired()
    {
      if (isRepaired()) return false;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
      {
        Debug.LogError("Player is null chicos a ver");
        return false;
      }

      return Vector3.Distance(player.transform.position, transform.position) < activationDistance;
    }

    private void ListenForRepair()
    {
      repairingInstance.start();
      startTime = Time.time;

      repairingInstance.getDescription(out EventDescription description);
      description.getLength(out int length);
      soundDuration = (length / 1000f) / difficultyMultiplier;
      repairThreshold[0] = soundDuration * 0.4f;
      repairThreshold[1] = soundDuration * 0.6f;

      beingRepaired = true;
    }

    private void StopListenForRepair()
    {
      repairingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      beingRepaired = false;
    }

    private void ListenKeyToRepair()
    {
      if (!beingRepaired) { return; }

      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (Time.time - startTime >= soundDuration)
        {
          startTime = Time.time;
        }

        float keyPressTime = Time.time - startTime;

        if (keyPressTime > repairThreshold[0] && keyPressTime < repairThreshold[1])
        {
          Repair();
          StopListenForRepair();
        }

        else
        {
          StopListenForRepair();
          RuntimeManager.PlayOneShot(failEvent, transform.position);
          StartCoroutine(WaitForCooldown());
        }
      }
    }

    private IEnumerator WaitForCooldown()
    {
      yield return new WaitForSeconds(tryCooldown);
      ListenForRepair();
    }
}
