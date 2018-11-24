using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlaySoundEffectInteraction : InteractionEvent
{
    [Header("Play Sound effect")]
    public AudioSource audioSource;

    public InteractionEvent next;

    protected override IEnumerator DoExecute()
    {
        audioSource.Play();
        
        yield return new WaitWhile (() => audioSource.isPlaying);

        if (next)
            yield return next.Execute();
    }
}