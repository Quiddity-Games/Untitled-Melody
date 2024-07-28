using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class ContrastLayer : MonoBehaviour
{
    [SerializeField] private Color _contrastLayerHigh;


    [SerializeField] private Color _contrastLayerLow;
    private SpriteRenderer layerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        layerRenderer = GetComponent<SpriteRenderer>();
        Settings.Contrast.OnValueChanged.AddListener(UpdateContrast);
        Settings.ContrastEnabled.OnValueChanged.AddListener(ToggleContrast);
        ToggleContrast(Settings.ContrastEnabled.Value);
        // For Justin + Save System: save out the index of the colours and set loadedColor to grab it on start.
        // For now, the default will always be 0/black.
        // Same thing with the opacity.Right now, it's taking a value between 0-100 and dividing it by 100 to get the alpha.
        // Feel free to change it to a number that's easier to save out--I figured whole numbers worked best!

/**
        if (_contrastLayerEnabled.Value)
            layerRenderer.enabled = true;
        else
            layerRenderer.enabled = false;
            */
    }

    public void ToggleContrast(bool enabled)
    {   
        layerRenderer.gameObject.SetActive(enabled);
    }

    public void UpdateContrast(float newOpacity)
    {
        if(newOpacity > 0)
        {
            _contrastLayerHigh.a = 0 + newOpacity;
            layerRenderer.color = _contrastLayerHigh;
        }        
        else
        {
            _contrastLayerLow.a = 0 - newOpacity;
            layerRenderer.color = _contrastLayerLow;
        }
    }
}
