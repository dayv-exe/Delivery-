using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  #region var
  public SoundManager smScript;
  [HideInInspector] public int score = 5; // num of unpopped balloons
  public Color[] availableBalloonColors = new Color[5] {
    Color.red,
    Color.green,
    Color.blue,
    Color.yellow,
    Color.cyan
    },
    backgroundColor = new Color[5]{
      new Color(.64f, .64f, .64f),
      new Color(.5f, .59f, 1f),
      new Color(1f, .5f, .59f),
      new Color(1f, .74f, .5f),
      new Color(.74f, .5f, 1f)
    };
  // list of colors available for balloons to use
  Vector2 prevScreen = Vector2.zero;
  public GameObject thePackage, theObstacle_1, theObstacle_2, theObstacle_3, theDropOffArea;
  int maxDiff = 1;
  public Sprite help, pause;
  public Button pauseBtn;
  public TextMeshProUGUI diffLevelTxt;

  // for retry
  public GameObject retryPnl, levelUpPnl, pausePnl, tutorialPnl, levelPnl;
  public TextMeshProUGUI lvlTxt;
  bool isSuspended = false;
  public string statToShowOnPause = "diff"; // diff, deliveries, plays
  string[] allowedStats = new string[3] { "diff", "deliveries", "plays" };
  int deliveriesInARow = 0, failsInARow = 0; // must pass current level twice to move to next level

  public Button soundToggleBtn;
  public Sprite soundOn, soundOff;
  #endregion

  #region func
  void OnApplicationFocus(bool hasFocus)
  {
    if (!hasFocus && !isSuspended)
    {
      suspendGame();
    }
  }
  void Start()
  {
    cache.loadGameData();
    startGame();
    suspendGame();
  }

  int gamesSinceLastAd = 0;
  int adBuffer = 4; // show ad after every 5 games
  void FixedUpdate()
  {
    setSoundIcon();
    diffLevelTxt.text = statToShowOnPause == "plays" ? "Played: " + cache.noOfPlays :
    statToShowOnPause == "deliveries" ? "Deliveries: " + cache.noOfDeliveries :
    "Difficulty: " + cache.getDifficulty();

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (isSuspended) resumeGame();
      else suspendGame();
    }

    lvlTxt.text = cache.getLevel() + "";
    if (prevScreen != new Vector2(Screen.height, Screen.width))
    {
      // updates ui everytime screen resolution changes
      prevScreen.x = Screen.height;
      prevScreen.y = Screen.width;
      setScreenBounds();
    }
  }
  void startDelivery()
  {
  
    cache.playNewSound("");
    cache.resetSound();

    if (cache.noOfDeliveries < 3) maxDiff = 0;
    else if (cache.noOfDeliveries < 10) maxDiff = 1;
    else if (cache.noOfDeliveries < 20) maxDiff = 2;
    else maxDiff = 5;
    // set drop area width (based on difficulty)
    // toggle stagnant of fluid drop area (based on difficulty)
    // spawn drop area

    // calculate and set obstacle positions (based on difficulty)

    // set package vertical speed (based on difficulty)
    // set number of balloons (based on difficulty)
    // spawn package

    // *note:
    // default package spawn height = 2
    // default package spawn x pos = 3.5
    // drop off area = -4.75
    // highest obstacle should be located at a y of .65
    // lowest obstacle at a y of -3.35
    // y gaps between obstacles should be 1
    // placing landing spot in center of land area is easy, end of landing spot is medium, and .5 away from edge of landing spot is hard
    // total num of obstacles allowed is 5

    // THE SCENE SET UP ALGORITHM
    // 1) choose spawn side for the package
    // 2) choose a random pos for the drop off area (opposite the spawn side)
    // 3) based on difficulty choose the width of the drop off area
    // 4) based on difficulty pin point location on dorp off area where package should land
    // 5) get the ideal path for package to land there
    // 6) based on difficulty place obstacles around the path
    // 7) spawn the package

    // DIFFICULTY:
    // barrier (num) per difficulty
    // barrier (move) per difficulty
    // barrier (offset) per difficulty
    // barrier (steep angle) per difficulty
    // barrier (close) if clears 3 in-a-row

    // dropoff (small width) per difficulty
    // dropoff (moving) per difficulty

    // diff lvl 0:
    // barrier num = 0 - 1
    // barrier move = 0
    // barrier offset = 0% - 10%
    // barrier steep angle = false
    // dropoff small width = true
    // dropoff moving = false

    // diff lvl 1:
    // barrier num = 1 - 2
    // barrier move = 0
    // barrier offset = 10% - 20%
    // barrier steep angle = false
    // dropoff small width = true
    // dropoff moving = false

    // diff lvl 2:
    // barrier num = 2 - 3
    // barrier move = 0 - 1
    // barrier offset = 20% - 30%
    // barrier steep angle = true
    // dropoff small width = 70% - 80%
    // dropoff moving = true (then barrier: 0 - 1)

    // diff lvl 3:
    // barrier num = 3 - 4
    // barrier move = 1 - 2
    // barrier offset = 40% - 50%
    // barrier steep angle = true
    // dropoff small width = 60% - 70%
    // dropoff moving = true

    // diff lvl 4:
    // barrier num = 4
    // barrier move = 2
    // barrier offset = 50%
    // barrier steep angle = true
    // dropoff small width = 50% - 60%
    // dropoff moving = true

    Destroy(GameObject.Find("gameArea"));
    GameObject gameArea = new GameObject("gameArea");
    gameArea.transform.position = Vector3.zero;

    #region DIFFICULTY

    float preciseDropOffArea =
    cache.getDifficulty() == 0 ? UnityEngine.Random.Range(0, 11) :
    cache.getDifficulty() == 1 ? UnityEngine.Random.Range(10, 21) :
    cache.getDifficulty() == 2 ? UnityEngine.Random.Range(20, 31) :
    cache.getDifficulty() == 3 ? UnityEngine.Random.Range(30, 41) :
    UnityEngine.Random.Range(40, 51); // the point on dropoff area package should drop, range: 0% - 50% (0 being dead center, to 50 being the most extreme point on dropoff area)

    bool moveDropOff = cache.getDifficulty() < 2 ? false : Convert.ToBoolean(UnityEngine.Random.Range(0, 2)); // to swing dropoff

    float dropOffAreaWidth =
    cache.getDifficulty() < 2 ? 101 :
    cache.getDifficulty() == 2 ? UnityEngine.Random.Range(70, 90) :
    cache.getDifficulty() == 3 ? UnityEngine.Random.Range(60, 90) :
    UnityEngine.Random.Range(60, 101); // the percentage of the drop of area available for use in percentage(0 - 100)

    bool useSteeperPath = cache.getDifficulty() < 2 ? false : Convert.ToBoolean(UnityEngine.Random.Range(0, 2)); // makes the ideal angle of decent steeper

    int totalBarriersAllowed =
    cache.getDifficulty() == 0 ? UnityEngine.Random.Range(0, 2) :
    cache.getDifficulty() == 1 ? UnityEngine.Random.Range(1, 3) :
    cache.getDifficulty() == 2 ? UnityEngine.Random.Range(2, 4) :
    cache.getDifficulty() == 3 ? UnityEngine.Random.Range(3, 5) :
    4; // 0 - 4 allowed

    if (cache.getDifficulty() == 2 && moveDropOff)
    {
      totalBarriersAllowed = UnityEngine.Random.Range(1, 2);
    }

    if (cache.noOfPlays < 2)
    {
      totalBarriersAllowed = 0;
      Camera.main.backgroundColor = backgroundColor[backgroundColor.Length - 1];
    }

    int totalSwingingBarriersAllowed =
    cache.getDifficulty() == 0 ? 0 :
    cache.getDifficulty() == 1 ? 0 :
    cache.getDifficulty() == 2 ? UnityEngine.Random.Range(0, 2) :
    cache.getDifficulty() == 3 ? UnityEngine.Random.Range(1, 3) :
    2; // 0 - 2 allowed

    #endregion

    bool spawnSideLeft = UnityEngine.Random.Range(0, 2) == 1; // 0 for left, 1 for right
    float dropOffAreaLoc = UnityEngine.Random.Range(0f, 1.7f); // the location of the drop off area (0 through 1.7)
    float realDropOffArea = createDropOff(dropOffAreaLoc, preciseDropOffArea, dropOffAreaWidth, spawnSideLeft, theParent: gameArea, swing: moveDropOff); // to create dropoff and find the exact point on the drop off area an ideal path should be plotted for

    pathFinderScript pfs = GameObject.Find(spawnSideLeft ? "pathFinder" : "pathFinder_negative").GetComponent<pathFinderScript>();
    pfs.plotPath(realDropOffArea, spawnSideLeft, useSteeperPath);

    GameObject a_obstacle = createNewObstacle(1, new Vector3(pfs.barrier_x(0), cache.barrierPos[0], 0), 2, spawnSideLeft, theParent: gameArea);
    GameObject b_obstacle = createNewObstacle(2, new Vector3(pfs.barrier_x(1), cache.barrierPos[0], 0), 2, spawnSideLeft, theParent: gameArea);
    GameObject c_obstacle = createNewObstacle(3, new Vector3(pfs.barrier_x(2), cache.barrierPos[1], 0), 2, spawnSideLeft, theParent: gameArea);
    GameObject d_obstacle = createNewObstacle(4, new Vector3(pfs.barrier_x(3), cache.barrierPos[1], 0), 2, spawnSideLeft, theParent: gameArea);

    #region Barrier Activation

    GameObject[] listOfBarriers = new GameObject[4] { a_obstacle, b_obstacle, c_obstacle, d_obstacle };
    GameObject[] activeBars = new GameObject[totalBarriersAllowed];
    int activeBarsLoc = 0;

    totalBarriersAllowed = totalBarriersAllowed > 4 ? 4 : totalBarriersAllowed < 0 ? 0 :
    totalBarriersAllowed > 1 && cache.noOfBarriersSpawned == 0 ? 1 :
    totalBarriersAllowed;

    totalSwingingBarriersAllowed = totalSwingingBarriersAllowed > totalBarriersAllowed ? totalBarriersAllowed : totalSwingingBarriersAllowed > 2 ? 2 : totalSwingingBarriersAllowed < 0 ? 0 : totalSwingingBarriersAllowed;
    while (totalBarriersAllowed > 0)
    {
      // to activate a num of random barriers

      // gets a random inactivated barrier
      int randObs = UnityEngine.Random.Range(0, listOfBarriers.Length);

      // decides wheather to activate it
      bool activate = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

      if (activate)
      {
        // if it is to be activated
        listOfBarriers[randObs].SetActive(true);
        cache.noOfBarriersSpawned++;
        activeBars[activeBarsLoc] = listOfBarriers[randObs];
        activeBarsLoc++;
        totalBarriersAllowed--; // reduces total by one

        // removes the activated barrier from list of inactive
        GameObject[] _temp = new GameObject[listOfBarriers.Length - 1];
        int currentTemp = 0;
        for (int i = 0; i < listOfBarriers.Length; i++)
        {
          if (i != randObs)
          {
            _temp[currentTemp] = listOfBarriers[i];
            currentTemp++;
          }
        }

        listOfBarriers = _temp;
        _temp = null;
      }
    }

    listOfBarriers = null;

    #endregion
    // to swing a num of random barriers

    // gets a random barrier
    #region Barrier Swing Activation

    while (totalSwingingBarriersAllowed > 0)
    {
      // 
      int randSwingObs = UnityEngine.Random.Range(0, activeBars.Length);
      bool allowSwing = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

      if (allowSwing)
      {
        // if it is to be swung
        activeBars[randSwingObs].GetComponent<obstacleScript>().setSwing(true);
        totalSwingingBarriersAllowed--;  // reduces total by one

        // removes the swinging barrier from list of non-swinging
        GameObject[] _temp = new GameObject[activeBars.Length - 1];
        int tempLoc = 0;
        for (int i = 0; i < activeBars.Length; i++)
        {
          if (randSwingObs != i)
          {
            _temp[tempLoc] = activeBars[i];
            tempLoc++;
          }
        }

        activeBars = _temp;
        _temp = null;
      }
    }

    activeBars = null;

    #endregion

    spawnPackage(spawnSideLeft);
  }

  float thePos_x(GameObject dropOff, string tipName, float percentageAllowed)
  {
    // calcs the new pos of the tips of the dropoffArea given the percentage of half that is available
    GameObject tip = cache.findInParent(dropOff, tipName); // gets the tip on the opp side of where package would be spawned
    float xPos = (percentageAllowed / 100f) * tip.transform.localPosition.x; // gets the actual x pos where the tip should now be
    tip.transform.localPosition = new Vector3(xPos, 0, 1f); // sets the tip pos
    return tip.transform.position.x; // returns the global pos of the tip;
  }

  void spawnPackage(bool spawnSideLeft)
  {
    Destroy(GameObject.Find("package"));
    GameObject newPackage = Instantiate(thePackage) as GameObject;
    newPackage.name = "package";
    newPackage.GetComponent<packageScript>().pushLeft = spawnSideLeft;
    newPackage.transform.position = new Vector3(spawnSideLeft ? -3.2f : 3.2f, 2f, 1f);
    newPackage.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
  }

  float createDropOff(float loc, float presiceLoc, float allowedWidthInPercent, bool packageSpawnSideLeft, GameObject theParent, bool swing = false)
  {
    // min allowed width in percent is how much of half of the drop off area should be available for use

    // precise loc safety
    presiceLoc = presiceLoc > 50 ? 50 :
    presiceLoc < 0 ? presiceLoc = 0 :
    presiceLoc;

    presiceLoc /= 100;

    if (loc < 0)
    {
      // loc safety
      Debug.LogError("x pos cannot be less than 0!");
      Debug.Log("set a pos then chose what side package should spawn at, which would determine dropoff location");
      return 0;
    }

    Transform parent = theParent.transform; // parent

    // allowed percentage safety
    allowedWidthInPercent = allowedWidthInPercent < 1 ? 1 :
    allowedWidthInPercent > 100 ? 100 :
    allowedWidthInPercent;

    // spawns and positions the drop off area
    GameObject dropOff = Instantiate(theDropOffArea, parent) as GameObject;
    dropOff.name = "dropOffArea";
    dropOff.transform.localPosition = new Vector3(packageSpawnSideLeft ? loc : -loc, -4, 1);
    dropOff.transform.localScale = new Vector3(.5f, .5f, .5f);

    if (allowedWidthInPercent < 100 && !swing)
    {
      // IF WIDTH OF DROPOFF AREA IS TO BE REDUCED
      // calc using left and right tips, how much of half of the dropoff would have to be covered and then create a new obstacle to cover that much

      // package would be spawned from left side
      if (packageSpawnSideLeft)
      {
        GameObject ob = createNewObstacle(2, new Vector3(thePos_x(dropOff, "tip_right", allowedWidthInPercent), cache.barrierPos[2], 1), 2, packageSpawnSideLeft, theParent: theParent);
        ob.SetActive(true);
      }

      // package would be spawned from right side
      else
      {
        GameObject ob = createNewObstacle(2, new Vector3(thePos_x(dropOff, "tip_left", allowedWidthInPercent), cache.barrierPos[2], 1), 2, packageSpawnSideLeft, theParent: theParent);
        ob.SetActive(true);
      }
    }

    // to get the precise loc multiply the percentage provided with the the tip_left local pos, then set the landing spot to that loc and return its global pos
    float precisePos = cache.findInParent(dropOff, "tip_left").transform.localPosition.x * presiceLoc;
    GameObject spot = cache.findInParent(dropOff, "packageLandingSpot");
    spot.transform.localPosition = new Vector3(packageSpawnSideLeft ? precisePos : -precisePos, 0, 1);
    dropOff.GetComponent<dropOffControl>().setSwing(swing, packageSpawnSideLeft);
    return spot.transform.position.x;
  }

  GameObject createNewObstacle(int index, Vector3 pos, int size, bool spawnSideLeft, GameObject theParent, bool swing = false)
  {
    // if use safety is enabled, it prevents obstacle from being spawned at a pos greater than 0 which would be impossible for package to cross
    // creates a new sprite based on the type of obstacle selected with the pivot position set to match the side of the screen the obstacle would be spawned at

    // index 1's face right if spawn side is left and left if spawn side is right
    // index 2's face left if spawn side is right and right if spawn side is left

    bool pivotLeft = index % 2 == 0 ? spawnSideLeft ? true : false : spawnSideLeft ? false : true;
    Transform parent = theParent.transform; // parent

    size = UnityEngine.Random.Range(0, 4);
    // to get the correct sprite and offset for each length of barriers
    GameObject ourObstacle = size == 1 ? theObstacle_1 : size == 2 ? theObstacle_2 : theObstacle_3;

    // extracts texture from obstacle prefab
    Texture2D tex = ourObstacle.GetComponent<SpriteRenderer>().sprite.texture;

    // creates a new sprite from extracted texture with the appropriate pivot point
    Sprite obs = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(pivotLeft ? 0 : 1, 0.5f));

    // spawns the obstacle, position, recalculate collider, set parent, and set sprite for the gameObj
    GameObject obstacle = Instantiate(ourObstacle) as GameObject;
    obstacle.GetComponent<SpriteRenderer>().sprite = obs;
    float offset = obstacle.GetComponent<obstacleScript>().offset_x;
    obstacle.GetComponent<obstacleScript>().pivotLeft = pivotLeft;
    obstacle.GetComponent<obstacleScript>().setSwing(swing);
    obstacle.GetComponent<PolygonCollider2D>().offset = new Vector2(pivotLeft ? offset : -offset, 0);
    obstacle.transform.SetParent(parent);
    obstacle.transform.localPosition = pos;
    obstacle.name = "obstacle_" + size;
    obstacle.SetActive(false);
    return obstacle;
  }
  void setScreenBounds()
  {
    SpriteRenderer rink = GameObject.Find("rink").GetComponent<SpriteRenderer>();
    float orthoSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f; //to calc the new orthographic size
    cache.theDefaultOrthographicSize = rink.bounds.size.x * 800 / 480 * 0.5f; //to calc the new orthographic size
    Camera.main.orthographicSize = orthoSize; //to set the new orthographic size
    cache.OrthographicBounds(Camera.main); //to get screen bounds
  }
  public Color pickColor(int colorIndex)
  {
    // returns a color for the color index chosen
    // removes the color from the list(making it impossible to used again)
    Color pickedColor = availableBalloonColors[colorIndex];
    Color[] temp = new Color[availableBalloonColors.Length - 1];
    int tempIndex = 0;
    for (int i = 0; i < availableBalloonColors.Length; i++)
    {
      if (availableBalloonColors[i] != pickedColor)
      {
        temp[tempIndex] = availableBalloonColors[i];
        tempIndex++;
      }
    }

    availableBalloonColors = temp;
    temp = null;
    tempIndex = 0;
    return pickedColor;
  }

  int maxRepeatLvl = 2, maxFailLvl = 2;
  public void startGame(bool nextLevel = false)
  {
    setNoTouchAreas();
    Camera.main.backgroundColor = backgroundColor[UnityEngine.Random.Range(0, backgroundColor.Length)];
    cache.noOfPlays++;
    isSuspended = false;
    // reset score
    // respawn scene
    hideGameOverPnl();
    hideLevelUp();

    deliveriesInARow = !nextLevel ? 0 : deliveriesInARow + 1;
    cache.setLevel(cache.getLevel() + Convert.ToInt32(nextLevel));
    if (!nextLevel)
    {
      failsInARow++;
      if (failsInARow > maxFailLvl) failsInARow = maxFailLvl;
      deliveriesInARow = 0;
    }
    else
    {
      failsInARow = 0;
    }

    if (deliveriesInARow == maxRepeatLvl)
    {
      // level up
      deliveriesInARow = 0;
      cache.setDifficulty(cache.getDifficulty() + Convert.ToInt32(nextLevel));
      if (cache.getDifficulty() > maxDiff) cache.setDifficulty(maxDiff);
      Debug.Log("level up " + cache.getDifficulty() + ", max diff " + maxDiff);
    }
    else if (failsInARow == maxFailLvl)
    {
      // level down
      failsInARow = 0;
      cache.setDifficulty(cache.getDifficulty() > 0 ? cache.getDifficulty() - 1 : cache.getDifficulty());
    }
    cache.level = cache.getLevel();
    availableBalloonColors = new Color[5] { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan };

    if (nextLevel) cache.saveGame();

    startDelivery();
  }
  public void endGame()
  {
    isSuspended = true;
    // if collidded with obstacle then show retry pnl
    showGameOverPnl();
    gamesSinceLastAd++; // potentially show ad
  }
  public void levelUp()
  {
    isSuspended = true;
    // if collidded with dropoff then move to next level
    showLevelUp();
    gamesSinceLastAd++; // potentially show ad
  }
  public void suspendGame(bool forTutorial = false)
  {
    if (forTutorial) showTutorialPnl();
    else showPausePnl();
    Time.timeScale = 0;
    isSuspended = true;
  }
  public void resumeGame()
  {
    hidePausePnl();
    hideTutorialPnl();
    isSuspended = false;
    StartCoroutine(resumeWithSlowMo());
    // remove pause pnl
    // unfreeze time
  }
  void setNoTouchAreas()
  {
    // to set areas on the screen where balloons would not be allowed to pop
    foreach(GameObject area in GameObject.FindGameObjectsWithTag("NoTouchArea"))
    {
      if (area.GetComponent<BoxCollider2D>() != null) Destroy(area.GetComponent<BoxCollider2D>());
      BoxCollider2D bc = area.AddComponent<BoxCollider2D>();
      bc.isTrigger = true;
      bc.size = new Vector2(Screen.width, area.GetComponent<RectTransform>().rect.height);
    }
  }
  IEnumerator resumeWithSlowMo()
  {
    while (Time.timeScale < 1)
    {
      Time.timeScale += .2f;
      yield return new WaitForSeconds(.1f);
    }

    StopCoroutine(resumeWithSlowMo());
  }

  void hideLevelUp()
  {
    levelUpPnl.SetActive(false);
  }
  void showLevelUp()
  {
    levelUpPnl.SetActive(true);
  }
  void hideGameOverPnl()
  {
    retryPnl.SetActive(false);
  }
  void showGameOverPnl()
  {
    retryPnl.SetActive(true);
  }
  void hidePausePnl()
  {
    pausePnl.SetActive(false);
  }
  void showPausePnl()
  {
    statToShowOnPause = allowedStats[UnityEngine.Random.Range(0, allowedStats.Length)];
    statToShowOnPause = allowedStats[0];
    pausePnl.SetActive(true);
  }
  void hideTutorialPnl()
  {
    pauseBtn.image.sprite = pause;
    levelPnl.SetActive(true);
    tutorialPnl.SetActive(false);
    GetComponent<TutorialManager>().destroyAllHighlighters();
  }
  void showTutorialPnl()
  {
    isSuspended = true;
    levelPnl.SetActive(false);
    hidePausePnl();
    tutorialPnl.SetActive(true);
  }

  public void startTutorial()
  {
    // to be called for tutorial
    // finds all game objects and explains them
    Time.timeScale = .1f;

    pauseBtn.image.sprite = help;
    TutorialManager tut = GetComponent<TutorialManager>();

    GameObject drop = GameObject.Find("dropOffArea");
    GameObject bar = GameObject.FindGameObjectWithTag("obstacle");
    GameObject pak = GameObject.Find("package");

    if (drop == null && bar == null && pak == null)
    {
      resumeGame();
      return;
    }

    showTutorialPnl();
    if (drop != null) tut.highlight(drop, "Land Package Here");
    if (bar != null) tut.highlight(bar, "Avoid The Obstacles");
    if (pak != null) tut.highlight(pak, "The Package");

    Time.timeScale = 0;
  }

  public bool getIsSuspended() => isSuspended;

  public void toggleSound()
  {
    cache.playSound = !cache.playSound;
    cache.saveGame();
  }

  void setSoundIcon()
  {
    soundToggleBtn.image.sprite = cache.playSound ? soundOn : soundOff;
  }
  #endregion
}
