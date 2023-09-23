using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// Used to create the text bubble and set all of the object's values from the Ink flow.
/// </summary>

public class TextBubbleUI : MonoBehaviour
{
    [SerializeField] LayoutElement iconLayout;
    public Image IconImage;
    public Image BubbleImage;
    [Header("Text Objects")]
    public TextMeshProUGUI SenderNameText;
    public TextMeshProUGUI MessageText;
    [Header("Layout Groups")]
    public LayoutGroup BubbleLayoutGroup;
    public LayoutGroup MessageLayoutGroup;
    public CanvasGroup CanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup.alpha = 0f;
    }

#if UNITY_EDITOR
    [Space(10)]
    [Tooltip("Only for inspector use.")]
    [SerializeField] BubbleAlignment currentAlignment;
    [SerializeField] bool previewAlignment;
    private void OnValidate()
    {
        if (previewAlignment)
        {
            switch (currentAlignment)
            {
                case BubbleAlignment.Left:
                    SetBubbleAlignment(TextAnchor.LowerLeft);
                    break;
                case BubbleAlignment.Right:
                    SetBubbleAlignment(TextAnchor.LowerRight);
                    break;
            }

            previewAlignment = false;
        }
    }
#endif

    void GetBubbleFormatting(TextingAspectRatioFormat aspect, TextAnchor textAnchor)
    {
        // Set padding on left/right
        if (textAnchor == TextAnchor.LowerLeft || textAnchor == TextAnchor.UpperLeft)
        {
            BubbleLayoutGroup.padding.left = aspect.IconEdgePadding;
            BubbleLayoutGroup.padding.right = aspect.BubbleEdgePadding;
        } else if (textAnchor == TextAnchor.LowerRight || textAnchor == TextAnchor.UpperRight)
        {
            BubbleLayoutGroup.padding.right = aspect.IconEdgePadding;
            BubbleLayoutGroup.padding.left = aspect.BubbleEdgePadding;
        }

        // Set icon size
        iconLayout.minWidth = aspect.IconSize;
        iconLayout.minHeight = aspect.IconSize;
        iconLayout.preferredWidth = aspect.IconSize;
        iconLayout.preferredHeight = aspect.IconSize;

        // Set font size
        SenderNameText.fontSize = aspect.SenderFontSize;
        MessageText.fontSize = aspect.MessageFontSize;
        (BubbleLayoutGroup as HorizontalLayoutGroup).spacing = aspect.LayoutSpacing;
    }

    /// <summary>
    /// Sets the alignment of the bubble.
    /// </summary>
    /// <param name="textAnchor"></param>
    public void SetBubbleAlignment(TextAnchor textAnchor)
    {
        gameObject.SetActive(false);
        BubbleLayoutGroup.childAlignment = textAnchor;
        MessageLayoutGroup.childAlignment = textAnchor;

        if (textAnchor == TextAnchor.LowerLeft || textAnchor == TextAnchor.UpperLeft)
        {
            SenderNameText.alignment = TextAlignmentOptions.MidlineLeft;
            (BubbleLayoutGroup as HorizontalLayoutGroup).reverseArrangement = false;
        }
        else if (textAnchor == TextAnchor.LowerRight || textAnchor == TextAnchor.UpperRight)
        {
            (BubbleLayoutGroup as HorizontalLayoutGroup).reverseArrangement = true;
            SenderNameText.alignment = TextAlignmentOptions.MidlineRight;
        }

        GetBubbleFormatting(ScreenAspectRatio.AspectRatio, textAnchor);
    }

    /// <summary>
    /// Parse information for the text bubble and set the variables.
    /// </summary>
    /// <param name="currentLine"></param>
    public void SetTextBubbleInformation(string currentLine, string mainCharacterName, string speakerName)
    {
        CharacterDialogueInfo senderUI = DialogueController.Instance.CharactersDictionary[speakerName];

        currentLine = DialogueController.Instance.RemoveTags(DialogueController.Instance.ParseEmojis(currentLine));

        // Set bubble alignment (left or right).
        if (speakerName.Contains(mainCharacterName))
            SetBubbleAlignment(TextAnchor.LowerRight);
        else
            SetBubbleAlignment(TextAnchor.LowerLeft);

        IconImage.sprite = senderUI.IconSprite;
        SenderNameText.text = speakerName;
        SenderNameText.color = senderUI.FontColor;

        MessageText.text = currentLine;
        MessageText.color = senderUI.FontColor;

        BubbleImage.color = senderUI.TextBoxColor;
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

[Serializable]
public class CharacterUIElements
{
    public string CharacterName;
    public Sprite IconSprite;
    public Color TextBoxColor;
    public Color FontColor;

    public CharacterUIElements(string name, Sprite sprite, Color boxColor, Color fontColor)
    {
        CharacterName = name;
        IconSprite = sprite;
        TextBoxColor = boxColor;
        FontColor = fontColor;
    }
}
