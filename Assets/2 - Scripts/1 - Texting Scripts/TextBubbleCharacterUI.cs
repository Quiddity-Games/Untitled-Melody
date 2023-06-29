using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Used as a dictionary to keep track of character icons, using character name as a means of tracking.
/// Dictionary is created on Awake by the <see cref="DialogueController"/>. Must be attached to the GameObject holding the Dialogue Controller.
/// </summary>

[Serializable]
[RequireComponent(typeof(DialogueController))]
public class TextBubbleUIElements
{
    public string CharacterName;
    public Sprite IconSprite;
    public Color TextBoxColor;
    public Color FontColor;
}

[Serializable]
public class TextBubbleCharacterUI : MonoBehaviour
{
    public static TextBubbleCharacterUI Instance;
    public string MainCharacterName;
    public List<TextBubbleUIElements> CharacterUIElements;

    private void Awake()
    {
        Instance = this;
    }
}
