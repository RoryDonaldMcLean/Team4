using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimelineTriggerArea : MonoBehaviour
{
    public TimelinePlaybackManager timelinePlaybackManager;
    public bool Switch = true;

    void OnTriggerEnter(Collider theCollision)
    {
        
        if (Switch == true)
        {
            if (theCollision.name.Contains("Player") || theCollision.name.Contains("Player2") || theCollision.gameObject.layer.Equals(LayerMask.NameToLayer("PlayerLayer")))
            {
                timelinePlaybackManager.Switch = true;
            }
        }
    }
}
