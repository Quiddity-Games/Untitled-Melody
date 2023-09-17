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

/// <summary>
/// Handles the resizing of UI for the Dreamworld canvas.
/// </summary>

public class DreamworldDialogueController : MonoBehaviour
{
    public static DreamworldDialogueController Instance;

    [SerializeField] Story InkStory;
    [SerializeField] TextAsset inkTextAsset;
    [Space(10)]
    [SerializeField] string mainCharacterName;
    [SerializeField] bool updateCharacterList;
    [SerializeField] List<CharacterUIInfo> charactersInStory = new();

    [Space(15)]
    [SerializeField] Button continueButton;
    [SerializeField] Image leftPortrait;
    [SerializeField] Image rightPortrait;
    [SerializeField] CanvasGroup ellipsesContainer;

    [Space(10)]
    [SerializeField] RectTransform textContainerTransform;
    [SerializeField] Vector2 textContainerBaseSize;
    [SerializeField] Vector2 textContainerTypingSize;
    [SerializeField] Image textBoxImage;
    [SerializeField] TextMeshProUGUI senderNameText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] List<string> linesBeforeChoice = new();
    private int currentLineIndex;
    private Dictionary<string, CharacterDialogueInfo> charactersInStoryDictionary = new();

    private CanvasGroup leftPortraitCanvasGroup;
    private CanvasGroup rightPortraitCanvasGroup;

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
        currentLineIndex = 0;
        GetLinesBeforeChoice();

        continueButton.onClick.AddListener(()=> PlayDialogue(1.2f));

        leftPortrait.sprite = charactersInStoryDictionary["Memi"].IconSprite;
        leftPortraitCanvasGroup = leftPortrait.gameObject.GetComponent<CanvasGroup>();

        rightPortrait.sprite = charactersInStoryDictionary["Amika"].IconSprite;
        rightPortraitCanvasGroup = rightPortrait.gameObject.GetComponent<CanvasGroup>();
    }

    void GetDictionaryValues()
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
    }

    void PlayDialogue(float typingDuration)
    {
        if (currentLineIndex >= linesBeforeChoice.Count)
        {
            currentLineIndex = 0;
            return;
        }
        else
            StartCoroutine(PlayDialogue(typingDuration));

        IEnumerator PlayDialogue(float typingDuration)
        {
            string speakerName = ParseSpeaker(linesBeforeChoice[currentLineIndex]);

            float time = 0f;
            ResizeTextBox(speakerName, TextBoxState.Typing);

            while (time < typingDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            messageText.DOFade(0f, 0f);
            SetTextBoxUI(linesBeforeChoice[currentLineIndex], speakerName);
            ResizeTextBox(speakerName, TextBoxState.Message);
            currentLineIndex++;
        }
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

    private void ResizeTextBox(string speakerName, TextBoxState state)
    {
        switch (state)
        {
            case TextBoxState.Message:
                DOTween.Sequence().Join(ellipsesContainer.DOFade(0f, 0.15f)).
                    InsertCallback(0.15f, () => ellipsesContainer.gameObject.SetActive(false)).
                    Append(textContainerTransform.DOSizeDelta(textContainerBaseSize, 0.3f)).
                    InsertCallback(0.3f, () => messageText.gameObject.SetActive(true)).
                    Append(messageText.DOFade(1f, 0.15f));
                break;
            case TextBoxState.Typing:
                CanvasGroup activePortrait = null;
                CanvasGroup inactivePortrait = null;

                if (speakerName.Contains(mainCharacterName))
                {
                    activePortrait = leftPortraitCanvasGroup;
                    inactivePortrait = rightPortraitCanvasGroup;
                }
                else
                {
                    activePortrait = rightPortraitCanvasGroup;
                    inactivePortrait = leftPortraitCanvasGroup;
                }
                
                ellipsesContainer.DOFade(0f, 0f);

                DOTween.Sequence().Join(messageText.DOFade(0f, 0.15f)).
                    Join(activePortrait.DOFade(0f, 0.15f)).
                    InsertCallback(0.15f, () => messageText.gameObject.SetActive(false)).
                    Append(textContainerTransform.DOSizeDelta(textContainerTypingSize, 0.3f)).
                    Join(inactivePortrait.DOFade(1f, 0.15f)).
                    InsertCallback(0.15f, () => SetTextBoxUI(linesBeforeChoice[currentLineIndex], ParseSpeaker(linesBeforeChoice[currentLineIndex]))).
                    InsertCallback(0.3f, () => ellipsesContainer.gameObject.SetActive(true)).
                    Append(ellipsesContainer.DOFade(1f, 0.2f));
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
