using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorDisplayController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _cursorSpriteRenderer;

    [SerializeField] private NoteTracker _noteTracker;
    
    [Serializable]
    private struct DisplaySprites
    {
        public Sprite defaultSprite;
        public Sprite missSprite;
        public Sprite badSprite;
        public Sprite goodSprite;
        public Sprite greatSprite;
        public Sprite perfectSprite;
    }


    [SerializeField] private DisplaySprites sprites;

    private void Start()
    {
        _noteTracker.HitCallback += UpdateSprite;
    }
    
    
    private void UpdateSprite(NoteTracker.HitInfo hitInfo)
    {
        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.MISS:
                _cursorSpriteRenderer.sprite = sprites.missSprite;
                break;
            case NoteTracker.BeatRating.BAD:
                _cursorSpriteRenderer.sprite = sprites.badSprite;
                break;
            case NoteTracker.BeatRating.GOOD:
                _cursorSpriteRenderer.sprite = sprites.goodSprite;
                break;
            case NoteTracker.BeatRating.GREAT:
                _cursorSpriteRenderer.sprite = sprites.greatSprite;
                break;
            case NoteTracker.BeatRating.PERFECT:
                _cursorSpriteRenderer.sprite = sprites.perfectSprite;
                break;
            default:
                _cursorSpriteRenderer.sprite = sprites.defaultSprite;
                break;
        }
        
        StartCoroutine(ResetDisplay());
    }


    private IEnumerator ResetDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        _cursorSpriteRenderer.sprite = sprites.defaultSprite;
    }
}
