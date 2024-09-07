using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  static float prevVol = 1f;
  public static AudioClip ballonPop, levelUp, gameOver;
  static AudioSource audioSrc;

  void Start()
  {
    ballonPop = Resources.Load<AudioClip>("ballon_pop");
    levelUp = Resources.Load<AudioClip>("new_purchase");
    gameOver = Resources.Load<AudioClip>("fail");

    audioSrc = GetComponent<AudioSource>();
    audioSrc.volume = 1;
  }

  public static void playSound(string clip, float volume = 1f)
  {
    if (audioSrc.isPlaying) audioSrc.Stop();

    switch (clip)
    {
      case "ballon_pop":
        audioSrc.PlayOneShot(ballonPop);
        break;

      case "game_over":
        audioSrc.PlayOneShot(gameOver);
        break;

      case "level_up":
        audioSrc.PlayOneShot(levelUp);
        break;
    }
  }

  string prevClip = "";
  void FixedUpdate()
  {
    if (prevClip != cache.currentSound)
    {
      prevClip = cache.currentSound;
      cache.resetSound();
    }

    if (!cache.playedCurrentSound)
    {
      cache.playedCurrentSound = true;
      playSound(cache.currentSound);
    }

    audioSrc.volume = System.Convert.ToInt32(cache.playSound);
  }
}
