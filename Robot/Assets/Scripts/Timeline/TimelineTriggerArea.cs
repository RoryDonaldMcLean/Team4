using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimelineTriggerArea : MonoBehaviour
{

    [Header("Component References")]
    public TimelinePlaybackManager timelinePlaybackManager;

    [Header("Settings")]
    public string playerString = "Crate";
    public string playerString2 = "Crate2";

    [Header("Switch Player Collision On")]
    public bool Switch = true;

    [Header("Exit Level")]
    public bool Exit = false;

    private int Collisions = 0;


    void OnTriggerEnter(Collider theCollision)
    {
       

        if (theCollision.name.Contains("Crate") || theCollision.name.Contains("Crate2"))
        {
            Debug.Log("" + Collisions);
            Collisions ++;
        }

        if (Collisions == 2)
        {
            timelinePlaybackManager.PlayerEnteredZone();
        }

        if (Switch == true)
        {
            if (theCollision.name.Contains("Player") || theCollision.name.Contains("Player2"))
            {
                timelinePlaybackManager.PlayerEnteredZone();
                timelinePlaybackManager.Switch = true;
            }
        }

        if (Exit == true)
        {
            if (theCollision.name.Contains("Player") || theCollision.name.Contains("Player2"))
            {
                timelinePlaybackManager.PlayerEnteredZone();
                
            }
        }

    }

    void OnTriggerExit(Collider theCollision)
    {
        timelinePlaybackManager.PlayerExitedZone();
    }

 
}
