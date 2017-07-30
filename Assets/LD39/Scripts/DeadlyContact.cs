using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyContact : MonoBehaviour
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
        GlobalState.GameOver.StartGameOver(true);
      }
      other.attachedRigidbody.velocity *= 0.25f;
    }
  }

}
