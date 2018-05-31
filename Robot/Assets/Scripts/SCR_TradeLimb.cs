using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SCR_TradeLimb : MonoBehaviour 
{
	//array of Hinges for each Player
	//0 is left arm
	//1 is right arm
	//2 is left leg
	//3 is right leg
	//public Transform[] Hinges;

	//public GameObject leftArmPrefab;
	//GameObject leftArm;
	public List<GameObject> limbs = new List<GameObject>();

	//Used for the Xinput Plug in. Tracks the state of the controllers for player 1 and 2
	GamePadState state;
	GamePadState prevState;

	public GameObject p1, p2;

	// Use this for initialization
	void Start () 
	{
		//at the start of the game have the left arm be on top of the first hinge.
		//leftArm = (GameObject)Instantiate (leftArmPrefab, Hinges [0].position, Hinges [0].rotation);

		//make9 the left arm a child of the player
		//leftArm.transform.parent = gameObject.transform;

		childObjectLimbFinder();
	}

	private void childObjectLimbFinder()
	{
		//loop through all the child objects attached to player
		for(int i=0; i < this.transform.childCount; i++)
		{
			//find the object that has the "limb" in it's name
			if (this.transform.GetChild(i).name.Contains("area"))
			{
				//loop through all of that objects children, they should all be the hinges OR Limbs
				for(int u =0; u < this.transform.GetChild(i).childCount; u++)
				{
					//add each of them to the limbs list
					limbs.Add(this.transform.GetChild(i).GetChild(u).gameObject);
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//update the game controller
		prevState =state;
		state = GamePad.GetState (PlayerIndex.One);

		//when the trade limb button is pressed
		if (prevState.Buttons.Y == ButtonState.Released &&
			state.Buttons.Y == ButtonState.Pressed || Input.GetKey(KeyCode.K))
		{
			//Debug.Log ("123123");
			for (int i = 0; i < limbs.Count; i++)
			{
				if (limbs [i].name.Contains("Arm"))
				{
					//throwcode exchance limb
					//update limbs list (its a hinge again)
					Exchange(limbs[i], i);
				}
			}

			//else its a hinge, do nithuing!!!!!

		}


	}

	void Exchange(GameObject limbToTrade, int referenceNumber)
	{
		if (gameObject.tag == "Player1")
		{
			//limbs [0].GetComponent<LimsOwner> ().lim = LimsOwners.Player2;
			Transform otherPlayersLimb = GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[referenceNumber-4].transform;

			//Debug.Log (otherPlayersLimb);
			limbToTrade.GetComponent<LimsOwner>().limbTrade(otherPlayersLimb, GameObject.FindGameObjectWithTag("Player2").GetComponentsInChildren<Transform>()[2]);

		}



	}
}
