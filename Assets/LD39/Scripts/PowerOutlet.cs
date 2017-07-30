using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutlet : MonoBehaviour
{

  // Configuration:
  public float chargeSpeed = 0.1f;
  public ParticleSystem sparks;

  // State:
  private bool needsSparks;
  private bool hasSparks;

  // Messages:

  void Awake()
  {
    sparks.enableEmission = false;
  }

  void FixedUpdate()
  {
    if(needsSparks != hasSparks)
    {
      hasSparks = needsSparks;
      sparks.enableEmission = needsSparks;
    }
    needsSparks = false;
  }

  void OnTriggerStay2D(Collider2D other)
  {
    if(other.tag == "Player")
    {
      GlobalState.Power.powerLevel += chargeSpeed * Time.deltaTime;
      if(GlobalState.Power.powerLevel > 1.0f)
      {
        GlobalState.Power.powerLevel = 1.0f;
      }
      else
      {
        needsSparks = true;
      }
    }
  }

}
