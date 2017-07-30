using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tween
{

  // Transform Tweens:

  public static IEnumerator Translate(Transform transform, Vector3? startPos, Vector3? endPos,
        float length, Easing.EaseMethod easing = null, bool global = true)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Vector3 start = startPos ?? (global ? transform.position : transform.localPosition);
    Vector3 end = endPos ?? (global ? transform.position : transform.localPosition);

    while(time < length)
    {
      float t = easing(time / length);

      if(global)
      {
        transform.position = (1-t) * start + t * end;
      }
      else
      {
        transform.localPosition = (1-t) * start + t * end;
      }

      yield return null;
      time += Time.deltaTime;
    }

    if(global)
    {
      transform.position = end;
    }
    else
    {
      transform.localPosition = end;
    }
  }

  public static IEnumerator Rotate(Transform transform, Quaternion? startRot, Quaternion? endRot,
        float length, Easing.EaseMethod easing = null, bool global = false)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Quaternion start = startRot ?? (global ? transform.rotation : transform.localRotation);
    Quaternion end = endRot ?? (global ? transform.rotation : transform.localRotation);

    while(time < length)
    {
      float t = easing(time / length);

      if(global)
      {
        transform.rotation = Quaternion.SlerpUnclamped(start, end, t);
      }
      else
      {
        transform.localRotation = Quaternion.SlerpUnclamped(start, end, t);
      }

      yield return null;
      time += Time.deltaTime;
    }

    if(global)
    {
      transform.rotation = end;
    }
    else
    {
      transform.localRotation = end;
    }
  }

  public static IEnumerator Scale(Transform transform, Vector3? startScale, Vector3? endScale,
        float length, Easing.EaseMethod easing = null)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Vector3 start = startScale ?? transform.localScale;
    Vector3 end = endScale ?? transform.localScale;

    while(time < length)
    {
      float t = easing(time / length);

      transform.localScale = (1-t) * start + t * end;

      yield return null;
      time += Time.deltaTime;
    }

    transform.localScale = end;
  }

  public static IEnumerator Transform(Transform transform, Vector3? startPos,
        Quaternion? startRot, Vector3? startScale, Vector3? endPos, Quaternion? endRot,
        Vector3? endScale, float length, Easing.EaseMethod easing = null, bool global = true)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Vector3 startP, endP;
    Quaternion startR, endR;
    Vector3 startS = startPos ?? transform.localScale;
    Vector3 endS = endScale ?? transform.localScale;
    if(global)
    {
      startP = startPos ?? transform.position;
      endP = endPos ?? transform.position;
      startR = startRot ?? transform.rotation;
      endR = endRot ?? transform.rotation;
    }
    else
    {
      startP = startPos ?? transform.localPosition;
      endP = endPos ?? transform.localPosition;
      startR = startRot ?? transform.localRotation;
      endR = endRot ?? transform.localRotation;
    }

    while(time < length)
    {
      float t = easing(time / length);

      if(global)
      {
        transform.position = (1-t) * startP + t * endP;
        transform.rotation = Quaternion.SlerpUnclamped(startR, endR, t);
      }
      else
      {
        transform.localPosition = (1-t) * startP + t * endP;
        transform.localRotation = Quaternion.SlerpUnclamped(startR, endR, t);
      }
      transform.localScale = (1-t) * startS + t * endS;

      yield return null;
      time += Time.deltaTime;
    }

    if(global)
    {
      transform.position = endP;
      transform.rotation = endR;
    }
    else
    {
      transform.localPosition = endP;
      transform.localRotation = endR;
    }
    transform.localScale = endS;
  }

  public static IEnumerator Transform(Transform transform, Transform startTrans,
        Transform endTrans, float length, Easing.EaseMethod easing = null,
        bool trackStart = false, bool trackEnd = false, bool global = true)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Vector3 startPos, endPos;
    Quaternion startRot, endRot;
    Vector3 startScale = startTrans != null ? startTrans.localScale : transform.localScale;
    Vector3 endScale = endTrans != null ? endTrans.localScale : transform.localScale;
    if(global)
    {
      startPos = (startTrans ?? transform).position;
      endPos = (endTrans ?? transform).position;
      startRot = (startTrans ?? transform).rotation;
      endRot = (endTrans ?? transform).rotation;
    }
    else
    {
      startPos = (startTrans ?? transform).localPosition;
      endPos = (endTrans ?? transform).localPosition;
      startRot = (startTrans ?? transform).localRotation;
      endRot = (endTrans ?? transform).localRotation;
    }

    while(time < length)
    {
      float t = easing(time / length);

      if(trackStart)
      {
        if(startTrans != null)
        {
          startPos = global ? startTrans.position : startTrans.localPosition;
          startRot = global ? startTrans.rotation : startTrans.localRotation;
          startScale = startTrans.localScale;
        }
      }
      if(trackEnd)
      {
        if(endTrans != null)
        {
          endPos = global ? endTrans.position : endTrans.localPosition;
          endRot = global ? endTrans.rotation : endTrans.localRotation;
          endScale = endTrans.localScale;
        }
      }

      if(global)
      {
        transform.position = (1-t) * startPos + t * endPos;
        transform.rotation = Quaternion.SlerpUnclamped(startRot, endRot, t);
      }
      else
      {
        transform.localPosition = (1-t) * startPos + t * endPos;
        transform.localRotation = Quaternion.SlerpUnclamped(startRot, endRot, t);
      }
      transform.localScale = (1-t) * startScale + t * endScale;

      yield return null;
      time += Time.deltaTime;
    }

    if(global)
    {
      transform.position = endPos;
      transform.rotation = endRot;
    }
    else
    {
      transform.localPosition = endPos;
      transform.localRotation = endRot;
    }
    transform.localScale = endScale;
  }

  // Sprite Tweens:

  public static IEnumerator SpriteColor(SpriteRenderer sprite, Color? startColor, Color? endColor,
        float length, Easing.EaseMethod easing = null)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Color start = startColor ?? sprite.color;
    Color end = endColor ?? sprite.color;

    while(time < length)
    {
      float t = easing(time / length);

      sprite.color = (1-t) * start + t * end;

      yield return null;
      time += Time.deltaTime;
    }

    sprite.color = end;
  }
  // UI Tweens:

  // Updating any UI elements causes all canvas elements to rebuild, this method could cause lag!

  public static IEnumerator UIColor(Graphic element, Color? startColor, Color? endColor,
        float length, Easing.EaseMethod easing = null)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    Color start = startColor ?? element.color;
    Color end = endColor ?? element.color;

    while(time < length)
    {
      float t = easing(time / length);

      element.color = (1-t) * start + t * end;

      yield return null;
      time += Time.deltaTime;
    }

    element.color = end;
  }

  public static IEnumerator FadeGroup(CanvasGroup canvasGroup, float? startAlpha, float? endAlpha,
        float length, Easing.EaseMethod easing = null)
  {
    easing = easing ?? Easing.EASE_LINEAR;
    float time = 0;
    float start = startAlpha ?? canvasGroup.alpha;
    float end = endAlpha ?? canvasGroup.alpha;

    while(time < length)
    {
      float t = easing(time / length);

      canvasGroup.alpha = (1-t) * start + t * end;

      yield return null;
      time += Time.deltaTime;
    }

    canvasGroup.alpha = end;
  }

  // Generic Tween:

  public static IEnumerator GenericTween(Action<float> setOutput, float length,
      Easing.EaseMethod easing = null, Action endCallback = null)
  {
    if(easing == null)
    {
      easing = Easing.EASE_LINEAR;
    }
    float time = 0;
    while(time < length)
    {
      float t = easing(time / length);

      setOutput(t);
      yield return null;
      time += Time.deltaTime;
    }
    setOutput(1);
    if(endCallback != null)
    {
      endCallback();
    }
  }

}

public static class Easing
{

  // You can use an AnimationCurve.Evaluate in place of an EaseMethod for a custom timing function
  public delegate float EaseMethod(float f);

  public static readonly EaseMethod EASE_LINEAR       = (t => t);
  public static readonly EaseMethod EASE_QUAD_IN      = (t => t*t);
  public static readonly EaseMethod EASE_QUAD_OUT     = (t => t * (2-t));
  public static readonly EaseMethod EASE_QUAD_IN_OUT  = EaseInOut(EASE_QUAD_IN, EASE_QUAD_OUT);
  public static readonly EaseMethod EASE_CUBIC_IN     = (t => t*t*t);
  public static readonly EaseMethod EASE_CUBIC_OUT    = (t => { --t; return (t*t*t + 1); });
  public static readonly EaseMethod EASE_CUBIC_IN_OUT = EaseInOut(EASE_CUBIC_IN, EASE_CUBIC_OUT);
  public static readonly EaseMethod EASE_SIN_IN       = (t => 1 - Mathf.Cos((Mathf.PI/2)*t));
  public static readonly EaseMethod EASE_SIN_OUT      = (t => Mathf.Sin((Mathf.PI/2)*t));
  public static readonly EaseMethod EASE_SIN_IN_OUT   = EaseInOut(EASE_SIN_IN, EASE_SIN_OUT);
  public static readonly EaseMethod EASE_EXP_IN       = (t => Mathf.Pow(2, 10*(t-1)));
  public static readonly EaseMethod EASE_EXP_OUT      = (t => -Mathf.Pow(2,-10*t) + 1 );
  public static readonly EaseMethod EASE_EXP_IN_OUT   = EaseInOut(EASE_EXP_IN, EASE_EXP_OUT);
  public static readonly EaseMethod EASE_CIRC_IN      = (t => 1 - Mathf.Sqrt(1 - t*t));
  public static readonly EaseMethod EASE_CIRC_OUT     = (t => {--t; return Mathf.Sqrt(1 - t*t);});
  public static readonly EaseMethod EASE_CIRC_IN_OUT  = EaseInOut(EASE_CIRC_IN, EASE_CIRC_OUT);
  public static readonly EaseMethod EASE_ELASTIC_OUT  = EaseElastic(3.33333333333f);
  public static readonly EaseMethod EASE_BOUNCE_OUT   = EaseBounce(3);

  public static EaseMethod EaseInvert(EaseMethod ease)
  {
    return (t => 1 - ease(1 - t));
  }

  public static EaseMethod EaseInOut(EaseMethod easeIn, EaseMethod easeOut, float midPoint = 0.5f)
  {
    return (t => ((t < midPoint) ?
          (midPoint * easeIn(t / midPoint)) :
          ((1 - midPoint) * easeOut((t-midPoint)/(1-midPoint)) + midPoint)));
  }

  public static EaseMethod EaseElastic(float bounces)
  {
    return (t => (Mathf.Pow(2, (-10*t)) * Mathf.Sin((t-1/(4*bounces))*(2*Mathf.PI)*bounces) + 1));
  }

  public static EaseMethod EaseBounce(int bounces)
  {
    float[] bounceValues = new float[bounces * 4];
    {
      float sum = 0;
      float shift = 0;
      float width = 1;
      for(int i = 0; i < bounces; ++i)
      {
        sum += width;
        width *= 1.5f;
      }
      width /= 1.5f;
      sum -= width * 0.5f;
      for(int i = 0; i < bounces; ++i)
      {
        float height = Mathf.Sqrt(shift);
        bounceValues[i * 4] = shift;
        bounceValues[i * 4 + 1] = 2 * sum / width;
        bounceValues[i * 4 + 1] = bounceValues[i * 4 + 1] * bounceValues[i * 4 + 1] * (1-height);
        bounceValues[i * 4 + 2] = height;
        bounceValues[i * 4 + 3] = shift - (width/sum * 0.5f);
        width /= 1.5f;
        shift += width * 1.25f / sum;
      }
      bounceValues[3] = 0;
    }
    return ( t => {
      for(int i = bounces - 1; i >= 0; --i)
      {
        if(t > bounceValues[i * 4 + 3])
        {
          t -= bounceValues[i * 4];
          return t * t * bounceValues[i * 4 + 1] + bounceValues[i * 4 + 2];
        }
      }
      return 0;
    });
  }

  public static EaseMethod EaseFromEnum(EaseEnum ease)
  {
    switch(ease)
    {
      case EaseEnum.LINEAR:       { return EASE_LINEAR;       }
      case EaseEnum.QUAD_IN:      { return EASE_QUAD_IN;      }
      case EaseEnum.QUAD_OUT:     { return EASE_QUAD_OUT;     }
      case EaseEnum.QUAD_IN_OUT:  { return EASE_QUAD_IN_OUT;  }
      case EaseEnum.CUBIC_IN:     { return EASE_CUBIC_IN;     }
      case EaseEnum.CUBIC_OUT:    { return EASE_CUBIC_OUT;    }
      case EaseEnum.CUBIC_IN_OUT: { return EASE_CUBIC_IN_OUT; }
      case EaseEnum.SIN_IN:       { return EASE_SIN_IN;       }
      case EaseEnum.SIN_OUT:      { return EASE_SIN_OUT;      }
      case EaseEnum.SIN_IN_OUT:   { return EASE_SIN_IN_OUT;   }
      case EaseEnum.EXP_IN:       { return EASE_EXP_IN;       }
      case EaseEnum.EXP_OUT:      { return EASE_EXP_OUT;      }
      case EaseEnum.EXP_IN_OUT:   { return EASE_EXP_IN_OUT;   }
      case EaseEnum.CIRC_IN:      { return EASE_CIRC_IN;      }
      case EaseEnum.CIRC_OUT:     { return EASE_CIRC_OUT;     }
      case EaseEnum.CIRC_IN_OUT:  { return EASE_CIRC_IN_OUT;  }
      case EaseEnum.ELASTIC_OUT:  { return EASE_ELASTIC_OUT;  }
      case EaseEnum.BOUNCE_OUT:   { return EASE_BOUNCE_OUT;   }
    }
    return EASE_LINEAR;
  }

  public enum EaseEnum
  {
    LINEAR, QUAD_IN, QUAD_OUT, QUAD_IN_OUT, CUBIC_IN, CUBIC_OUT, CUBIC_IN_OUT, SIN_IN, SIN_OUT,
    SIN_IN_OUT, EXP_IN, EXP_OUT, EXP_IN_OUT, CIRC_IN, CIRC_OUT, CIRC_IN_OUT, ELASTIC_OUT,
    BOUNCE_OUT
  }

}
