using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEditor;
using System.Text.RegularExpressions;

/// <summary>
/// Dialogue controller which the active <see cref="DialogueCanvasUI"/> derives its values and static methods from.
/// On Awake, it creates itself an instance and gathers the dictionary values for global tags and character UI elements.
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

    #region Variables: Character UI 
    [Space(10)]
    [Header("Character UI")]
    public string MainCharacterName;
    [SerializeField] bool updateCharacterList;
    [SerializeField] List<CharacterUIInfo> charactersInStory = new();
    #endregion

    #region Hidden Variables
    [HideInInspector] public Story InkStory;
    [HideInInspector] public int CurrentBubbleIndex;
    [HideInInspector] public List<string> LinesBeforeChoice = new();
    [HideInInspector] public List<TextBubbleUI> BubblesBeforeChoice = new();
    [HideInInspector] public List<TextOptionUI> CurrentOptions = new();

    public Dictionary<string, string> GlobalTagsDictionary = new();
    public Dictionary<string, CharacterDialogueInfo> CharactersInStoryDictionary = new();
    public static VoidCallback InitializeDialogue;
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

        InitializeDialogue += GetLinesBeforeChoice;
        InitializeDialogue += CreateTextTypingBubbles;
        InitializeDialogue += SelectPlatform;

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

        if (updateCharacterList && InkTextAsset)
        {
            updateCharacterList = false;
            charactersInStory.Clear();

            List<string> names = new List<string>();

            string pattern = @"(?:[Ss]peaker\:.)(\w+)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(InkTextAsset.text);

            foreach (Match match in m)
            {
                if (!names.Contains(match.Groups[1].Value))
                    names.Add(match.Groups[1].Value);
            }

            foreach (string name in names)
            {
                CharacterDialogueInfo asset = (CharacterDialogueInfo)AssetDatabase.LoadAssetAtPath(
                    "Assets/2 - Scripts/1 - Texting Scripts/0 - Character UI Info/" + name + " Dialogue Info.asset",
                    typeof(CharacterDialogueInfo));

                CharacterUIInfo info = new(name, asset);

                if (!charactersInStory.Contains(info))
                    charactersInStory.Add(info);
            }
        }
#endif
    }

    /// <summary>
    /// Check the platform and set the correct UI as current.
    /// Can be used in-editor by using the <see cref="Platform"/> dropdown in the inspector.
    /// Called on <see cref="Awake"/>.
    /// </summary>
    public void SelectPlatform()
    {
        dialogueCanvas.ResizeCanvasForPlatform(ScreenAspectRatio.AspectRatio);
        dialogueCanvas.GetReferencesFromController();

        autoplaySkipUI.ResizeMenuForPlatform(ScreenAspectRatio.AspectRatio);
    }

    /// <summary>
    /// Create dictionary of <see cref="CharacterUIElements"/> items.
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

        // Get character UI elements.
        for (int i = 0; i < charactersInStory.Count; i++)
        {
            CharactersInStoryDictionary.Add(charactersInStory[i].CharacterName, charactersInStory[i].Info);
        }
    }

    /// <summary>
    /// Get a list of strings as all lines before choices, including tags.
    /// Called on <see cref="Start"/>.
    /// </summary>
    public void GetLinesBeforeChoice()
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
        TextBubbleUI textBubble = Instantiate(TextBubblePrefab, dialogueCanvas.BodyScrollContent.transform).GetComponent<TextBubbleUI>();
        BubblesBeforeChoice.Add(textBubble);

        string speakerName = textBubble.ParseSpeaker(line);
        FadeInUI(textBubble.CanvasGroup, BubbleFadeDuration);

        textBubble.SetTextBubbleInformation(line, MainCharacterName, speakerName);
    }

    /// <summary>
    /// Create the typing bubbles with values from <see cref="CharacterUIElements"/> and hide them in the inspector.
    /// Called on <see cref="Start"/>, simultaneously with <see cref="GetLinesBeforeChoice"/>.
    /// </summary>
    public void CreateTextTypingBubbles()
    {
        foreach (CharacterUIInfo ui in charactersInStory)
        {
            TextTypingUI typingUI = Instantiate(TypingBubblePrefab, dialogueCanvas.BodyScrollContent.transform).GetComponent<TextTypingUI>();

            if (ui.CharacterName.Equals(MainCharacterName))
            {
                typingUI.GetBubbleFormatting(ScreenAspectRatio.AspectRatio, TextAnchor.LowerRight);
                RightTypingBubble = typingUI;
            } else
            {
                typingUI.GetBubbleFormatting(ScreenAspectRatio.AspectRatio, TextAnchor.LowerLeft);
                LeftTypingBubble = typingUI;
            }

            typingUI.SetBubbleColor(MainCharacterName, ui.CharacterName);
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
        GetCurrentTypingBubble(MainCharacterName);

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
        if (speakerName.Equals(MainCharacterName))
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
