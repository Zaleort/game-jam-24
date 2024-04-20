using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public static GameController Instance { get; private set; }
  public static Player player;
  public static List<Structure> structures;

  public static int DIFFICULTY_EASY = 0;
  public static int DIFFICULTY_MEDIUM = 1;
  public static int DIFFICULTY_HARD = 2;
  public int difficulty = DIFFICULTY_EASY;

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
    difficulty += 1;
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
