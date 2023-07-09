using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.Events;

/// <summary>
/// Handles the entirety of the display of the texting UI, using references from the scene's <see cref="DialogueController"/>.
/// Attached to the Dialogue UI Canvas gameObject.
/// </summary>

[Serializable]
public class DialogueCanvasUI : MonoBehaviour
{
    #region Variables: Buttons
    [Header("Buttons")]
    [SerializeField] Button autoskipMenuButton;
    [SerializeField] Button startDialogueButton;
    public Button ContinueDialogueButton;
    [SerializeField] TextOptionUI[] dialogueOptions;
    #endregion

    #region Variables: Cellphone UI
    [Header("Phone UI: Overall")]
    [SerializeField] RectTransform backgroundTransform;
    [SerializeField] GameObject phoneContainer;
    private CanvasGroup phoneContainerCanvasGroup;
    private RectTransform phoneContainerTransform;

    [Header("Phone UI: Header")]
    [SerializeField] CanvasGroup headerCanvasGroup;
    [SerializeField] Image headerIcon;
    [SerializeField] TextMeshProUGUI headerText;

    [Header("Phone UI: Body")]
    [SerializeField] RectTransform bodyScrollViewTransform;
    [FormerlySerializedAs("bodyScrollContent")]
    public GameObject BodyScrollContent;
    [SerializeField] ScrollRect bodyScrollRect;
    [SerializeField] GameObject textOptionsContainer;
    #endregion

    #region Variables: Autoplay/Skip
    [Header("Phone UI: Autoplay/Skip")]
    [SerializeField] GameObject autoplaySkipContainer;
    public TextMeshProUGUI autoplayText;
    #endregion

    #region Hidden Variables
    [HideInInspector] float textContainerTop;
    [HideInInspector] float textContainerRight;
    [HideInInspector] float textContainerLeft;
    [HideInInspector] float textContainerBottom;

    private float canvasFadeDuration;
    private float startDelayDuration;
    private float bubbleFadeDuration;
    private float currentTypingDelayDuration;
    private float autoplayDelayDuration;

    // Typing bubble delays
    private float longTypingDelayDuration;
    private float midTypingDelayDuration;
    private float shortTypingDelayDuration;

    // Character info
    private Dictionary<string, TextBubbleUIElements> characterUIDictionary;
    private string mainCharacterName;

    #endregion

    void Start()
    {
        // Set up UI layout and visibility.
        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.gameObject.transform as RectTransform);
        phoneContainerCanvasGroup = phoneContainer.GetComponent<CanvasGroup>();

        phoneContainerCanvasGroup.alpha = 0f;
        headerCanvasGroup.alpha = 0f;

#if UNITY_ANDROID
        autoskipMenuButton.gameObject.SetActive(false);
        autoskipMenuButton.interactable = false;
#endif
    }

    public void ResizeCanvasForPlatform(TextingAspectRatioFormat format)
    {
        backgroundTransform.offsetMax = new Vector2(format.BackgroundOffsetMax.x, backgroundTransform.offsetMax.y);

        phoneContainerTransform = phoneContainer.transform as RectTransform;

        phoneContainerTransform.offsetMin = format.PhoneContainerOffsetMin;
        phoneContainerTransform.offsetMax = format.PhoneContainerOffsetMax;

        // Get current size of texting body container.
        textContainerLeft = bodyScrollViewTransform.offsetMin.x;
        textContainerRight = bodyScrollViewTransform.offsetMax.x;
        textContainerTop = bodyScrollViewTransform.offsetMax.y;
        textContainerBottom = bodyScrollViewTransform.offsetMin.y;
    }

    /// <summary>
    /// Get all starting references from the <see cref="DialogueController"/>.
    /// Called from the Dialogue Controller after <see cref="DialogueController.Awake"/>.
    /// </summary>
    public void GetReferencesFromController()
    {
        #region Get values from DialogueController
        canvasFadeDuration = DialogueController.Instance.CanvasFadeDuration;
        startDelayDuration = DialogueController.Instance.StartDelayDuration;
        bubbleFadeDuration = DialogueController.Instance.BubbleFadeDuration;
        autoplayDelayDuration = DialogueController.Instance.AutoplayDelayDuration;
        characterUIDictionary = DialogueController.Instance.CharacterUIDictionary;
        longTypingDelayDuration = DialogueController.Instance.LongTypingDelayDuration;
        midTypingDelayDuration = DialogueController.Instance.MidTypingDelayDuration;
        shortTypingDelayDuration = DialogueController.Instance.ShortTypingDelayDuration;

        mainCharacterName = TextBubbleCharacterUI.Instance.MainCharacterName;

        // Give functions to buttons.
        startDialogueButton.onClick.AddListener(ShowDialogueUI);
        ContinueDialogueButton.onClick.AddListener(PlayDialogue);
        autoskipMenuButton.onClick.AddListener(delegate { AutoplaySkipUI.Instance.DisplayAutoplayMenu(true); });
        #endregion
    }

    /// <summary>
    /// Set the header text and icon on <see cref="Start"/>.
    /// </summary>
    void GetHeaderText()
    {
        headerIcon.sprite = characterUIDictionary[DialogueController.Instance.GlobalTagsDictionary["Conversation"]].IconSprite;
        headerText.text = DialogueController.Instance.GlobalTagsDictionary["Conversation"];

        LayoutRebuilder.ForceRebuildLayoutImmediate(headerCanvasGroup.gameObject.transform as RectTransform);
        DialogueController.Instance.FadeInUI(headerCanvasGroup, bubbleFadeDuration);
    }

    /// <summary>
    /// Show dialogue UI after delay on <see cref="Start"/>.
    /// </summary>
    void ShowDialogueUI()
    {
        DialogueController.Instance.FadeInUI(phoneContainerCanvasGroup, canvasFadeDuration);
        GetHeaderText();
        startDialogueButton.gameObject.SetActive(false);
        DialogueController.Instance.CanPrintDialogue = false;

        StartCoroutine(PlayFirstLine(canvasFadeDuration));

        IEnumerator PlayFirstLine(float duration)
        {
            float time = 0;
            duration += startDelayDuration;

            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            DialogueController.Instance.CanPrintDialogue = true;

            if (DialogueController.Instance.AutoplayEnabled)
                AutoplayDialogue();
            else
                PlayDialogue();
        }
    }

    /// <summary>
    /// Displays all current instantiated bubbles in order until dialogue ends or choices appear.
    /// </summary>
    public void PlayDialogue()
    {
        if (DialogueController.Instance.CanPrintDialogue)
        {
            DialogueController.Instance.CanPrintDialogue = false;

            List<TextBubbleUI> bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice;
            int currentBubbleIndex = DialogueController.Instance.CurrentBubbleIndex;

            if (bubblesBeforeChoice.Count > 0 && currentBubbleIndex < bubblesBeforeChoice.Count)
            {
                // Get the correctly aligned typing bubble.
                TextTypingUI typingBubble = DialogueController.Instance.GetCurrentTypingBubble(bubblesBeforeChoice[currentBubbleIndex].SenderNameText.text);

                StartCoroutine(ShowTextTypingBubble());

                IEnumerator ShowTextTypingBubble()
                {
                    // Set the typing bubble to the bottom.
                    typingBubble.gameObject.transform.SetAsLastSibling();

                    typingBubble.gameObject.SetActive(true);
                    DialogueController.Instance.FadeInUI(typingBubble.CanvasGroup, bubbleFadeDuration);
                    TMP_TextInfo info = bubblesBeforeChoice[currentBubbleIndex].MessageText.GetTextInfo(bubblesBeforeChoice[currentBubbleIndex].MessageText.text);

                    float duration = 0f;
                    float time = 0f;

                    // Change the duration of the typing bubble based on line length.
                    if (info.characterCount >= 100)
                        duration = longTypingDelayDuration;
                    else if (info.lineCount >= 50 && info.lineCount <= 100)
                        duration = midTypingDelayDuration;
                    else
                        duration = shortTypingDelayDuration;

                    DialogueController.Instance.CurrentTypingDelayDuration = duration;

                    while (time < duration)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }

                    if (!bubblesBeforeChoice[currentBubbleIndex].gameObject.activeInHierarchy)
                        ShowNextTextBubble(DialogueController.Instance.BubblesBeforeChoice, DialogueController.Instance.CurrentBubbleIndex);
                }
            }
            else
            {
                // If there are choices to be shown, show them.
                if (DialogueController.Instance.InkStory.currentChoices.Count > 0)
                    DisplayChoices();
            }
        }
    }

    /// <summary>
    /// Used to print the next text bubble after the typing bubble animation clears.
    /// </summary>
    /// <param name="bubblesBeforeChoice"></param>
    /// <param name="currentBubbleIndex"></param>
    void ShowNextTextBubble(List<TextBubbleUI> bubblesBeforeChoice, int currentBubbleIndex)
    {
        StartCoroutine(InputDelay());

        if (DialogueController.Instance.CurrentTypingBubble.gameObject.activeInHierarchy)
            DialogueController.Instance.CurrentTypingBubble.gameObject.SetActive(false);

        //Debug.Log(bubblesBeforeChoice[currentBubbleIndex].SenderNameText.text + ": " + bubblesBeforeChoice[currentBubbleIndex].MessageText.text);

        // Check the current index of the shown bubble, and set it to true.
        bubblesBeforeChoice[currentBubbleIndex].gameObject.SetActive(true);
        DialogueController.Instance.FadeInUI(bubblesBeforeChoice[currentBubbleIndex].CanvasGroup, bubbleFadeDuration);
        bodyScrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the container.

        DialogueController.Instance.CurrentBubbleIndex++;

        IEnumerator InputDelay()
        {
            yield return new WaitForSeconds(bubbleFadeDuration);
            DialogueController.Instance.CanPrintDialogue = true;
        }

    }

    /// <summary>
    /// Creates a container for options and instantiates a number of option buttons based on Ink.
    /// </summary>
    public void DisplayChoices()
    {
        // Reset all current text bubble values for DialogueController.
        DialogueController.Instance.LinesBeforeChoice.Clear();
        DialogueController.Instance.BubblesBeforeChoice.Clear();
        DialogueController.Instance.CurrentBubbleIndex = 0;

        List<TextOptionUI> currentOptions = DialogueController.Instance.CurrentOptions;
        Story inkStory = DialogueController.Instance.InkStory;

        // Show a typing bubble animation.
        DialogueController.Instance.CurrentTypingBubble.transform.SetAsLastSibling();
        TextTypingUI typingBubble = DialogueController.Instance.RightTypingBubble;
        typingBubble.gameObject.SetActive(true);
        DialogueController.Instance.FadeInUI(typingBubble.CanvasGroup, bubbleFadeDuration);

        ContinueDialogueButton.gameObject.SetActive(false);
        
        // Create a button for each available choice.
        for (int i = 0; i < inkStory.currentChoices.Count; i++)
        {
            dialogueOptions[i].OptionText.text = inkStory.currentChoices[i].text;
            dialogueOptions[i].gameObject.SetActive(true);
            dialogueOptions[i].OptionButton.onClick.AddListener(delegate
            {
                foreach (TextOptionUI option in dialogueOptions)
                {
                    option.gameObject.SetActive(false);
                }
            });

            DialogueController.Instance.FadeInUI(dialogueOptions[i].OptionCanvasGroup, bubbleFadeDuration + dialogueOptions[i].OptionIndex * 0.15f);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.transform as RectTransform);

        bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, (textOptionsContainer.transform as RectTransform).sizeDelta.y);

    }

    /// <summary>
    /// Reset main text container size values after making a choice.
    /// </summary>
    public void ResetTextContainerSize()
    {
        // Reset container size values.
        bodyScrollViewTransform.offsetMin = new Vector2(textContainerLeft, textContainerBottom);
        bodyScrollViewTransform.offsetMax = new Vector2(textContainerRight, textContainerTop);

        DialogueController.Instance.CanPrintDialogue = true; // Re-enable continue button.
    }

    /// <summary>
    /// Used to autoplay dialogue if the setting is on.
    /// Only prints after a bubble has finished printing.
    /// </summary>
    public void AutoplayDialogue()
    {
        StartCoroutine(Autoplay());
    }

    IEnumerator Autoplay()
    {
        int bubblesBeforeChoice = DialogueController.Instance.BubblesBeforeChoice.Count;
        int currentBubbleIndex = DialogueController.Instance.CurrentBubbleIndex;

        while (currentBubbleIndex < bubblesBeforeChoice - 1 && DialogueController.Instance.AutoplayEnabled)
        {
            while (!DialogueController.Instance.CanPrintDialogue)
            {
                yield return null;
            }

            if (DialogueController.Instance.AutoplayEnabled)
            {
                PlayDialogue();
                yield return new WaitForSeconds(autoplayDelayDuration + currentTypingDelayDuration);
            }
            yield return null;
        }

        yield break;
    }
}
