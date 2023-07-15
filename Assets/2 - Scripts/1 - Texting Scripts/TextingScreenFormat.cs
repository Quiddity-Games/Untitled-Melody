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
public struct TextingAspectRatioFormat
{
    enum ScreenSize { Landscape, Portrait }

    [HideInInspector] [SerializeField] string aspectRatioName;
    public Vector2 AspectRatio;
    [SerializeField] ScreenSize screenSize;
    public Vector2 BackgroundOffsetMax;
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

    public TextingAspectRatioFormat (Vector2 aspectRatio, Vector2 backgroundOffset, int bubblePadding, int iconPadding, float iconSize, int senderSize,
        int messageSize, int spacing, Vector2 phoneOffsetMin, Vector2 phoneOffsetMax, float autoplayPivot, float autoplayAnchor)
    {
        aspectRatioName = "";
        AspectRatio = aspectRatio;
        screenSize = ScreenSize.Landscape;
        BackgroundOffsetMax = backgroundOffset;
        BubbleEdgePadding = bubblePadding;
        IconEdgePadding = iconPadding;
        IconSize = iconSize;
        SenderFontSize = senderSize;
        MessageFontSize = messageSize;
        LayoutSpacing = spacing;
        PhoneContainerOffsetMin = phoneOffsetMin;
        PhoneContainerOffsetMax = phoneOffsetMax;
        AutoplayMenuPivotX = autoplayPivot;
        AutoplayMenuAnchorX = autoplayAnchor;

        GetInspectorInformation(aspectRatio);
    }

    public void GetInspectorInformation(Vector2 aspectRatio)
    {
        aspectRatioName = aspectRatio.x + ":" + aspectRatio.y;
        screenSize = aspectRatio.x > aspectRatio.y ? ScreenSize.Landscape : ScreenSize.Portrait;
    }
}
