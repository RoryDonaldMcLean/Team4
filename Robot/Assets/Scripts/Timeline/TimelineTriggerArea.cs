using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTriggerArea : MonoBehaviour
{

    [Header("Component References")]
    public TimelinePlaybackManager timelinePlaybackManager;

    [Header("Settings")]
    public string playerString = "Player1";
    public string playerString2 = "Player1";


    void OnTriggerEnter(Collider theCollision)
    {
        if (theCollision.gameObject.name == "Crate" && theCollision.gameObject.name == "Crate2")
        {
            timelinePlaybackManager.PlayerEnteredZone();
        }
        else
        {
            timelinePlaybackManager.PlayerEnteredZone();
        }
    }

    void OnTriggerExit(Collider theCollision)
    {
        timelinePlaybackManager.PlayerExitedZone();
    }
}
