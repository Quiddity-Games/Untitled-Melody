using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOptionUI : MonoBehaviour
{
    public TextMeshProUGUI OptionText;
    public Button OptionButton;
    public CanvasGroup OptionCanvasGroup;
    public int OptionIndex;

    private void Start()
    {
        OptionButton.onClick.AddListener(() => TextingDialogueController.TextingUI.InkStory.ChooseChoiceIndex(OptionIndex));
        OptionButton.onClick.AddListener(TextingDialogueController.TextingUI.ChoiceMadeCallback);
    }
}
