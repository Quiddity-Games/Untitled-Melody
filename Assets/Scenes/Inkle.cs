using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using Ink.Runtime;
using TMPro;

[Serializable] 
public class Inkle : MonoBehaviour
{
    public TextAsset inktext;
    public Sprite receiversprite;
    public Sprite sendersprite;
    public GameObject scrollviewcontent;
    public Scrollbar vertscrollbar;
    public GameObject textbubbleprefab;
    public ScrollRect scrollview;
    TextGenerator textgen;
    public GameObject[] dialogueresponses = new GameObject[4];
    Story inkstory;
    public bool receiverspeaking = true;
    float currentdialoguelength = 0.0f;
    float phonewidth;
    float phoneheight;
    float vertscrollbarwidth;
    float horzscrollbarheight;
    int count = 0;
    GameObject cellphone;

    //vert./horiz. offset distance of textbubbles from sides of screen or each other 
    public float verttextbubbleoffset;
    public float horztextbubbleoffset;

    //vert./horiz. offset dstance of text inside textbubble
    public float verttextoffsetinsidebubble;
    public float horztextoffsetinsidebubble;

    //recommended values for textrecwidth/height of textbubble prefab in inspector, based on max size of text on screen, 
    public float textrectwidth; //float textrectwidth = phonewidth - vertscrollbarwidth - (horztextbubbleoffset * 2.0f)) - (horztextoffsetinsidebubble * 2.0f);
    public float textrectheight; //float textrectheight = phoneheight - horzscrollbarwidth - (verttextbubbleoffset * 2.0f)) - (verttextoffsetinsidebubble * 2.0f);

    void Awake()
    {
        //initialize ink story with the dialogue when object loads
        textgen = new TextGenerator();
        inkstory = new Story(inktext.text);
        cellphone = GameObject.Find("Cell Phone");
        RectTransform cellphonerect = cellphone.GetComponent(typeof(RectTransform)) as RectTransform;
        phonewidth = cellphonerect.rect.width;
        phoneheight = cellphonerect.rect.height;
        //RectTransform cellphonerecthorizontal = cellphone.transform.GetChild(1).gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
        RectTransform cellphonerectvertical = cellphone.transform.GetChild(1).gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
       //horzscrollbarheight = cellphonerecthorizontal.rect.height;
        vertscrollbarwidth = cellphonerectvertical.rect.width;
}

    // Update is called once per frame
    void Update()
    {
        if (count > 0) //this is here because unity doesn't resize the content rect until after this update is called on this frame, it puts the scrollbar at the bottom after the resizing took place
        {
            count--;
            vertscrollbar.value = 0.0f;
        }
        //dialogue running
        //resize content rect, instantiate text bubble prefab with sized speech bubbles 
        if (Input.GetMouseButtonDown(0) && !dialogueresponses[0].activeSelf)
        {
            continueDialogue();
            vertscrollbar.value = 0.0f;
            count += 1;
        }
    }

    void continueDialogue()
    {
        if (inkstory.canContinue)
        {
            string textstring = inkstory.Continue();
            if (textstring == "&receiver\n")   //if found marker indicating receiver is speaking, or sender is speaking
            {
                receiverspeaking = true;
                textstring = inkstory.Continue();
            }
            else if (textstring == "&sender\n")
            {
                receiverspeaking = false;
                textstring = inkstory.Continue();
            }
            textstring = textstring.Replace("emoji1", "<sprite=0>");
            GameObject newtextbubble = Instantiate(textbubbleprefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity); //instantiate new textbubble
            RectTransform bubbleimagerect = newtextbubble.transform.GetChild(0).gameObject.GetComponentInChildren<RectTransform>();
            newtextbubble.transform.SetParent(scrollviewcontent.transform, false);
            TextMeshProUGUI textcomponent = newtextbubble.transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>(); //determine height/width of text to set dimensions of speech bubble
            textcomponent.text = textstring;
            //TextGenerationSettings gensettings = textcomponent.GetGenerationSettings(textcomponent.rectTransform.rect.size);
            textcomponent.ForceMeshUpdate();
            Vector2 dimensions = textcomponent.GetPreferredValues(textstring);
            float textwidth = dimensions.x;
            float textheight = dimensions.y;
            //float textheight = textcomponent.GetPreferredHeight(textstring, gensettings);
            //float textwidth = textgen.GetPreferredWidth(textstring, gensettings) + (horztextoffsetinsidebubble * 2.0f);
            if (textwidth > phonewidth - vertscrollbarwidth + (horztextbubbleoffset * 2.0f) - (horztextoffsetinsidebubble * 2.0f))
            {
                textwidth = phonewidth - vertscrollbarwidth - (horztextbubbleoffset * 2.0f) - (horztextoffsetinsidebubble * 2.0f);
                Debug.Log("sadadsdsa");
            }
            bubbleimagerect.sizeDelta = new Vector2(textwidth, textheight);
            float halfhorztextdistance = textwidth / 2.0f;
            float halfverttextdistance = textheight / 2.0f;
            //float textrectwidth = phonewidth - vertscrollbarwidth - (horztextbubbleoffset * 2.0f)) - (horztextoffsetinsidebubble * 2.0f);
            //float textrectheight = phoneheight - horzscrollbarwidth - (verttextbubbleoffset * 2.0f)) - (verttextoffsetinsidebubble * 2.0f);

            if (receiverspeaking) //speechbubble color/location determined by if receiver or sender is speaking
            {
                newtextbubble.transform.localPosition = new Vector3(horztextbubbleoffset + 55.0f, -currentdialoguelength - halfverttextdistance - verttextbubbleoffset, 0.0f);
                SpriteRenderer profilepicture = newtextbubble.transform.GetChild(1).GetChild(0).gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                profilepicture.sprite = receiversprite;
                //newtextbubble.GetComponent<Image>().color = new Vector4(0.0f, 0.05f, 1.0f, 1.0f);
                bubbleimagerect.localPosition = new Vector3(bubbleimagerect.localPosition.x + halfhorztextdistance - 47.5f, bubbleimagerect.localPosition.y, 0.0f);
                bubbleimagerect.GetComponent<Image>().color = new Vector4(0.0f, 0.05f, 1.0f, 1.0f);
                //textcomponent.transform.localposition = newtextbubble.transform.localposition;
                textcomponent.transform.localPosition = new Vector3(textcomponent.transform.localPosition.x - halfhorztextdistance + (textrectwidth / 2.0f) + horztextoffsetinsidebubble, textcomponent.transform.localPosition.y, 0.0f);
                //textcomponent.transform.localPosition = new Vector3(textcomponent.transform.localPosition.x - halfhorztextdistance + (textrectwidth / 2.0f) + horztextoffsetinsidebubble, textcomponent.transform.localPosition.y, 0.0f);
                textcomponent.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                //newtextbubble.transform.localPosition = new Vector3((phonewidth - vertscrollbarwidth - horztextbubbleoffset) - halfhorztextdistance, -currentdialoguelength - halfverttextdistance - verttextbubbleoffset, 0.0f);
                newtextbubble.transform.localPosition = new Vector3((phonewidth - vertscrollbarwidth - horztextbubbleoffset) - 40.0f, -currentdialoguelength - halfverttextdistance - verttextbubbleoffset, 0.0f);
                SpriteRenderer profilepicture = newtextbubble.transform.GetChild(1).GetChild(0).gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                profilepicture.sprite = sendersprite;
                //newtextbubble.GetComponent<Image>().color = new Vector4(195.0f, 195.0f, 195.0f, 255.0f);
                bubbleimagerect.localPosition = new Vector3(bubbleimagerect.localPosition.x - halfhorztextdistance + 47.5f, bubbleimagerect.localPosition.y, 0.0f);
                bubbleimagerect.GetComponent<Image>().color = new Vector4(195.0f, 195.0f, 195.0f, 255.0f);
                newtextbubble.transform.GetChild(1).localPosition = new Vector3(-newtextbubble.transform.GetChild(1).localPosition.x, 0.0f, 0.0f);
                textcomponent.transform.localPosition = new Vector3(textcomponent.transform.localPosition.x - halfhorztextdistance + (textrectwidth / 2.0f) + horztextoffsetinsidebubble, textcomponent.transform.localPosition.y, 0.0f);
                textcomponent.color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            }

            currentdialoguelength += textheight + verttextbubbleoffset;
            if (currentdialoguelength > phoneheight)
            { //resize scrollview content rect if number of messages exceeds size of "phone screen"
                scrollviewcontent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollviewcontent.GetComponent<RectTransform>().sizeDelta.x, currentdialoguelength + 20.0f);
            }
            //Debug.Log(scrollview.scrollOffset);
        }
        else
        //if encountered a choice, show choice options on screen
        {
            for (int i = 0; i < inkstory.currentChoices.Count; i++)
            {
                dialogueresponses[i].SetActive(true);
                dialogueresponses[i].GetComponentInChildren<Text>().text = inkstory.currentChoices[i].text;
            }
        }
    }

    //callback function for when choice button is clicked
    public void choiceMadeCallback(int choice)
    {
        //make choice based on button pressed, hide UI part of it
        if (inkstory.currentChoices.Count > 0)
        {
            //Debug.Log("Count " + inkstory.currentChoices.Count);
            for (int i = 0; i < inkstory.currentChoices.Count; i++)
            {
                dialogueresponses[i].GetComponentInChildren<Text>().text = "";
                dialogueresponses[i].SetActive(false);
            }
            inkstory.ChooseChoiceIndex(choice);
            inkstory.Continue();
            continueDialogue();
            //dialogue.GetComponentInChildren<Text>().text = inkstory.Continue();
        }
    }
}
