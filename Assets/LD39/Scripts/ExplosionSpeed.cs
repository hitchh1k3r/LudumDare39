using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpeed : MonoBehaviour
{

  // Configuration:
  public float setSpeed;

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
        GlobalState.Explosion.moveSpeed = new Vector3(setSpeed, 0, 0);
      }
    }
  }

}
