using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Serialization;

/// <summary>
/// Dialogue controller which the active <see cref="DialogueCanvasUI"/> derives its values and static methods from.
/// On Awake, it creates itself an instance and gathers the dictionary values for global tags and character UI elements. <see cref="TextBubbleCharacterUI"/>
/// </summary>

public enum BubbleAlignment { Left, Right }

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    #region Variables: Canvas
    public TextAsset InkTextAsset;

    public bool AutoplayEnabled;
    [Space(5)]
    [Header("Debug tools, only for inspector use.")]
    [SerializeField] TextingScreenFormat aspectRatioValues;
    [SerializeField] bool previewAspectRatio;
    [SerializeField] Vector2 aspectRatio;
    [Space(5)]
    [Header("Scene Components")]
    [SerializeField] DialogueCanvasUI dialogueCanvas;
    [SerializeField] AutoplaySkipUI autoplaySkipUI;
    #endregion

    #region Variables: Timers
    [Header("Phone UI: Timers")]
    public float CanvasFadeDuration;
    public float StartDelayDuration;
    public float BubbleFadeDuration;
    public float AutoplayDelayDuration;
    [HideInInspector] public bool CanPrintDialogue;
    #endregion

    #region Variables: Typing Bubbles
    [Header("Phone UI: Typing Bubbles")]
    [HideInInspector] public float CurrentTypingDelayDuration;
    public float ShortTypingDelayDuration;
    public float MidTypingDelayDuration;
    public float LongTypingDelayDuration;
    [HideInInspector] public TextTypingUI CurrentTypingBubble;
    [HideInInspector] public TextTypingUI LeftTypingBubble;
    [HideInInspector] public TextTypingUI RightTypingBubble;
    #endregion

    #region Variables: Prefabs
    [Header("Prefabs")]
    public GameObject TextBubblePrefab;
    public GameObject OptionButtonPrefab;
    public GameObject TypingBubblePrefab;
    #endregion

    #region Hidden Variables
    [HideInInspector] public Story InkStory;
    [HideInInspector] public int CurrentBubbleIndex;
    [HideInInspector] public List<string> LinesBeforeChoice = new List<string>();
    [HideInInspector] public List<TextBubbleUI> BubblesBeforeChoice = new List<TextBubbleUI>();
    [HideInInspector] public List<TextOptionUI> CurrentOptions = new List<TextOptionUI>();

    public Dictionary<string, string> GlobalTagsDictionary = new Dictionary<string, string>();
    public Dictionary<string, TextBubbleUIElements> CharacterUIDictionary = new Dictionary<string, TextBubbleUIElements>();
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_STANDALONE
        aspectRatioValues = null;
#endif

        Instance = this;
        InkStory = new Story(InkTextAsset.text);
        CurrentBubbleIndex = 0;
        GetDictionaryValues();
    }

    void Start()
    {
        GetLinesBeforeChoice();
        CreateTextTypingBubbles();
        SelectPlatform();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (previewAspectRatio)
        {
            for (int i = 0; i < aspectRatioValues.TextingFormatList.Count; i++)
            {
                if (aspectRatio == aspectRatioValues.TextingFormatList[i].AspectRatio)
                {
                    dialogueCanvas.ResizeCanvasForPlatform(aspectRatioValues.TextingFormatList[i]);
                    autoplaySkipUI.ResizeMenuForPlatform(aspectRatioValues.TextingFormatList[i]);
                }
            }

            previewAspectRatio = false;
        }
#endif
    }

    /// <summary>
    /// Check the platform and set the correct UI as current.
    /// Can be used in-editor by using the <see cref="Platform"/> dropdown in the inspector.
    /// Called on <see cref="Awake"/>.
    /// </summary>
    void SelectPlatform()
    {
        dialogueCanvas.ResizeCanvasForPlatform(ScreenAspectRatio.AspectRatio);
        dialogueCanvas.GetReferencesFromController();

        autoplaySkipUI.ResizeMenuForPlatform(ScreenAspectRatio.AspectRatio);
    }

    /// <summary>
    /// Create dictionary of <see cref="TextBubbleCharacterUI.CharacterUIElements"/> items.
    /// Called on <see cref="Awake"/>.
    /// </summary>
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
    /// Called on <see cref="Start"/>.
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
                CreateTextBubble(parsedLine);
            }
        }
    }

    /// <summary>
    /// Create a text bubble using the parsed text.
    /// </summary>
    /// <param name="line"></param>
    void CreateTextBubble(string line)
    {
        GameObject textBubble = Instantiate(TextBubblePrefab, dialogueCanvas.BodyScrollContent.transform);
        BubblesBeforeChoice.Add(textBubble.GetComponent<TextBubbleUI>());

        TextBubbleUI ui = textBubble.GetComponent<TextBubbleUI>();
        string speakerName = ui.ParseSpeaker(line);
        FadeInUI(ui.CanvasGroup, BubbleFadeDuration);

        ui.SetTextBubbleInformation(line, TextBubbleCharacterUI.Instance.MainCharacterName, CharacterUIDictionary[speakerName]);
    }

    /// <summary>
    /// Create the typing bubbles with values from <see cref="TextBubbleCharacterUI"/> and hide them in the inspector.
    /// Called on <see cref="Start"/>, simultaneously with <see cref="GetLinesBeforeChoice"/>.
    /// </summary>
    void CreateTextTypingBubbles()
    {
        foreach (TextBubbleUIElements ui in TextBubbleCharacterUI.Instance.CharacterUIElements)
        {
            GameObject typingBubble = Instantiate(TypingBubblePrefab, dialogueCanvas.BodyScrollContent.transform);
            TextTypingUI typingUI = typingBubble.GetComponent<TextTypingUI>();

            if (ui.CharacterName.Equals(TextBubbleCharacterUI.Instance.MainCharacterName))
            {
                typingUI.GetBubbleFormatting(ScreenAspectRatio.AspectRatio, TextAnchor.LowerRight);
                RightTypingBubble = typingUI;
            } else
            {
                typingUI.GetBubbleFormatting(ScreenAspectRatio.AspectRatio, TextAnchor.LowerLeft);
                LeftTypingBubble = typingUI;
            }

            typingUI.SetBubbleColor(TextBubbleCharacterUI.Instance.MainCharacterName, ui);
        }
    }

    /// <summary>
    /// Used when a choice is selected.
    /// Clears all current options objects before grabbing the next set of text objects.
    /// Called by <see cref="DialogueCanvasUI.DisplayChoices"/>.
    /// </summary>
    /// <param name="choice"></param>
    public void ChoiceMadeCallback()
    {
        // Select route and hide buttons.
        GetCurrentTypingBubble(TextBubbleCharacterUI.Instance.MainCharacterName);

        CurrentOptions.Clear();
        GetLinesBeforeChoice();

        dialogueCanvas.ResetTextContainerSize();
        dialogueCanvas.ContinueDialogueButton.gameObject.SetActive(true);

        BubblesBeforeChoice[BubblesBeforeChoice.Count - 1].gameObject.SetActive(false);
        CurrentTypingBubble.gameObject.SetActive(false);
        CanPrintDialogue = true;

        if (AutoplayEnabled)
            dialogueCanvas.AutoplayDialogue();
        else
            dialogueCanvas.PlayDialogue();
    }

    /// <summary>
    /// Sets the autoplay variable and changes the text on the UI accordingly.
    /// Called when <see cref="currentDialogueCanvas.autoplayToggleButton"/> has its value changed.
    /// </summary>
    /// <returns></returns>
    public bool SetAutoplay()
    {
        AutoplayEnabled = !AutoplayEnabled;

        if (AutoplayEnabled)
            dialogueCanvas.autoplayText.text = "Autoplay\n(ON)";
        else
            dialogueCanvas.autoplayText.text = "Autoplay\n(OFF)";

        return AutoplayEnabled;
    }

    /// <summary>
    /// Used to skip directly to the next choice. Shows all bubbles and then <see cref="DisplayChoices"/>.
    /// </summary>
    public void SkipToChoice()
    {
        List<TextBubbleUI> bubblesBeforeChoice = BubblesBeforeChoice;
        Story inkStory = InkStory;
        AutoplaySkipUI.Instance.DisplayAutoplayMenu(false);

        if (bubblesBeforeChoice.Count > 0)
        {
            for (int i = 0; i < bubblesBeforeChoice.Count; i++)
            {
                if (!bubblesBeforeChoice[i].gameObject.activeInHierarchy)
                {
                    bubblesBeforeChoice[i].gameObject.SetActive(true);
                    FadeInUI(bubblesBeforeChoice[i].CanvasGroup, BubbleFadeDuration);
                }
            }
        }

        if (inkStory.currentChoices.Count > 0)
            dialogueCanvas.DisplayChoices();
    }

    /// <summary>
    /// Used to autoplay dialogue if the setting is on.
    /// Only prints after a bubble has finished printing.
    /// </summary>
    public void AutoplayDialogue()
    {
        StartCoroutine(StartAutoplay());
    }

    IEnumerator StartAutoplay()
    {
        while (CurrentBubbleIndex < BubblesBeforeChoice.Count - 1 && AutoplayEnabled)
        {
            while (!CanPrintDialogue)
            {
                yield return null;
            }

            if (AutoplayEnabled)
            {
                dialogueCanvas.PlayDialogue();
                yield return new WaitForSeconds(AutoplayDelayDuration + CurrentTypingDelayDuration);
            }
            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// Gets the current speaker's bubble to correctly show the typing animation.
    /// Called by <see cref="DialogueCanvasUI.PlayDialogue"/>.
    /// </summary>
    /// <param name="speakerName"></param>
    /// <returns></returns>
    public TextTypingUI GetCurrentTypingBubble(string speakerName)
    {
        if (speakerName.Equals(TextBubbleCharacterUI.Instance.MainCharacterName))
            CurrentTypingBubble = RightTypingBubble;
        else
            CurrentTypingBubble = LeftTypingBubble;

        return CurrentTypingBubble;
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
}
