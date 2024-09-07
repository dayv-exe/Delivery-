using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelUpScript : MonoBehaviour
{
  // CONTROLS THE LEVEL UP PNL
  #region var
  [HideInInspector] public GameManager gm;
  public bool doneWithAnim = false;
  public GameObject confetti;
  bool setConfetti = false;
  #endregion

  #region func
  private void OnEnable() {
    setConfetti = false;
  }
  void FixedUpdate()
  {
    if (!setConfetti && doneWithAnim)
    {
      setConfetti = true;
      GameObject conf = Instantiate(confetti) as GameObject;
      conf.transform.position = transform.GetChild(0).transform.position;
      Destroy(conf, 3f);
    }
  }
}
#endregion