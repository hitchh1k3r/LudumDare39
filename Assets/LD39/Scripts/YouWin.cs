using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{

  // State:
  private bool triggered;

  // Messages:

  void OnTriggerStay2D(Collider2D other)
  {
    if(other.tag == "Player")
    {
      if(!triggered)
      {
        triggered = true;
        GlobalState.Explosion.enabled = false;
        GlobalState.GameOver.enabled = false;
        GlobalState.GameOver.isGameOver = true;
        StartCoroutine(GoToEnd());
      }
    }
  }

  // Coroutines:

  private IEnumerator GoToEnd()
  {
    yield return Tween.FadeGroup(GlobalState.GameOver.blackFader, 0, 1, 0.25f);
    SceneManager.LoadScene(1);
  }

}
