using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

public class DreamworldDialogueCanvas : MonoBehaviour
{
    public static DreamworldDialogueCanvas Instance;

    [SerializeField] Button previousButton;
    public Button AutoplayOnButton;
    public Button AutoplayOffButton;
    [SerializeField] Button skipButton;
    [SerializeField] Button continueButton;

    [Space(10)]
    [SerializeField] RectTransform finishButtonTransform;
    [SerializeField] RectTransform finishButtonMidSection;
    public Button FinishButton;

    [Space(15)]
    [SerializeField] Vector2 typingPosition; // Negative X axis for left align. Set to positive in inspector.
    [SerializeField] Image leftPortrait;
    [SerializeField] Image rightPortrait;
    [SerializeField] CanvasGroup ellipsesContainer;
    [SerializeField] Image[] ellipsesImages;

    [Space(10)]
    public CanvasGroup GradientCanvasGroup;
    public CanvasGroup ContentCanvasGroup;
    [SerializeField] RectTransform textContainerTransform;
    [SerializeField] Vector2 textContainerBaseSize;
    [SerializeField] Vector2 textContainerTypingSize;
    [SerializeField] Image textBoxImage;
    [SerializeField] TextMeshProUGUI senderNameText;
    public TextMeshProUGUI MessageText;

    private Vector2 finishButtonOriginalPosition;
    private Vector2 textContainerOriginalPosition;
    private RectTransform leftPortraitRect;
    private RectTransform rightPortraitRect;
    private CanvasGroup leftPortraitCanvasGroup;
    private CanvasGroup rightPortraitCanvasGroup;
    private DreamworldDialogueController dialogueController;

    private void Awake()
    {
        Instance = this;
        dialogueController = GetComponent<DreamworldDialogueController>();
    }

    private void OnEnable()
    {
        DialogueController.SubscribeButtonEvents += InitializeButtonEvents;
    }

    private void OnDestroy()
    {
        DialogueController.SubscribeButtonEvents -= InitializeButtonEvents;
    }

    // Start is called before the first frame update
    void Start()
    {
        ContentCanvasGroup.alpha = 0f;
        GradientCanvasGroup.alpha = 0f;

        foreach (CharacterUIInfo info in dialogueController.CharactersInStory)
        {
            if (info.CharacterName != dialogueController.MainCharacterName)
            {
                InitializePortraits(info.CharacterName);
                break;
            }
        }

        finishButtonOriginalPosition = finishButtonTransform.anchoredPosition;
        FinishButton.gameObject.SetActive(false);
        finishButtonTransform.anchoredPosition = new Vector2(620f, finishButtonTransform.anchoredPosition.y);
        finishButtonMidSection.sizeDelta = new Vector2(0f, finishButtonMidSection.sizeDelta.y);
    }

    private void InitializePortraits(string conversationPartner)
    {
        leftPortrait.sprite = dialogueController.CharactersDictionary[conversationPartner].IconSprite;
        leftPortraitCanvasGroup = leftPortrait.gameObject.GetComponent<CanvasGroup>();
        leftPortraitCanvasGroup.alpha = 0f;
        leftPortraitRect = leftPortrait.gameObject.GetComponent<RectTransform>();

        rightPortrait.sprite = dialogueController.CharactersDictionary[dialogueController.MainCharacterName].IconSprite;
        rightPortraitCanvasGroup = rightPortrait.gameObject.GetComponent<CanvasGroup>();
        rightPortraitCanvasGroup.alpha = 0f;
        rightPortraitRect = rightPortrait.gameObject.GetComponent<RectTransform>();

        textContainerOriginalPosition = new Vector2(textContainerTransform.anchoredPosition.x, textContainerTransform.anchoredPosition.y);
        textContainerTransform.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void InitializeButtonEvents()
    {
        continueButton.onClick.AddListener(() => DreamworldDialogueController.DreamworldUI.PlayDialogue(true));
        previousButton.onClick.AddListener(() => DreamworldDialogueController.DreamworldUI.PlayDialogue(false));
        AutoplayOnButton.onClick.AddListener(() => DreamworldDialogueController.DreamworldUI.ToggleAutoplay(true));
        AutoplayOffButton.onClick.AddListener(() => DreamworldDialogueController.DreamworldUI.ToggleAutoplay(false));
        skipButton.onClick.AddListener(() => StartCoroutine(DreamworldDialogueController.DreamworldUI.SkipDialogue()));
        FinishButton.onClick.AddListener(() => DreamworldDialogueController.DreamworldUI.PlayDialogue(true));

        AutoplayOnButton.gameObject.SetActive(true);
        AutoplayOffButton.gameObject.SetActive(false);
    }

    public void FadeInInitialDialogue()
    {
        DOTween.Sequence().Join(GradientCanvasGroup.DOFade(1f, 0.5f)).
            Append(ContentCanvasGroup.DOFade(1f, 0.5f)).
            Insert(1.25f, textContainerTransform.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.15f));
    }

    public void ShowFinishButton(bool finished)
    {
        if (finished)
        {
            DOTween.Sequence().
                Join(finishButtonMidSection.DOSizeDelta(new Vector2(0f, finishButtonMidSection.sizeDelta.y), 0.15f)).
                Join(finishButtonTransform.DOAnchorPosX(620f, 0.15f)).
                AppendCallback(() => FinishButton.gameObject.SetActive(false));
        }
        else
        {
            FinishButton.gameObject.SetActive(true);
            DOTween.Sequence().
                Insert(0.1f, finishButtonMidSection.DOSizeDelta(new Vector2(175f, finishButtonMidSection.sizeDelta.y), 0.15f)).
                Join(finishButtonTransform.DOAnchorPosX(finishButtonOriginalPosition.x, 0.25f));
        }
    }

    public void SetButtonsInteractable(bool buttonState)
    {
        continueButton.interactable = buttonState;

        // Disable previous button if on first line (or less).
        if (dialogueController.CurrentLineIndex <= 0)
            previousButton.interactable = false;
        else
            previousButton.interactable = buttonState;

        // Disable autoplay and skip button if seen all lines.
        if (dialogueController.MostRecentLineIndex >= dialogueController.LastLineIndex)
        {
            AutoplayOnButton.interactable = false;
            AutoplayOffButton.interactable = false;
            skipButton.interactable = false;

            AutoplayOnButton.gameObject.SetActive(true);
            AutoplayOffButton.gameObject.SetActive(false);

            dialogueController.AutoplayEnabled = false;
        }
        else
        {
            AutoplayOnButton.interactable = buttonState;
            AutoplayOffButton.interactable = buttonState;
            skipButton.interactable = buttonState;
        }
    }

    public void SetTextBoxUI(string currentLine, string speakerName)
    {
        currentLine = dialogueController.RemoveTags(dialogueController.ParseEmojis(currentLine));

        senderNameText.text = speakerName;
        senderNameText.color = dialogueController.CharactersDictionary[speakerName].FontColor;
        senderNameText.outlineColor = dialogueController.CharactersDictionary[speakerName].TextBoxColor;

        MessageText.text = currentLine;
        MessageText.color = dialogueController.CharactersDictionary[speakerName].FontColor;

        textBoxImage.color = dialogueController.CharactersDictionary[speakerName].TextBoxColor;
    }

    public void ShowMessageUI(bool isRightAligned, float fadeDuration, float textBoxDuration, float insertDelay)
    {
        DOTween.Sequence().
                    Join(ellipsesContainer.DOFade(0f, fadeDuration));

        // Play sequence immediately after initial sequence; disable ellipses, fade name, fade and animate portraits, move/resize text box.
        DOTween.Sequence().
            Insert(fadeDuration, senderNameText.DOFade(1f, fadeDuration)).
            Insert(fadeDuration, isRightAligned ? rightPortraitCanvasGroup.DOFade(1f, fadeDuration) : leftPortraitCanvasGroup.DOFade(1f, fadeDuration)).
            Join(isRightAligned ? rightPortraitRect.DOAnchorPosY(0f, fadeDuration) : leftPortraitRect.DOAnchorPosY(0f, fadeDuration)).
            Insert(fadeDuration, textContainerTransform.DOAnchorPos(textContainerOriginalPosition, textBoxDuration)).
            Insert(fadeDuration, textContainerTransform.DOSizeDelta(textContainerBaseSize, textBoxDuration)).
            InsertCallback(fadeDuration, () => ellipsesContainer.gameObject.SetActive(false)).
            InsertCallback(fadeDuration, () => senderNameText.gameObject.SetActive(true));

        // Play sequence after all other sequences completed; turns message gameObject on and fades in.
        DOTween.Sequence().
            InsertCallback(insertDelay, () => MessageText.gameObject.SetActive(true)).
            Insert(insertDelay, MessageText.DOFade(1f, fadeDuration));
    }

    public void ShowTypingUI(bool isRightAligned, float fadeDuration, float textBoxDuration, float insertDelay, string speakerName, int lineIndex)
    {
        foreach (Image img in ellipsesImages)
            img.color = dialogueController.CharactersDictionary[speakerName].FontColor;

        // Play sequence immediately; fades portraits and animates in Y axis, fades message text.
        DOTween.Sequence().
            Join(ellipsesContainer.DOFade(0f, 0f)).
            Join(senderNameText.DOFade(0f, fadeDuration)).
            Join(leftPortraitCanvasGroup.DOFade(0f, fadeDuration)).Join(leftPortraitRect.DOAnchorPosY(-30f, fadeDuration)).
            Join(rightPortraitCanvasGroup.DOFade(0f, fadeDuration)).Join(rightPortraitRect.DOAnchorPosY(-30f, fadeDuration)).
            Join(MessageText.DOFade(0f, fadeDuration));

        // Play sequence immediately after initial sequence; fades sender name, change position/size of text box to typing, disable message gameObject.
        DOTween.Sequence().
            Insert(fadeDuration, textContainerTransform.DOSizeDelta(textContainerTypingSize, textBoxDuration)).
            Join(isRightAligned ? textContainerTransform.DOAnchorPosX(typingPosition.x, textBoxDuration) :
                textContainerTransform.DOAnchorPosX(-typingPosition.x, textBoxDuration)).
            InsertCallback(fadeDuration, () => senderNameText.gameObject.SetActive(false)).
            InsertCallback(fadeDuration, () => MessageText.gameObject.SetActive(false)).
            InsertCallback(fadeDuration, () => SetTextBoxUI(dialogueController.LinesBeforeChoice[lineIndex], speakerName)).
            InsertCallback(fadeDuration, () => senderNameText.alignment = isRightAligned ? TextAlignmentOptions.Right : TextAlignmentOptions.Left);

        // Play sequence after all other sequences completed; turns ellipses gameObject on and fades them.
        DOTween.Sequence().
            InsertCallback(insertDelay, () => ellipsesContainer.gameObject.SetActive(true)).
            Insert(insertDelay, ellipsesContainer.DOFade(1f, fadeDuration));
    }
}
