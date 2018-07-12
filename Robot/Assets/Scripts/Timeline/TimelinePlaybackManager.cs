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
    public KeyCode interactKey;
    public KeyCode interactKey2;
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
        ToggleInteractUI(false);
    }

    public void PlayerEnteredZone()
    {
        //Checks if the players in the set zone
        playerInZone = true;
        ToggleInteractUI(playerInZone);
    }

    public void PlayerExitedZone()
    {
        //If he is not
        playerInZone = false;

        ToggleInteractUI(playerInZone);

    }

    void Update()
    {
        
        //If his in the zone and the timelines off
        if (playerInZone && !timelinePlaying)
        {
            //Checks the buttons
            var activateTimelineInput = Input.GetKey(interactKey);
            var activateTimelineInput2 = Input.GetKey(interactKey2);

            //If both players
            if (both == true)
            {
                //If there is no interact key it plays the timeline on collision
                if (interactKey == KeyCode.None && interactKey2 == KeyCode.None)
                {
                    PlayTimeline();
                }
                else
                {
                    //If both interact keys are hit then you play the timeline
                    if (activateTimelineInput && activateTimelineInput2)
                    {
                        PlayTimeline();
                        ToggleInteractUI(false);
                    }
                }

            }
            //If one player
            else if (both == false)
            {
                //If there is no interact key it plays the timeline on collision
                if (interactKey == KeyCode.None && interactKey2 == KeyCode.None)
                {
                    PlayTimeline();
                }
                else
                {
                    //If the interact key is hit then you play the timeline
                    if (activateTimelineInput)
                    {
                        PlayTimeline();
                        ToggleInteractUI(false);
                    }
                }
            }


        }
    }

    public void PlayTimeline()
    {
        if (setPlayerTimelinePosition)
        {
            //Sets the players possition durring the cutscene if stationary
            SetPlayerToTimelinePosition();
        }

        if (playableDirector)
        {
            //Plays the directer you set
            playableDirector.Play();

        }


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

        //Turns off movement
        ToggleInput(false);
        yield return new WaitForSeconds(timelineDuration);
        //After the duration turns it on
        ToggleInput(true);

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
        if (LoseLimb == true)
        {
            LoseLimbs();
        }



    }

    void LoseLimbs()
    {
        if (Player1 == true)
        {
            playerObject.GetComponent<SCR_TradeLimb>().DropDownLims(LimbToLose1);
        }

        if (Player2 == true)
        {
            playerObject2.GetComponent<SCR_TradeLimb>().DropDownLims(LimbToLose2);
        }
    }

    void ToggleInput(bool newState)
    {
        if (disablePlayerInput)
        {
            //Disables the players movement Depending on the cutscene
            playerObject.GetComponent<Movement_>().enabled = newState;
            playerObject2.GetComponent<Movement_>().enabled = newState;

            //Disables The players limb trading
            playerObject.GetComponent<SCR_TradeLimb>().enabled = newState;
            playerObject2.GetComponent<SCR_TradeLimb>().enabled = newState;

        }
    }


    void ToggleInteractUI(bool newState)
    {
        if (displayUI)
        {
            interactDisplay.SetActive(newState);
        }
    }

    void SetPlayerToTimelinePosition()
    {
        //Sets the players possition durring the cutscene if stationary
        playerObject.transform.position = playerTimelinePosition.position;
        playerObject.transform.localRotation = playerTimelinePosition.rotation;
        //Sets the players possition durring the cutscene if stationary
        playerObject2.transform.position = playerTimelinePosition.position;
        playerObject2.transform.localRotation = playerTimelinePosition.rotation;
    }

}