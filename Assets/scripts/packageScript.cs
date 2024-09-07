using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packageScript : MonoBehaviour
{
  #region var
  public GameObject balloonPopFx;
  GameManager gm;
  [HideInInspector] public bool allowPopping = true;
  [HideInInspector] public bool pushLeft = true;
  bool touchedDropOff = false;
  bool doneSettingColors = false; // to prevent balloons' colors from being set repeatedly
  bool stillMoving = false; // set to true if the velocity of last frame is less than velocity of current frame
  bool packageIsInPlay = false; // activates package after it has fully entered screen
  #endregion

  #region func
  void Awake()
  {
    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
  }
  void Start()
  {
    gm = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  void setBalloonColors()
  {
    // choses a random color for all children of the package
    if (doneSettingColors || transform.GetChild(0) == null) return;
    for (int i = 0; i < transform.childCount; i++)
    {
      transform.GetChild(i).GetComponent<balloonController>().setColor(gm.pickColor(Random.Range(0, gm.availableBalloonColors.Length)));
    }

    doneSettingColors = true;
  }

  void addVerticalForce()
  {
    // adds force on the x axis of the package, pushing it across the screen
    // force added should take into accout the spawn side of the package
    // negative force to go from right to left, positive for the opposite
    GetComponent<Rigidbody2D>().AddForce(new Vector2(pushLeft ? 1.5f : -1.5f, 0f));
  }

  void FixedUpdate()
  {
    packageIsInPlay = !packageIsInPlay ? Mathf.Abs(transform.position.x) <= 2.15f : true;

    if (cache.noOfPlays == 1 && Mathf.Abs(transform.position.x) <= 2.2f) showFirstTut();
    if (cache.noOfBarriersSpawned == 1 && Mathf.Abs(transform.position.x) <= 2.2f) showSecondTut();

    if (Mathf.Abs(transform.position.y) >= 5f && packageIsInPlay) end();
    else if (Mathf.Abs(transform.position.x) >= 3f && packageIsInPlay) end();

    stillMoving = GetComponent<Rigidbody2D>().velocity.y != 0;

    setBalloonColors();
    addVerticalForce();

    if ((Input.touchCount > 0 || Input.GetKey(KeyCode.Space)) && Mathf.Abs(transform.position.x) <= 2.15f && !gm.getIsSuspended())
    {
      if (Input.touchCount > 0)
      {
        Touch t = Input.GetTouch(0);
        if (cache.checkClicked(t.position) == "pauseBtn" || cache.checkClicked(t.position) == "soundToggle" || cache.checkClicked(t.position) == "NoTouchArea") return;
      }
      // when a touch is registered
      allowPopping = false; // prevents another balloon from popping until the current has been popped
      if (transform.childCount < 1 && !allowPopping) return;
      if (touchedDropOff) return;
      transform.GetChild(0).GetComponent<balloonController>().pop(); // pops balloon
      if (gm.score > 0) gm.score--;
    }
  }

  bool shownTut1 = false;
  void showFirstTut()
  {
    if (!shownTut1)
    {
      shownTut1 = true;
      gm.startTutorial();
    }
  }

  bool shownTut2 = false;
  void showSecondTut()
  {
    if (!shownTut2)
    {
      shownTut2 = true;
      gm.startTutorial();
    }
  }

  public void freezePackage()
  {
    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
  }

  void popNextBalloon()
  {
    transform.GetChild(0).GetComponent<balloonController>().pop(); // pops balloon
  }

  void end()
  {
    cache.playNewSound("game_over");
    freezePackage();
    gm.endGame();
  }

  private void OnCollisionEnter2D(Collision2D col)
  {
    // if the package collides with an obstacle
    // end the game
    if (col.gameObject.tag == "obstacle")
    {
      end();
    }
    if (col.gameObject.tag == "dropoff")
    {
      touchedDropOff = true;
      col.gameObject.GetComponent<dropOffControl>().stopSwing();
    }
  }

  private void OnCollisionStay2D(Collision2D col)
  {
    if (col.gameObject.tag == "dropoff")
    {
      if (transform.childCount > 3) popNextBalloon();

      if (stillMoving) return;

      cache.playNewSound("level_up");
      freezePackage();
      col.gameObject.GetComponent<dropOffControl>().stopSwing();
      gm.levelUp();
      cache.noOfDeliveries++;
    }
  }

  // 3, 5
  #endregion

}
