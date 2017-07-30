using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{

  // Configuration:
  public float slowAnimationSpeed = 0.25f;
  public float fastAnimationSpeed = 4f;
  public Color baseColor;
  public Color goodColor;
  public Color badColor;

  // Refernaces:
  public RectTransform powerRect;
  public RawImage powerImage;

  // Public State:
  [Range(0,1)]
  public float powerLevel = 1.0f;

  // State:
  private float timer;

  // Messages:

  void Update()
  {
    powerRect.sizeDelta = new Vector2(powerRect.sizeDelta.x, 100 * powerLevel);

    Color fadeColor = Color.Lerp(badColor, goodColor, powerLevel);
    timer += Time.deltaTime * (powerLevel * slowAnimationSpeed +
          (1 - powerLevel) * fastAnimationSpeed);
    if(timer > 2.0f)
    {
      timer -= 2;
      if(timer > 2)
      {
        timer = 0;
      }
    }
    if(timer < 1.0f)
    {
      float t = Easing.EASE_QUAD_IN_OUT(timer);
      powerImage.color = Color.Lerp(baseColor, fadeColor, t);
    }
    else
    {
      float t = Easing.EASE_QUAD_IN_OUT(timer - 1);
      powerImage.color = Color.Lerp(fadeColor, baseColor, t);
    }
  }

}
