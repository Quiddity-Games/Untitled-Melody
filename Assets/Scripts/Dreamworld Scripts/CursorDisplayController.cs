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
                return;
            case NoteTracker.BeatRating.BAD:
                _cursorSpriteRenderer.sprite = sprites.badSprite;
                return;
            case NoteTracker.BeatRating.GOOD:
                _cursorSpriteRenderer.sprite = sprites.goodSprite;
                return;
            case NoteTracker.BeatRating.GREAT:
                _cursorSpriteRenderer.sprite = sprites.greatSprite;
                return;
            case NoteTracker.BeatRating.PERFECT:
                _cursorSpriteRenderer.sprite = sprites.perfectSprite;
                return;
        }
    }
}
