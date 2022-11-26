using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    GameObject player;
    List<GameObject> movingHazards = new List<GameObject>();
    List<Vector3> movingHazardRespawnLocations = new List<Vector3>();

    public static RespawnManager instance { get; private set; }  //Singleton

    public bool stopHazardsMove;

    public GameObject collectable;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");

        movingHazards.AddRange(GameObject.FindGameObjectsWithTag("Moving Hazard")); //Fills out the moving hazard list with all moving hazard gameObjects in the level

        for(int i = 0; i < movingHazards.Count; i++)
        {
            movingHazardRespawnLocations.Add(movingHazards[i].gameObject.GetComponent<Transform>().position);   //Records the starting position of each moving hazard gameObject
        }

        stopHazardsMove = false;
    }

    /// <summary>
    /// Causes various things to happen when the player is "killed," or force to respawn
    /// </summary>
    public void OnDeath()
    {
        stopHazardsMove = true;

        player.transform.position = Checkpoint.currentCheckpoint.transform.position;    //Respawns the player at their most recent checkpoint
        
        for (int i = 0; i < movingHazards.Count; i++)
        {
            movingHazards[i].gameObject.GetComponent<Transform>().position = movingHazardRespawnLocations[i];   //Returns all moving hazards to their original positions
            movingHazards[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);   //Resets their velocity to zero
        }

        Collectable.numCollected -= Collectable.tempNumCollected;
        Collectable.tempNumCollected = 0;

        for(int i = 0; i < Collectable.tempCollectableList.Count; i++)
        {
            Instantiate(collectable, Collectable.tempCollectableList[i], Quaternion.identity);
        }

        Collectable.collectibleText.text = "" + Collectable.numCollected + " / " + Collectable.numCollectables;
    }
}
