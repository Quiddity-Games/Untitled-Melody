using UnityEngine;
using UnityEngine.UI;

public class TextTypingUI : MonoBehaviour
{
    public Image BubbleImage;
    public Image[] EllipsesImages;

    [Header("Layout Groups")]
    public LayoutGroup BubbleLayoutGroup;
    public CanvasGroup CanvasGroup;

    [Space(10)]
    [SerializeField] bool previewAlignment;
    [Tooltip("Only for inspector use.")]
    [SerializeField] BubbleAlignment currentAlignment;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (previewAlignment)
        {
            switch (currentAlignment)
            {
                case BubbleAlignment.Left:
                    SetBubbleAlignment(TextAnchor.LowerLeft);
                    break;
                case BubbleAlignment.Right:
                    SetBubbleAlignment(TextAnchor.LowerRight);
                    break;
            }

            previewAlignment = false;
        }
    }

    /// <summary>
    /// Sets the alignment of the bubble.
    /// </summary>
    /// <param name="textAnchor"></param>
    void SetBubbleAlignment(TextAnchor textAnchor)
    {
        BubbleLayoutGroup.childAlignment = textAnchor;
    }

    /// <summary>
    /// Sets the alignment and padding of the bubble.
    /// </summary>
    /// <param name="textAnchor"></param>
    public void GetBubbleFormatting(TextingAspectRatioFormat aspect, TextAnchor textAnchor)
    {
        BubbleLayoutGroup.childAlignment = textAnchor;

        // Set padding on left/right
        BubbleLayoutGroup.padding.left = aspect.IconEdgePadding;
        BubbleLayoutGroup.padding.right = aspect.IconEdgePadding;
    }

    /// <summary>
    /// Parse information for the text bubble and set the variables.
    /// </summary>
    public void SetBubbleColor(string mainCharacterName, CharacterUIElements senderUI)
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
