using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorFlash : MonoBehaviour
{

  // Configuration:
  public Color colorA, colorB;

  // Referances:
  public SpriteRenderer sprite;

  // Messages:

  void Update()
  {
    sprite.color = Color.Lerp(colorA, colorB, Random.Range(0.0f, 1.0f));
  }

}
