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
    private GameObject playerObject;

    [Header("Player 2 Settings")]
    public string player2Tag = "Player2";
    private GameObject playerObject2;

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

    private bool playerInZone = false;
    private bool timelinePlaying = false;
    private float timelineDuration;
    private GameObject target;

    void Start()
    {
        //Finds the player objects
        playerObject = GameObject.FindWithTag(playerTag);
        playerObject2 = GameObject.FindWithTag(player2Tag);
        target = GameObject.FindGameObjectWithTag("SwitchObject");
    }

    public void PlayerEnteredZone()
    {
        //Checks if the players in the set zone
        playerInZone = true;
    }

    public void PlayerExitedZone()
    {
        //If he is not
        playerInZone = false;


    }

    void Update()
    {
        
        //If his in the zone and the timelines off
        /*if (playerInZone && !timelinePlaying)
        {
            //Checks the buttons
			var activateTimelineInput = Input.GetKey(GameManager.Instance.playerSetting.currentButton [9]);
			var activateTimelineInput2 = Input.GetKey(GameManager.Instance.playerSetting.currentButton [22]);


            //If one player
           	if (both == false)
            { 
                //If the interact key is hit then you play the timeline
				if (activateTimelineInput || activateTimelineInput2)
                {
					Debug.Log ("the fuck is this script 2");
                    PlayTimeline();
                }
                
            }


        }*/
    }

    public void PlayTimeline()
    {
        if (playableDirector)
        {
			//Debug.Log ("or here?");
            //Plays the directer you set
            playableDirector.Play();

        }
		//Debug.Log ("FUCKYOU!");

        //Sets the zone you just used to false
        triggerZoneObject.SetActive(false);

        //Plays the timeline
        timelinePlaying = true;

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
        }
        else if (playTimelineOnlyOnce)
        {
            playerInZone = false;
        }

        timelinePlaying = false;

        if (Switch == true)
        {
            target.SetActive(false);
        }


    }

   


}