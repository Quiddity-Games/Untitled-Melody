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

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance;

    public static VoidCallback InitializeDialogue;
    public static VoidCallback OnDialogueStart;
    public static VoidCallback OnContinue;
    public static VoidCallback OnLineShown;
    public static VoidCallback SubscribeButtonEvents;

    public Action<string> OnLoadNextChunk; // Does something when the next chunk is parsed (i.e. make text bubbles)

    public GameEvent OnDialogueEnd;

    [Header("Ink & Characters")]
    public Story InkStory;
    [SerializeField] TextAsset inkTextAsset;
    [Space(10)]
    [SerializeField] StringVariable mainCharacterName;
    [HideInInspector] public string MainCharacterName;
    public List<CharacterUIInfo> CharactersInStory = new();
    public List<string> LinesBeforeChoice = new();

    [Header("Delay Durations")]
    public float AutoplayDelayDuration;

    public Dictionary<string, string> GlobalTagsDictionary = new();
    public Dictionary<string, CharacterDialogueInfo> CharactersDictionary = new();

    public int CurrentLineIndex;
    public int LastLineIndex;
    [HideInInspector] public bool LastChunkLoaded;

    public bool CanPrintDialogue;
    public bool AutoplayEnabled;

    [Header("Playtest Settings")]
    public BoolVariable PlayDialogueOnStart;

    public virtual void Awake()
    {
        DOTween.Init();
        Instance = this;
        InkStory = new Story(inkTextAsset.text);
        CurrentLineIndex = 0;
        MainCharacterName = mainCharacterName.Value;
        InitializeCharacterDictionary();

        InitializeDialogue += GetLinesBeforeChoice;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        SubscribeButtonEvents?.Invoke();
        InitializeDialogue?.Invoke();
    }

    private void InitializeCharacterDictionary()
    {
        // Create dictionary of character UI elements.
        for (int i = 0; i < CharactersInStory.Count; i++)
        {
            CharactersDictionary.Add(CharactersInStory[i].CharacterName, CharactersInStory[i].Info);
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
                if (!tag.ToLower().Contains("end"))
                {
                    parsedLine += ("#" + tag + "\n");
                }
                else
                {
                    LastChunkLoaded = true;
                }
            }
            parsedLine += currentLine;

            if (!string.IsNullOrEmpty(parsedLine))
            {
                LinesBeforeChoice.Add(parsedLine);
                OnLoadNextChunk?.Invoke(parsedLine); // If action has subscriptions, invoke. Create text bubbles.
            }
        }

        LastLineIndex = LinesBeforeChoice.Count - 1; // Get the index of the last line.
    }

    /// <summary>
    /// Get the speaker of a given line.
    /// </summary>
    /// <param name="currentLine"></param>
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

    public string ParseEmojis(string currentLine)
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

    /// <summary>
    /// Remove the tags from the parsed line.
    /// </summary>
    /// <param name="currentLine"></param>
    /// <returns></returns>
    public string RemoveTags(string currentLine)
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying && inkTextAsset && CharactersInStory.Count < 1)
        {
            List<string> names = new();

            string pattern = @"(?:[Ss]peaker\:.)(\w+)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(inkTextAsset.text);

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

                if (!CharactersInStory.Contains(info))
                    CharactersInStory.Add(info);
            }
        }
    }
#endif
}
