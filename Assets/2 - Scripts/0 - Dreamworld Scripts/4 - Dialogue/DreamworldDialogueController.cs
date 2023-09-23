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
    public static DreamworldDialogueController DreamworldUI;
    public int MostRecentLineIndex;

    private PlayerControl _playerControl;

    private enum TextBoxState { Message, Typing }

    public override void Awake()
    {
        base.Awake();
        DreamworldUI = this;
    }

    private void OnEnable()
    {
        InitializeDialogue += InitializeUI;
        OnLineShown += ShowLine;
    }

    private void OnDestroy()
    {
        InitializeDialogue -= InitializeUI;
        OnLineShown -= ShowLine;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        CurrentLineIndex = -1;
        MostRecentLineIndex = -1;

        _playerControl = new();
        _playerControl.Dreamworld.Dash.performed += StartDialogue;
        _playerControl.Enable();

        AutoplayEnabled = false;
    }

    private void InitializeUI()
    {
        ShowLine();

        string speakerName = ParseSpeaker(LinesBeforeChoice[0]);
        DreamworldDialogueCanvas.Instance.SetTextBoxUI(LinesBeforeChoice[0], speakerName);
        ResizeTextBox(speakerName, true, TextBoxState.Typing);
    }

    private void StartDialogue(InputAction.CallbackContext ctx)
    {
        _playerControl.Dreamworld.Dash.performed -= StartDialogue;
        DOTween.Sequence().AppendCallback(() => DreamworldDialogueCanvas.Instance.FadeInInitialDialogue()).
            InsertCallback(0.85f, () => PlayDialogue(true));
    }

    public void PlayDialogue(bool forward)
    {
        if (forward && CurrentLineIndex >= LastLineIndex)
        {
            DreamworldDialogueCanvas.Instance.FinishButton.gameObject.SetActive(false);
            DOTween.Sequence().Join(DreamworldDialogueCanvas.Instance.ContentCanvasGroup.DOFade(0f, 1.5f)).InsertCallback(3f, () => OnDialogueEnd.Raise());
            return;
        }

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
            if (CurrentLineIndex > MostRecentLineIndex)
                MostRecentLineIndex = CurrentLineIndex;

            CanPrintDialogue = false;
            string speakerName = ParseSpeaker(LinesBeforeChoice[CurrentLineIndex]);
            ResizeTextBox(speakerName, forward && MostRecentLineIndex <= CurrentLineIndex ? false : true, TextBoxState.Typing);

            if (forward && MostRecentLineIndex <= CurrentLineIndex)
            {
                float time = 0f;
                ResizeTextBox(speakerName, false, TextBoxState.Typing);
                DreamworldDialogueCanvas.Instance.SetButtonsInteractable(false);

                while (time < typingDuration)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
            }

            DreamworldDialogueCanvas.Instance.MessageText.DOFade(0f, 0f);
            ResizeTextBox(speakerName, forward && MostRecentLineIndex <= CurrentLineIndex ? false : true, TextBoxState.Message);
            OnLineShown?.Invoke();
        }
    }

    public IEnumerator SkipDialogue()
    {
        MostRecentLineIndex = LastLineIndex;

        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(false);
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

        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(true);
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

    private void ShowLine()
    {
        CanPrintDialogue = true;
        DreamworldDialogueCanvas.Instance.SetButtonsInteractable(true);

        if (CurrentLineIndex >= LastLineIndex)
        {
            DreamworldDialogueCanvas.Instance.ShowLineUI(false);
        } else if (DreamworldDialogueCanvas.Instance.FinishButton.gameObject.activeSelf)
        {
            DreamworldDialogueCanvas.Instance.ShowLineUI(true);
        }

        if (MostRecentLineIndex == LastLineIndex)
            MostRecentLineIndex = LinesBeforeChoice.Count;
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
