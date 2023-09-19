using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Grabs information for resizing the UI to suit other screens.
/// </summary>
public class ScreenAspectRatio : MonoBehaviour
{
    // Texting UI formatting values.
    public static Dictionary<string, TextingAspectRatioFormat> TextingFormatDictionary = new Dictionary<string, TextingAspectRatioFormat>();
    public static TextingAspectRatioFormat AspectRatio;
    public static TextingScreenFormat TextingFormatting;

    [SerializeField] CanvasType canvasType;
    public ScriptableObject CanvasInformation;
    [Space(15)]
    [SerializeField] bool previewAspectRatio;
    [SerializeField] Vector2 aspectRatio;

    [Serializable]
    enum CanvasType { Dreamworld, Dialogue }

    // Start is called before the first frame update
    void Awake()
    {
        switch (canvasType)
        {
            case CanvasType.Dialogue:
                TextingFormatting = CanvasInformation as TextingScreenFormat;
                foreach (TextingAspectRatioFormat txt in TextingFormatting.TextingFormatList)
                    TextingFormatDictionary.Add((txt.AspectRatio.x / txt.AspectRatio.y).ToString("#.00"), CreateDictionaryEntry(txt));
                break;
        }

        switch (canvasType)
        {
            case CanvasType.Dialogue:
                AspectRatio = GetTextingFormatValues();
                break;
        }
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
        format.BackgroundOffsetMax = text.BackgroundOffsetMax;

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
    /// Get the current aspect ratio and its values for formatting texting UI.
    /// </summary>
    /// <returns></returns>
    TextingAspectRatioFormat GetTextingFormatValues()
    {
        string ratio = Camera.main.aspect.ToString("#.00");
        TextingAspectRatioFormat aspect = new TextingAspectRatioFormat();

        if (TextingFormatDictionary.ContainsKey(ratio))
        {
            aspect = TextingFormatDictionary[ratio];
        }

        return aspect;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (previewAspectRatio)
            {
                for (int i = 0; i < TextingFormatting.TextingFormatList.Count; i++)
                {
                    if (aspectRatio == TextingFormatting.TextingFormatList[i].AspectRatio)
                    {
                        FindObjectOfType<DialogueCanvasUI>().ResizeCanvasForPlatform(TextingFormatting.TextingFormatList[i]);
                        FindObjectOfType<AutoplaySkipUI>().ResizeMenuForPlatform(TextingFormatting.TextingFormatList[i]);
                    }
                }

                previewAspectRatio = false;
            }
        }
    }
#endif
}
