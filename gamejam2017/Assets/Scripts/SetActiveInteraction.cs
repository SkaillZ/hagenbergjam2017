using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SetActiveInteraction : InteractionEvent
{
    [Header("Set Active")]
    public GameObject gameObjectToSet;
    public bool active = true;

    public InteractionEvent next;

    protected override IEnumerator DoExecute()
    {
        gameObjectToSet.SetActive(active);

        if (next)
            yield return next.Execute();
    }
}