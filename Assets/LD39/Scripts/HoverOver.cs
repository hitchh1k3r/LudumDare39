using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOver : MonoBehaviour
{

  // Referances:
  public Transform trackPos;

  // Messages:

  void Update()
  {
    transform.position = trackPos.position;
  }

}
