using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegion : MonoBehaviour
{

  // Configuration:
  public Vector3 camOffset = new Vector3(8.75f, 5.55f, 0);
  public float camFieldOfView = 4.75f;

  // Static State:
  private static CameraRegion currentRegion;

  // Messages:

  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.tag == "Player")
    {
      currentRegion = this;
      GlobalState.Camera.targetFOV = camFieldOfView;
      GlobalState.Camera.camTargetOffset = camOffset;
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if(other.tag == "Player" && currentRegion == this)
    {
      GlobalState.Camera.targetFOV = 4.75f;
      GlobalState.Camera.camTargetOffset = new Vector3(8.75f, 5.55f, 0);
    }
  }

}
