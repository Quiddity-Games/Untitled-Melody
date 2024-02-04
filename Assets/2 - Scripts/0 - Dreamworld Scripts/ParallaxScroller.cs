using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParallaxScroller : MonoBehaviour
{
    private enum ScrollBehaviour { None, Parallax, Follow }
    [SerializeField] ScrollBehaviour _behaviour;
    [SerializeField] private float _scrollMultiplier;

    [SerializeField] SongObject songObject;
    [Header("Vertical Scroll")]
    [SerializeField] float _endYPosition;

    private Transform _cameraTransform;
    private Vector3 _previousCameraPos;
    private float cameraX;
    private float cameraY;

    // Start is called before the first frame update
    void Start()
    {
        switch (_behaviour)
        {
            case ScrollBehaviour.Parallax:
                _cameraTransform = Camera.main.transform;
                transform.position = new Vector2(_cameraTransform.position.x, _cameraTransform.position.y);
                _previousCameraPos = _cameraTransform.position;
                break;
            case ScrollBehaviour.Follow:
                cameraX = Camera.main.transform.position.x;
                cameraY = Camera.main.transform.position.y;
                transform.position = new Vector3(cameraX, cameraY, transform.position.z);
                break;
            case ScrollBehaviour.None:
                break;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (_behaviour)
        {
            case ScrollBehaviour.Parallax:
                Vector3 deltaMove = _cameraTransform.position - _previousCameraPos;
                transform.position += deltaMove * _scrollMultiplier;
                _previousCameraPos = _cameraTransform.position;
                break;
            case ScrollBehaviour.Follow: // Behaviours follows the camera 1-for-1
                cameraX = Camera.main.transform.position.x;
                cameraY = Camera.main.transform.position.y;
                transform.position = new Vector3(cameraX, cameraY, transform.position.z);
                break;
            case ScrollBehaviour.None:
                break;
        }
    }

    public void StartScrollY()
    {
        transform.DOLocalMoveY(_endYPosition, songObject.song.length * _scrollMultiplier, false);
    }
}
