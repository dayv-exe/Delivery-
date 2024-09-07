using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFinderScript : MonoBehaviour
{
  public bool leftSlant = false; // what direction relative to top is the bottom facing
  float[] max_XPos = new float[4] { -1f, 1f, -1f, 1f };
  // Start is called before the first frame update
  void Start()
  {

  }

  public void plotPath(float dropOffLoc, bool spawnSideLeft = true, bool useSteeperPath = false)
  {
    // min gap between obstacles: 1.1f
    float minGap;

    minGap = cache.difficulty == 0 ? 1.15f :
    cache.difficulty == 1 ? 1.14f :
    cache.difficulty == 2 ? 1.12f :
    cache.difficulty == 3 ? 1.1f :
    useSteeperPath ? .934f : 1.075f;

    GameObject rightSpot = cache.findInParent(gameObject, "right_spot");
    GameObject leftSpot = cache.findInParent(gameObject, "left_spot");
    float x_loc = leftSlant ? dropOffLoc + .2f : dropOffLoc - .2f;
    transform.position = new Vector3(x_loc, -3.51f, 1f);
    rightSpot.transform.localPosition = new Vector3(0.0232f, 1f, 1f);
    rightSpot.transform.position = new Vector3(rightSpot.transform.position.x, rightSpot.transform.position.y, 1);
    leftSpot.transform.localPosition = new Vector3(-0.0232f, 1f, 1f);

    max_XPos[0] = spawnSideLeft ? rightSpot.transform.position.x : leftSpot.transform.position.x;
    max_XPos[1] = spawnSideLeft ? max_XPos[0] + minGap : max_XPos[0] - minGap;

    float right_x = rightSpot.transform.position.x;
    float left_x = leftSpot.transform.position.x;

    float steepOffset = .25f;

    if (spawnSideLeft)
    {
      rightSpot.transform.position = new Vector3(right_x, cache.barrierPos[1], 1);
      rightSpot.transform.localPosition = new Vector3(0.0232f, rightSpot.transform.localPosition.y, 1);
      max_XPos[2] = (rightSpot.transform.position.x);
    }
    else
    {
      leftSpot.transform.position = new Vector3(left_x, cache.barrierPos[1], 1);
      leftSpot.transform.localPosition = new Vector3(-0.0232f, leftSpot.transform.localPosition.y, 1);
      max_XPos[2] = (leftSpot.transform.position.x);
    }

    max_XPos[3] = spawnSideLeft ? max_XPos[2] + minGap : max_XPos[2] - minGap;

    if (useSteeperPath)
    {
      max_XPos[3] = spawnSideLeft ? max_XPos[3] - steepOffset : max_XPos[3] + steepOffset;
      max_XPos[2] = spawnSideLeft ? (rightSpot.transform.position.x) - steepOffset : (leftSpot.transform.position.x) + steepOffset;
    }
  }

  public float barrier_x(int pos)
  {
    return max_XPos[pos];
  }

  // Update is called once per frame
  void Update()
  {

  }
}
