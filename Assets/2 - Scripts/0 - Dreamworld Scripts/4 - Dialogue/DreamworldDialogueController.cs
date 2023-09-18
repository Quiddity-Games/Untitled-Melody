using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Ink.Runtime;
using System.Text.RegularExpressions;
using System.Linq;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the resizing of UI for the Dreamworld canvas.
/// </summary>

public class DreamworldDialogueController : MonoBehaviour
{
    public static DreamworldDialogueController Instance;

    [SerializeField] GameEvent onDialogueEnd;

    [SerializeField] Story InkStory;
    [SerializeField] TextAsset inkTextAsset;
    [Space(10)]
    [SerializeField] string mainCharacterName;
    [SerializeField] bool updateCharacterList;
    [SerializeField] List<CharacterUIInfo> charactersInStory = new();

    [Space(15)]
    [SerializeField] Button previousButton;
    [SerializeField] Button autoplayOnButton;
    [SerializeField] Button autoplayOffButton;
    [SerializeField] Button skipButton;
    [SerializeField] Button continueButton;

    [Space(10)]
    [SerializeField] RectTransform finishButtonTransform;
    [SerializeField] RectTransform finishButtonMidSection;
    [SerializeField] Button finishButton;

    [Space(15)]
    [SerializeField] Vector2 typingPosition; // Negative X axis for left align. Set to positive in inspector.
    [SerializeField] Image leftPortrait;
    [SerializeField] Image rightPortrait;
    [SerializeField] CanvasGroup ellipsesContainer;

    [Space(10)]
    [SerializeField] CanvasGroup contentCanvasGroup;
    [SerializeField] RectTransform textContainerTransform;
    [SerializeField] Vector2 textContainerBaseSize;
    [SerializeField] Vector2 textContainerTypingSize;
    [SerializeField] Image textBoxImage;
    [SerializeField] TextMeshProUGUI senderNameText;
    [SerializeField] TextMeshProUGUI messageText;

    private List<string> linesBeforeChoice = new();
    private int currentLineIndex;
    private int mostRecentLineIndex;
    private int lastLineIndex;
    private Dictionary<string, CharacterDialogueInfo> charactersInStoryDictionary = new();

    private Vector2 finishButtonOriginalPosition;
    private Vector2 textContainerOriginalPosition;
    private RectTransform leftPortraitRect;
    private RectTransform rightPortraitRect;
    private CanvasGroup leftPortraitCanvasGroup;
    private CanvasGroup rightPortraitCanvasGroup;

    private bool canPrintDialogue;
    private bool autoplayEnabled;

    private PlayerControl _playerControl;

    private enum TextBoxState { Message, Typing }

    private void Awake()
    {
        Instance = this;
        InkStory = new Story(inkTextAsset.text);
        GetDictionaryValues();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (updateCharacterList && inkTextAsset)
        {
            updateCharacterList = false;
            charactersInStory.Clear();

            List<string> names = new();

            string pattern = @"(?:[Ss]peaker\:.)(\w+)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(inkTextAsset.text);

            foreach(Match match in m)
            {
                if (!names.Contains(match.Groups[1].Value))
                    names.Add(match.Groups[1].Value);
            }

            foreach(string name in names)
            {
                CharacterDialogueInfo asset = (CharacterDialogueInfo)AssetDatabase.LoadAssetAtPath(
                    "Assets/2 - Scripts/1 - Texting Scripts/0 - Character UI Info/" + name + " Dialogue Info.asset",
                    typeof(CharacterDialogueInfo));

                CharacterUIInfo info = new(name, asset);

                if (!charactersInStory.Contains(info))
                    charactersInStory.Add(info);
            }
        }
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        currentLineIndex = -1;
        mostRecentLineIndex = -1;
        contentCanvasGroup.alpha = 0f;

        foreach (CharacterUIInfo info in charactersInStory)
        {
            if (info.CharacterName != mainCharacterName)
            {
                InitializePortraits(info.CharacterName);
                break;
            }
        }

        finishButtonOriginalPosition = finishButtonTransform.anchoredPosition;
        finishButton.gameObject.SetActive(false);
        finishButtonTransform.anchoredPosition = new Vector2(620f, finishButtonTransform.anchoredPosition.y);
        finishButtonMidSection.sizeDelta = new Vector2(0f, finishButtonMidSection.sizeDelta.y);

        GetLinesBeforeChoice();
        SetButtonEvents();
        OnLineShown();

        string speakerName = ParseSpeaker(linesBeforeChoice[0]);
        SetTextBoxUI(linesBeforeChoice[0], speakerName);
        ResizeTextBox(speakerName, true, TextBoxState.Typing);

        _playerControl = new();
        _playerControl.Dreamworld.Dash.performed += StartDialogue;
        _playerControl.Enable();
    }

    private void SetButtonEvents()
    {
        continueButton.onClick.AddListener(() => PlayDialogue(true));
        previousButton.onClick.AddListener(() => PlayDialogue(false));
        autoplayOnButton.onClick.AddListener(() => ToggleAutoplay(true));
        autoplayOffButton.onClick.AddListener(() => ToggleAutoplay(false));
        skipButton.onClick.AddListener(() => StartCoroutine(SkipDialogue()));
        finishButton.onClick.AddListener(() => PlayDialogue(true));
        
        previousButton.interactable = false;
        continueButton.interactable = false;
        autoplayOnButton.interactable = false;
        autoplayOffButton.interactable = false;
        skipButton.interactable = false;

        autoplayOnButton.gameObject.SetActive(true);
        autoplayOffButton.gameObject.SetActive(false);

        autoplayEnabled = false;
    }

    private void InitializePortraits(string conversationPartner)
    {
        leftPortrait.sprite = charactersInStoryDictionary[conversationPartner].IconSprite;
        leftPortraitCanvasGroup = leftPortrait.gameObject.GetComponent<CanvasGroup>();
        leftPortraitRect = leftPortrait.gameObject.GetComponent<RectTransform>();

        rightPortrait.sprite = charactersInStoryDictionary[mainCharacterName].IconSprite;
        rightPortraitCanvasGroup = rightPortrait.gameObject.GetComponent<CanvasGroup>();
        rightPortraitRect = rightPortrait.gameObject.GetComponent<RectTransform>();

        textContainerOriginalPosition = new Vector2(textContainerTransform.anchoredPosition.x, textContainerTransform.anchoredPosition.y);
        textContainerTransform.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void GetDictionaryValues()
    {
        // Get character UI elements.
        for (int i = 0; i < charactersInStory.Count; i++)
        {
            charactersInStoryDictionary.Add(charactersInStory[i].CharacterName, charactersInStory[i].Info);
        }
    }

    /// <summary>
    /// Get a list of strings as all lines before choices, including tags.
    /// Called on <see cref="Start"/>.
    /// </summary>
    private void GetLinesBeforeChoice()
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
                linesBeforeChoice.Add(parsedLine);
            }
        }

        lastLineIndex = linesBeforeChoice.Count - 1;
    }

    private void StartDialogue(InputAction.CallbackContext ctx)
    {
        _playerControl.Dreamworld.Dash.performed -= StartDialogue;
        DOTween.Sequence().Join(contentCanvasGroup.DOFade(1f, 0.5f)).
            Insert(1f, textContainerTransform.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.15f)).
            InsertCallback(0.85f, () => PlayDialogue(true));
    }

    private void PlayDialogue(bool forward)
    {
        if (forward && currentLineIndex >= lastLineIndex)
        {
            finishButton.gameObject.SetActive(false);
            DOTween.Sequence().Join(contentCanvasGroup.DOFade(0f, 1.5f)).InsertCallback(3f, () => onDialogueEnd.Raise());
            return;
        }

        if (forward)
            currentLineIndex++;
        else
        {
            if (currentLineIndex - 1 < 0)
                currentLineIndex = 0;
            else
                currentLineIndex--;
        }

        StartCoroutine(PlayDialogue(1.75f));

        IEnumerator PlayDialogue(float typingDuration)
        {
            if (currentLineIndex > mostRecentLineIndex)
                mostRecentLineIndex = currentLineIndex;

            canPrintDialogue = false;
            string speakerName = ParseSpeaker(linesBeforeChoice[currentLineIndex]);
            ResizeTextBox(speakerName, forward && mostRecentLineIndex <= currentLineIndex ? false : true, TextBoxState.Typing);

            if (forward && mostRecentLineIndex <= currentLineIndex)
            {
                float time = 0f;
                ResizeTextBox(speakerName, false, TextBoxState.Typing);
                SetButtonsInteractable(false);

                while (time < typingDuration)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
            }

            messageText.DOFade(0f, 0f);
            ResizeTextBox(speakerName, forward && mostRecentLineIndex <= currentLineIndex ? false : true, TextBoxState.Message);
            OnLineShown();
        }
    }

    IEnumerator SkipDialogue()
    {
        mostRecentLineIndex = lastLineIndex;

        SetButtonsInteractable(false);
        autoplayOnButton.interactable = false;

        if (autoplayEnabled)
        {
            StopCoroutine(StartAutoplay());
            ToggleAutoplay(false);
        }

        while (currentLineIndex < lastLineIndex)
        {
            PlayDialogue(true);
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }

        SetButtonsInteractable(true);
        yield break;
    }

    private void ToggleAutoplay(bool on)
    {
        autoplayEnabled = on;

        if (on)
        {
            autoplayOnButton.gameObject.SetActive(false);
            autoplayOffButton.gameObject.SetActive(true);
            StartCoroutine(StartAutoplay());
        } else
        {
            autoplayOnButton.gameObject.SetActive(true);
            autoplayOffButton.gameObject.SetActive(false);
        }
    }

    IEnumerator StartAutoplay()
    {
        while (currentLineIndex < lastLineIndex && autoplayEnabled)
        {
            while (!canPrintDialogue)
            {
                yield return null;
            }

            PlayDialogue(true);
            yield return new WaitForSeconds(1.75f + 1.15f + 2f); // Duration of typing animation + duration of message animation + delay of 2s for reading.
            yield return null;
        }

        yield break;
    }

    private void SetButtonsInteractable(bool on)
    {
        if (currentLineIndex <= 0)
            previousButton.interactable = false;
        else
            previousButton.interactable = on;

        previousButton.interactable = on;
        continueButton.interactable = on;

        if (mostRecentLineIndex >= lastLineIndex)
        {
            autoplayOnButton.interactable = false;
            autoplayOffButton.interactable = false;
            skipButton.interactable = false;

            autoplayOnButton.gameObject.SetActive(true);
            autoplayOffButton.gameObject.SetActive(false);

            autoplayEnabled = false;
        } else
        {
            autoplayOnButton.interactable = true;
            autoplayOffButton.interactable = true;
            skipButton.interactable = true;
        }
    }

    private void OnLineShown()
    {
        canPrintDialogue = true;
        SetButtonsInteractable(true);

        if (currentLineIndex >= lastLineIndex)
        {
            finishButton.gameObject.SetActive(true);
            DOTween.Sequence().
                Insert(0.1f, finishButtonMidSection.DOSizeDelta(new Vector2(175f, finishButtonMidSection.sizeDelta.y), 0.15f)).
                Join(finishButtonTransform.DOAnchorPosX(finishButtonOriginalPosition.x, 0.25f));
        } else if (finishButton.gameObject.activeSelf)
        {
            DOTween.Sequence().
                Join(finishButtonMidSection.DOSizeDelta(new Vector2(0f, finishButtonMidSection.sizeDelta.y), 0.15f)).
                Join(finishButtonTransform.DOAnchorPosX(620f, 0.15f)).
                AppendCallback(() => finishButton.gameObject.SetActive(false));
        }

        if (mostRecentLineIndex == lastLineIndex)
            mostRecentLineIndex = linesBeforeChoice.Count;
    }

    private void SetTextBoxUI(string currentLine, string speakerName)
    {
        currentLine = RemoveTags(ParseEmojis(currentLine));

        senderNameText.text = speakerName;
        senderNameText.color = charactersInStoryDictionary[speakerName].FontColor;

        messageText.text = currentLine;
        messageText.color = charactersInStoryDictionary[speakerName].FontColor;

        textBoxImage.color = charactersInStoryDictionary[speakerName].TextBoxColor;
    }

    private void ResizeTextBox(string speakerName, bool noDelay, TextBoxState state)
    {
        bool isRightAligned = false;

        if (speakerName.Contains(mainCharacterName))
            isRightAligned = true;

        float fadeDuration = noDelay ? 0f : 0.15f;
        float textBoxDuration = noDelay ? 0f : 0.3f;

        float insertDelay = fadeDuration + textBoxDuration;

        switch (state)
        {
            case TextBoxState.Message:
                DOTween.Sequence().
                    Join(ellipsesContainer.DOFade(0f, fadeDuration)).
                    Join(senderNameText.DOFade(0f, 0f));

                // Play sequence immediately after initial sequence; disable ellipses, fade name, fade and animate portraits, move/resize text box.
                DOTween.Sequence().
                    Insert(fadeDuration, senderNameText.DOFade(1f, fadeDuration)).
                    Insert(fadeDuration, isRightAligned ? rightPortraitCanvasGroup.DOFade(1f, fadeDuration) : leftPortraitCanvasGroup.DOFade(1f, fadeDuration)).
                    Join(isRightAligned ? rightPortraitRect.DOAnchorPosY(0f, fadeDuration) : leftPortraitRect.DOAnchorPosY(0f, fadeDuration)).
                    Insert(fadeDuration, textContainerTransform.DOAnchorPos(textContainerOriginalPosition, textBoxDuration)).
                    Insert(fadeDuration, textContainerTransform.DOSizeDelta(textContainerBaseSize, textBoxDuration)).
                    InsertCallback(fadeDuration, () => ellipsesContainer.gameObject.SetActive(false)).
                    InsertCallback(fadeDuration, () => senderNameText.gameObject.SetActive(true));

                // Play sequence after all other sequences completed; turns message gameObject on and fades in.
                DOTween.Sequence().
                    InsertCallback(insertDelay, () => messageText.gameObject.SetActive(true)).
                    Insert(insertDelay, messageText.DOFade(1f, fadeDuration));
                break;

            case TextBoxState.Typing:
                // Play sequence immediately; fades portraits and animates in Y axis, fades message text.
                DOTween.Sequence().
                    Join(ellipsesContainer.DOFade(0f, 0f)).
                    Join(senderNameText.DOFade(0f, 0.15f)).
                    Join(leftPortraitCanvasGroup.DOFade(0f, fadeDuration)).Join(leftPortraitRect.DOAnchorPosY(-30f, fadeDuration)).
                    Join(rightPortraitCanvasGroup.DOFade(0f, fadeDuration)).Join(rightPortraitRect.DOAnchorPosY(-30f, fadeDuration)).
                    Join(messageText.DOFade(0f, fadeDuration));

                // Play sequence immediately after initial sequence; fades sender name, change position/size of text box to typing, disable message gameObject.
                DOTween.Sequence().
                    Insert(fadeDuration, textContainerTransform.DOSizeDelta(textContainerTypingSize, textBoxDuration)).
                    Join(isRightAligned ? textContainerTransform.DOAnchorPosX(typingPosition.x, textBoxDuration) :
                        textContainerTransform.DOAnchorPosX(-typingPosition.x, textBoxDuration)).
                    InsertCallback(fadeDuration, () => senderNameText.gameObject.SetActive(false)).
                    InsertCallback(fadeDuration, () => messageText.gameObject.SetActive(false)).
                    InsertCallback(fadeDuration, () => SetTextBoxUI(linesBeforeChoice[currentLineIndex], speakerName)).
                    InsertCallback(fadeDuration, () => senderNameText.alignment = isRightAligned ? TextAlignmentOptions.Right : TextAlignmentOptions.Left);
                
                // Play sequence after all other sequences completed; turns ellipses gameObject on and fades them.
                DOTween.Sequence().
                    InsertCallback(insertDelay, () => ellipsesContainer.gameObject.SetActive(true)).
                    Insert(insertDelay, ellipsesContainer.DOFade(1f, fadeDuration));
                break;
        }
    }

    /// <summary>
    /// Get the speaker of a given line.
    /// </summary>
    /// <param name="currentLine"></param>
    /// <param name="globalTags"></param>
    /// <returns></returns>
    public string ParseSpeaker(string currentLine)
    {
        string speakerName = "";

        // Parse speaker name. Searches for "#Speaker: X" tag.
        if (Regex.IsMatch(currentLine, @"\#[Ss]peaker\:.*"))
        {
            Regex speakerRx = new Regex(@"[Ss]peaker\:\s(\w+)");
            MatchCollection speakerMatch = speakerRx.Matches(currentLine);
            foreach (Match m in speakerMatch)
            {
                speakerName = m.Groups[1].Value;
            }
        }

        return speakerName;
    }

    string ParseEmojis(string currentLine)
    {
        // Parse emoji names. Searching for "[emoji:x]". If it finds a match, it switches the text out for a sprite tag.
        if (Regex.IsMatch(currentLine, @"\[emoji\:.*\]", RegexOptions.IgnoreCase))
        {
            string emojiName = "";
            Regex emojiRx = new Regex(@"\[emoji\:(\w+)\]");
            MatchCollection emojiMatch = emojiRx.Matches(currentLine);
            foreach (Match m in emojiMatch)
            {
                emojiName = m.Groups[1].Value;
                currentLine = Regex.Replace(currentLine, @"\[emoji\:" + emojiName + @"\]", "<sprite name=" + emojiName + ">");
            }
        }

        return currentLine;
    }

    string RemoveTags(string currentLine)
    {
        // Remove tags from lines. Searches for "#TAGNAME: X".
        if (Regex.IsMatch(currentLine, @"\#\w+\:.*\w+[\r\n]", RegexOptions.IgnoreCase))
        {
            Regex lineRx = new Regex(@"(\#\w+\:\s\w+\.*[\r\n]+)");
            MatchCollection lineMatch = lineRx.Matches(currentLine);
            foreach (Match m in lineMatch)
            {
                currentLine = Regex.Replace(currentLine, @"(\#\w+\:\s\w+\.*[\r\n]+)", "");
            }
        }

        return currentLine;
    }
}
