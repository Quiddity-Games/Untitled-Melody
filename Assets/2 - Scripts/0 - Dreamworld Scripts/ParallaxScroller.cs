using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField] private float _scrollMultiplier;
    private Transform _cameraTransform;
    private Vector3 _previousCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        transform.position = new Vector2(_cameraTransform.position.x, _cameraTransform.position.y);
        _previousCameraPos = _cameraTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMove = _cameraTransform.position - _previousCameraPos;
        transform.position += deltaMove * _scrollMultiplier;
        _previousCameraPos = _cameraTransform.position;
    }
}
