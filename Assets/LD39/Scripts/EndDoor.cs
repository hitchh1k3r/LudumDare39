using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{

  // Configuration:
  public float controllerOffset = 0.708f;
  public float doorHeight;
  public Color triggeredColor;
  public float openTime = 5.0f;

  // Refernaces:
  public Transform doorContainer;
  public Transform door;
  public BoxCollider2D doorCollider;
  public SpriteRenderer light;

  // State:
  private bool triggered;

  // Messages:

  void Awake()
  {
    doorCollider.enabled = true;
  }

  void Update()
  {
    if(!triggered && GlobalState.Player.transform.position.x >
          transform.position.x + controllerOffset)
    {
      light.color = triggeredColor;
      triggered = true;
      StartCoroutine(RaiseDoor());
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

  // Coroutines:

  private IEnumerator RaiseDoor()
  {
    StartCoroutine(Tween.GenericTween(t => {
        door.localPosition = (0.01f * (1 - t)) * Random.onUnitSphere;
      }, openTime, Easing.EASE_CUBIC_OUT));
    StartCoroutine(Tween.Translate(doorContainer, null, doorContainer.transform.position +
          doorHeight * Vector3.up, openTime, Easing.EASE_CUBIC_IN));
    yield return new WaitForSeconds(openTime * 0.85f);
    doorCollider.enabled = false;
  }

}
