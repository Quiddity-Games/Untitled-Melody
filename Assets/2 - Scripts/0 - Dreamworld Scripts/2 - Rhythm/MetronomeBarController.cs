using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetronomeBarController : MonoBehaviour
{

    public NoteTracker _NoteTracker;
    public GameObject metronomeBar;  //Standard prefab for a metronome bar
    private Color metronomeBarColor;    //Metronome bars change color as the player clicks/taps to show whether they player "hit" or "missed" the beat
    public bool metronomeBarDebugMode;  //Lets the dev test the rhythm "forgiveness" value by having the bars change color to show if they _will_ be hits/missed _before_ the player even clicks/taps
    GameObject newMetronomeBarL;
    GameObject newMetronomeBarR;

    [SerializeField] private float height;
    [SerializeField] private float width;

    private GameObject dreamworldUICanvas;    //Canvas parented to the player, used to display text UI that should be attached to the player
    public AnimationCurve linearCurve;  //Used for lerp calculations

    private bool spawnNewMetronomeBars;   //Determines when the rhythm indicator should spawn a new set of (initially unmoving) metronome bars
    private bool startMovingMetronomeBars;    //Determines when those metronome bars should start moving towards e/

    private float rhythmIndicatorTimer; //Timer specifically dedicated to the rhythm indicator, aka the "metronome bars" above the player's head

    private float twoBeatsLength;

    private float timeToMove;

    private int delay;
    public GameObject panelPrefab;

    private GameObject panel;

    private Coroutine Left;
    private Coroutine Right;

    private Coroutine spawner;
    // Start is called before the first frame update
 
    void Start()
    {
        _NoteTracker.Load();
        Init();
        Settings.SecondaryBars.OnValueChanged.AddListener(Toggle);
        Toggle(Settings.SecondaryBars.Value);
    }

    void OnDestroy()
    {
        Settings.SecondaryBars.OnValueChanged.RemoveListener(Toggle);
        _NoteTracker.onBeatEnter -= HandleBars;
    }

    public void Toggle(bool enabled)
    {
        if(panel == null)
        {

        }
        if (enabled)
        {
            _NoteTracker.onBeatEnter += HandleBars;
            panel.SetActive(true);
        }
        else
        {
            _NoteTracker.onBeatEnter -= HandleBars;
            panel.SetActive(false);
            if(newMetronomeBarR != null && newMetronomeBarL != null)
            {
                StopCoroutine(Left);
                StopCoroutine(Right);
                StopCoroutine(spawner);
                Destroy(newMetronomeBarR);
                Destroy(newMetronomeBarL);
            }
        }
    }

    private void Init()
    {
        dreamworldUICanvas = DreamworldDialogueController.Instance.gameObject;
        twoBeatsLength = _NoteTracker.GetTwoBeatsLength();
        rhythmIndicatorTimer -= ((8f * _NoteTracker.GetTwoBeatsLength())); //Offsets rhythmIndicatorTimer so that the "metronome bars" above the player's head don't start appearing until the percussion beats of the "wishing well" song begin, roughly four measures in
        panel = Instantiate(panelPrefab, dreamworldUICanvas.transform);
        panel.transform.SetAsFirstSibling();
        panel.SetActive(false);
    }
    
    /// <summary>
    /// Checks if/when a new pair of "metronome bars" should appear.
    /// </summary>
    public void UpdateRhythmIndicator()
    {
      

        startMovingMetronomeBars = false;

        rhythmIndicatorTimer += Time.deltaTime;

        //Triggers when it's time for a new pair of metronome bars to appear
        if(rhythmIndicatorTimer >= twoBeatsLength
           && newMetronomeBarL == null
           && newMetronomeBarR == null)
        {
            SpawnNewMetronomeBars();
        }

        //Triggers when it's time for those metronome bars to start moving towards each other
        if(rhythmIndicatorTimer >= twoBeatsLength*2)
        {
            rhythmIndicatorTimer -= twoBeatsLength*2;

        }

      

 
    }
    
     /// <summary>
    /// The function/coroutine that tells the two metronome bars currently onscreen to start moving towards each other.
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator MoveRhythmIndicatorBarVisual(GameObject bar)
     {
         yield return new WaitForSeconds(twoBeatsLength/2);
        Vector3 startPos = bar.GetComponent<Transform>().localPosition;
        Vector3 endPos = new Vector3(0, startPos.y, 0);

        bool instaDestroyBar = true;   //Used to determine whether/not a bar should be instantly destroyed (if the bar completed its movement without the player clicking/tapping in time), or freeze and fade away in place (if the player clicked/tapped before the bar disappeared)

        float t = 0;

        while(t < 1)
        {
            bar.GetComponent<RectTransform>().anchoredPosition = Vector3.LerpUnclamped(startPos, endPos, linearCurve.Evaluate(t));

            t += Time.deltaTime / (twoBeatsLength/4);

            //Changes bar color _before_ player clicks -- to give away if it will be a hit/not -- but only if debug mode is on
            if(metronomeBarDebugMode == true)
            {
                bar.GetComponent<Image>().color = metronomeBarColor;
            }

            /*
            if(Input.GetMouseButtonDown(0))
            {

                bar.GetComponent<Image>().color = metronomeBarColor;    //Changes the bar's color after the player has clicked, so they can see whether/not they hit it

                t = 1;

                instaDestroyBar = false;
            }
            */

            yield return 0;
        }

        //Either destroys the bar, or causes it to fade away _then_ destroy, depending on whether or not the player "hits" it before it disappears
        if(instaDestroyBar == true)
        {
            Destroy(bar);

        }
        else
        {
            float alpha = bar.GetComponent<Image>().color.a;

            while(alpha >= 0)
            {
                bar.GetComponent<Image>().color = new Color(bar.GetComponent<Image>().color.r, bar.GetComponent<Image>().color.g, bar.GetComponent<Image>().color.b, alpha);
                alpha -= 0.02f; //Future Revision Note: Maybe for faster rhythms/songs, the alpha should fade faster, b/c more bars are coming in faster?

                yield return 0;
            }

            Destroy(bar);
        }

        yield return 0;
    }


     void HandleBars()
     {

         spawner = StartCoroutine(SpawnNewMetronomeBars());
     }
     private IEnumerator SpawnNewMetronomeBars()
     {
         yield return new WaitForSeconds(twoBeatsLength/2);

         newMetronomeBarL = Instantiate(metronomeBar, new Vector3(-width, height, 0), Quaternion.identity);
         newMetronomeBarL.GetComponent<RectTransform>().SetParent(dreamworldUICanvas.transform, false);
         newMetronomeBarL.GetComponent<RectTransform>().anchoredPosition = new Vector3(-width, height, 0);

         newMetronomeBarR = Instantiate(metronomeBar, new Vector3(width, height, 0), Quaternion.identity);
         newMetronomeBarR.GetComponent<RectTransform>().SetParent(dreamworldUICanvas.transform, false);
         newMetronomeBarR.GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
         
         Left = StartCoroutine(MoveRhythmIndicatorBarVisual(newMetronomeBarL));
         Right = StartCoroutine(MoveRhythmIndicatorBarVisual(newMetronomeBarR));
    }
     
}
