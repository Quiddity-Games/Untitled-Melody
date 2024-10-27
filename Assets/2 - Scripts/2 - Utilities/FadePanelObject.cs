using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadePanelObject : MonoBehaviour
{
    [SerializeField] private float _fadeDuration;
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        DOTween.Sequence().Append(canvasGroup.DOFade(0, _fadeDuration)).AppendCallback(() => Destroy(this));
    }
}
