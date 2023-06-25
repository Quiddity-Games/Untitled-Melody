using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOptionUI : MonoBehaviour
{
    public TextMeshProUGUI OptionText;
    public Button OptionButton;
    public int OptionIndex;

    [Space(10)]
    [Header("Colors")]
    public Color FadedFontColor;
    public Color OptionFontColour;

    // Start is called before the first frame update
    void Start()
    {
        if (!OptionButton)
            OptionButton = GetComponent<Button>();

        if (DialogueController.Instance.CharacterUIDictionary.ContainsKey(TextBubbleCharacterUI.Instance.MainCharacterName))
        {
            OptionFontColour = DialogueController.Instance.CharacterUIDictionary[TextBubbleCharacterUI.Instance.MainCharacterName].FontColor;
        }
    }
}
