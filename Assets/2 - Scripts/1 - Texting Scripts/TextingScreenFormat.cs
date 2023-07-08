using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;
using UnityEngine;

public class TextingScreenFormat : ScriptableObject
{
    [FormerlySerializedAs("TextingFormatting")]
    public List<TextingAspectRatioFormat> TextingFormatList = new List<TextingAspectRatioFormat>();
}

/// <summary>
/// An inspector-friendly and editable class used to reference and set values for each screen size.
/// </summary>
[Serializable]
public class TextingAspectRatioFormat
{
    enum ScreenSize { Landscape, Portrait }

    public Vector2 AspectRatio;
    [SerializeField] ScreenSize screenSize;
    [Space(5)]
    [Header("Text Bubbles")]
    [FormerlySerializedAs("LeftOffset")] public int BubbleEdgePadding;
    [FormerlySerializedAs("RightOffset")] public int IconEdgePadding;
    [Space(5)]
    public float IconSize;
    public int SenderFontSize;
    public int MessageFontSize;
    public int LayoutSpacing;
    [Space(5)]
    [Header("Phone Container")]
    public Vector2 PhoneContainerOffsetMin;
    public Vector2 PhoneContainerOffsetMax;
    [Space(5)]
    [Header("Autoskip Menu")]
    public float AutoplayMenuPivotX;
    public float AutoplayMenuAnchorX;
}
