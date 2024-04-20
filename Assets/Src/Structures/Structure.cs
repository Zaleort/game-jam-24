using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Structure : MonoBehaviour
{
    private float damage = 100f;

    [FMODUnity.EventRef]
    public string repairedEvent;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Repair()
    {
        damage -= 25f;

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
}
