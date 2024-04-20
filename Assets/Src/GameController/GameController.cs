using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public static GameController Instance { get; private set; }
  public static Player player;
  public static Structure[] structures;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void StructureIsRepaired()
  {
    player.RefillOxygen(15);
  }

  public void GameOver()
  {

  }
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
