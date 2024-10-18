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
        Settings.MetronomeBlink.OnValueChanged.AddListener(Toggle);
        Toggle(Settings.MetronomeBlink.Value);
    }

    void OnDestroy()
    {
        Settings.MetronomeBlink.OnValueChanged.RemoveListener(Toggle);
    }
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
   void Blink()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(BlinkRoutine());
        }
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        cursor.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        cursor.material = originalMaterial;
    }
}
