using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;
using System.Text.RegularExpressions;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    public TextAsset InkTextAsset;
    private DialogueCanvasUI currentDialogueCanvas;
    [SerializeField] DialogueCanvasUI dialogueCanvas;
    [SerializeField] DialogueCanvasUI mobileDialogueCanvas;

    [Header("Phone UI: Overall")]
    public float CanvasFadeDuration;
    public float StartDelayDuration;
    public float BubbleFadeDuration;

    [Header("Prefabs")]
    public GameObject TextBubblePrefab;
    public GameObject OptionButtonPrefab;

    [HideInInspector] public Story InkStory;
    [HideInInspector] public TextBubbleCharacterUI textBubbleCharacterUI;

    [HideInInspector] public int CurrentBubbleIndex;
    [HideInInspector] public List<string> LinesBeforeChoice = new List<string>();
    [HideInInspector] public List<TextBubbleUI> BubblesBeforeChoice = new List<TextBubbleUI>();
    [HideInInspector] public List<TextOptionUI> CurrentOptions = new List<TextOptionUI>();

    public Dictionary<string, string> GlobalTagsDictionary = new Dictionary<string, string>();
    public Dictionary<string, TextBubbleUIElements> CharacterUIDictionary = new Dictionary<string, TextBubbleUIElements>();


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        if (Application.platform == RuntimePlatform.Android)
        {
            currentDialogueCanvas = mobileDialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(true);
            dialogueCanvas.gameObject.SetActive(false);
        } else
        {
            currentDialogueCanvas = dialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(false);
            dialogueCanvas.gameObject.SetActive(true);
        }

        InkStory = new Story(InkTextAsset.text);
        GetDictionaryValues();
    }

    private void Start()
    {
        GetLinesBeforeChoice();
    }

    void GetDictionaryValues()
    {
        // Get global tags.
        for (int i = 0; i < InkStory.globalTags.Count; i++)
        {
            if (InkStory.globalTags[i].Contains("Conversation: "))
            {
                GlobalTagsDictionary.Add("Conversation", InkStory.globalTags[i].Replace("Conversation: ", ""));
            }
        }

        List<TextBubbleUIElements> characterUIElements = TextBubbleCharacterUI.Instance.CharacterUIElements;

        // Get character UI elements.
        for (int i = 0; i < characterUIElements.Count; i++)
        {
            CharacterUIDictionary.Add(characterUIElements[i].CharacterName, characterUIElements[i]);
        }
    }

    /// <summary>
    /// Get a list of strings as all lines before choices, including tags.
    /// </summary>
    void GetLinesBeforeChoice()
    {
        while (InkStory.canContinue)
        {
            string currentLine = InkStory.Continue();
            List<string> currentTags = InkStory.currentTags;

            string parsedLine = "";

            foreach (string tag in currentTags)
            {
                parsedLine += ("#" + tag + "\n");
            }
            parsedLine += currentLine;

            if (!string.IsNullOrEmpty(parsedLine))
            {
                LinesBeforeChoice.Add(parsedLine);
                currentDialogueCanvas.CreateTextBubble(parsedLine);
            }
        }
    }

    /// <summary>
    /// Used to fade in UI with a CanvasGroup component attached.
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="duration"></param>
    public void FadeInUI(CanvasGroup canvasGroup, float duration)
    {
        StartCoroutine(FadeIn(duration));

        IEnumerator FadeIn(float duration)
        {
            float time = 0;

            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }
    }

    /// <summary>
    /// Used when a choice is selected.
    /// Clears all current options objects before grabbing the next set of text objects.
    /// </summary>
    /// <param name="choice"></param>
    public void ChoiceMadeCallback(int choice)
    {
        if (InkStory.currentChoices.Count > 0)
        {
            // Select route and destroy buttons.
            InkStory.ChooseChoiceIndex(choice);

            for (int i = 0; i < CurrentOptions.Count; i++)
            {
                Destroy(CurrentOptions[i].gameObject);
            }

            CurrentOptions.Clear();
            GetLinesBeforeChoice();

            currentDialogueCanvas.PlayDialogue();
            currentDialogueCanvas.ResetTextContainerSize();
            currentDialogueCanvas.ContinueDialogueButton.gameObject.SetActive(true);
        }
    }
}
