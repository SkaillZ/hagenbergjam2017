using System.Collections;
using UnityEngine;

public class InteractionConditional : InteractionEvent
{
    [Header("Condition")]
    public EventConditionType conditionType;
    public string flagName;
    public Item item;
    public bool invert = false;

    [Header("Result")]
    public InteractionEvent trueEvent;
    public InteractionEvent falseEvent;

    protected override IEnumerator DoExecute()
    {
        bool result = GetResult();
        if (invert)
            result = !result;
        
        if (result)
        {
            if (trueEvent)
                yield return trueEvent.Execute();
        }
        else
        {
            success = false;
            if (falseEvent)
                yield return falseEvent.Execute();
        }
    }
    
    public bool GetResult()
    {
        switch (conditionType)
        {
            case EventConditionType.None:
                return true;

            case EventConditionType.GameFlagSet:
                return GameManager.Instance.IsFlagSet(flagName);
                
            case EventConditionType.HasItem:
                return GameManager.Instance.HasItem(item);
              
            default:
                return false;
        }
    }
}