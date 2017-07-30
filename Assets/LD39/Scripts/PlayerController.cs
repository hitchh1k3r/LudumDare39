using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  // Configuration:
  public float moveSpeed = 5;
  public float runMultiplier = 1.5f;
  public float runPower = 0.0166666666667f;
  public float turboPower = 0.1f;

  // Referances:
  public Animator playerAni;
  public Rigidbody2D physics;

  // State:
  private MoveFacing moveFacing = MoveFacing.RIGHT;
  private MoveState moveState = MoveState.IDLE;
  private float xMovement;
  private bool airborne;

  // Messages:

  void Update()
  {
    if(!GlobalState.GameOver.isGameOver)
    {
      float powerLevel = GlobalState.Power.powerLevel;
      MoveState newState = MoveState.IDLE;
      MoveFacing newFacing = MoveFacing.UNCHANGED;
      float xSpeed = Input.GetAxis("Horizontal") * moveSpeed *
            ((Input.GetButton("Run") && powerLevel > 0.25f) ? runMultiplier : 1);
      xMovement = xSpeed;

      if(Input.GetButtonDown("Jump") && !airborne)
      {
        if(powerLevel > 0.25f)
        {
          powerLevel -= 0.075f;
          physics.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        }
        else
        {
          // NOT ENOUGH POWER!!!
        }
      }
      if(xSpeed < 0)
      {
        xSpeed *= -1;
        newFacing = MoveFacing.LEFT;
      }
      if(xSpeed > 0)
      {
        powerLevel -= Time.deltaTime * runPower;
        if(Input.GetButton("Run") && powerLevel > 0.25f)
        {
          powerLevel -= Time.deltaTime * turboPower;
        }
        if(powerLevel <= 0)
        {
          xSpeed *= 0.1f;
          xMovement *= 0.1f;
          newState = MoveState.CRAWLING;
        }
        else
        {
          if(powerLevel < 0.1f)
          {
            xSpeed *= 0.85f;
            xMovement *= 0.85f;
          }
          newState = MoveState.RUNNING;
        }
      }
      if(newState == MoveState.CRAWLING)
      {
        playerAni.SetFloat("Speed", xSpeed * 20 / moveSpeed);
      }
      else
      {
        playerAni.SetFloat("Speed", xSpeed / moveSpeed);
      }

      if(newState != moveState)
      {
        moveState = newState;
        playerAni.SetBool("crawling", moveState == MoveState.CRAWLING);
      }
      if(newFacing != moveFacing && moveState != MoveState.IDLE)
      {
        moveFacing = newFacing;
        if(moveFacing == MoveFacing.LEFT)
        {
          transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
          transform.localScale = new Vector3(1, 1, 1);
        }
      }
      GlobalState.Power.powerLevel = powerLevel;
    }
    else
    {
      xMovement = 0;
      playerAni.SetFloat("Speed", 0);
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if(collision.gameObject.tag == "Ground")
    {
      airborne = false;
    }
  }

  void FixedUpdate()
  {
    physics.velocity = new Vector2(xMovement, physics.velocity.y);
    if(physics.velocity.y < -0.01f || physics.velocity.y > 0.01f)
    {
      airborne = true;
    }
  }

  // PlayerController //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*

  public enum MoveFacing
  {
    UNCHANGED,
    LEFT,
    RIGHT
  }

  public enum MoveState
  {
    IDLE,
    RUNNING,
    CRAWLING
  }

}
