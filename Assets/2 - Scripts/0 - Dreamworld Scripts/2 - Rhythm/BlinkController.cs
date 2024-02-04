using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    public NoteTracker _NoteTracker;
    
    [SerializeField] private float duration;
    
    [SerializeField] private SpriteRenderer cursor;

    [SerializeField] private Material flashMaterial;
    
    private Material originalMaterial;

    private Coroutine flashRoutine;

    void Start()
    {
        originalMaterial = cursor.material;
        SettingsManager.Instance().GetAccSettings().onUpdate += HandleBlinkSetting;
        HandleBlinkSetting();

    }

 

    void HandleBlinkSetting()
    {
        Toggle(SettingsManager.Instance().GetAccSettings().MetronomeBlink);
    }

    // Start is called before the first frame update
    public void Toggle(bool onEnabled)
    {
        if(onEnabled){
        _NoteTracker.onBeatTrigger += Blink;
        }
        else
        {
            _NoteTracker.onBeatTrigger -= Blink;
        }
    }

    // Update is called once per frame
    void Blink()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(BlinkRoutine());
        }
        Debug.Log("Flash");
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        cursor.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        cursor.material = originalMaterial;
    }
}
