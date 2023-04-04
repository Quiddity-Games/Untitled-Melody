using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for making the camera follow the player. Attached to the camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    public float smoothSpeed;   //This is set equal to one of the two following variables in the GameManager.cs script
    public float dashingSmoothSpeed;    //The smooth speed when the player is dashing (triggered in GameManager.cs)
    public float checkpointSmoothSpeed; //The smooth speed when the player is respawning at a checkpoint (triggered in GameManager.cs)

    [SerializeField] Vector3 offset;    //Makes it so the camera doesn't have to be pointing *directly* at the player

    /// <summary>
    /// Updates the camera's position.
    /// </summary>
    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
