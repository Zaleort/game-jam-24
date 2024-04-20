using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float oxygen = 100f;
    public float oxygenLossPerSecond = 1.0f;
    public bool oxygenLossActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ConsumeOxygen();
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

      CheckIfDead();
    }

    public void RefillOxygen(float quantity)
    {
      oxygen += quantity;
      if (oxygen > 100)
      {
        oxygen = 100;
      }
    }

    private void CheckIfDead()
    {
      if (oxygen <= 0)
      {
        GameController.Instance.GameOver();
      } 
    }
}
