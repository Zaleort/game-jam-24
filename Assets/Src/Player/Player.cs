using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float oxygen = 100f;
    public float oxygenLossPerSecond = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ConsumeOxygen();
    }

    public void ConsumeOxygen()
    {
      oxygen -= oxygenLossPerSecond * Time.deltaTime;

      CheckIfDead();
    }

    private void CheckIfDead()
    {
      if (oxygen <= 0)
      {
        GameController.Instance.GameOver();
      } 
    }
}
