using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MoveInteraction : InteractionEvent
{
    [Header("Move")]
    public GameObject gameObjectToMove;
    public Transform position;
    public float duration = 5f;

    public InteractionEvent next;

    protected override IEnumerator DoExecute()
    {
        yield return gameObjectToMove.transform.DOMove(position.position, duration).WaitForCompletion();

        if (next)
            yield return next.Execute();
    }
}