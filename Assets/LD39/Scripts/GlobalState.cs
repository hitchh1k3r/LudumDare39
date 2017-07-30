using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{

  // State:
  public GameOver gameOver;
  public PlayerController player;
  public ExplosionController explosion;
  public PowerMeter power;
  public CameraController cameraController;

  // Accessors:
  public static CameraController Camera { get { return instance.cameraController; } private set {} }
  public static PowerMeter Power { get { return instance.power; } private set {} }
  public static ExplosionController Explosion { get { return instance.explosion; } private set {} }
  public static PlayerController Player { get { return instance.player; } private set {} }
  public static GameOver GameOver { get { return instance.gameOver; } set { instance.gameOver = value; } }

  // Instance:
  private static GlobalState instance;

  // Messages:

  void Awake()
  {
    instance = this;
  }

}
