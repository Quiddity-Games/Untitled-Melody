using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
/// <summary>
/// Used as a dictionary to keep track of character icons, using character name as a means of tracking.
/// </summary>
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
