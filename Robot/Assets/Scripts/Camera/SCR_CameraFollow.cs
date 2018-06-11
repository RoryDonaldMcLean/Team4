using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraFollow : MonoBehaviour 
{
	Camera cam;
	Transform t1, t2, followThis;

	GameObject blockWall, endPoint;
	GameObject[] Travel;

	List<GameObject> CameraTransitionPoints = new List<GameObject>();

	GameObject[] CameraPuzzlePoints;

	float distanceP1, distanceP2;
	float smallestDistance;
	public bool followP1, followP2;
	public bool leftPuzzle;

	private bool endOfTravel = false;

	float timeTakenDuringLerp = 1.0f;
	bool isLerping = false;
	Vector3 startPosition;
	Vector3 endPosition;

	float timeStartedLerping;

	//the time it takes for the rotation to happen.
	float speed = 0.2f;

	float timeLeft = 2.0f;

	private bool delay = false;

	private int level = 0;

	// Use this for initialization
	void Start () 
	{
		cam = Camera.main;
		t1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		t2 = GameObject.FindGameObjectWithTag ("Player2").transform;

		Travel = GameObject.FindGameObjectsWithTag("Travel");
		if (Travel.Length > 0)
		{
			ChangeCameraPoints();
		}

		Invoke("DelayPuzzleLerp", 10.0f);
	}

	private void DelayPuzzleLerp()
	{
		delay = true;
	}

	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (smallestDistance);

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
		else
		{
            //get the distance between the end point and the players
            distanceP1 = (t1.transform.position - CameraTransitionPoints[2].transform.position).magnitude;
            distanceP2 = (t2.transform.position - CameraTransitionPoints[2].transform.position).magnitude;

            //get who ever is closest to the end point for the travel camera
            GetSmallestDistance();

            //if the shortest distance is player 1's distance, the camera 
            //should follow player 1.
            if (smallestDistance == distanceP1)
            {
                followP1 = true;
                //make the camera look at player1
                followThis = t1.transform;

            }
            else if (smallestDistance == distanceP2)
            {
                followP2 = true;
                //make the camera look at player2
                followThis = t2.transform;
            }

			if(!endOfTravel)
			{
				//if the players have reached close enough to the end point of the travel section
				if (smallestDistance <= 5.0f)
				{
					Debug.Log ("b0ss");
					ChangeCameraPoints();
					endOfTravel = true;
				}
			}
            //lerp camera to new position
            //camera will switch to the traveling camera which follows the leading player
            TravelCamera();
		}
	}
		
	void StartLerpingPuzzlePoint()
	{
		isLerping = true;
		endOfTravel = false;
		timeStartedLerping = Time.time;

		startPosition = cam.transform.position;
	
		endPosition = new Vector3 (CameraTransitionPoints [1].transform.position.x,
			CameraTransitionPoints [1].transform.position.y,
			CameraTransitionPoints [1].transform.position.z);

		//rotate to the same angle as the camera puzzle point
		cam.transform.rotation = Quaternion.Lerp (cam.transform.rotation, 
			CameraTransitionPoints [1].transform.rotation, Time.time * speed);

		leftPuzzle = false;

	}

	void StartLerpingTravelPoint()
	{
		isLerping = true;
		timeStartedLerping = Time.time;

		//lerp toward the CameraTransitionPoints
		startPosition = cam.transform.position;
		endPosition = new Vector3 (CameraTransitionPoints [0].transform.position.x,
			CameraTransitionPoints [0].transform.position.y,
			CameraTransitionPoints [0].transform.position.z);


		//rotate to the same angle as the travel point
		cam.transform.rotation = Quaternion.Lerp (cam.transform.rotation, 
			CameraTransitionPoints [0].transform.rotation, Time.time * speed);

		leftPuzzle = true;

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



	/// <Travel Camera Code>
	void GetSmallestDistance()
	{
		smallestDistance = Mathf.Min (distanceP1, distanceP2);
	}

	void TravelCamera()
	{
		if (leftPuzzle == true)
		{
			//follow player 1
			if (followP1 == true)
			{
				cam.transform.position = 
					new Vector3 (cam.transform.position.x,
						cam.transform.position.y,
						t1.transform.position.z);
				followP1 = false;
			}
			//follow player 2
			if (followP2 == true)
			{
				cam.transform.position = 
					new Vector3 (cam.transform.position.x,
						cam.transform.position.y,
						t2.transform.position.z);
				followP2 = false;
			}
		}
	}

	void FixedUpdate()
	{
		if (isLerping)
		{
			float timeSinceStarted = Time.time - timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			cam.transform.position = Vector3.Lerp (startPosition, endPosition, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				isLerping = false;
			}
		}
	}


	public void Exit()
	{
		StartLerpingTravelPoint();
	}

	public void Enter()
	{
		if (delay)
		{
			StartLerpingPuzzlePoint ();
			timeLeft = 2.0f;
		} 
		else
		{
			FixedCameraFollowSmooth (cam, t1, t2);
		}
	}

	void ChangeCameraPoints()
	{
		GameObject levelControl = GameObject.FindGameObjectWithTag ("GameController");
		CameraTransitionPoints.Clear ();

		level++;
		levelControl.GetComponent<LevelController>().Level(level);

		Transform[] meh = Travel[level-1].GetComponentsInChildren<Transform>();
		foreach(Transform travelPoint in meh)
		{
			if (travelPoint != Travel[level-1].transform)
			{
				//Debug.Log("sdsd" + travelPoint.transform.position);
				CameraTransitionPoints.Add (travelPoint.gameObject);
			}
		}	 
	}
}
