using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForkInteraction : TextBoxInteraction
{
    [Header("Fork (arrays MUST be the same size)")]
    public string[] options;
    public InteractionEvent[] outcomes;
	public string[] hidingFlags;

    private int optionIndex = 0;

    protected override IEnumerator DoExecute()
    {
		List<string> filteredOptions = new List<string> ();
		for (int i = 0; i < hidingFlags.Length; i++) {
			if (string.IsNullOrEmpty(hidingFlags[i])) {
				filteredOptions.Add (options[i]);
				continue;
			}
			if (!GameManager.Instance.IsFlagSet (hidingFlags [i])) {
				filteredOptions.Add (options[i]);
			}
		}
	
		List<InteractionEvent> filteredOutcomes = new List<InteractionEvent> ();
		for (int i = 0; i < hidingFlags.Length; i++) {
			if (string.IsNullOrEmpty(hidingFlags[i])) {
				filteredOutcomes.Add (outcomes[i]);
				continue;
			}
			if (!GameManager.Instance.IsFlagSet (hidingFlags [i])) {
				filteredOutcomes.Add (outcomes[i]);
			}
		}

		if (hidingFlags.Length == 0) {
			filteredOptions = options.ToList ();
			filteredOutcomes = outcomes.ToList ();
		}

		if (filteredOptions.Count != 0) {

			var ui = GameManager.Instance.gameUI;
			yield return ui.ShowOptions (characterName, message, filteredOptions.ToArray ());
			optionIndex = ui.lastChoiceResult;

			if (filteredOutcomes [optionIndex]) {
				Debug.Log ("fork: chose " + filteredOptions [optionIndex]);
				yield return filteredOutcomes [optionIndex].Execute ();
			}
		}
    }
}