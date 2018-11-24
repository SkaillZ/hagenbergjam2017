using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SetTimeInteraction : InteractionEvent
{
    [Header("Set Time")]
    public float time = 10f;

    public InteractionEvent next;

    protected override IEnumerator DoExecute()
    {
        GameManager.Instance.timerValue = GameManager.Instance.timerSeconds - time;

        if (next)
            yield return next.Execute();
    }
}