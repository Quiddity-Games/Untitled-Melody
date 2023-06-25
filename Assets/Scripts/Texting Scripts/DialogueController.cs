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
    enum Platform { PC, Android }

    #region Variables: Canvas
    public TextAsset InkTextAsset;
    public bool Autoplay;
    [Tooltip("Only for inspector use.")]
    [SerializeField] Platform platform;
    [Space(5)]
    private DialogueCanvasUI currentDialogueCanvas;
    [FormerlySerializedAs("dialogueCanvas")]
    [SerializeField] DialogueCanvasUI desktopDialogueCanvas;
    [SerializeField] DialogueCanvasUI mobileDialogueCanvas;
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
    public float ShortTypingDelayDuration;
    public float MidTypingDelayDuration;
    public float LongTypingDelayDuration;
    [HideInInspector] public CanvasGroup CurrentTypingBubble;
    [HideInInspector] public CanvasGroup LeftTypingBubble;
    [HideInInspector] public CanvasGroup RightTypingBubble;
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
        Instance = this;

        SelectPlatform();

        InkStory = new Story(InkTextAsset.text);
        GetDictionaryValues();
    }

    void Start()
    {
        GetLinesBeforeChoice();
        CreateTextTypingBubbles();
    }

    /// <summary>
    /// Check the platform and set the correct UI as current.
    /// Can be used in-editor by using the <see cref="Platform"/> dropdown in the inspector.
    /// </summary>
    void SelectPlatform()
    {
#if UNITY_EDITOR
        if (platform == Platform.Android)
        {
            currentDialogueCanvas = mobileDialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(true);
            desktopDialogueCanvas.gameObject.SetActive(false);
        }
        else
        {
            currentDialogueCanvas = desktopDialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(false);
            desktopDialogueCanvas.gameObject.SetActive(true);
        }

#elif UNITY_STANDALONE
        if (Application.platform == RuntimePlatform.Android)
        {
            currentDialogueCanvas = mobileDialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(true);
            desktopDialogueCanvas.gameObject.SetActive(false);
        }
        else
        {
            currentDialogueCanvas = desktopDialogueCanvas;
            mobileDialogueCanvas.gameObject.SetActive(false);
            desktopDialogueCanvas.gameObject.SetActive(true);
        }
#endif

        currentDialogueCanvas.GetReferencesFromController();
        
    }

    /// <summary>
    /// Create dictionary of <see cref="TextBubbleCharacterUI.CharacterUIElements"/> items.
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
    /// Create the typing bubbles with values from <see cref="TextBubbleCharacterUI"/> and hide them in the inspector.
    /// </summary>
    void CreateTextTypingBubbles()
    {
        foreach (TextBubbleUIElements ui in TextBubbleCharacterUI.Instance.CharacterUIElements)
        {
            GameObject typingBubble = Instantiate(TypingBubblePrefab, currentDialogueCanvas.TextTypingContainer.transform);
            TextTypingUI typingUI = typingBubble.GetComponent<TextTypingUI>();

            if (ui.CharacterName.Equals(TextBubbleCharacterUI.Instance.MainCharacterName))
            {
                typingUI.SetBubbleAlignment(TextAnchor.LowerRight);
                RightTypingBubble = typingUI.CanvasGroup;
            } else
            {
                typingUI.SetBubbleAlignment(TextAnchor.LowerLeft);
                LeftTypingBubble = typingUI.CanvasGroup;
            }

            typingUI.SetBubbleColor(TextBubbleCharacterUI.Instance.MainCharacterName, ui);
        }
    }

    /// <summary>
    /// Gets the current speaker's bubble to correctly show the typing animation.
    /// </summary>
    /// <param name="speakerName"></param>
    /// <returns></returns>
    public CanvasGroup GetCurrentBubble(string speakerName)
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
            GetCurrentBubble(TextBubbleCharacterUI.Instance.MainCharacterName);

            for (int i = 0; i < CurrentOptions.Count; i++)
            {
                Destroy(CurrentOptions[i].gameObject);
            }

            CurrentOptions.Clear();
            GetLinesBeforeChoice();

            currentDialogueCanvas.ResetTextContainerSize();
            currentDialogueCanvas.ContinueDialogueButton.gameObject.SetActive(true);

            CanPrintDialogue = true;

            if (Autoplay)
                currentDialogueCanvas.AutoplayDialogue();
            else
                currentDialogueCanvas.PlayDialogue();
        }
    }

    /// <summary>
    /// Sets the autoplay variable and changes the text on the UI accordingly.
    /// </summary>
    /// <returns></returns>
    public bool SetAutoplay()
    {
        Autoplay = !Autoplay;

        if (Autoplay)
            currentDialogueCanvas.autoplayText.text = "Autoplay\n(ON)";
        else
            currentDialogueCanvas.autoplayText.text = "Autoplay\n(OFF)";

        return Autoplay;
    }
}
