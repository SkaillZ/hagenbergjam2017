using System.Collections;
using UnityEngine;

public class TextBoxInteraction : InteractionEvent
{
    [Header("Textbox")]
    public TextBoxType type = TextBoxType.Dialogue;
    public string characterName;
    [TextArea(3,10)] public string message;
    public InteractionEvent next;
    
    protected override IEnumerator DoExecute()
    {
        var ui = GameManager.Instance.gameUI;
        yield return ui.ShowTextBox(characterName, message);

        if (next)
            yield return next.Execute();
    }
}

public enum TextBoxType
{
    Dialogue,
    Monologue,
    System
}