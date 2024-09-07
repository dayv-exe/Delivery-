using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropOffControl : MonoBehaviour
{
  #region var
  bool swing = false, packageSpawnLeft = true; // for moving barrier difficulty
  [HideInInspector] public float swingRangeInPercent = 20f; // how far drop off should swing (100% would mean from one end of the screen to the other)
  Vector2 originalPos;
  Vector2 movePos;

  #endregion

  #region func
  void Start()
  {
    // saves original positions
    if (swing)
    {
      transform.position = new Vector3(packageSpawnLeft ? 2f : -2f, transform.position.y, 1);
    }
    originalPos = transform.position;
    movePos = transform.position;

    if (swing) StartCoroutine(swingObstacle());
  }

  public void stopSwing()
  {
    swing = false;
  }

  public void setSwing(bool isSwing, bool isPackageSpawnLeft)
  {
    swing = isSwing;
    packageSpawnLeft = isPackageSpawnLeft;
  }

  void FixedUpdate()
  {
  }

  IEnumerator swingObstacle()
  {
    float finalPos = packageSpawnLeft ? -2f : 2f; // sets the final pos to be on the same side as package spawn side 
    bool returning = false;
    // float addToMove = 0.01125f;
    float addToMove = 0.01125f * 1.5f;
    while (swing)
    {
      if ((cache.approximatelyEqual(transform.localPosition.x, finalPos)) || returning)
      {
        // if dropoff is at final pos, move to original pos
        returning = true;
        float move = !packageSpawnLeft ? transform.localPosition.x - addToMove : transform.localPosition.x + addToMove;
        transform.localPosition = new Vector3(move, transform.localPosition.y, 1);

        if (transform.localPosition.x == originalPos.x) returning = false;
      }
      else
      {
        // if dropoff is at original pos, move final pos
        float move = !packageSpawnLeft ? transform.localPosition.x + addToMove : transform.localPosition.x - addToMove;
        transform.localPosition = new Vector3(move, transform.localPosition.y, 1);
      }
      // yield return new WaitForSeconds(.01f);
      yield return new WaitForFixedUpdate();
    }
  }
  #endregion
}
