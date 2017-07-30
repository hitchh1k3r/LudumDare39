using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Intro : MonoBehaviour
{

  // Configuration:
  public Color pcColor, npcColor;
  public BlastDoor[] doors;

  // Referances:
  public TMP_Text text;
  public CanvasGroup textFade;

  // State:
  private float messageLength = -1;

  // Messages:

  IEnumerator Start()
  {
    GlobalState.Explosion.enabled = false;
    GlobalState.GameOver.enabled = false;
    GlobalState.GameOver.isGameOver = true;

    if(PlayerPrefs.HasKey("SavePoint"))
    {
      int resumeAt = PlayerPrefs.GetInt("SavePoint");
      GlobalState.GameOver.blackFader.alpha = 0;
      GlobalState.GameOver.whiteFader.alpha = 1;
      foreach(BlastDoor door in doors)
      {
        if(resumeAt > door.savePoint)
        {
          door.RemoveDoor();
        }
        else if(resumeAt == door.savePoint)
        {
          GlobalState.Player.transform.position = door.transform.position +
                new Vector3(door.controllerOffset, -10.612f, -0.5f);
          Vector3 expPos = new Vector3(door.transform.position.x +
                door.exposionStopOffset - 10.0f, 0, 10);
          GlobalState.Explosion.transform.position = expPos;
          GlobalState.Explosion.enabled = true;
        }
      }
      yield return new WaitForSeconds(0.5f);
      yield return Tween.FadeGroup(GlobalState.GameOver.whiteFader, 1, 0, 0.5f);
    }
    else
    {
      yield return ShowMessage("I'm seeing a neutron cascade!", npcColor);
      yield return ShowMessage("The reactor is going critical...", pcColor);
      yield return ShowMessage("Lower the control rods!", npcColor);
      yield return ShowMessage("It's too late, the boron is reacting!", pcColor);
      yield return ShowMessage("You have bionic legs, right?", npcColor);
      messageLength = 1.0f;
      StartCoroutine(ShowMessage("RUN!!!", npcColor));
      yield return new WaitForSeconds(0.5f);
      yield return Tween.FadeGroup(GlobalState.GameOver.blackFader, 1, 0, 0.5f);
    }

    GlobalState.GameOver.enabled = true;
    GlobalState.Explosion.enabled = true;
    GlobalState.GameOver.isGameOver = false;
  }

  // Coroutine:

  private IEnumerator ShowMessage(string message, Color color)
  {
    text.maxVisibleCharacters = 0;
    text.text = message;
    text.color = color;
    text.ForceMeshUpdate();
    textFade.alpha = 1;
    int maxChar = text.textInfo.characterCount;
    for(int i = 0; i <= maxChar; ++i)
    {
      text.maxVisibleCharacters = i;
      if(!Input.GetButton("Cancel") && !Input.GetButton("Submit") &&
            !Input.GetButton("Jump"))
      {
        yield return new WaitForSeconds(0.02f);
      }
      else
      {
        ++i;
        ++i;
        text.maxVisibleCharacters = i;
        yield return null;
      }
    }

    float acc = 0;
    if(messageLength > 0)
    {
      while(acc < messageLength)
      {
        acc += Time.deltaTime;
        yield return null;
      }
    }
    else
    {
      while(Input.GetButton("Cancel") || Input.GetButton("Submit") || Input.GetButton("Jump"))
      {
        yield return null;
      }
      while(!Input.GetButtonDown("Cancel") && !Input.GetButtonDown("Submit") &&
            !Input.GetButtonDown("Jump") && acc < 1.25f)
      {
        acc += Time.deltaTime;
        yield return null;
      }
    }
    yield return Tween.FadeGroup(textFade, 1, 0, 0.25f);
    text.maxVisibleCharacters = 0;
  }

}
