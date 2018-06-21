using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCam : MonoBehaviour {

    //The Array Of backgrounds
    [SerializeField]ScrollingBackground[] scrollables;

    Vector2 lastFramePos;


	// Use this for initialization
	void Start ()
    {
        lastFramePos = new Vector2(transform.position.x, transform.position.y);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        //Checks the last frame according to the new frame to know if we have moved
        if (lastFramePos == curPos)
            return;

        Vector2 pos;
        pos = lastFramePos - curPos;
        pos.x = pos.x - (int)pos.x;
        pos.y = pos.y - (int)pos.y;

        //For each Background it scrolls there position
        foreach (ScrollingBackground s in scrollables)
            s.Scroll(pos);

        //Takes the last frame and checks it against the current pos
        lastFramePos = curPos;
	}
}
