using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;

public class ContrastLayer : MonoBehaviour
{
    [SerializeField] private BoolVariable _contrastLayerEnabled;
    [SerializeField] private Color[] _contrastLayerColors;
    private int _savedColor;
    private float _savedOpacity;
    private SpriteRenderer layerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        layerRenderer = GetComponent<SpriteRenderer>();

        // For Justin + Save System: save out the index of the colours and set loadedColor to grab it on start.
        // For now, the default will always be 0/black.
        // Same thing with the opacity.Right now, it's taking a value between 0-100 and dividing it by 100 to get the alpha.
        // Feel free to change it to a number that's easier to save out--I figured whole numbers worked best!

        _savedColor = 0;
        _savedOpacity = 60;

        layerRenderer.color = new Color
        (
            _contrastLayerColors[_savedColor].r,
            _contrastLayerColors[_savedColor].g,
            _contrastLayerColors[_savedColor].b,
            _savedOpacity / 100
        );

        if (_contrastLayerEnabled.Value)
            layerRenderer.enabled = true;
        else
            layerRenderer.enabled = false;
    }

    public void ChangeLayerColor(Color newColor)
    {
        // Keep the colours to only be Black or White for now.
        layerRenderer.color = newColor;

        // If the colour passed was black, set the saved index to 0. Anything else will be 1/white.
        _savedColor = newColor == Color.black ? 0 : 1;

    }

    public void ChangeLayerOpacity(float newOpacity)
    {
        Color currentColor = layerRenderer.color;

        layerRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newOpacity);

        _savedOpacity = Mathf.RoundToInt(newOpacity * 100);
    }
}
