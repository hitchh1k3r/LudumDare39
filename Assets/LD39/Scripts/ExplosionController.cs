using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

  // Configuration:
  public Vector3 moveSpeed = new Vector3(7, 0, 0);
  public float startPlayerPos = 3;

  // Referances:
  public Transform cam;

  // State:
  private bool started;
  private bool paused;

  // Messages:

  void Update()
  {
    if(paused)
    {
      Time.timeScale = 1;
      return;
    }

    if(started)
    {
      Vector3 effectiveSpeed = moveSpeed;
      float playerOffset = GlobalState.Player.transform.position.x - transform.position.x;
      if(playerOffset < 0)
      {
        GlobalState.GameOver.StartGameOver(false);
        Time.timeScale = 1;
        if(playerOffset < -30)
        {
          enabled = false;
        }
      }
      else
      {
        if(playerOffset < 5)
        {
          float scale = playerOffset;
          scale = 3 * scale * scale * scale / 500.0f + 0.25f;
          Time.timeScale = scale;
          if(GlobalState.Power.powerLevel > 0.01f)
          {
            effectiveSpeed *= scale;
          }
        }
      }
      Vector3 newPos = transform.position + Time.deltaTime * effectiveSpeed;
      newPos.y = cam.position.y;
      transform.position = newPos;
    }
    else
    {
      if(GlobalState.Player.transform.position.x > startPlayerPos)
      {
        started = true;
      }
    }
  }

  // Utilities:

  public void Pause()
  {
    paused = true;
  }

  public void Resume()
  {
    paused = false;
  }

}
