using System;
using System.Collections;
using UnityEngine;

public abstract class InteractionEvent : MonoBehaviour
{
    [Header("Flag that is set afterwards")]
    public string successFlag;
    public int points = 0;
    public int wastedTime = 0;

    [HideInInspector] public bool success = true;
    
    public IEnumerator Execute()
    {
        GameManager.Instance.isTimerRunning = false;
        
        var player = GameManager.Instance.player;
        player.isInteracting = true;
        
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