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
        OptionButton.onClick.AddListener(() => DialogueController.Instance.InkStory.ChooseChoiceIndex(OptionIndex));
        OptionButton.onClick.AddListener(DialogueController.Instance.ChoiceMadeCallback);
    }
}
