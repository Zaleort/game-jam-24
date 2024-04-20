using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private float damage = 100f;

    void Start()
    {
        
    }

    void Update()
    {
      if (isRepaired() == true)
      {
        GameController.Instance.StructureIsRepaired();
      }
    }

    public void Repair()
    {
        damage -= 25f;
    }

    private bool isRepaired()
    {
        return damage <= 0;
    }
}
