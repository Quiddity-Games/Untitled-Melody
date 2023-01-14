using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    public float smoothSpeed;
    public float dashingSmoothSpeed;    //The smooth speed when the player is dashing (triggered by other scripts)
    public float checkpointSmoothSpeed; //The smooth speed when the player is respawning at a checkpoint (triggered by other scripts)
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
