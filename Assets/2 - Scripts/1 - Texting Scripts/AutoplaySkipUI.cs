using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class AutoplaySkipUI : MonoBehaviour
{
    public static AutoplaySkipUI Instance;
    private Animator animator;
    [SerializeField] RectTransform panelTransform;
    [SerializeField] Button screenWideButton;
    [SerializeField] Toggle autoplayToggleButton;
    [SerializeField] Button skipToChoiceButton;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        autoplayToggleButton.onValueChanged.AddListener(delegate { TextingDialogueController.TextingUI.SetAutoplay(); });
        skipToChoiceButton.onClick.AddListener(TextingDialogueController.TextingUI.SkipToChoice);
        screenWideButton.onClick.AddListener(delegate { DisplayAutoplayMenu(false); });
    }

    /// <summary>
    /// Used to animate the autoplay and skip to choice options menu.
    /// </summary>
    /// <param name="on"></param>
    public void DisplayAutoplayMenu(bool on)
    {
        if (on)
        {
            Time.timeScale = 0f;
            animator.SetTrigger("Open");
        }
        else
        {
            Time.timeScale = 1f;
            animator.SetTrigger("Close");

            if (TextingDialogueController.TextingUI.AutoplayEnabled)
                TextingDialogueCanvas.Instance.AutoplayDialogue();
        }
    }

    public void ResizeMenuForPlatform(TextingAspectRatioFormat format)
    {
        panelTransform.pivot = new Vector2(format.AutoplayMenuPivotX, panelTransform.pivot.y);
        panelTransform.anchoredPosition = new Vector2(format.AutoplayMenuAnchorX, panelTransform.anchoredPosition.y);
    }
}
