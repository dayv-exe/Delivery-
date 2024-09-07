using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class highlighterScript : MonoBehaviour
{
  public GameObject leftCont, rightCont;
  public TextMeshPro leftDescription, rightDescription;

  public void initializeHighlighter(string description, bool onLeftSide, GameObject lockOn = null)
  {
    leftDescription.text = description;
    rightDescription.text = description;

    leftCont.SetActive(!onLeftSide);
    rightCont.SetActive(onLeftSide);

    if (lockOn != null)
    {
      StartCoroutine(monitor(lockOn));
    }
  }

  IEnumerator monitor(GameObject target)
  {
    while (true)
    {
      if (target == null)
      {
        Destroy(gameObject);
      }

      yield return new WaitForSeconds(.01f);
    }
  }
}
