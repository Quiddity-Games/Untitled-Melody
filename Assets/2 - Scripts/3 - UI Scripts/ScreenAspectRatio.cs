using System;
using System.Collections.Generic;
using UnityEngine;

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
                {
                    if (!TextingFormatDictionary.ContainsKey((txt.AspectRatio.x / txt.AspectRatio.y).ToString("#.00")))
                        TextingFormatDictionary.Add((txt.AspectRatio.x / txt.AspectRatio.y).ToString("#.00"), CreateDictionaryEntry(txt));
                }

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
        TextingAspectRatioFormat format = new(
            text.AspectRatio,
            text.BackgroundOffsetMax,
            text.BubbleEdgePadding,
            text.IconEdgePadding,
            text.IconSize,
            text.SenderFontSize,
            text.MessageFontSize,
            text.LayoutSpacing,
            text.PhoneContainerOffsetMin,
            text.PhoneContainerOffsetMax,
            text.AutoplayMenuPivotX,
            text.AutoplayMenuAnchorX);

        return format;
    }

    /// <summary>
    /// Get the current aspect ratio and its values for formatting texting UI.
    /// </summary>
    /// <returns></returns>
    TextingAspectRatioFormat GetTextingFormatValues()
    {
        string ratio = Camera.main.aspect.ToString("#.00");
        TextingAspectRatioFormat aspect = null;

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
                        FindObjectOfType<TextingDialogueCanvas>().ResizeCanvasForPlatform(TextingFormatting.TextingFormatList[i]);
                    }
                }

                previewAspectRatio = false;
            }
        }
    }
#endif
}
