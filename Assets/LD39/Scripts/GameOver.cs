using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

  // Referances:
  public RectTransform rectTrans;
  public CanvasGroup goFader;
  public CanvasGroup restartFader;
  public CanvasGroup resetFader;
  public CanvasGroup quitFader;
  public CanvasGroup whiteFader;
  public CanvasGroup blackFader;

  // Public State:
  public bool isGameOver;

  // State:
  private float timeout = 0.5f;
  private float resetTimer = 0.0f;

  // Messages:

  void Awake()
  {
    goFader.gameObject.SetActive(true);
    restartFader.gameObject.SetActive(true);
    goFader.alpha = 0;
    restartFader.alpha = 0;
    quitFader.alpha = 0;
    whiteFader.alpha = 0;
    resetFader.alpha = 0;
  }

  void Update()
  {
    if(isGameOver)
    {
      if(timeout > 0)
      {
        timeout -= Time.deltaTime;
      }
      else if(Input.GetButtonDown("Jump"))
      {
        SceneManager.LoadScene(0);
      }
      else if(Input.GetButtonDown("Cancel"))
      {
        Application.Quit();
      }
      if(Input.GetButton("Reset") && PlayerPrefs.HasKey("SavePoint"))
      {
        resetTimer += Time.deltaTime;
        if(resetTimer > 2.0f)
        {
          PlayerPrefs.DeleteKey("SavePoint");
          SceneManager.LoadScene(0);
        }
        else
        {
          blackFader.alpha = resetTimer * 0.5f;
        }
      }
      else
      {
        resetTimer = 0;
        blackFader.alpha = 0;
      }
    }
  }

  // Utilities:

  public void StartGameOver(bool fadeDeath)
  {
    Time.timeScale = 1;
    if(fadeDeath)
    {
      GlobalState.Explosion.enabled = false;
      StartCoroutine(Tween.FadeGroup(whiteFader, 0, 1, 0.75f));
    }
    isGameOver = true;
    StartCoroutine(GameOverCor());
  }

  // Coroutines:

  private IEnumerator GameOverCor()
  {
    yield return Tween.GenericTween(t => {
        goFader.alpha = t;
        rectTrans.anchorMin = new Vector2(-1+t, 0);
        rectTrans.anchorMax = new Vector2(t, 1);
      }, 1.0f, Easing.EASE_QUAD_OUT);
    yield return Tween.GenericTween(t => {
        restartFader.alpha = t;
        quitFader.alpha = t;
      }, 0.5f);
    if(PlayerPrefs.HasKey("SavePoint"))
    {
      yield return Tween.GenericTween(t => {
          resetFader.alpha = t;
        }, 0.5f);
    }
  }

}
