using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
  public GameObject theHighlighter;
  // controls the game tutorial

  public void highlight(Vector2 theGlobalPos, string description)
  {
    GameObject h = Instantiate(theHighlighter) as GameObject;
    h.tag = "highlight";
    h.transform.position = theGlobalPos;
    h.GetComponent<highlighterScript>().initializeHighlighter(description, theGlobalPos.x < 0 ? false : true);
  }

  public void highlight(GameObject theGameObject, string description)
  {
    Vector2 theGlobalPos = theGameObject.transform.position;
    GameObject h = Instantiate(theHighlighter) as GameObject;
    h.tag = "highlight";
    h.transform.position = theGlobalPos;
    h.GetComponent<highlighterScript>().initializeHighlighter(description, theGlobalPos.x < 0 ? false : true, theGameObject);
  }

  public void destroyAllHighlighters()
  {
    foreach (GameObject highlighter in GameObject.FindGameObjectsWithTag("highlight"))
    {
      Destroy(highlighter);
    }
  }
}
