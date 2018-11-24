using System.Collections;
using UnityEngine;

public class ItemInteraction : InteractionEvent
{
    [Header("Item (gives item by default)")]
    public Item item;
    public bool take = false;
    public InteractionEvent next;

    protected override IEnumerator DoExecute()
    {
        if (!take)
        {
            GameManager.Instance.GiveItem(item);
        }
        else
        {
            GameManager.Instance.TakeItem(item);
        }

        if (next)
            yield return next.Execute();
    }
}