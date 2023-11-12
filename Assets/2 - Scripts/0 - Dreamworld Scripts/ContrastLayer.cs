using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class ContrastLayer : MonoBehaviour
{
    [SerializeField] private BoolVariable contrastLayerEnabled;
    [SerializeField] private BoolVariable isContrastLayerBlack;
    [SerializeField] private FloatVariable opacity;
    private SpriteRenderer layerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        layerRenderer = GetComponent<SpriteRenderer>();

        if (isContrastLayerBlack.Value)
            layerRenderer.color = Color.black;
        else
            layerRenderer.color = Color.white;

        if (contrastLayerEnabled.Value)
            layerRenderer.enabled = true;
        else
            layerRenderer.enabled = false;

        UpdateOpacity();
    }

    void UpdateOpacity()
    {
        Color currentColor = layerRenderer.color;

        layerRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, opacity.Value);
    }
}
