using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkControllerText : MonoBehaviour
{
    public NoteTracker _NoteTracker;
    
    [SerializeField] private float duration;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material flashMaterial;
    
    private Material originalMaterial;

    private Coroutine flashRoutine;

    void Start()
    {
        originalMaterial = meshRenderer.material;
        Settings.MetronomeBlink.OnValueChanged.AddListener(Toggle);
        Toggle(Settings.MetronomeBlink.Value);
    }

    void OnDestroy()
    {
        Settings.MetronomeBlink.OnValueChanged.RemoveListener(Toggle);
        if(_NoteTracker != null)
        {
            _NoteTracker.onBeatTrigger -= Blink;
        }
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
        meshRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        meshRenderer.material = originalMaterial;
    }
}
