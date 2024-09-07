using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloonController : MonoBehaviour
{
  #region var
  public SpriteRenderer main_balloon, main_knot, rope;
  bool destroying = false;

  #endregion

  #region func
  void Start()
  {
    setSortingLayerOrder();
  }
  void showDestroyFx()
  {
    if (destroying) return;
    destroying = true;
    GameObject destroyedFx = Instantiate(transform.parent.GetComponent<packageScript>().balloonPopFx, transform) as GameObject;
    destroyedFx.GetComponent<AudioSource>().enabled = false;
    destroyedFx.transform.GetChild(2).gameObject.SetActive(false);
    destroyedFx.transform.SetParent(null);
    destroyedFx.transform.position = transform.position;
    destroyedFx.transform.localScale = transform.lossyScale;
    Destroy(destroyedFx, 4f);
  }
  public void setColor(Color theColor)
  {
    // changes the color of the balloon to any chosen color
    main_balloon.color = theColor;
    main_knot.color = theColor;
  }
  void setSortingLayerOrder()
  {
    // to make the balloons render properly
    // get main's children
    // get knot's children
    // get rope
    // multiply their order in layer by (balloons sibling index + 1) * 10

    int currentBalloonsIndex = transform.GetSiblingIndex();
    GameObject ballon_main = main_balloon.transform.parent.gameObject;
    GameObject knot_main = main_knot.transform.parent.gameObject;


    // for balloon main
    for (int i = 0; i < ballon_main.transform.childCount; i++)
    {
      SpriteRenderer currentSprite = ballon_main.transform.GetChild(i).GetComponent<SpriteRenderer>();
      currentSprite.sortingOrder = currentSprite.sortingOrder + (currentBalloonsIndex * 10);
    }

    // for knot main
    for (int i = 0; i < knot_main.transform.childCount; i++)
    {
      SpriteRenderer currentSprite = knot_main.transform.GetChild(i).GetComponent<SpriteRenderer>();
      currentSprite.sortingOrder = currentSprite.sortingOrder + (currentBalloonsIndex * 10);
    }

    // for rope
    rope.sortingOrder = rope.sortingOrder + (currentBalloonsIndex * 10);
  }
  void FixedUpdate()
  {
    // adds an upwards force to the package to simulate its descent being slowed down by the balloons
    // when a balloon is popped, the package looses some of its lift
    // default force 1.8f
    transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1.8f));
  }
  public void pop()
  {
    // to destroy the balloon, but play pop animation first
    showDestroyFx(); // shows the explosion prefab
    gameObject.SetActive(false); // deactivates the balloon
    Destroy(gameObject, 0.14f); // destroys it after 15 milliseconds

    // *note: only after one balloon has been completely destroyed would another be ready for destruction
  }

  private void OnDestroy()
  {
    // allows next balloon to be destroyed
    SoundManager.playSound("ballon_pop");
    transform.parent.GetComponent<packageScript>().allowPopping = true;
  }

  #endregion
}
