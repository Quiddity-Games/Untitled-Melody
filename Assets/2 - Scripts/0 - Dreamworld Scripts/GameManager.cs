using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles a number of universal aspects of the game, primarily information related to collectables and other aspects of the game that need to be reset when the player dies/respawns. Attached to the GameManager gameObject.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }  //Singleton

    GameObject player;
    List<GameObject> movingHazards = new List<GameObject>();    //A list of moving obstacles in the level, which need to be moved back to their original positions when the player dies/respawns
    List<Vector3> movingHazardRespawnLocations = new List<Vector3>();   //Locaions where said obstacles need to return

    public bool stopHazardsMove;    //Referenced by the scripts of moving hazards; used to freeze their velocity when the player is respawning so they don't retain their previous velocity and drift out of place

    public GameObject collectable;
    public TMP_Text collectibleUIText;  //On-screen text indicating how many collectables out of the total that the player has acquired
    public int numCollectables; //Total number of collectables in the level
    public int numCollected;    //Number of collectables the player has acquired so far

    //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies
    public int tempNumCollected;
    public List<Vector3> tempCollectableList;

    //Information related to a point system associated with collecting/losing collectables; currently disabled b/c it wasn't fun/interesting to playtesters
    /*
    public int score;
    public GameObject scoreCounter;
    public int scoreDeathPenalty;

    public int tempScore;
    */

    //Used to track the number of successive dashes the player has pulled off; disabled for now b/c playtesters didn't find it fun/interesting
    /*
    public int dashCombos;
    public GameObject comboTracker;
    */

    public GameObject gameCamera;

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

        numCollectables = GameObject.FindGameObjectsWithTag("Collectable").Length;  //Determines how many collectables there are in the level, and sets the UI accordingly

        //Populates the UI with the correct numbers of found/total collectables in the level
        collectibleUIText = GameObject.Find("CollectibleText").GetComponent<TMP_Text>();
        collectibleUIText.text = "" + numCollected + " / " + numCollectables;

        tempCollectableList = new List<Vector3>();

        //Disabling scoring functionality for now
        //score = 0;

        //Disabling combo functionality for now
        //dashCombos = 0;
    }

    void Update()
    {
        //Disabling scoring functionality for now
        /*
        if(score < 0)
        {
            score = 0;
        }

        scoreCounter.GetComponent<TMP_Text>().text = score.ToString();
        */

        //Old code used to track the number of successive dashes the player has pulled off; cut for now based on playtesters not finding this feature useful/fun
        /*
        if(dashCombos > 0)
        {
            comboTracker.GetComponent<TMP_Text>().text = "x" + dashCombos.ToString();
        } else
        {
            comboTracker.GetComponent<TMP_Text>().text = "";
        }
        */
    }

    /// <summary>
    /// Causes various things to happen when the player is "killed," or forced to respawn; called by all lethal obstacles/objects that make contact with the player.
    /// </summary>
    /// <param name="thisObjectCalledMe"></param>
    public void OnDeath(string thisObjectCalledMe)
    {
        Debug.Log("OnDeath() has been called by " + thisObjectCalledMe);

        stopHazardsMove = true; //Freezes all moving hazards in the level, so that they're stationary when the player respawns

        //Moves the player back to their most recent checkpoint
        player.transform.position = new Vector2(Checkpoint.currentCheckpoint.transform.position.x, Checkpoint.currentCheckpoint.transform.position.y);    //Respawns the player at their most recent checkpoint
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //Moves the moving hazards back to their default locations
        for (int i = 0; i < movingHazards.Count; i++)
        {
            movingHazards[i].gameObject.GetComponent<Transform>().position = movingHazardRespawnLocations[i];   //Returns all moving hazards to their original positions
            movingHazards[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);   //Resets their velocity to zero to prevent momentum drift
        }

        //Resets the number of collectables the player has acquired since their previous checkpoint
        numCollected -= tempNumCollected;
        tempNumCollected = 0;

        //Disabling scoring functionality for now
        /*
        score -= (tempScore + scoreDeathPenalty);   //Subtracts the point value of all the fragments the player collected between now and their last spawn, as well as an extra penalty for dying
        tempScore = 0;
        */

        //Respawns the collectables the player lost upon their death to their original places in the level, so that the player can re-collect them
        for(int i = 0; i < tempCollectableList.Count; i++)
        {
            Instantiate(collectable, tempCollectableList[i], Quaternion.identity);
        }

        tempCollectableList.Clear();

        collectibleUIText.text = "" + numCollected + " / " + numCollectables;

        //Disabling combo functionality for now
        //dashCombos = 0; //Resets player's combos on death

        gameCamera.GetComponent<CameraFollow>().smoothSpeed = gameCamera.GetComponent<CameraFollow>().checkpointSmoothSpeed;   //Slows down camera smooth speed so that the move back to the checkpoint isn't as visually jarring
    }
}