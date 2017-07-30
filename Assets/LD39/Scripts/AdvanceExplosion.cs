using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceExplosion : MonoBehaviour
{

  // Configuration:
  public BlastDoor[] doorsToDestroy;
  public float setExpostion;

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
        Vector3 expPos = GlobalState.Explosion.transform.position;
        if(expPos.x < setExpostion)
        {
          foreach(BlastDoor door in doorsToDestroy)
          {
            door.RemoveDoor();
          }
          expPos.x = setExpostion;
          GlobalState.Explosion.transform.position = expPos;
          GlobalState.Explosion.Resume();
        }
      }
    }
  }

}
