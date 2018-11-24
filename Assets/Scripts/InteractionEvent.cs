using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionEvent : MonoBehaviour
{
    [Header("Flag that is set afterwards")]
    public string successFlag;
    public int points = 0;
    public int wastedTime = 0;
    public AudioSource sound;
    public Image fader;
    
    [Header("Death (fader must be set)")]
    public bool leadToDeath = false;
    public string deathMessage;

    [HideInInspector]
    public bool success = true;
    
    public IEnumerator Execute()
    {
        GameManager.Instance.isTimerRunning = false;
        
        var player = GameManager.Instance.player;
        player.isInteracting = true;

        float fade = 0f;

        if (fader && !leadToDeath)
        {
            while (fade < 1f)
            {
                fade += 0.05f;
                fader.color = new Color(0f, 0f, 0f, fade);
                yield return null;
            }
        }

        if (sound != null)
        {
            sound.Play();
        }
        
        if (fader && !leadToDeath)
        {
            while (fade > 0f)
            {
                fade -= 0.05f;
                fader.color = new Color(0f, 0f, 0f, fade);
                yield return null;
            }
        }
        
        yield return DoExecute();
        if (success)
            GameManager.Instance.SetFlag(successFlag);
        player.isInteracting = false;

        if (points != 0)
        {
            GameManager.Instance.AddPoints(points);
        }

        if (wastedTime != 0)
        {
            GameManager.Instance.SubtractTime(wastedTime);
        }

        if (leadToDeath)
        {
            GameManager.Instance.deathMessage = deathMessage;
            if (fader)
            {
                while (fade < 1f)
                {
                    fade += 0.01f;
                    fader.color = new Color(1f, 1f, 1f, fade);
                    yield return null;
                }
                GameManager.Instance.StartCoroutine(GameManager.Instance.DeathRoutine(false));
            }
        }
        
        GameManager.Instance.isTimerRunning = true;
    }
    
    protected abstract IEnumerator DoExecute();
}

public enum EventConditionType
{
    None,
    GameFlagSet,
    HasItem
}