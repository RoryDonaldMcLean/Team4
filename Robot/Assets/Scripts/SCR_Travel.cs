using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Travel : MonoBehaviour 
{
	public Camera mainCamera;
	public Camera travelCamera;

	public GameObject p1, p2, endPoint, TravelArea;

	float distanceP1;
	float distanceP2;
	float SmallestDistance;
	public bool followP1;
	public bool followP2;
	public bool leftPuzzle;


	// Use this for initialization
	void Start () 
	{
		travelCamera.enabled = false;
		mainCamera.enabled = true;


	}
	
	// Update is called once per frame
	void Update () 
	{
		//get the distance between the end point and the players
		distanceP1 = (p1.transform.position - endPoint.transform.position).magnitude;
		distanceP2 = (p2.transform.position - endPoint.transform.position).magnitude;


		GetSmallestDistance ();

		if (SmallestDistance == distanceP1)
		{
			followP1 = true;
		} 
		else if (SmallestDistance == distanceP2)
		{
			followP2 = true;
		}
		

		if (leftPuzzle == true)
		{
			//follow player 1
			if (followP1 == true)
			{
				travelCamera.transform.position = 
					new Vector3 (travelCamera.transform.position.x,
						travelCamera.transform.position.y,
						p1.transform.position.z);
				followP1 = false;
			}
			//follow player 2
			if (followP2 == true)
			{
				travelCamera.transform.position = 
					new Vector3 (travelCamera.transform.position.x,
						travelCamera.transform.position.y,
						p2.transform.position.z);
				followP2 = false;
			}
		}
			
	}

	void GetSmallestDistance()
	{
		//find out which player is closest to the end point
		SmallestDistance = Mathf.Min (distanceP1, distanceP2);
	}


	void OnTriggerExit(Collider col)
	{
		if ((col.gameObject.tag == "Player1") || (col.gameObject.tag == "Player2"))
		{
			leftPuzzle = true;
			mainCamera.enabled = false;
			travelCamera.enabled = true;
			TravelArea.SetActive (true);
			//Debug.Log ("left puzzle area");
			//p1.GetComponent<Movement_>().enabled = false;
			//p2.GetComponent<Movement_> ().enabled = false;

			//prevent movement in the x axis when in travel mode
			//p1.GetComponent<Rigidbody> ().constraints &= ~RigidbodyConstraints.FreezePositionX;
			//p1.GetComponent<Rigidbody> ().constraints &= ~RigidbodyConstraints.FreezeRotationX;
			//p1.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationY;
			//p1.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationZ;

			//p2.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX;

		}
	}

	void OnTriggerEnter(Collider col)
	{
		if ((col.gameObject.tag == "Player1") || (col.gameObject.tag == "Player2"))
		{
			leftPuzzle = false;
			mainCamera.enabled = true;
			travelCamera.enabled = false;
			TravelArea.SetActive (false);
			//p1.GetComponent<Movement_>().enabled = true;
			//p2.GetComponent<Movement_> ().enabled = true;

			//p1.GetComponent<Rigidbody> ().constraints &= ~RigidbodyConstraints.FreezePositionX;

			//p1.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			//p2.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		}
	}
}
