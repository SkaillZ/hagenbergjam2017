﻿using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject resultFade;
    public Image restartFade;
    public Text secondsText;
    public Text pointsDecText;

    public int lastChoiceResult = 0;

    private float pointFade = 0f;
    private float secondsFade = 0f;

    private void Awake()
    {
        GameManager.Instance.gameUI = this;
        nameContainer.SetActive(false);
        messageContainer.SetActive(false);
        choicesParent.gameObject.SetActive(false);
        resultsScreen.SetActive(false);
        secondsText.color = new Color(secondsText.color.r, secondsText.color.g, secondsText.color.b, 0f);
        pointsDecText.color = new Color(pointsDecText.color.r, pointsDecText.color.g, pointsDecText.color.b, 0f);
    }

    private void Update()
    {
        var val = Mathf.Lerp(0.25f, 0.35f, Mathf.Sin(GameManager.Instance.timerValue));
        timer.gameObject.transform.localScale = new Vector3(val, val, 1);
        int remainingTime = GameManager.Instance.GetRemainingSeconds();
        int minutes = remainingTime / 60;
        int seconds = remainingTime % 60;
        timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (pointFade > 0f)
        {
            pointFade -= 0.02f;
            pointsDecText.color = new Color(pointsDecText.color.r, pointsDecText.color.g, pointsDecText.color.b, pointFade);
        }
        
        if (secondsFade > 0f)
        {
            secondsFade -= 0.02f;
            secondsText.color = new Color(secondsText.color.r, secondsText.color.g, secondsText.color.b, secondsFade);
        }
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
        //<500 grottenschlecht, < 1000 schlecht, > 1000 gut

        string resultText = "";
        if (!string.IsNullOrEmpty(GameManager.Instance.deathMessage))
        {
            resultText = GameManager.Instance.deathMessage;
        }
        else
        {
            resultText = "Your last day sucked like always.";
            if (points > 500)
            {
                resultText = "Your last day was pretty okay.";
            }
            if (points > 1000)
            {
                resultText = "Your last day was pretty good!";
            }
        }

        pointsText.text = resultText + "\n\nThat would be " + points + " points!";
        
        
        nameContainer.SetActive(false);
        messageContainer.SetActive(false);
        textReadyIndicator.SetActive(false);
        choicesParent.gameObject.SetActive(false);
        
        resultFade.gameObject.SetActive(true);
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

    public void Quit()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void PlayAgain()
    {
        StartCoroutine(RestartGame());
    }

    private float fade = 0f;

    public IEnumerator RestartGame()
    {
        while (fade < 1)
        {
            yield return null;
            fade += 0.05f;
            restartFade.color = new Color(0f, 0f, 0f, fade);
        }

        GameManager.Instance.timerValue = 0f;
        GameManager.Instance.player.dead = false;
        GameManager.Instance.isTimerRunning = true;
        GameManager.Instance.startedRoutine = false;
        GameManager.Instance.gameFlags.Clear();
        GameManager.Instance.inventory.Clear();
        GameManager.Instance.deathMessage = null;
        SceneManager.LoadScene(1);
        yield return null;
        GameManager.Instance.timerValue = 0f;
        GameManager.Instance.player.dead = false;
        GameManager.Instance.isTimerRunning = true;
        GameManager.Instance.startedRoutine = false;
        GameManager.Instance.gameFlags.Clear();
        GameManager.Instance.inventory.Clear();
        GameManager.Instance.deathMessage = null;
        Time.timeScale = 1f;
        
        while (fade < 1)
        {
            yield return null;
            fade -= 0.05f;
            restartFade.color = new Color(0f, 0f, 0f, fade);
        }
        GameManager.Instance.timerValue = 0f;

        Debug.Log(Time.timeScale);
    }

    public void ShowPointsIncrease(int points)
    {
        pointsDecText.text = "+ " + points + " points";
        pointFade = 2f;
    }

    public void ShowTimeDecrease(int seconds)
    {
        secondsText.text = "- " + seconds + " seconds";
        secondsFade = 2f;
    }
}
