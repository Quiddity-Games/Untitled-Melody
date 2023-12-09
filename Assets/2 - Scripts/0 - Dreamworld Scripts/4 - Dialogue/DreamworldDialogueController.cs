using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Ink.Runtime;
using System.Text.RegularExpressions;
using System.Linq;
using RoboRyanTron.Unite2017.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the resizing of UI for the Dreamworld canvas.
/// </summary>

public class DreamworldDialogueController : DialogueController
{
    #region Variables
    public static DreamworldDialogueController DreamworldUI;
    public int MostRecentLineIndex;
    private PlayerControl _playerControl;

    [Header("Show On Dialogue End")]
    [SerializeField] CanvasGroup welcomeMessage;
    [SerializeField] GameObject timerBar;

    private enum TextBoxState { Message, Typing }
    #endregion

    public override void Awake()
    {
        base.Awake();
        DreamworldUI = this;
    }

    private void OnEnable()
    {
        if (PlayDialogueOnStart.Value)
        {
            InitializeDialogue += InitializeUI;
            OnLineShown += ShowLine;
        }
    }

    private void OnDestroy()
    {
        if (PlayDialogueOnStart.Value)
        {
            InitializeDialogue -= InitializeUI;
            OnLineShown -= ShowLine;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _playerControl = new();
        _playerControl.Enable();
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, StartDialogue);

        if (PlayDialogueOnStart.Value)
        {
            CurrentLineIndex = -1;
            MostRecentLineIndex = -1;

            AutoplayEnabled = false;

            welcomeMessage.alpha = 0f;
            timerBar.SetActive(false);
        } else
        {
            welcomeMessage.alpha = 1f;
            timerBar.SetActive(true);
        }
    }

    private void InitializeUI()
    {
        ShowLine(); // Set the first textbox's UI.

        string speakerName = ParseSpeaker(LinesBeforeChoice[0]);
        DreamworldDialogueCanvas.Instance.SetTextBoxUI(LinesBeforeChoice[0], speakerName);
        ResizeTextBox(speakerName, true, TextBoxState.Typing);
    }

    /// <summary>
    /// Click to start the dialogue. Unsubscribes the event from input, and fades the canvas in.
    /// </summary>
    /// <param name="ctx"></param>
    private void StartDialogue()
    {
        DreamworldEventManager.Instance.DeregisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, StartDialogue);

        if (PlayDialogueOnStart.Value)
        {
            DOTween.Sequence().AppendCallback(() => DreamworldDialogueCanvas.Instance.FadeInInitialDialogue()).
            InsertCallback(0.85f, () => PlayDialogue(true));
        } else
        {
            DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.DIALOGUE_END);
        }
    }

    public void PlayDialogue(bool forward)
    {
        // Finish the dialogue if playing forwards.
        if (forward && CurrentLineIndex >= LastLineIndex)
        {
            DreamworldDialogueCanvas.Instance.FinishButton.gameObject.SetActive(false);
            DOTween.Sequence().
                Join(DreamworldDialogueCanvas.Instance.GradientCanvasGroup.DOFade(0f, 1.25f)).
                Join(DreamworldDialogueCanvas.Instance.ContentCanvasGroup.DOFade(0f, 1.25f)).
                InsertCallback(3f, () =>
                {
                    DreamworldEventManager.Instance.CallVoidEvent(DreamworldVoidEventEnum.DIALOGUE_END);
                    welcomeMessage.alpha = 1f;
                    timerBar.SetActive(true);
                });
            return;
        }

        // Adjust the current line index.
        if (forward)
            CurrentLineIndex++;
        else
        {
            if (CurrentLineIndex - 1 < 0)
                CurrentLineIndex = 0;
            else
                CurrentLineIndex--;
        }

        StartCoroutine(PlayDialogue(1.75f));

        IEnumerator PlayDialogue(float typingDuration)
        {
            CanPrintDialogue = false;
            string speakerName = ParseSpeaker(LinesBeforeChoice[CurrentLineIndex]);

            // Change the textbox to the typing animation.
            ResizeTextBox(speakerName, CurrentLineIndex > MostRecentLineIndex ? false : true, TextBoxState.Typing);

            // If playing forwards and not seen all lines, disable button interactions and wait for typing duration.
            if (forward && CurrentLineIndex > MostRecentLineIndex)
            {
                float time = 0f;
                DreamworldDialogueCanvas.Instance.SetButtonsInteractable(CanPrintDialogue);

                while (time < typingDuration)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
            }

            DreamworldDialogueCanvas.Instance.MessageText.DOFade(0f, 0f);

            // Show the full message in the textbox.
            ResizeTextBox(speakerName, CurrentLineIndex > MostRecentLineIndex ? false : true, TextBoxState.Message);
            OnLineShown?.Invoke();
        }
    }

    public IEnumerator SkipDialogue()
    {
        MostRecentLineIndex = LastLineIndex;

        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(CanPrintDialogue);
        DreamworldDialogueCanvas.Instance.AutoplayOnButton.interactable = false;

        if (AutoplayEnabled)
        {
            StopCoroutine(StartAutoplay());
            ToggleAutoplay(false);
        }

        while (CurrentLineIndex < LastLineIndex)
        {
            PlayDialogue(true);
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }

        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(CanPrintDialogue);
        yield break;
    }

    public void ToggleAutoplay(bool on)
    {
        AutoplayEnabled = on;

        if (on)
        {
            DreamworldDialogueCanvas.Instance.AutoplayOnButton.gameObject.SetActive(false);
            DreamworldDialogueCanvas.Instance.AutoplayOffButton.gameObject.SetActive(true);
            StartCoroutine(StartAutoplay());
        } else
        {
            DreamworldDialogueCanvas.Instance.AutoplayOnButton.gameObject.SetActive(true);
            DreamworldDialogueCanvas.Instance.AutoplayOffButton.gameObject.SetActive(false);
        }
    }

    IEnumerator StartAutoplay()
    {
        while (CurrentLineIndex < LastLineIndex && AutoplayEnabled)
        {
            while (!CanPrintDialogue)
            {
                yield return null;
            }

            PlayDialogue(true);
            yield return new WaitForSeconds(1.75f + 1.15f + 2f); // Duration of typing animation + duration of message animation + delay of 2s for reading.
            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// Method to call animation/tweening methods from <see cref="DreamworldDialogueCanvas"/>.
    /// </summary>
    private void ShowLine()
    {
        CanPrintDialogue = true;
        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(CanPrintDialogue);

        // On the last line, show the finish button.
        if (CurrentLineIndex >= LastLineIndex)
        {
            DreamworldDialogueCanvas.Instance.ShowFinishButton(false);
        } else if (DreamworldDialogueCanvas.Instance.FinishButton.gameObject.activeSelf)
        {
            DreamworldDialogueCanvas.Instance.ShowFinishButton(true);
        }

        // Adjust the current line index.
        if (CurrentLineIndex > MostRecentLineIndex)
        {
            MostRecentLineIndex = CurrentLineIndex;
            return;
        }

        if (MostRecentLineIndex == LastLineIndex)
        {
            MostRecentLineIndex = LinesBeforeChoice.Count;
            return;
        }
    }

    private void ResizeTextBox(string speakerName, bool noDelay, TextBoxState state)
    {
        bool isRightAligned = false;

        if (speakerName.Contains(MainCharacterName))
            isRightAligned = true;

        float fadeDuration = noDelay ? 0f : 0.15f;
        float textBoxDuration = noDelay ? 0f : 0.3f;

        float insertDelay = fadeDuration + textBoxDuration;

        switch (state)
        {
            case TextBoxState.Message:
                DreamworldDialogueCanvas.Instance.ShowMessageUI(isRightAligned, fadeDuration, textBoxDuration, insertDelay);
                break;

            case TextBoxState.Typing:
                DreamworldDialogueCanvas.Instance.ShowTypingUI(isRightAligned, fadeDuration, textBoxDuration, insertDelay, speakerName, CurrentLineIndex);
                break;
        }
    }
}
