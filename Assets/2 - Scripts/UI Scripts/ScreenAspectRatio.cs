using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenAspectRatio : MonoBehaviour
{
    public static Dictionary<string, TextingAspectRatioFormat> AspectRatioDictionary = new Dictionary<string, TextingAspectRatioFormat>();
    public static TextingAspectRatioFormat AspectRatio;

    [FormerlySerializedAs("textingFormatting")]
    public List<TextingAspectRatioFormat> TextingFormatting = new List<TextingAspectRatioFormat>();

    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (TextingAspectRatioFormat txt in TextingFormatting)
        {
            AspectRatioDictionary.Add((txt.AspectRatio.x / txt.AspectRatio.y).ToString("#.00"), CreateDictionaryEntry(txt));
        }
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        AspectRatio = GetAspectRatio();
    }

    /// <summary>
    /// Create dictionary entries for each item in the public <see cref="TextingFormatting"/> list.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    TextingAspectRatioFormat CreateDictionaryEntry(TextingAspectRatioFormat text)
    {
        TextingAspectRatioFormat format = new TextingAspectRatioFormat();

        format.AspectRatio = text.AspectRatio;

        format.IconSize = text.IconSize;
        format.IconEdgePadding = text.IconEdgePadding;
        format.BubbleEdgePadding = text.BubbleEdgePadding;

        format.SenderFontSize = text.SenderFontSize;
        format.MessageFontSize = text.MessageFontSize;
        format.LayoutSpacing = text.LayoutSpacing;

        format.PhoneContainerOffsetMin = text.PhoneContainerOffsetMin;
        format.PhoneContainerOffsetMax = text.PhoneContainerOffsetMax;

        format.AutoplayMenuPivotX = text.AutoplayMenuPivotX;
        format.AutoplayMenuAnchorX = text.AutoplayMenuAnchorX;

        return format;
    }

    /// <summary>
    /// Get the current aspect ratio and its values for formatting.
    /// </summary>
    /// <returns></returns>
    TextingAspectRatioFormat GetAspectRatio()
    {
        //float ratio = Screen.width / Screen.height;
        string ratio = mainCamera.aspect.ToString("#.00");
        TextingAspectRatioFormat aspect = new TextingAspectRatioFormat();

        if (AspectRatioDictionary.ContainsKey(ratio))
        {
            aspect = AspectRatioDictionary[ratio];
        }

        return aspect;
    }
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
