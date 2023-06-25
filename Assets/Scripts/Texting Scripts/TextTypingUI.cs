using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTypingUI : MonoBehaviour
{

    private Color defaultColor = Color.white;

    public Image BubbleImage;
    public Image[] EllipsesImages;

    [Header("Layout Groups")]
    public HorizontalLayoutGroup BubbleLayoutGroup;
    public CanvasGroup CanvasGroup;

    [Space(10)]
    [Tooltip("Only for inspector use.")]
    [SerializeField] BubbleAlignment previewAlignment;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        switch (previewAlignment)
        {
            case BubbleAlignment.Left:
                SetBubbleAlignment(TextAnchor.LowerLeft);
                break;
            case BubbleAlignment.Right:
                SetBubbleAlignment(TextAnchor.LowerRight);
                break;
            default:
                SetBubbleAlignment(TextAnchor.LowerLeft);
                break;
        }
    }

    /// <summary>
    /// Sets the alignment of the bubble.
    /// </summary>
    /// <param name="textAnchor"></param>
    public void SetBubbleAlignment(TextAnchor textAnchor)
    {
        BubbleLayoutGroup.childAlignment = textAnchor;
    }

    /// <summary>
    /// Parse information for the text bubble and set the variables.
    /// </summary>
    public void SetBubbleColor(string mainCharacterName, TextBubbleUIElements senderUI)
    {
        // Set bubble alignment (left or right).
        if (senderUI.CharacterName.Contains(mainCharacterName))
            SetBubbleAlignment(TextAnchor.LowerRight);
        else
            SetBubbleAlignment(TextAnchor.LowerLeft);

        BubbleImage.color = senderUI.TextBoxColor;

        foreach (Image img in EllipsesImages)
        {
            img.color = senderUI.FontColor;
        }
    }
}
