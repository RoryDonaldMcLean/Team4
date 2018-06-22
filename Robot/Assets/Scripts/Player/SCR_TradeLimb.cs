using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class SCR_TradeLimb : MonoBehaviour
{
	//UI elements for both players
	GameObject UILimbImage;
	GameObject UILimbImage2;

    public List<GameObject> limbs = new List<GameObject>();

    GamePadState state;
    GamePadState prevState;
    GamePadState player2State;
    GamePadState player2PrevState;
    private string otherPlayerTag = "";


    //Affects the particle systems on the children
    ParticleSystem[] childrenParticleSytems;
    bool disabledRelevantPSEmissions = false;
    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    private GameObject leftArmFly, rightArmFly, leftLegFly, rightLegFly;

	List<GameObject> limbsUI = new List<GameObject>();
	List<GameObject> limbsUI2 = new List<GameObject>();
   
	// Use this for initialization
    private void Start()
    {
        InitialisePlayerLimbs();
        SetPlayerTag();
		//get all the limbUI images, add them to a list and set them all to inactive to start
		UILimbImage = GameObject.FindGameObjectWithTag ("UILimb");
		for (int i = 0; i < UILimbImage.transform.childCount; i++)
		{
			limbsUI.Add (UILimbImage.transform.GetChild (i).gameObject);
			limbsUI [i].SetActive (false);
		}


		UILimbImage2 = GameObject.FindGameObjectWithTag ("UILimb2");
		for (int i = 0; i < UILimbImage2.transform.childCount; i++)
		{
			limbsUI2.Add (UILimbImage2.transform.GetChild (i).gameObject);
			limbsUI2 [i].SetActive (false);
		}
    }

    private void childObjectLimbFinder()
    {
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.GetChild(i).name.Contains("area"))
            {
                //loop through all of that objects children, they should all be the hinges OR Limbs
                for (int u = 0; u < this.transform.GetChild(i).childCount; u++)
                {
                    //add each of them to the limbs list
                    limbs.Add(this.transform.GetChild(i).GetChild(u).gameObject);
                }
            }
        }
    }

    private void InitialisePlayerLimbs()
    {
        childObjectLimbFinder();
        LimbDetails();
    }

    private void SetPlayerTag()
    {
        Movement_[] players = GameObject.FindObjectsOfType<Movement_>();
        foreach (Movement_ player in players)
        {
            if (player.tag != this.transform.tag)
            {
                otherPlayerTag = player.tag;
            }
        }
    }

    protected virtual void LimbDetails()
    {
        //to be overwritten by inhertance
    }

    // Update is called once per frame
    public void Update()
    {
		ProcessInput ();

        childrenParticleSytems = gameObject.GetComponentsInChildren<ParticleSystem>();

        // Process each child's particle system and disable its emission module.
        // For each child, we disable all emission modules of its children.
        if (!disabledRelevantPSEmissions)
        {
            foreach (ParticleSystem childPS in childrenParticleSytems)
            {
                // Get the emission module of the current child particle system [childPS].
                ParticleSystem.EmissionModule childPSEmissionModule = childPS.emission;
                // Disable the child's emission module.
                childPSEmissionModule.enabled = false;

                // Get all particle systems from the children of the current child [childPS].
                ParticleSystem[] grandchildrenParticleSystems = childPS.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem grandchildPS in grandchildrenParticleSystems)
                {
                    // Get the emission module from the particle system of the current grand child.
                    ParticleSystem.EmissionModule grandchildPSEmissionModule = grandchildPS.emission;
                    // Disable the grandchild's emission module.
                    grandchildPSEmissionModule.enabled = false;
                }
            }
        }


        if(leftArmFly)
        {
            if(leftArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(leftArmFly);
                leftArmFly = null;
                Exchange("LeftArm", otherPlayerTag);
            }
        }

        if (rightArmFly)
        {
            if (rightArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightArmFly);
                rightArmFly = null;
                Exchange("RightArm", otherPlayerTag);
            }
        }

        if (leftLegFly)
        {
            if (leftLegFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(leftLegFly);
                leftLegFly = null;
                Exchange("LeftLeg", otherPlayerTag);
            }
        }
        if (rightLegFly)
        {
            if (rightLegFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightLegFly);
                rightLegFly = null;
                Exchange("RightLeg", otherPlayerTag);
            }
        }
    }

	public void UICheck()
	{
		//player 1 
		//checks that the player has that limb, if so activate the correct UI element
		if (player2)
		{
			if (limbs [0].name.Contains ("LeftArm"))
			{
				limbsUI [0].SetActive (true);
			} 
			else
			{
				limbsUI [0].SetActive (false);
			}

			if (limbs [1].name.Contains ("RightArm"))
			{
				limbsUI [1].SetActive (true);
			} 
			else
			{
				limbsUI [1].SetActive (false);
			}

			if (limbs [2].name.Contains ("LeftLeg"))
			{
				limbsUI [2].SetActive (true);
			} 
			else
			{
				limbsUI [2].SetActive (false);
			}

			if (limbs [3].name.Contains ("RightLeg"))
			{
				limbsUI [3].SetActive (true);
			} 
			else
			{
				limbsUI [3].SetActive (false);
			}
		} else
		{
			//player 2
			if (limbs [0].name.Contains ("LeftArm"))
			{
				limbsUI2 [0].SetActive (true);
			} 
			else
			{
				limbsUI2 [0].SetActive (false);
			}

			if (limbs [1].name.Contains ("RightArm"))
			{
				limbsUI2 [1].SetActive (true);
			} 
			else
			{
				limbsUI2 [1].SetActive (false);
			}

			if (limbs [2].name.Contains ("LeftLeg"))
			{
				limbsUI2 [2].SetActive (true);
			} 
			else
			{
				limbsUI2 [2].SetActive (false);
			}

			if (limbs [3].name.Contains ("RightLeg"))
			{
				limbsUI2 [3].SetActive (true);
			} 
			else
			{
				limbsUI2 [3].SetActive (false);
			}
		}
	}


	public void ProcessInput()
	{
		//update the game controller
		prevState = state;
		state = GamePad.GetState(PlayerIndex.One);
		player2PrevState = player2State;
		player2State = GamePad.GetState(PlayerIndex.Two);

		//////////////////////////////////////////////////////////////////////////
		//DropDown
		if (player2)
		{	//player 1 controls
			UICheck ();
			if (prevState.Buttons.RightShoulder == ButtonState.Pressed || Input.GetKey(KeyCode.Z))
			{
				if (prevState.ThumbSticks.Right.Y > 0.1f || Input.GetKey(KeyCode.Alpha1))
				{
					if (limbs[0].name.Contains("LeftArm"))
						DropDownLims("LeftArm");
				}
				if (prevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha2))
				{
					if (limbs[1].name.Contains("RightArm"))
						DropDownLims("RightArm");
				}
				if (prevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha4))
				{
					if (limbs[2].name.Contains("LeftLeg"))
						DropDownLims("LeftLeg");
				}
				if (prevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha3))
				{
					if (limbs[3].name.Contains("RightLeg"))
						DropDownLims("RightLeg");
				}
			}
		}
		else
		{	//player 2 controls
			UICheck ();
			if (player2PrevState.Buttons.RightShoulder == ButtonState.Released && player2State.Buttons.RightShoulder == ButtonState.Pressed || Input.GetKey(KeyCode.L))
			{
				if (player2PrevState.ThumbSticks.Right.Y > 0.1f || Input.GetKey(KeyCode.Alpha6))
				{
					if (limbs[0].name.Contains("LeftArm"))
						DropDownLims("LeftArm");
				}
				if (player2PrevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha7))
				{
					if (limbs[1].name.Contains("RightArm"))
						DropDownLims("RightArm");
				}
				if (player2PrevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha9))
				{
					if (limbs[2].name.Contains("LeftLeg"))
						DropDownLims("LeftLeg");
				}
				if (player2PrevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha8))
				{
					if (limbs[3].name.Contains("RightLeg"))
						DropDownLims("RightLeg");
				}
			}
		}
		////////////////////////////////////
		if (player2)
		{	//player 1 controls
			if ((prevState.Buttons.LeftShoulder == ButtonState.Pressed) || (Input.GetKey (KeyCode.LeftShift)))
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
				UILimbImage.transform.position = UIposition;
				UILimbImage.SetActive (true);
				SpecificLimbExchange ();
			} 
			else
			{
				UILimbImage.SetActive (false);
			}
		}
		else
		{
			//player 2 controls here
			if (player2PrevState.Buttons.LeftShoulder == ButtonState.Pressed || Input.GetKey (KeyCode.RightShift))
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
				UILimbImage2.transform.position = UIposition;
				UILimbImage2.SetActive (true);
				SpecificLimbExchange ();
			}
			else
			{
				UILimbImage2.SetActive (false);
			}
		}
	}


    private void SpecificLimbExchange()
    {
        //this is what would change which limb you exchange
        //player 1 left arm
		if (prevState.ThumbSticks.Right.Y > 0.1f || Input.GetKey(KeyCode.Alpha1))
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                //find the other player
                LimFly("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");
            }
        }

        //player2 left arm
		if (player2PrevState.ThumbSticks.Right.Y > 0.1f || Input.GetKey(KeyCode.Alpha6))
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                LimFly("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");
            }
        }


        //player1 right arm
		if (prevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha2))
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");
            }
        }

        //player2 right arm
		if (player2PrevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha7))
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");
            }
        }

		//player 1 left leg
		if (prevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha3))
        {
			if (limbs[3].name.Contains("RightLeg"))
			{
				LimFly("RightLeg", otherPlayerTag);
				RemoveLimb("RightLeg");
			}
        }

        //player2 right leg
		if (player2PrevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha8))
        {
			if (limbs[3].name.Contains("RightLeg"))
			{
				LimFly("RightLeg", otherPlayerTag);
				RemoveLimb("RightLeg");
			}
        }
			
        //player1 right leg
		if (prevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha4))
        {
			if (limbs[2].name.Contains("LeftLeg"))
			{
				//find the other player
				LimFly("LeftLeg", otherPlayerTag);
				RemoveLimb("LeftLeg");
			}
        }

        //player2 left leg
		if (player2PrevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha9))
        {
			if (limbs[2].name.Contains("LeftLeg"))
			{
				LimFly("LeftLeg", otherPlayerTag);
				RemoveLimb("LeftLeg");
			}
        }
    }

    private int LimbNumber(string newLimbName)
    {
        int limbNumber = -1;
        switch (newLimbName)
        {
            case "LeftArm":
                limbNumber = 0;
                break;
            case "RightArm":
                limbNumber = 1;
                break;
            case "LeftLeg":
                limbNumber = 2;
                break;
            case "RightLeg":
                limbNumber = 3;
                break;
        }
        return limbNumber;
    }

    public bool LimbLightGiveLimb(string typeOfLimbRequired, GameObject boxLimbObject)
    {
        for(int i = 0; i < limbs.Count; i++)
        {
            if(limbs[i].name.Contains(typeOfLimbRequired))
            {
                //give to box
                GameObject newLimb = Instantiate(Resources.Load("Prefabs/Player/" + limbs[i].name)) as GameObject;
                newLimb.name = limbs[i].name;
                newLimb.transform.position = boxLimbObject.transform.position;
                newLimb.transform.parent = boxLimbObject.transform.parent;

                newLimb.layer = LayerMask.NameToLayer("LightTrigger");
           
                Destroy(boxLimbObject);

                //remove limb
                RemoveLimb(limbs[i].name);

                return true;
            }
        }
        return false;        
    }

    public bool LimbLightTakeLimb(GameObject boxLimbLocation)
    {
        string nameOfLimbToRemoveFromBox = boxLimbLocation.name;
        int limbNumber = LimbNumber(nameOfLimbToRemoveFromBox);
        if(limbs[limbNumber].name.Contains("Hinge"))
        {
            Exchange(nameOfLimbToRemoveFromBox, this.gameObject.tag);
            GameObject hinge = Instantiate(Resources.Load("Prefabs/Player/Hinge")) as GameObject;
            hinge.transform.position = boxLimbLocation.transform.position;
            hinge.transform.parent = boxLimbLocation.transform.parent;
            hinge.name = "Hinge";


            hinge.GetComponent<ParticleSystem>().Stop();

            Destroy(boxLimbLocation);

            return true;
        }
        return false;
    }

    protected void Exchange(string newLimbName, string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        int limbNumber = LimbNumber(newLimbName);
        GameObject newLimb = Instantiate(Resources.Load("Prefabs/Player/" + newLimbName)) as GameObject;
        newLimb.name = newLimbName;
        List<GameObject> tempList = player.GetComponent<SCR_TradeLimb>().limbs;
        newLimb.transform.position = tempList[limbNumber].transform.position;
        newLimb.transform.parent = tempList[limbNumber].transform.parent;

        Destroy(tempList[limbNumber]);
        tempList.RemoveAt(limbNumber);

        tempList.Insert(limbNumber, newLimb);
    }

    private void RemoveLimb(string limbToRemove)
    {
        int limbNumber = LimbNumber(limbToRemove);
        GameObject hinge = Instantiate(Resources.Load("Prefabs/Player/Hinge")) as GameObject;
        hinge.name = "Hinge";
        List<GameObject> tempList = this.GetComponent<SCR_TradeLimb>().limbs;
        hinge.transform.position = tempList[limbNumber].transform.position;
        hinge.transform.parent = tempList[limbNumber].transform.parent;

        Destroy(tempList[limbNumber]);
        tempList.RemoveAt(limbNumber);

        tempList.Insert(limbNumber, hinge);
    }

    private void DropDownLims(string limbToRemove)
    {
        List<GameObject> tempList = this.GetComponent<SCR_TradeLimb>().limbs;
        int limbNumber = LimbNumber(limbToRemove);
        Vector3 dropDownLimsPosition = tempList[limbNumber].transform.position;
        RemoveLimb(limbToRemove);
        GameObject dropDownLims = Instantiate(Resources.Load("Prefabs/Player/" + limbToRemove)) as GameObject;

        dropDownLims.AddComponent<SphereCollider>();
        dropDownLims.AddComponent<Rigidbody>();
        dropDownLims.name = limbToRemove;
        dropDownLims.transform.position = dropDownLimsPosition;
    }

    private void PickUpLims(GameObject pickUpObject)
    {
        string pickupName = pickUpObject.name;
        Destroy(pickUpObject);
        Exchange(pickupName, this.gameObject.tag);
    }

    private void LimFly(string limName, string targetPlayerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetPlayerTag);
        int limbNumber = LimbNumber(limName);
        Vector3 p = this.limbs[limbNumber].GetComponent<Transform>().position;
        Quaternion q = this.limbs[limbNumber].GetComponent<Transform>().rotation;
        GameObject limToFly = Instantiate(Resources.Load("Prefabs/Player/" + limName), p, q) as GameObject;

		//while the object is flying. turn off its capusleCollider
		//if it has limb even has a capsuleCollider
		if (limToFly.GetComponent<CapsuleCollider> ())
		{	//if it does turn it off
			limToFly.GetComponent<CapsuleCollider> ().enabled = false;
		}

        limToFly.AddComponent<LimsFly>();
        limToFly.GetComponent<LimsFly>().SetStartPosition(this.limbs[limbNumber].GetComponent<Transform>().position);
        limToFly.GetComponent<LimsFly>().SetLimsNunber(limbNumber);
        limToFly.GetComponent<LimsFly>().SetTargetPlayer(player);

        switch (limName)
        {
            case "LeftArm":
                leftArmFly = limToFly;
                break;
            case "RightArm":
                rightArmFly = limToFly;
                break;
            case "LeftLeg":
                leftLegFly = limToFly;
                break;
            case "RightLeg":
                rightLegFly = limToFly;
                break;
        }
    }

    /// <summary>
    /// Pickup
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {	//player 1
        if (player2)
        {
			if (other.name == "LeftArm" && prevState.Buttons.B == ButtonState.Pressed || other.name == "LeftArm" && Input.GetKey(KeyCode.Q))
                PickUpLims(other.gameObject);
			if (other.name == "RightArm" && prevState.Buttons.B == ButtonState.Pressed || other.name == "RightArm" && Input.GetKey(KeyCode.Q))
                PickUpLims(other.gameObject);
			if (other.name == "LeftLeg" && prevState.Buttons.B == ButtonState.Pressed || other.name == "LeftLeg" && Input.GetKey(KeyCode.Q))
                PickUpLims(other.gameObject);
			if (other.name == "RightLeg" && prevState.Buttons.B == ButtonState.Pressed || other.name == "RightLeg" && Input.GetKey(KeyCode.Q))
                PickUpLims(other.gameObject);
        }
        else
        {	//player 2
			if (other.name == "LeftArm" && player2PrevState.Buttons.B == ButtonState.Pressed || other.name == "LeftArm" && Input.GetKey(KeyCode.K))
                PickUpLims(other.gameObject);
			if (other.name == "RightArm" && player2PrevState.Buttons.B == ButtonState.Pressed || other.name == "RightArm" && Input.GetKey(KeyCode.K))
                PickUpLims(other.gameObject);
			if (other.name == "LeftLeg" && player2PrevState.Buttons.B == ButtonState.Pressed || other.name == "LeftLeg" && Input.GetKey(KeyCode.K))
                PickUpLims(other.gameObject);
			if (other.name == "RightLeg" && player2PrevState.Buttons.B == ButtonState.Pressed || other.name == "RightLeg" && Input.GetKey(KeyCode.K))
                PickUpLims(other.gameObject);
        }
    }
}