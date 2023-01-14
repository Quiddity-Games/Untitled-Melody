using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }  //Singleton

    GameObject player;
    List<GameObject> movingHazards = new List<GameObject>();
    List<Vector3> movingHazardRespawnLocations = new List<Vector3>();

    public bool stopHazardsMove;

    public GameObject collectable;
    public TMP_Text collectibleText;
    public int numCollectables;
    public int numCollected;
    public int tempNumCollected;
    public List<Vector3> tempCollectableList;

    public int score;
    public int tempScore;
    public GameObject scoreCounter;
    public int scoreDeathPenalty;

    public int dashCombos;
    public GameObject comboTracker;

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
        collectibleText = GameObject.Find("CollectibleText").GetComponent<TMP_Text>();
        collectibleText.text = "" + numCollected + " / " + numCollectables;

        tempCollectableList = new List<Vector3>();

        score = 0;

        //Disabling combo functionality for now
        //dashCombos = 0;
    }

    void Update()
    {
        if (score < 0)
        {
            score = 0;
        }

        scoreCounter.GetComponent<TMP_Text>().text = score.ToString();

        //Disabling combo functionality for now
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
    /// Causes various things to happen when the player is "killed," or force to respawn
    /// </summary>
    public void OnDeath()
    {
        stopHazardsMove = true;

        player.transform.position = new Vector2(Checkpoint.currentCheckpoint.transform.position.x, Checkpoint.currentCheckpoint.transform.position.y);    //Respawns the player at their most recent checkpoint
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        for (int i = 0; i < movingHazards.Count; i++)
        {
            movingHazards[i].gameObject.GetComponent<Transform>().position = movingHazardRespawnLocations[i];   //Returns all moving hazards to their original positions
            movingHazards[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);   //Resets their velocity to zero to prevent momentum drift
        }

        numCollected -= tempNumCollected;
        tempNumCollected = 0;

        score -= (tempScore + scoreDeathPenalty);   //Subtracts the point value of all the fragments the player collected between now and their last spawn, as well as an extra penalty for dying
        tempScore = 0;

        for(int i = 0; i < tempCollectableList.Count; i++)
        {
            Instantiate(collectable, tempCollectableList[i], Quaternion.identity);
        }

        collectibleText.text = "" + numCollected + " / " + numCollectables;

        tempNumCollected = 0;
        tempCollectableList.Clear();

        //Disabling combo functionality for now
        //dashCombos = 0; //Resets player's combos on death
    }
}