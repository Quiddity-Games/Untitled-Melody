using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;
using DG.Tweening;

/// <summary>
/// Handles the entirety of the display of the texting UI, using references from the scene's <see cref="TextingDialogueController"/>.
/// Attached to the Dialogue UI Canvas gameObject.
/// </summary>

[Serializable]
public class TextingDialogueCanvas : MonoBehaviour
{
    public static TextingDialogueCanvas Instance;

    #region Variables: Buttons
    [Header("Buttons")]
    [SerializeField] Button autoskipMenuButton;
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

    #region Variables: Timers
    [Header("Phone UI: Timers")]
    [SerializeField] float canvasFadeDuration;
    [SerializeField] float startDelayDuration;
    #endregion

    #region Variables: Typing Bubbles
    [Header("Phone UI: Typing Bubbles")]
    [SerializeField] float shortTypingDelayDuration;
    [SerializeField] float midTypingDelayDuration;
    [SerializeField] float longTypingDelayDuration;
    #endregion

    #region Variables: Autoplay/Skip
    [Header("Phone UI: Autoplay/Skip")]
    [SerializeField] GameObject autoplaySkipContainer;
    public TextMeshProUGUI autoplayText;
    #endregion

    #region Hidden Variables
    private float textContainerTop;
    private float textContainerRight;
    private float textContainerLeft;
    private float textContainerBottom;
    private float currentTypingDelayDuration;
    private bool loadedLastChunk;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        DialogueController.SubscribeButtonEvents += InitializeButtonEvents;
    }

    private void OnDestroy()
    {
        DialogueController.SubscribeButtonEvents -= InitializeButtonEvents;
    }

    private void Start()
    {
        // Set up UI layout and visibility.
        LayoutRebuilder.ForceRebuildLayoutImmediate(textOptionsContainer.gameObject.transform as RectTransform);
        phoneContainerCanvasGroup = phoneContainer.GetComponent<CanvasGroup>();

        phoneContainerCanvasGroup.alpha = 0f;
        headerCanvasGroup.alpha = 0f;

        // Show the phone UI after 4 seconds.
        DOTween.Sequence().InsertCallback(4f, ShowDialogueUI);
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

    private void InitializeButtonEvents()
    {
        // Give functions to buttons.
        ContinueDialogueButton.onClick.AddListener(PlayDialogue);
        autoskipMenuButton.onClick.AddListener(delegate { AutoplaySkipUI.Instance.DisplayAutoplayMenu(true); });
    }

    /// <summary>
    /// Set the header text and icon on <see cref="Start"/>.
    /// </summary>
    private void GetHeaderText()
    {
        headerIcon.sprite = DialogueController.Instance.CharactersDictionary[DialogueController.Instance.GlobalTagsDictionary["Conversation"]].IconSprite;
        headerText.text = DialogueController.Instance.GlobalTagsDictionary["Conversation"];

        LayoutRebuilder.ForceRebuildLayoutImmediate(headerCanvasGroup.gameObject.transform as RectTransform);
        headerCanvasGroup.DOFade(1f, TextingDialogueController.TextingUI.BubbleFadeDuration);
        //TextingDialogueController.TextingUI.FadeInUI(headerCanvasGroup, TextingDialogueController.TextingUI.BubbleFadeDuration);
    }

    /// <summary>
    /// Show dialogue UI after delay on <see cref="Start"/>.
    /// </summary>
    private void ShowDialogueUI()
    {
        ContinueDialogueButton.interactable = false;
        phoneContainerCanvasGroup.DOFade(1f, canvasFadeDuration);
        GetHeaderText();
        DialogueController.Instance.CanPrintDialogue = false;

        DOTween.Sequence().InsertCallback(canvasFadeDuration + startDelayDuration, () =>
        {
            DialogueController.Instance.CanPrintDialogue = true;
            if (DialogueController.Instance.AutoplayEnabled)
                AutoplayDialogue();
            else
                PlayDialogue();
        });
    }

    private void EndDialogue()
    {
        Debug.Log("END TEXTING SCENE");
    }

    /// <summary>
    /// Displays all current instantiated bubbles in order until dialogue ends or choices appear.
    /// </summary>
    public void PlayDialogue()
    {
        if (DialogueController.Instance.CanPrintDialogue)
        {
            DialogueController.Instance.CanPrintDialogue = false;

            List<TextBubbleUI> bubblesBeforeChoice = TextingDialogueController.TextingUI.BubblesBeforeChoice;
            int currentBubbleIndex = DialogueController.Instance.CurrentLineIndex;

            if (bubblesBeforeChoice.Count > 0 && currentBubbleIndex < bubblesBeforeChoice.Count)
            {
                // Get the correctly aligned typing bubble.
                TextTypingUI typingBubble = TextingDialogueController.TextingUI.GetCurrentTypingBubble(bubblesBeforeChoice[currentBubbleIndex].SenderNameText.text);

                StartCoroutine(ShowTextTypingBubble());

                IEnumerator ShowTextTypingBubble()
                {
                    // Set the typing bubble to the bottom.
                    typingBubble.gameObject.transform.SetAsLastSibling();

                    typingBubble.gameObject.SetActive(true);
                    typingBubble.CanvasGroup.DOFade(1f, TextingDialogueController.TextingUI.BubbleFadeDuration);
                    //TextingDialogueController.TextingUI.FadeInUI(typingBubble.CanvasGroup, TextingDialogueController.TextingUI.BubbleFadeDuration);
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

                    currentTypingDelayDuration = duration;
                    ContinueDialogueButton.interactable = false;

                    while (time < duration)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }

                    if (!bubblesBeforeChoice[currentBubbleIndex].gameObject.activeInHierarchy)
                        ShowNextTextBubble(TextingDialogueController.TextingUI.BubblesBeforeChoice, DialogueController.Instance.CurrentLineIndex);
                    
                    ContinueDialogueButton.interactable = true;
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
    private void ShowNextTextBubble(List<TextBubbleUI> bubblesBeforeChoice, int currentBubbleIndex)
    {
        if (TextingDialogueController.TextingUI.CurrentTypingBubble.gameObject.activeInHierarchy)
            TextingDialogueController.TextingUI.CurrentTypingBubble.gameObject.SetActive(false);

        // Check the current index of the shown bubble, and set it to true.
        bubblesBeforeChoice[currentBubbleIndex].gameObject.SetActive(true);
        bubblesBeforeChoice[currentBubbleIndex].CanvasGroup.DOFade(1f, TextingDialogueController.TextingUI.BubbleFadeDuration);

        bodyScrollRect.verticalNormalizedPosition = 0f; // Scroll to the bottom of the container.

        DialogueController.Instance.CurrentLineIndex++;

        if (DialogueController.Instance.LastChunkLoaded
            && DialogueController.Instance.CurrentLineIndex > DialogueController.Instance.LastLineIndex)
        {
            ContinueDialogueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
            ContinueDialogueButton.onClick.RemoveAllListeners();
            ContinueDialogueButton.onClick.AddListener(EndDialogue);
            DialogueController.Instance.OnDialogueEnd.Raise();
            return;
        } else
        {
            DOTween.Sequence().InsertCallback(TextingDialogueController.TextingUI.BubbleFadeDuration + 0.1f, () => DialogueController.Instance.CanPrintDialogue = true);
        }
    }

    /// <summary>
    /// Creates a container for options and instantiates a number of option buttons based on Ink.
    /// </summary>
    public void DisplayChoices()
    {
        // Reset all current text bubble values for DialogueController.
        DialogueController.Instance.LinesBeforeChoice.Clear();
        TextingDialogueController.TextingUI.BubblesBeforeChoice.Clear();
        DialogueController.Instance.CurrentLineIndex = 0;

        List<TextOptionUI> currentOptions = TextingDialogueController.TextingUI.CurrentOptions;
        Story inkStory = DialogueController.Instance.InkStory;

        // Show a typing bubble animation.
        TextingDialogueController.TextingUI.CurrentTypingBubble.transform.SetAsLastSibling();
        TextTypingUI typingBubble = TextingDialogueController.TextingUI.RightTypingBubble;
        typingBubble.gameObject.SetActive(true);
        typingBubble.CanvasGroup.DOFade(1f, TextingDialogueController.TextingUI.BubbleFadeDuration);

        ContinueDialogueButton.gameObject.SetActive(false);
        
        // Create a button for each available choice.
        for (int i = 0; i < inkStory.currentChoices.Count; i++)
        {
            dialogueOptions[i].OptionCanvasGroup.alpha = 0f;
            dialogueOptions[i].OptionText.text = inkStory.currentChoices[i].text;
            dialogueOptions[i].gameObject.SetActive(true);
            dialogueOptions[i].OptionButton.onClick.AddListener(delegate
            {
                foreach (TextOptionUI option in dialogueOptions)
                {
                    option.gameObject.SetActive(false);
                }
            });

            dialogueOptions[i].OptionCanvasGroup.DOFade(1f,
                TextingDialogueController.TextingUI.BubbleFadeDuration + dialogueOptions[i].OptionIndex * 0.15f);
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
        int bubblesBeforeChoice = TextingDialogueController.TextingUI.BubblesBeforeChoice.Count;
        int currentBubbleIndex = DialogueController.Instance.CurrentLineIndex;
        ContinueDialogueButton.interactable = false;

        while (currentBubbleIndex < bubblesBeforeChoice - 1 && DialogueController.Instance.AutoplayEnabled)
        {
            while (!DialogueController.Instance.CanPrintDialogue)
            {
                yield return null;
            }

            if (DialogueController.Instance.AutoplayEnabled)
            {
                PlayDialogue();
                yield return new WaitForSeconds(DialogueController.Instance.AutoplayDelayDuration + currentTypingDelayDuration);
            }
            yield return null;
        }

        yield break;
    }
}
