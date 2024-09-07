using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
  #region var
  public float offset_x; // offset for collider
  [HideInInspector] public bool pivotLeft = true;
  bool swing = false; // for moving barrier difficulty
  Vector2 originalPos;
  Vector2 movePos;

  // for swing
  Vector3 touchPos, touchDirection;
  float moveSpeed = 2.5f;
  #endregion

  #region func
  void Start()
  {
    // saves original positions
    originalPos = transform.position;
    movePos = transform.position;

    // locks rigid body if this would not swing
    if (!swing) { GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; }
  }

  public void setSwing(bool shouldSwing)
  {
    swing = shouldSwing;
  }

  bool isOnLeft(bool useWorldPos = true)
  {
    // returns true if sprite pivot point is left
    return pivotLeft;
  }

  void FixedUpdate()
  {
    if (swing)
    {
      // if gameobject is to swing
      // get the pos to be swung to
      float maxSwingOffset = .4f; // .4
      float maxSwingOffsetPos = isOnLeft() ? originalPos.x + maxSwingOffset : originalPos.x - maxSwingOffset;

      float orgiOffset = .075f;
      float orgiPos = isOnLeft() ? originalPos.x - orgiOffset : originalPos.x + orgiOffset;

      if (cache.approximatelyEqual(transform.position.x, /*originalPos.x*/ orgiPos) || transform.position.x == originalPos.x)
      {
        // if is at correct pos then swing to offset pos
        movePos = new Vector2(maxSwingOffsetPos, originalPos.y);
      }
      else if (cache.approximatelyEqual(transform.position.x, maxSwingOffsetPos, 0.075f))
      {
        // if is at max offset pos swing back to correct pos
        // movePos = originalPos;
        movePos = new Vector2(orgiPos, transform.position.y);
      }
    }
    else
    {
      // if swing is false keep at original pos
      movePos = originalPos;
    }

    moveTo(movePos);
  }

  void moveTo(Vector3 pos)
  {
    // to move with rigid body gameobject to any pos
    touchDirection = (pos - transform.position);
    GetComponent<Rigidbody2D>().velocity = new Vector2(touchDirection.x * moveSpeed, 0f);
  }
  #endregion
}
