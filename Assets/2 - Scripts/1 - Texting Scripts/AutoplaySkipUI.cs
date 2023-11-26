using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class AutoplaySkipUI : MonoBehaviour
{
    public static AutoplaySkipUI Instance;
    public Toggle autoplayToggleButton;
    public Button skipToChoiceButton;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        autoplayToggleButton.onValueChanged.AddListener(delegate {
            TextingDialogueController.TextingUI.SetAutoplay();
            if (TextingDialogueController.TextingUI.AutoplayEnabled)
                TextingDialogueCanvas.Instance.AutoplayDialogue();
        });
        skipToChoiceButton.onClick.AddListener(TextingDialogueController.TextingUI.SkipToChoice);

        autoplayToggleButton.interactable = false;
        skipToChoiceButton.interactable = false;
    }
}
