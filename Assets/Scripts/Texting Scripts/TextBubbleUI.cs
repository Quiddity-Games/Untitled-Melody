using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using System.Text.RegularExpressions;

public enum BubbleAlignment { Left, Right }

public class TextBubbleUI : MonoBehaviour
{
    public Image IconSprite;
    [Header("Text Objects")]
    public TextMeshProUGUI SenderNameText;
    public TextMeshProUGUI MessageText;
    [Header("Layout Groups")]
    public HorizontalLayoutGroup BubbleLayoutGroup;
    public VerticalLayoutGroup MessageLayoutGroup;
    public CanvasGroup CanvasGroup;

    [Space(10)]
    [Tooltip("Only for inspector use.")]
    [SerializeField] BubbleAlignment previewAlignment;

    // Start is called before the first frame update
    void Start()
    {
        if (!BubbleLayoutGroup)
            BubbleLayoutGroup = GetComponent<HorizontalLayoutGroup>();

        CanvasGroup.alpha = 0f;
    }

    private void OnValidate()
    {
        switch (previewAlignment)
        {
            case BubbleAlignment.Left:
                SetBubbleAlignment(TextAnchor.LowerLeft);
                break;
            case BubbleAlignment.Right:
                SetBubbleAlignment(TextAnchor.LowerRight);
                break;
            default:
                SetBubbleAlignment(TextAnchor.LowerLeft);
                break;
        }
    }

    /// <summary>
    /// Sets the alignment of the bubble.
    /// </summary>
    /// <param name="textAnchor"></param>
    public void SetBubbleAlignment(TextAnchor textAnchor)
    {
        BubbleLayoutGroup.childAlignment = textAnchor;
        MessageLayoutGroup.childAlignment = textAnchor;

        if (textAnchor == TextAnchor.LowerLeft || textAnchor == TextAnchor.UpperLeft)
            BubbleLayoutGroup.reverseArrangement = false;
        else if (textAnchor == TextAnchor.LowerRight || textAnchor == TextAnchor.UpperRight)
            BubbleLayoutGroup.reverseArrangement = true;
    }

    /// <summary>
    /// Parse information for the text bubble and set the variables.
    /// </summary>
    /// <param name="currentLine"></param>
    public void SetTextBubbleInformation(string currentLine, string speakerName, string mainCharacterName, Sprite senderIcon)
    {
        currentLine = RemoveTags(ParseEmojis(currentLine));

        // Set bubble alignment (left or right).
        if (speakerName.Contains(mainCharacterName))
            SetBubbleAlignment(TextAnchor.LowerRight);
        else
            SetBubbleAlignment(TextAnchor.LowerLeft);

        SenderNameText.text = speakerName;
        MessageText.text = currentLine;
        IconSprite.sprite = senderIcon;
    }

    /// <summary>
    /// Get the speaker of a given line.
    /// </summary>
    /// <param name="currentLine"></param>
    /// <param name="globalTags"></param>
    /// <returns></returns>
    public string ParseSpeaker(string currentLine, Dictionary<string, string> globalTags)
    {
        string speakerName = "";

        // Parse speaker name. Searches for "#Speaker: X" tag.
        if (Regex.IsMatch(currentLine, @"\#[Ss]peaker\:.*"))
        {
            Regex speakerRx = new Regex(@"[Ss]peaker\:\s(\w+)");

            // If there is a "#Speaker: X" tag, use the alias instead.
            if (globalTags.ContainsKey("Alias") && !string.IsNullOrEmpty(globalTags["Alias"]))
            {
                speakerName = globalTags["Alias"];
            }
            else
            {
                MatchCollection speakerMatch = speakerRx.Matches(currentLine);
                foreach (Match m in speakerMatch)
                {
                    speakerName = m.Groups[1].Value;
                }
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
