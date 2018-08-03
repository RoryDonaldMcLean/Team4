using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class SCR_TradeLimb : MonoBehaviour
{
    #region varibles
    //UI elements for both players
    GameObject UILimbImage;
	GameObject UILimbImage2;

    public List<GameObject> limbs = new List<GameObject>();
    public List<MeshRenderer> hinges;

    private string otherPlayerTag = "";

    //Affects the particle systems on the children
    ParticleSystem[] childrenParticleSytems;
    bool disabledRelevantPSEmissions = false;
    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool isBlue = false;

    private GameObject leftArmFly, rightArmFly, leftLegFly, rightLegFly;

	List<GameObject> limbsUI = new List<GameObject>();
	List<GameObject> limbsUI2 = new List<GameObject>();

	public int playerNum;
    #endregion

    #region Monobehaviour function
    // Use this for initialization
    private void Start()
    {
        InitialisePlayerLimbs();
        SetPlayerTag();

        //get all the limbUI images, add them to a list and set them all to inactive to start
        UILimbImage = GameObject.FindGameObjectWithTag("UILimb");

        if (UILimbImage != null)
        {
            for (int i = 0; i < UILimbImage.transform.childCount; i++)
            {
                limbsUI.Add(UILimbImage.transform.GetChild(i).gameObject);
                limbsUI[i].SetActive(false);
            }
        }
        UILimbImage2 = GameObject.FindGameObjectWithTag("UILimb2");
        if (UILimbImage2 != null)
        {
            for (int i = 0; i < UILimbImage2.transform.childCount; i++)
            {
                limbsUI2.Add(UILimbImage2.transform.GetChild(i).gameObject);
                limbsUI2[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
        if (inputDevice == null)
        {
            //Debug.Log ("no controllers plugged in");
            ProcessInput();
        }
        else
        {
            ProcessInputInControl(inputDevice);
        }

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

        if (leftArmFly)
        {
            if (leftArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(leftArmFly);
                leftArmFly = null;
                Exchange("LeftArm", otherPlayerTag);
                AkSoundEngine.PostEvent("Arm_Attach", gameObject);
            }
        }

        if (rightArmFly)
        {
            if (rightArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightArmFly);
                rightArmFly = null;
                Exchange("RightArm", otherPlayerTag);
                AkSoundEngine.PostEvent("Arm_Attach", gameObject);
            }
        }

        if (leftLegFly)
        {
            if (leftLegFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(leftLegFly);
                leftLegFly = null;
                Exchange("LeftLeg", otherPlayerTag);
                AkSoundEngine.PostEvent("Arm_Attach", gameObject);
            }
        }
        if (rightLegFly)
        {
            if (rightLegFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightLegFly);
                rightLegFly = null;
                Exchange("RightLeg", otherPlayerTag);
                AkSoundEngine.PostEvent("Arm_Attach", gameObject);
            }
        }
    }

    #endregion
    
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

    private void childObjectHingeFinder()
    {
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.GetChild(i).name.Contains("mixamorig:Hips"))
            {
                MeshRenderer[] hingeObject = this.transform.GetChild(i).GetComponentsInChildren<MeshRenderer>(true);
                hinges = new List<MeshRenderer>(hingeObject);
            }
        }
    }

    private void InitialisePlayerLimbs()
    {
        childObjectLimbFinder();
        childObjectHingeFinder();
        LimbDetails();
    }

    private void SetPlayerTag()
    {
        InControlMovement[] players = GameObject.FindObjectsOfType<InControlMovement>();
        foreach (InControlMovement player in players)
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

    public void UICheck()
	{
		//player 1 
		//checks that the player has that limb, if so activate the correct UI element
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{
            List<GameObject> ls = !GameManager.Instance.whichAndroid.player1ControlBlue ? limbsUI : limbsUI2;

            if (limbs [0].activeSelf)
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UITopSegment_Arm_StateActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UITopSegment_Arm_StateNonActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [2].activeSelf)
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIBottomSegment_Arm_StateActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIBottomSegment_Arm_StateNonActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [1].activeSelf)
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UILeftSegment_Leg_StateActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UILeftSegment_Leg_StateNonActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [3].activeSelf)
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIRightSegment_Leg_StateActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIRightSegment_Leg_StateNonActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}
		} 
		else
		{
            List<GameObject> ls = GameManager.Instance.whichAndroid.player1ControlBlue ? limbsUI : limbsUI2;

            //player 2
            if (limbs [0].activeSelf)
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UITopSegment_Arm_StateActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UITopSegment_Arm_StateNonActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [2].activeSelf)
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIBottomSegment_Arm_StateActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIBottomSegment_Arm_StateNonActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [1].activeSelf)
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UILeftSegment_Leg_StateActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UILeftSegment_Leg_StateNonActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}

			if (limbs [3].activeSelf)
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIRightSegment_Leg_StateActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/UIRightSegment_Leg_StateNonActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}
		}
	}
		
    public void ProcessInput()
	{
		//update the game controller
		if((UILimbImage != null)&&(UILimbImage2 != null)) UICheck();

		////////////////////////////////////
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{	//player 1 controls
            GameObject ui = !GameManager.Instance.whichAndroid.player1ControlBlue ? UILimbImage : UILimbImage2;
            if (Input.GetKey (GameManager.Instance.playerSetting.currentButton[10]))
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
                if (ui != null) 
				{
					ui.transform.position = UIposition;
					ui.SetActive (true);
				}
				SpecificLimbExchange();
			} 
			else
			{
				if(ui != null) ui.SetActive (false);
			}
		}
		else
		{
			//player 2 controls here
            GameObject ui = GameManager.Instance.whichAndroid.player1ControlBlue ? UILimbImage : UILimbImage2;
            if (Input.GetKey (GameManager.Instance.playerSetting.currentButton[23]))
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
                if (ui != null) 
				{
					ui.transform.position = UIposition;
					ui.SetActive (true);
				}
				SpecificLimbExchange();
			}
			else
			{
				if(ui != null) ui.SetActive (false);
			}
		}
	}

	void ProcessInputInControl (InputDevice inputDevice)
	{
		if((UILimbImage != null)&&(UILimbImage2 != null)) UICheck();

		//UI stuff
		if (playerNum == 0)
		{
			if(inputDevice.LeftBumper.IsPressed)
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
				if (UILimbImage != null) 
				{
					UILimbImage.transform.position = UIposition;
					UILimbImage.SetActive (true);
				}

				SpecificLimbExchangeIncontrol(inputDevice);
			} 
			else
			{
				if (UILimbImage != null)
				{
					UILimbImage.SetActive (false);
				}
			}
		}

		if (playerNum == 1)
		{
			if(inputDevice.LeftBumper.IsPressed)
			{
				Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
				if (UILimbImage2 != null) 
				{
					UILimbImage2.transform.position = UIposition;
					UILimbImage2.SetActive (true);
				}

				SpecificLimbExchangeIncontrol(inputDevice);
			} 
			else
			{
				if (UILimbImage2 != null)
				{
					UILimbImage2.SetActive (false);
				}
			}
		}
	}

    private void SpecificLimbExchange()
    {
        GameObject targetPlayer = GameObject.FindGameObjectWithTag(otherPlayerTag);
        //this is what would change which limb you exchange
        //player 1 left arm
        if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
        {
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[4]))
            {

                if (limbs[0].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                {
                    //find the other player
                    LimFly("LeftArm", otherPlayerTag);
                    RemoveLimb("LeftArm");
                }
            }
            //player1 right arm
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[5]))
            {
                if (limbs[2].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                {
                    LimFly("RightArm", otherPlayerTag);
                    RemoveLimb("RightArm");
                }
            }
            //player1 left leg
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[6]))
            {
                if (limbs[1].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[1].activeSelf)
                {
                    //find the other player
                    LimFly("LeftLeg", otherPlayerTag);
                    RemoveLimb("LeftLeg");
                }
            }
            //player 1 right leg
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[7]))
            {
                if (limbs[3].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                {
                    LimFly("RightLeg", otherPlayerTag);
                    RemoveLimb("RightLeg");
                }
            }
        }
        else
        {
            //player2 left arm
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[17]))
            {
                if (limbs[0].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                {
                    LimFly("LeftArm", otherPlayerTag);
                    RemoveLimb("LeftArm");
                }
            }
            //player2 right arm
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[18]))
            {
                if (limbs[2].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                {
                    LimFly("RightArm", otherPlayerTag);
                    RemoveLimb("RightArm");
                }
            }  
            //player2 left leg
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[19]))
            {
                if (limbs[1].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                {
                    LimFly("LeftLeg", otherPlayerTag);
                    RemoveLimb("LeftLeg");
                }
            }
            //player2 right leg
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[20]))
            {
                if (limbs[3].activeSelf && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                {
                    LimFly("RightLeg", otherPlayerTag);
                    RemoveLimb("RightLeg");
                }
            }
        }
    }

	void SpecificLimbExchangeIncontrol(InputDevice inputDevice)
	{
		GameObject targetPlayer = GameObject.FindGameObjectWithTag(otherPlayerTag);

		//player  left arm
		if(inputDevice.RightStickY > 0.5f)
		{
			if (limbs[0].name.Contains("LeftArm") && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[0].name.Contains("LeftArm"))
			{
				//find the other player
				LimFly("LeftArm", otherPlayerTag);
				RemoveLimb("LeftArm");
			}
		}


		//player right arm
		if(inputDevice.RightStickY < -0.5f)
		{
			if (limbs[2].name.Contains("RightArm") && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[1].name.Contains("RightArm"))
			{
				LimFly("RightArm", otherPlayerTag);
				RemoveLimb("RightArm");
			}
		}

		//player1 left leg
		if(inputDevice.RightStickX < -0.5f)
		{
			if (limbs[1].name.Contains("LeftLeg") && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[2].name.Contains("LeftLeg"))
			{
				//find the other player
				LimFly("LeftLeg", otherPlayerTag);
				RemoveLimb("LeftLeg");
			}
		}

		//player right leg
		if(inputDevice.RightStickX > 0.5f)
		{
			if (limbs[3].name.Contains("RightLeg") && !targetPlayer.GetComponent<SCR_TradeLimb>().limbs[3].name.Contains("RightLeg"))
			{
				LimFly("RightLeg", otherPlayerTag);
				RemoveLimb("RightLeg");
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
            case "LeftLeg":
                limbNumber = 1;
                break;
            case "RightArm":
                limbNumber = 2;
                break;
            case "RightLeg":
                limbNumber = 3;
                break;
        }
        return limbNumber;
    }

    private int HingeNumber(string newLimbName)
    {
        int limbNumber = -1;
        switch (newLimbName)
        {
            case "LeftLeg":
                limbNumber = 0;
                break;
            case "RightLeg":
                limbNumber = 1;
                break;
            case "LeftArm":
                limbNumber = 2;
                break;
            case "RightArm":
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

    protected void Exchange(string newLimbName, string playerTag, bool start = false)
    {
        if(playerTag != this.tag)
        {
            GameObject.FindGameObjectWithTag(playerTag).GetComponent<SCR_TradeLimb>().ToggleLimb(newLimbName);
        }
        else
        {
            ToggleLimb(newLimbName);
        }

        if (!start) AkSoundEngine.PostEvent("Arm_Detatch", gameObject);
    }

    private void RemoveLimb(string limbToRemove)
    {
        ToggleLimb(limbToRemove);
    }		

    public void ToggleLimb(string limb)
    {
        int limbNumber = LimbNumber(limb);
        int hingeNumber = HingeNumber(limb);

        bool limbState = limbs[limbNumber].activeSelf;
        limbs[limbNumber].SetActive(!limbState);
        hinges[hingeNumber].gameObject.SetActive(limbState);
    }

    private void PickUpLims(GameObject pickUpObject)
    {
        string pickupName = pickUpObject.name;
        Destroy(pickUpObject);

        Exchange(pickupName, this.gameObject.tag);

		AkSoundEngine.PostEvent ("Arm_Attach", gameObject);
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
    
    private void OnTriggerStay(Collider other)
    {	//player 1
		if (other.gameObject.transform.parent != this.gameObject.transform)
		{
			//controller
			var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
			if (inputDevice == null)
			{
				//keyboard
				if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
				{//player 1
					if (other.name == "LeftArm" && Input.GetKey (GameManager.Instance.playerSetting.currentButton [9]) && !GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						PickUpLims (other.gameObject);
					if (other.name == "RightArm" && Input.GetKey (GameManager.Instance.playerSetting.currentButton [9]) && !GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						PickUpLims (other.gameObject);

					if (other.name == "LeftLeg" && Input.GetKey (GameManager.Instance.playerSetting.currentButton [9]) && !GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						PickUpLims (other.gameObject);
					if (other.name == "RightLeg" && Input.GetKey (GameManager.Instance.playerSetting.currentButton [9]) && !GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						PickUpLims (other.gameObject);
				} else
				{//player 2
					if (other.name == "LeftArm" && Input.GetKey(GameManager.Instance.playerSetting.currentButton[22]) && !GetComponent<SCR_TradeLimb>().limbs[0].name.Contains("LeftArm"))
					PickUpLims(other.gameObject);
					if (other.name == "RightArm" && Input.GetKey (GameManager.Instance.playerSetting.currentButton [22]) && !GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						PickUpLims (other.gameObject);
				if (other.name == "LeftLeg" && Input.GetKey(GameManager.Instance.playerSetting.currentButton[22]) && !GetComponent<SCR_TradeLimb>().limbs[2].name.Contains("LeftLeg"))
					PickUpLims(other.gameObject);
				if (other.name == "RightLeg" && Input.GetKey(GameManager.Instance.playerSetting.currentButton[22]) && !GetComponent<SCR_TradeLimb>().limbs[3].name.Contains("RightLeg"))
					PickUpLims(other.gameObject);
				}

			}
			else
			{	//controller
				if (other.name == "LeftArm" && inputDevice.Action2.IsPressed && !GetComponent<SCR_TradeLimb>().limbs[0].name.Contains("LeftArm"))
					PickUpLims(other.gameObject);
				if (other.name == "RightArm" && inputDevice.Action2.IsPressed && !GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
					PickUpLims(other.gameObject);
					
				if (other.name == "LeftLeg" && inputDevice.Action2.IsPressed && !GetComponent<SCR_TradeLimb>().limbs[2].name.Contains("LeftLeg"))
					PickUpLims(other.gameObject);
				if (other.name == "RightLeg" && inputDevice.Action2.IsPressed && !GetComponent<SCR_TradeLimb>().limbs[3].name.Contains("RightLeg"))
					PickUpLims(other.gameObject);
				
			}

		}
			
    }
}