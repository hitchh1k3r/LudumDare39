using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingVideo : MonoBehaviour
{

  // Referances:
  public SpriteRenderer explosion, antiExplosion, expMid, antiExpMid;
  public ParticleSystem[] expParticles, antiParticles;
  public ParticleSystem plantFire, antiFire;
  public CanvasGroup screenFlash, fadeIn;
  public CanvasGroup endText, endCredit;
  public Transform hero;

  // State:
  private Vector3 basePos;
  private float rumble;
  private bool videoEnded;

  IEnumerator Start()
  {
    plantFire.enableEmission = false;
    antiFire.enableEmission = false;

    yield return Tween.FadeGroup(fadeIn, 1, 0, 0.5f);

    yield return new WaitForSeconds(0.5f);
    StartCoroutine(Tween.Translate(hero, null, new Vector3(3.5f, -3.55f, 0), 4.5f));
    yield return new WaitForSeconds(2.0f);

    rumble = 0.01f;
    yield return Flash(0.25f);
    yield return new WaitForSeconds(0.2f);

    rumble = 0.05f;
    StartCoroutine(Tween.Scale(explosion.transform, null, new Vector3(90, 90, 90), 6.0f,
          Easing.EASE_CIRC_IN));
    yield return Flash(0.5f);
    yield return new WaitForSeconds(0.075f);

    foreach(ParticleSystem ps in expParticles)
    {
      ps.enableEmission = false;
    }
    plantFire.enableEmission = true;

    yield return new WaitForSeconds(0.2f);

    rumble = 0.1f;
    yield return Flash(0.75f);

    yield return new WaitForSeconds(1.5f);

    rumble = 0.5f;
    StartCoroutine(Tween.Scale(antiExplosion.transform, null, new Vector3(90, 90, 90), 6.0f,
          Easing.EASE_CIRC_IN));
    yield return Flash(0.5f);
    yield return new WaitForSeconds(0.075f);
    rumble = 0.1f;
    foreach(ParticleSystem ps in antiParticles)
    {
      ps.enableEmission = false;
    }
    antiFire.enableEmission = true;

    yield return new WaitForSeconds(3.0f);
    rumble = 0.05f;
    yield return new WaitForSeconds(1.0f);
    rumble = 0.01f;
    yield return new WaitForSeconds(1.0f);
    rumble = 0.0f;
    yield return Tween.FadeGroup(endText, 0, 1, 1.0f);
    yield return new WaitForSeconds(2.0f);
    yield return Tween.FadeGroup(endCredit, 0, 1, 1.0f);
    videoEnded = true;
  }

  void Awake()
  {
    basePos = transform.position;
  }

  void Update()
  {
    if(!videoEnded)
    {
      transform.position = basePos + rumble * Random.onUnitSphere;
    }
    else
    {
      if(Input.GetButtonDown("Cancel"))
      {
        Application.Quit();
      }
    }
  }

  // Corutines:

  private IEnumerator Flash(float intensity)
  {
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = intensity;
    yield return new WaitForSeconds(0.01f);
    screenFlash.alpha = 0.0f;
  }

}
