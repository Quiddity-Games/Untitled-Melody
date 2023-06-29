using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongParser : MonoBehaviour
{

    public SongObject _song;

    public NoteTracker _NoteTracker;
    // Start is called before the first frame update
    void Start()
    {
        
        _NoteTracker.bpm = _song.bpm;
        _NoteTracker.totalTime = _song.song.length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
