using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraFollow : MonoBehaviour 
{
	public Camera cam;
	public Transform t1, t2;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		FixedCameraFollowSmooth (cam, t1, t2);
	}


	public void FixedCameraFollowSmooth(Camera cam, Transform t1, Transform t2)
	{
		//how many units should we keep from the players
		float zoomFactor = 1.5f;
		float followTimeDelta = 0.8f;

		//midpoint we're after
		Vector3 midpoint = (t1.position + t2.position) / 2.0f;

		//Distance between objects
		float distance = (t1.position - t2.position).magnitude;

		//move camera certian distance
		Vector3 cameraDestination = midpoint - cam.transform.forward *distance*zoomFactor;

		//adjust ortho size i we're using one of those
		if (cam.orthographic)
		{
			//the camera's forward vector is irrelevant, only this size will matter
			cam.orthographicSize = distance;

		}

		//you specified to use MoveTowards instead of slerp
		cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

		//snap when close enough to prevent annoying slerp behavior
		if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
			cam.transform.position = cameraDestination;
	}
}
