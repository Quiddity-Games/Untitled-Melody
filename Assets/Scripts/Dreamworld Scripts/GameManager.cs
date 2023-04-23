using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles a number of universal aspects of the game, primarily information related to collectables and other aspects of the game that need to be reset when the player dies/respawns. Attached to the GameManager gameObject.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }  //Singleton

    public bool stopHazardsMove;    //Referenced by the scripts of moving hazards; used to freeze their velocity when the player is respawning so they don't retain their previous velocity and drift out of place
    public GameObject collectable;
    public TMP_Text CollectibleUIText;  //On-screen text indicating how many collectables out of the total that the player has acquired
    public int numCollectables; //Total number of collectables in the level
    public int numCollected;    //Number of collectables the player has acquired so far
    public int tempNumCollected; //"Temporary" information about collectables the player has claimed since their last checkpoint; used to determine what collectables they should lose / that should be reset the next time the player dies
    public List<Vector3> tempCollectableList;
    public GameObject gameCamera;
    private List<Collectable> _collectables = new List<Collectable>();

    private GameObject _player;
    private List<GameObject> _movingHazards = new List<GameObject>();    //A list of moving obstacles in the level, which need to be moved back to their original positions when the player dies/respawns
    private List<Vector3> _movingHazardRespawnLocations = new List<Vector3>();   //Locaions where said obstacles need to return

    // Start is called before the first frame update
    void Start()
    {
        _collectables.AddRange(FindObjectsOfType<Collectable>());
        numCollectables = _collectables.Count;

        instance = this;

        _player = GameObject.FindGameObjectWithTag("Player");

        _movingHazards.AddRange(GameObject.FindGameObjectsWithTag("Moving Hazard")); //Fills out the moving hazard list with all moving hazard gameObjects in the level

        for(int i = 0; i < _movingHazards.Count; i++)
        {
            _movingHazardRespawnLocations.Add(_movingHazards[i].gameObject.GetComponent<Transform>().position);   //Records the starting position of each moving hazard gameObject
        }

        stopHazardsMove = false;

        //Populates the UI with the correct numbers of found/total collectables in the level
        CollectibleUIText = GameObject.Find("CollectibleText").GetComponent<TMP_Text>();
        CollectibleUIText.text = "" + numCollected + " / " + numCollectables;

        tempCollectableList = new List<Vector3>();

    }

    public void OnCollectableCollected(Vector3 collectablePosition)
    {
        numCollected++;
        tempNumCollected++;
        tempCollectableList.Add(collectablePosition);
        CollectibleUIText?.SetText($"{numCollected} / {numCollectables}");
    }

    public void Reset()
    {
        numCollected = 0;
        tempNumCollected = 0;
        tempCollectableList.Clear();

        foreach (var collectable in _collectables)
        {
            collectable.gameObject.SetActive(true);
        }
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
        _player.transform.position = new Vector2(Checkpoint.currentCheckpoint.transform.position.x, Checkpoint.currentCheckpoint.transform.position.y);    //Respawns the player at their most recent checkpoint
        _player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //Moves the moving hazards back to their default locations
        for (int i = 0; i < _movingHazards.Count; i++)
        {
            _movingHazards[i].gameObject.GetComponent<Transform>().position = _movingHazardRespawnLocations[i];   //Returns all moving hazards to their original positions
            _movingHazards[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);   //Resets their velocity to zero to prevent momentum drift
        }

        //Resets the number of collectables the player has acquired since their previous checkpoint
        numCollected -= tempNumCollected;
        tempNumCollected = 0;

        //Respawns the collectables the player lost upon their death to their original places in the level, so that the player can re-collect them
        for(int i = 0; i < tempCollectableList.Count; i++)
        {
            Instantiate(collectable, tempCollectableList[i], Quaternion.identity);
        }

        tempCollectableList.Clear();

        CollectibleUIText.text = "" + numCollected + " / " + numCollectables;

        //gameCamera.GetComponent<CameraFollow>().UpdateToCheckpointSpeed();   //Slows down camera smooth speed so that the move back to the checkpoint isn't as visually jarring
    }
}