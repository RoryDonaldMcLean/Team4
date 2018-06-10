using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_InPuzzle : MonoBehaviour 
{
	GameObject gameController;

	int leaveCounter = 0;

	int enterCounter = 0;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}

	void OnTriggerEnter(Collider col)
	{
		if ((col.gameObject.tag == "Player1") || (col.gameObject.tag == "Player2"))
		{
			leaveCounter = 0;
			enterCounter += 1;
			if (enterCounter >= 4)
			{
				gameController.GetComponent<SCR_CameraFollow> ().Enter ();
			}
		}
	}


	void OnTriggerExit(Collider col)
	{		
		if ((col.gameObject.tag == "Player1") || (col.gameObject.tag == "Player2"))
		{
			enterCounter = 0;
			leaveCounter +=1;
			//robots have two colliders attached to them thus the ontriggers will pop twice for each player
			if (leaveCounter >= 4)
			{
				gameController.GetComponent<SCR_CameraFollow> ().Exit ();
			}
		}
	}
}
