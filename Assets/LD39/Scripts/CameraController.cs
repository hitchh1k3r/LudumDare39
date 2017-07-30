using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

  // Configuration:
  public float camTrackSpeed = 10;
  public float camDistance = 200;
  public float minX;
  public float targetFOV = 4.75f;
  public Vector3 camTargetOffset = new Vector3(8.75f, 5.55f, 0);

  // Referances:
  public Camera cam;
  public Transform camTarget;

  // State:
  private float lastAspect;
  private float widthCalc;
  private Coroutine targetCor;
  private Coroutine fovCor;
  public float lastFOV;
  public Vector3 lastTargetOffset;

  // Messages:

  void Update()
  {
    if(camTargetOffset != lastTargetOffset)
    {
      lastTargetOffset = camTargetOffset;
      if(targetCor != null)
      {
        StopCoroutine(targetCor);
      }
      targetCor = StartCoroutine(Tween.Translate(camTarget, null, camTargetOffset, 1.0f, Easing.EASE_QUAD_OUT, false));
    }
    if(targetFOV < lastFOV - 0.01f || targetFOV > lastFOV + 0.01f)
    {
      lastFOV = targetFOV;
      if(fovCor != null)
      {
        StopCoroutine(fovCor);
      }
      float startFOV = cam.fieldOfView;
      fovCor = StartCoroutine(Tween.GenericTween(t => {
          cam.fieldOfView = (1-t) * startFOV + t * lastFOV;
        }, 1.0f, Easing.EASE_QUAD_OUT));
    }
    if(lastAspect < cam.aspect - 0.01f || lastAspect > cam.aspect + 0.01f)
    {
      lastAspect = cam.aspect;
      widthCalc = Mathf.Atan(2 * Mathf.Atan(cam.aspect *
            Mathf.Atan(0.25f * Mathf.Deg2Rad * cam.fieldOfView)));
    }
    float camWidth = camDistance * widthCalc;

    Vector3 newPos = Vector3.Lerp(transform.position, camTarget.position,
          DecayInterp(camTrackSpeed));
    newPos.z = -camDistance;
    if(newPos.x < minX + camWidth)
    {
      newPos.x = minX + camWidth;
    }
    transform.position = newPos;
  }

  // Utilities:

  private static float DecayInterp(float speed)
  {
    return 1 - (1 / Mathf.Pow(2, Time.unscaledDeltaTime * speed));
  }

}
