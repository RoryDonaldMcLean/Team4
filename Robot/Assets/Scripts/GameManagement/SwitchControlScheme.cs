using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class SwitchControlScheme : MonoBehaviour 
{
	int playerNum;

	GameObject player1, player2;
	GameObject MelodyDoor;

	// Use this for initialization
	void Start () 
	{
		player1 = GameObject.FindGameObjectWithTag ("Player1");
		player2 = GameObject.FindGameObjectWithTag ("Player2");
		MelodyDoor = GameObject.FindGameObjectWithTag ("Door");
	}
	
	// Update is called once per frame
	void Update () 
	{
		

		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
			player1.GetComponent<Movement_> ().enabled =true;
			player1.GetComponent<InControlMovement> ().enabled = false;

			player2.GetComponent<Movement_> ().enabled =true;
			player2.GetComponent<InControlMovement> ().enabled = false;

			if (MelodyDoor == null)
			{
				//its fine
			} 
			else
			{
				MelodyDoor.GetComponent<SCR_Melody> ().enabled = true;
				MelodyDoor.GetComponent<InControlMelody> ().enabled = false;
			}

		}
		else
		{
			//controllers are plugged in
			player1.GetComponent<Movement_>().enabled = false;
			player1.GetComponent<InControlMovement> ().enabled = true;

			player2.GetComponent<Movement_> ().enabled =false;
			player2.GetComponent<InControlMovement> ().enabled = true;


			MelodyDoor.GetComponent<SCR_Melody> ().enabled = false;
			MelodyDoor.GetComponent<InControlMelody> ().enabled = true;
		}
	}
}
