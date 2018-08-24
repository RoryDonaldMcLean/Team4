using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraFollow : MonoBehaviour 
{
	Camera cam;
	Transform t1, t2;

	GameObject blockWall, endPoint;
	GameObject[] Travel;

	List<GameObject> CameraTransitionPoints = new List<GameObject>();

	GameObject[] CameraPuzzlePoints;

	float distanceP1, distanceP2;
	float smallestDistance;
	public bool followP1, followP2;
	public bool leftPuzzle;

	Vector3 startPosition;
	Vector3 endPosition;

	//the time it takes for the rotation to happen.
	float timeLeft = 2.0f;

	private int level = 0;

	int levelCount;
	int MaxZoom;
	int MinZoom;

	// Use this for initialization
	void Start () 
	{
		levelCount = this.GetComponent<LevelController> ().currentLevel;
		cam = Camera.main;
		t1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		t2 = GameObject.FindGameObjectWithTag ("Player2").transform;

		Travel = GameObject.FindGameObjectsWithTag("Travel");
		if (Travel.Length > 0)
		{
			ChangeCameraPoints();
		}

	}
		

	// Update is called once per frame
	void Update () 
	{
		if (levelCount == 0)
		{
			MaxZoom = 29;
			MinZoom = 20;
				
		} else if (levelCount == 1)
		{
			MaxZoom = 32;
			MinZoom = 20;
		} else if (levelCount == 2)
		{
			MaxZoom = 32;
			MinZoom = 20;
		} else if (levelCount == 3)
		{
			MaxZoom = 32;
			MinZoom = 20;
		} else if (levelCount == 4)
		{
			MaxZoom = 32;
			MinZoom = 20;
		}

		//if you are still in the travel area
		if (leftPuzzle == false)
		{
			//bullshit temp way of doing it
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
			{
				//if you are still in the puzzle area use this method.
				FixedCameraFollowSmooth (cam, t1, t2);
			}
		} 

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

		//set a cap zoom in
		if (distance <= MinZoom)
		{
			distance = MinZoom;
		}
		//max zoom out
		if (distance >= MaxZoom)
		{
			distance = MaxZoom;
		}

		//Debug.Log ("camera distance: " + distance);

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


	void ChangeCameraPoints()
	{
		CameraTransitionPoints.Clear ();

		level++;

		Transform[] meh = Travel[level-1].GetComponentsInChildren<Transform>();
		foreach(Transform travelPoint in meh)
		{
			if (travelPoint != Travel[level-1].transform)
			{
				CameraTransitionPoints.Add (travelPoint.gameObject);
			}
		}	
	}
}
