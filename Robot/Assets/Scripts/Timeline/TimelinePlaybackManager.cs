using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;


public class TimelinePlaybackManager : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public GameObject triggerZoneObject;

    public bool Switch = false;

    private float timelineDuration;
    private GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("SwitchObject");
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

        if (Switch == true)
        {
            target.SetActive(false);
        }
			
    }

}
