using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class cache
{
  #region var
  public static int level = 1;
  public static int difficulty = 0;
  // FOR SCREEN BOUNDS
  public static float theDefaultOrthographicSize;
  public static float theOTX; //to get the outer most x of screen
  public static float theOTY; //to get the outer most y of screen
  static float distanceBtwBarriers = 1.3f;
  public static Vector2 packageSpawnPos = new Vector2(-3.5f, 2f);
  public static float dropOffArea_y = -4;
  public static float[] barrierPos = new float[3] { -.7f, -.7f - (distanceBtwBarriers * 1f), -.7f - (distanceBtwBarriers * 2f) };
  // DIFFICULTY MODES:
  // 1: extreme drop off pos = false, moving dropoff = false, gate barrier = false, tight Spaces = false, offset dropoff point = false, moving barrier = false, small dropoff area width = false

  #region stats
  public static int noOfPlays = 0;
  public static int noOfDeliveries = 0;
  public static int noOfBarriersSpawned = 0;
  #endregion
  #endregion

  public static string currentSound = "";
  public static bool playedCurrentSound = false;
  public static void playNewSound(string clip) => currentSound = clip;
  public static void resetSound() => playedCurrentSound = false;

  #region func
  public static bool playSound = true;
  public static string checkClicked(Touch touch)
  {
    // returns the gameobject that the raycast hit(clicked object)
    string theTouchedItem = "blank";
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

    if (hit.collider != null)
    {

      theTouchedItem = hit.collider.name;

    }

    return theTouchedItem;

  }

  public static string checkClicked(Vector3 mousePos)
  {
    // returns the gameobject that the raycast hit(clicked object)
    string theTouchedItem = "blank";
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

    if (hit.collider != null)
    {

      theTouchedItem = hit.collider.name;

    }

    return theTouchedItem;

  }

  //code copied from stackoverflow
  public static Bounds OrthographicBounds(this Camera camera)
  {

    if (!camera.orthographic)
    {
      Debug.Log(string.Format("The camera {0} is not Orthographic!", camera.name), camera);
      return new Bounds();
    }

    var t = camera.transform;
    var x = t.position.x;
    var y = t.position.y;
    var size = camera.orthographicSize * 2;
    var width = size * (float)Screen.width / Screen.height;
    var height = size;

    theOTX = width / 2;
    theOTY = -height / 2;

    return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
  }
  // ------------------------------------------------------
  public static int getLevel()
  {
    return level;
  }
  public static void setLevel(int currentLevel)
  {
    level = currentLevel;
  }
  public static int getDifficulty()
  {
    // range: 0 - 5
    return difficulty;
  }
  public static void setDifficulty(int currentDifficulty)
  {
    difficulty = currentDifficulty;
  }
  public static GameObject findInParent(GameObject parent, string childName)
  {
    GameObject childFound = null;
    for (int i = 0; i < parent.transform.childCount; i++)
      if (parent.transform.GetChild(i).gameObject.name == childName) childFound = parent.transform.GetChild(i).gameObject;

    return childFound;
  }
  public static bool approximatelyEqual(float a, float b, float margin = 0.05f)
  {
    float diff = 0;
    if (a > b)
    {
      diff = a - b;
    }
    else
    {
      diff = b - a;
    }

    return diff <= margin;
  }


  public static string gameDataDir() => Application.persistentDataPath + "/DeliveryProgress.dat";
  public static void saveGame()
  {
    SaveSystem.writeData();
  }
  public static void loadGameData()
  {
    if (File.Exists(gameDataDir()))
    {
      PlayerData data = SaveSystem.readData();

      // Debug.Log("level: " + data.level);
      // Debug.Log("difficulty: " + data.difficulty);
      // Debug.Log("plays: " + data.noOfPlays);
      // Debug.Log("deliveries: " + data.noOfDeliveries);
      // Debug.Log("barriers: " + data.noOfBarriersSpawned);

      level = data.level;
      difficulty = data.difficulty;
      noOfPlays = data.noOfPlays;
      noOfDeliveries = data.noOfDeliveries;
      noOfBarriersSpawned = data.noOfBarriersSpawned;
      playSound = data.playSound;
    }
  }
  public static void saveAndLoadGame()
  {
    saveGame();
    loadGameData();
  }
  public static void deleteGameData()
  {
    if (File.Exists(gameDataDir())) File.Delete(gameDataDir());
  }
  #endregion
}
