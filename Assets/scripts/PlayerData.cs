using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
  public int level = 1;
  public int difficulty = 0;
  public int noOfPlays = 0;
  public int noOfDeliveries = 0;
  public int noOfBarriersSpawned = 0;
  public bool playSound = true;

  public PlayerData()
  {
    level = cache.level;
    difficulty = cache.difficulty;
    noOfPlays = cache.noOfPlays;
    noOfDeliveries = cache.noOfDeliveries;
    noOfBarriersSpawned = cache.noOfBarriersSpawned;
    playSound = cache.playSound;
  }
}
