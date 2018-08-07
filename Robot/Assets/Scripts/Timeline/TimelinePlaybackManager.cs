using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class TimelinePlaybackManager : MonoBehaviour
{
    [Header("Timeline References")]
    public PlayableDirector playableDirector;

    [Header("Timeline Settings")]
    public bool playTimelineOnlyOnce;

    [Header("Player Input Settings")]
   // public KeyCode interactKey;
   // public KeyCode interactKey2;
    public bool disablePlayerInput = false;

    [Header("Both Buttons To Interact")]
    public bool both = false;

    [Header("Player Timeline Position")]
    public bool setPlayerTimelinePosition = false;
    public Transform playerTimelinePosition;

    [Header("Trigger Zone Settings")]
    public GameObject triggerZoneObject;

    [Header("UI Interact Settings")]
    public bool displayUI;
    public GameObject interactDisplay;

    [Header("Player 1 Settings")]
    public string playerTag = "Player1";

    [Header("Player 2 Settings")]
    public string player2Tag = "Player2";

    [Header("Switch")]
    public bool Switch = false;

    [Header("Do Players lose Limbs?")]
    public bool LoseLimb = false;

    [Header("Which Player/Players")]
    public bool Player1 = false;
    public bool Player2 = false;

    [Header("Which Limb?")]
    public string LimbToLose1 = "";
    public string LimbToLose2 = "";

    private float timelineDuration;
    private GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("SwitchObject");
    }

    public void PlayerEnteredZone()
    {
    }

    public void PlayerExitedZone()
    {


    }

   

    public void PlayTimeline()
    {
        if (playableDirector)
        {
			//Debug.Log ("or here?");
            //Plays the directer you set
            playableDirector.Play();
        }

        //Sets the zone you just used to false
        triggerZoneObject.SetActive(false);


        //Starts the timer
        StartCoroutine(WaitForTimelineToFinish());

        

    }

    IEnumerator WaitForTimelineToFinish()
    {
        //Finds the duration of the timeline
        timelineDuration = (float)playableDirector.duration;


        yield return new WaitForSeconds(timelineDuration);


        //Checks how many times to play yhe timeline 
        if (!playTimelineOnlyOnce)
        {
            triggerZoneObject.SetActive(true);
			Debug.Log ("hello");
        }
       

        if (Switch == true)
        {
            target.SetActive(false);
			Debug.Log ("goodbye");
        }


    }

   


}