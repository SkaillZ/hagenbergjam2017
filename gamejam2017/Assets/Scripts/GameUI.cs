using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject nameContainer;
    public GameObject messageContainer;
    public Text nameText;
    public Text messageText;
    public GameObject textReadyIndicator;

    public Transform choicesParent;
    public GameObject choicePrefab;

    public Text timer;

    public GameObject resultsScreen;
    public Text pointsText;

    public Transform inventoryParent;
    public GameObject inventoryItemPrefab;

    public int lastChoiceResult = 0;

    private void Awake()
    {
        GameManager.Instance.gameUI = this;
        nameContainer.SetActive(false);
        messageContainer.SetActive(false);
        choicesParent.gameObject.SetActive(false);
        resultsScreen.SetActive(false);
    }

    private void Update()
    {
        int remainingTime = GameManager.Instance.GetRemainingSeconds();
        int minutes = remainingTime / 60;
        int seconds = remainingTime % 60;
        timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public IEnumerator ShowTextBox(string name, string message)
    {
        yield return TypeText(name, message);

        nameContainer.SetActive(false);
        messageContainer.SetActive(false);
        textReadyIndicator.SetActive(false);
    }

    public IEnumerator TypeText(string name, string message)
    {
        if (!string.IsNullOrEmpty(name))
        {
            nameText.text = name;
            nameContainer.SetActive(true);
        }
        if (!string.IsNullOrEmpty(message))
        {
            messageText.text = string.Empty;
            messageContainer.SetActive(true);
            textReadyIndicator.SetActive(true);
        }
        
        yield return null;
        if (!string.IsNullOrEmpty(message) || !string.IsNullOrEmpty(name))
        {
            string typedText = "";
            int characterIndex = 0;
            float delta = 0;
            bool isTyping = true;
            while (!Input.GetButtonDown("Interact") || isTyping)
            {
                yield return null;
                if (!string.IsNullOrEmpty(message) && characterIndex < message.Length + 1)
                {
                    delta += Time.deltaTime;
                    if (delta > 0.01)
                    {
                        delta = 0;

                        typedText = message.Substring(0, characterIndex) + "<color=00000000>" +
                                    message.Substring(characterIndex) + "</color>";
                        characterIndex += 3;
                        messageText.text = typedText;
                    }

                    if (Input.GetButtonDown("Interact"))
                    {
                        characterIndex = message.Length;
                        isTyping = false;
                        yield return null;
                        messageText.text = message;
                    }
                }
                else
                {
                    characterIndex = message.Length;
                    messageText.text = message;
                    isTyping = false;
                }
            }
        }
        messageText.text = message;
    }

    public IEnumerator ShowOptions(string name, string message, string[] options)
    {
        yield return TypeText(name, message);
        
        foreach (Transform child in choicesParent) {
            GameObject.Destroy(child.gameObject);
        }

        int selection = -1;
        
        choicesParent.gameObject.SetActive(true);
        bool first = true;
        for (int i = 0; i < options.Length; i++)
        {
            var choice = Instantiate(choicePrefab);
            if (first)
                choice.GetComponentInChildren<Button>().Select();
            var tmpIdx = i;
            choice.GetComponentInChildren<Button>().onClick.AddListener(() => selection = tmpIdx);
            choice.transform.SetParent(choicesParent, false);
            choice.transform.localScale = new Vector3(1f, 1f, 1f);
            choice.GetComponentInChildren<Text>().text = options[i];
            first = false;
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(choicesParent.transform as RectTransform);

        yield return null;
        while (selection == -1)
        {
            yield return null;
        }

        lastChoiceResult = selection;
        
        nameContainer.SetActive(false);
        messageContainer.SetActive(false);
        textReadyIndicator.SetActive(false);
        choicesParent.gameObject.SetActive(false);
    }

    public void ShowResults(int points)
    {
        resultsScreen.SetActive(true);
        pointsText.text = "That would be " + points + " points!";
    }

    public void UpdateInventory()
    {
        foreach (Transform child in inventoryParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var item in GameManager.Instance.inventory)
        {
            var uiItem = Instantiate(inventoryItemPrefab);
            uiItem.transform.SetParent(inventoryParent, false);
            var spriteHolder = uiItem.GetComponentsInChildren<Image>()[1];
            spriteHolder.sprite = item.sprite;
            var tooltip = uiItem.GetComponentInChildren<Text>();
            tooltip.text = item.name;
            tooltip.transform.parent.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.transform as RectTransform);

            /*tooltip.transform.parent.SetParent(inventoryParent.parent);
            tooltip.transform.parent.gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.transform as RectTransform);*/
            

        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.transform as RectTransform);
    }
}
