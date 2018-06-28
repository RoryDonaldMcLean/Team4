using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTriggerArea : MonoBehaviour
{

    [Header("Component References")]
    public TimelinePlaybackManager timelinePlaybackManager;

    [Header("Settings")]
    public string playerString = "Crate";
    public string playerString2 = "Crate2";

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
        //else if (theCollision.name.Contains("Player") || theCollision.name.Contains("Player2"))
        //{
        //    timelinePlaybackManager.PlayerEnteredZone();
        //}
    }

    void OnTriggerExit(Collider theCollision)
    {
        timelinePlaybackManager.PlayerExitedZone();
    }

}
