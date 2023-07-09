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

    private Camera mainCamera;

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
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();

        switch (canvasType)
        {
            case CanvasType.Dialogue:
                AspectRatio = GetTextingFormatValues();
                DialogueController.InitializeDialogue.Invoke();
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
        //float ratio = Screen.width / Screen.height;
        string ratio = mainCamera.aspect.ToString("#.00");
        TextingAspectRatioFormat aspect = new TextingAspectRatioFormat();

        if (TextingFormatDictionary.ContainsKey(ratio))
        {
            aspect = TextingFormatDictionary[ratio];
        }

        return aspect;
    }
}
