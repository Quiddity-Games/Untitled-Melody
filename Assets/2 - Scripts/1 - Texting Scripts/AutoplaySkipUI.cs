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

    [SerializeField] AudioSource autoplaySkipSFXSource;
    [SerializeField] AudioClip toggleOnAudioClip;
    [SerializeField] AudioClip toggleOffAudioClip;

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
            {
                TextingDialogueCanvas.Instance.AutoplayDialogue();
                autoplaySkipSFXSource.PlayOneShot(toggleOnAudioClip);
            }
            else
            {
                autoplaySkipSFXSource.PlayOneShot(toggleOffAudioClip);
            }

        });
        skipToChoiceButton.onClick.AddListener(TextingDialogueController.TextingUI.SkipToChoice);

        autoplayToggleButton.interactable = false;
        skipToChoiceButton.interactable = false;
    }
}
