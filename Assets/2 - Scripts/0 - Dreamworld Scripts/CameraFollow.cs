using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for making the camera follow the player. Attached to the camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public enum SmoothSpeedType
    {
        Normal,
        Dashing,
        Checkpoint
    }

    private SmoothSpeedType _smoothSpeed = SmoothSpeedType.Normal;

    private Transform _player;

    private const float SmoothSpeedNormal = 10;
    private const float SmoothSpeedDashing = 10;    
    private const float SmoothSpeedCheckpoint = 2; 

    [SerializeField] private Vector3 _offset;    //Makes it so the camera doesn't have to be pointing *directly* at the player

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = _player.position + _offset;
        float smoothSpeedValue;
        switch (_smoothSpeed)
        {
            case SmoothSpeedType.Normal:
                smoothSpeedValue = SmoothSpeedNormal;
                break;
            case SmoothSpeedType.Dashing:
                smoothSpeedValue = SmoothSpeedDashing;
                break;
            case SmoothSpeedType.Checkpoint:
                smoothSpeedValue = SmoothSpeedCheckpoint;
                break;
            default:
                smoothSpeedValue = SmoothSpeedNormal;
                break;
        }
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeedValue * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void UpdateSpeed(SmoothSpeedType smoothSpeedType)
    {
        _smoothSpeed = smoothSpeedType;
    }
}