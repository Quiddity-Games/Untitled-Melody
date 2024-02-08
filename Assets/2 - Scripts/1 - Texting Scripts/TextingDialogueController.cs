using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using DG.Tweening;

/// <summary>
/// Dialogue controller which the active <see cref="TextingDialogueCanvas"/> derives its values and static methods from.
/// On Awake, it creates itself an instance and gathers the dictionary values for global tags and character UI elements.
/// </summary>

public enum BubbleAlignment { Left, Right }

public class TextingDialogueController : DialogueController
{
    public static TextingDialogueController TextingUI;

    #region Variables: Canvas
    [Header("Scene Components")]
    [SerializeField] TextingDialogueCanvas dialogueCanvas;
    [SerializeField] AutoplaySkipUI autoplaySkipUI;
    #endregion

    #region Variables: Typing Bubbles
    [Header("Phone UI: Typing Bubbles")]
    public float BubbleFadeDuration;
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
    [HideInInspector] public List<TextBubbleUI> BubblesBeforeChoice = new();
    [HideInInspector] public List<TextOptionUI> CurrentOptions = new();
    #endregion

    public override void Awake()
    {
        base.Awake();
        TextingUI = this;
    }

    private void OnEnable()
    {
        OnLoadNextChunk += CreateTextBubble;
        InitializeDialogue += SelectPlatform;
        InitializeDialogue += GetConversationTags;
        InitializeDialogue += CreateTextTypingBubbles;
    }

    private void OnDestroy()
    {
        OnLoadNextChunk -= CreateTextBubble;
        InitializeDialogue -= SelectPlatform;
        InitializeDialogue -= GetConversationTags;
        InitializeDialogue -= CreateTextTypingBubbles;
    }

    /// <summary>
    /// Check the platform and set the correct UI as current.
    /// Can be used in-editor by using the <see cref="Platform"/> dropdown in the inspector.
    /// Called on <see cref="Awake"/>.
    /// </summary>
    public void SelectPlatform()
    {
        dialogueCanvas.ResizeCanvasForPlatform(ScreenAspectRatio.AspectRatio);
    }

    /// <summary>
    /// Create dictionary of <see cref="CharacterUIElements"/> items.
    /// Called on <see cref="Awake"/>.
    /// </summary>
    private void GetConversationTags()
    {
        // Get global tags.
        for (int i = 0; i < InkStory.globalTags.Count; i++)
        {
            if (InkStory.globalTags[i].Contains("Conversation: "))
            {
                GlobalTagsDictionary.Add("Conversation", InkStory.globalTags[i].Replace("Conversation: ", ""));
            }
        }
    }

    /// <summary>
    /// Create a text bubble using the parsed text.
    /// </summary>
    /// <param name="line"></param>
    private void CreateTextBubble(string line)
    {
        TextBubbleUI textBubble = Instantiate(TextBubblePrefab, dialogueCanvas.BodyScrollContent.transform).GetComponent<TextBubbleUI>();
        BubblesBeforeChoice.Add(textBubble);

        string speakerName = ParseSpeaker(line);
        textBubble.CanvasGroup.DOFade(1f, BubbleFadeDuration);
        //FadeInUI(textBubble.CanvasGroup, BubbleFadeDuration);

        textBubble.SetTextBubbleInformation(line, MainCharacterName, speakerName);
    }

    /// <summary>
    /// Create the typing bubbles with values from <see cref="CharacterUIElements"/> and hide them in the inspector.
    /// Called on <see cref="Start"/>, simultaneously with <see cref="GetLinesBeforeChoice"/>.
    /// </summary>
    public void CreateTextTypingBubbles()
    {
        foreach (CharacterUIInfo ui in CharactersInStory)
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
    /// Called by <see cref="TextingDialogueCanvas.DisplayChoices"/>.
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
        {
            dialogueCanvas.autoplayText.text = "Autoplay\n(ON)";
            dialogueCanvas.ContinueDialogueButton.interactable = false;
        }
        else
        {
            dialogueCanvas.autoplayText.text = "Autoplay\n(OFF)";
            dialogueCanvas.ContinueDialogueButton.interactable = true;
        }

        return AutoplayEnabled;
    }

    /// <summary>
    /// Used to skip directly to the next choice. Shows all bubbles and then <see cref="DisplayChoices"/>.
    /// </summary>
    public void SkipToChoice()
    {
        List<TextBubbleUI> bubblesBeforeChoice = BubblesBeforeChoice;
        Story inkStory = InkStory;

        if (bubblesBeforeChoice.Count > 0)
        {
            for (int i = 0; i < bubblesBeforeChoice.Count; i++)
            {
                if (!bubblesBeforeChoice[i].gameObject.activeInHierarchy)
                {
                    bubblesBeforeChoice[i].gameObject.SetActive(true);
                    bubblesBeforeChoice[i].CanvasGroup.DOFade(1f, BubbleFadeDuration);
                    //FadeInUI(bubblesBeforeChoice[i].CanvasGroup, BubbleFadeDuration);
                }
            }
        }

        if (inkStory.currentChoices.Count > 0)
            dialogueCanvas.DisplayChoices();
    }

    /// <summary>
    /// Gets the current speaker's bubble to correctly show the typing animation.
    /// Called by <see cref="TextingDialogueCanvas.PlayDialogue"/>.
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

}
