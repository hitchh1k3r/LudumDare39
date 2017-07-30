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
  public float jumpPower = 0.075f;
  public float jumpChargeLength = 1.0f;

  // Referances:
  public Animator playerAni;
  public Rigidbody2D physics;
  public MeshRenderer lowPowerText;
  public MeshRenderer notEnoughPowerText;

  // State:
  private MoveFacing moveFacing = MoveFacing.RIGHT;
  private MoveState moveState = MoveState.IDLE;
  private float xMovement;
  private bool airborne;
  private Coroutine notEnoughCor;
  private bool powerPop;
  private float powerTimer;
  private float jumpChargeTime;
  private float jumpDechargeTime;
  private float jumpChargePayment;

  // Messages:

  void Awake()
  {
    notEnoughPowerText.enabled = false;
    lowPowerText.enabled = false;
  }

  void Update()
  {
    if(!GlobalState.GameOver.isGameOver)
    {
      float powerLevel = GlobalState.Power.powerLevel;
      MoveState newState = MoveState.IDLE;
      MoveFacing newFacing = MoveFacing.UNCHANGED;
      float xSpeed = Input.GetAxis("Horizontal") * moveSpeed *
            ((!airborne && Input.GetButton("Run") && powerLevel > jumpPower * 3) ?
              runMultiplier : 1);

      if(Input.GetAxisRaw("Vertical") < -0.5f && !airborne)
      {
        if(powerLevel > jumpPower * 3 || jumpChargeTime > 0)
        {
          xSpeed = 0;
          jumpDechargeTime = 0;
          jumpChargeTime += Time.deltaTime;
          if(jumpChargePayment < jumpPower)
          {
            float payment = jumpPower * Time.deltaTime / jumpChargeLength;
            if(jumpChargePayment + payment > jumpPower)
            {
              payment = jumpPower - jumpChargePayment;
            }
            jumpChargePayment += payment;
            powerLevel -= payment;
          }
        }
        else
        {
          if(notEnoughCor != null)
          {
            StopCoroutine(notEnoughCor);
          }
          notEnoughCor = StartCoroutine(NotEnoughPower());
        }
      }
      else
      {
        jumpDechargeTime += Time.deltaTime;
        if(jumpDechargeTime > 0.2f || airborne)
        {
          jumpDechargeTime = 0;
          jumpChargeTime = 0;
          if(jumpChargePayment > 0)
          {
            powerLevel += jumpChargePayment;
            jumpChargePayment = 0;
          }
        }
      }

      xMovement = xSpeed;

      if(Input.GetButtonDown("Jump") && !airborne)
      {
        if(powerLevel > jumpPower * 3 || jumpChargeTime > 0)
        {
          if(jumpChargeTime >= jumpChargeLength)
          {
            jumpChargePayment = 0;
            powerLevel -= jumpPower;
            physics.AddForce(new Vector2(0, 22.5f), ForceMode2D.Impulse);
          }
          else
          {
            powerLevel -= jumpPower;
            physics.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
          }
        }
        else
        {
          if(notEnoughCor != null)
          {
            StopCoroutine(notEnoughCor);
          }
          notEnoughCor = StartCoroutine(NotEnoughPower());
        }
      }
      if(xSpeed < 0)
      {
        xSpeed *= -1;
        newFacing = MoveFacing.LEFT;
      }
      if(xSpeed > 0)
      {
        if(!airborne)
        {
          powerLevel -= Time.deltaTime * runPower;
          if(Input.GetButton("Run") && powerLevel > jumpPower * 3)
          {
            powerLevel -= Time.deltaTime * turboPower;
          }
        }
        if(powerLevel <= 0)
        {
          xSpeed *= 0.4f;
          xMovement *= 0.4f;
          newState = MoveState.CRAWLING;
        }
        else
        {
          if(powerLevel < 0.05f)
          {
            xSpeed *= 0.85f;
            xMovement *= 0.85f;
          }
          newState = MoveState.RUNNING;
        }
      }
      if(newState == MoveState.CRAWLING)
      {
        playerAni.SetFloat("Speed", xSpeed * 5 / moveSpeed);
      }
      else
      {
        playerAni.SetFloat("Speed", xSpeed / moveSpeed);
      }

      if(newState != moveState)
      {
        moveState = newState;
      }
      playerAni.SetBool("crawling", powerLevel < 0.01f);
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
      if(powerLevel < 0.001f || powerPop)
      {
        powerTimer += Time.deltaTime;
        if(powerTimer < 0.1f)
        {
          notEnoughPowerText.enabled = false;
        }
        else
        {
          notEnoughPowerText.enabled = true;
        }
        if(powerTimer > 0.4f)
        {
          powerTimer = 0;
        }
        lowPowerText.enabled = false;
      }
      else
      {
        if(powerLevel < 0.3333333f)
        {
          powerTimer += Time.deltaTime;
          if(powerTimer < 0.25f)
          {
            lowPowerText.enabled = false;
          }
          else
          {
            lowPowerText.enabled = true;
          }
          if(powerTimer > 1.0f)
          {
            powerTimer = 0;
          }
        }
        else
        {
          lowPowerText.enabled = false;
        }
        notEnoughPowerText.enabled = false;
      }
      playerAni.SetBool("airborne", airborne);
      playerAni.SetFloat("jump charge", jumpChargeTime / jumpChargeLength);
    }
    else
    {
      xMovement = 0;
      playerAni.SetFloat("Speed", 0);
      notEnoughPowerText.enabled = false;
    }
  }

  void OnCollisionStay2D(Collision2D collision)
  {
    if(collision.gameObject.tag == "Ground" && collision.contacts[0].normal.y > 0.1f)
    {
      airborne = false;
    }
  }

  void FixedUpdate()
  {
    physics.velocity = new Vector2(xMovement, physics.velocity.y);
    if(physics.velocity.y < -0.3f || physics.velocity.y > 0.3f)
    {
      airborne = true;
    }
  }

  // Coroutines:

  private IEnumerator NotEnoughPower()
  {
    powerPop = true;
    yield return new WaitForSeconds(1.0f);
    powerPop = false;
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
