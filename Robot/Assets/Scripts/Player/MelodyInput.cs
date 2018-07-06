using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MelodyInput : MonoBehaviour 
{
	public int playerNum;

	GameObject MelodyDoor;

	// Use this for initialization
	void Start () 
	{
		MelodyDoor = GameObject.FindGameObjectWithTag ("Door");
	}
	
	// Update is called once per frame
	void Update () 
	{
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;

		if (inputDevice == null)
		{
			ProcessInput ();
		} 
		else
		{
			ProcessInputIncontrol (inputDevice);
		}
	}


	void ProcessInput()
	{

	}

	void ProcessInputIncontrol(InputDevice inputDevice)
	{
		//player 1
		if ((MelodyDoor.GetComponent<SCR_Door> ().Player1enteredBounds == true &&
		   MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Count <= 3))
		{
			if (playerNum == 0)
			{
				if (inputDevice.DPadUp.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (1);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} 
					else
					{
						Debug.Log ("Player1 has no left arm");
					}
				} 
				else if (inputDevice.DPadDown.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (2);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} 
					else
					{
						Debug.Log ("Player 1 has no right arm");
					}
				} 
				else if (inputDevice.DPadLeft.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (3);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} 
					else
					{
						Debug.Log ("Player 1 has no left leg");
					}
				}
				else if (inputDevice.DPadRight.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (4);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} 
					else
					{
						Debug.Log ("Player1 has no right leg");
					}
				}



			}
		}

		//player 2
		if ((MelodyDoor.GetComponent<SCR_Door> ().Player2enteredBounds == true &&
		    MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Count <= 3))
		{
			if (playerNum == 1)
			{
				if (inputDevice.DPadUp.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("PLayer2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (5);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} else
					{
						Debug.Log ("Player2 has no left arm");
					}
				} else if (inputDevice.DPadDown.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (6);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} else
					{
						Debug.Log ("Player2 has no right arm");
					}
				} else if (inputDevice.DPadLeft.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (7);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} else
					{
						Debug.Log ("Player2 has no left leg");
					}
				} else if (inputDevice.DPadRight.WasPressed)
				{
					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
					{
						MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (8);
						MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
					} else
					{
						Debug.Log ("Player2 has no right leg");
					}
				}
			}
		}


	}

}
