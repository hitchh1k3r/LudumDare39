using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDoor : MonoBehaviour
{

  // Configuration:
  public float controllerOffset = 0.708f;
  public float doorHeight;
  public Color triggeredColor;
  public float exposionStopOffset = -0.514f;

  // Refernaces:
  public Transform door;
  public BoxCollider2D doorCollider;
  public SpriteRenderer light;
  public SpriteRenderer doorHeat;
  public ParticleSystem doorFire;

  // State:
  private bool triggered;
  private bool burning;
  private float burnTimer;
  private Color doorHeatColor;
  private Vector3 doorBasePos;
  private Vector3 heatBasePos;
  private Vector3 explosionBasePos;

  // Messages:

  void Awake()
  {
    doorBasePos = door.localPosition;
    heatBasePos = doorHeat.transform.localPosition;
    door.transform.position = door.transform.position + doorHeight * Vector3.up;
    doorCollider.enabled = false;
    doorHeatColor = doorHeat.color;
    doorHeat.color = new Color(doorHeatColor.r, doorHeatColor.g, doorHeatColor.b, 0);
    doorFire.Stop();
  }

  void Update()
  {
    if(!triggered && GlobalState.Player.transform.position.x >
          transform.position.x + controllerOffset)
    {
      light.color = triggeredColor;
      triggered = true;
      doorCollider.enabled = true;
      StartCoroutine(Tween.Translate(door, null, door.transform.position - doorHeight * Vector3.up,
            0.75f, Easing.EASE_BOUNCE_OUT));
      if(GlobalState.Explosion.transform.position.x > transform.position.x + exposionStopOffset)
      {
        Vector3 movePos = GlobalState.Explosion.transform.position;
        movePos.x = transform.position.x + exposionStopOffset;
        GlobalState.Explosion.transform.position = movePos;
      }
    }
    if(!burning)
    {
      if(GlobalState.Explosion.transform.position.x > transform.position.x + exposionStopOffset)
      {
        explosionBasePos = GlobalState.Explosion.transform.position;
        burning = true;
        StartCoroutine(Tween.SpriteColor(doorHeat, null, doorHeatColor, 10.0f));
        doorFire.Play();
        GlobalState.Explosion.Pause();
      }
    }
    else if(burnTimer < 10.0f)
    {
      burnTimer += Time.deltaTime;
      if(burnTimer >= 10.0f)
      {
        doorFire.enableEmission = false;
        doorHeat.gameObject.SetActive(false);
        door.gameObject.SetActive(false);
        GlobalState.Explosion.Resume();
      }
      else
      {
        Vector3 doorShake = burnTimer * 0.005f * Random.onUnitSphere;
        door.localPosition = doorBasePos + doorShake;
        doorHeat.transform.localPosition = heatBasePos + doorShake;
        GlobalState.Explosion.transform.position = explosionBasePos +
              (burnTimer * 0.0025f + 0.0075f) * Random.onUnitSphere +
              (burnTimer * 0.11f) * Vector3.right;
      }
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireCube(transform.position + new Vector3(controllerOffset, -5.0f, 0),
          new Vector3(0.1f, 10.0f, 0.1f));
    if(door != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(door.position + doorHeight * Vector3.up, new Vector3(door.lossyScale.x,
            door.lossyScale.y, door.lossyScale.z));
    }
  }

}
