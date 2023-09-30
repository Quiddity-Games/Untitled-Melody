using UnityEngine;
using System;

[CreateAssetMenu]
public class CharacterDialogueInfo : ScriptableObject
{
    public Sprite IconSprite;
    public Color TextBoxColor;
    public Color FontColor;
}

[Serializable]
public class CharacterUIInfo
{
    public string CharacterName;
    public CharacterDialogueInfo Info;

    public CharacterUIInfo (string name, CharacterDialogueInfo info)
    {
        CharacterName = name;
        Info = info;
    }
}