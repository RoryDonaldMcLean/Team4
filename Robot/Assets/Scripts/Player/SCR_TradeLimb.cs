using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SCR_TradeLimb : MonoBehaviour
{
    public List<GameObject> limbs = new List<GameObject>();

    GamePadState state;
    GamePadState prevState;
    GamePadState player2State;
    GamePadState player2PrevState;
    private string otherPlayerTag = "";

    //Delete on Monday
    public AudioClip ArmSwap;
    public AudioSource ArmSwapSource;

    public AudioClip ArmAttach;
    public AudioSource ArmAttachSource;

    //Affects the particle systems on the children
    ParticleSystem[] childrenParticleSytems;
    bool disabledRelevantPSEmissions = false;
    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    private GameObject leftArmFly, rightArmFly, leftLegFly, rightLegFly;
    // Use this for initialization
    private void Start()
    {
        InitialisePlayerLimbs();
        SetPlayerTag();

        ArmSwapSource.clip = ArmSwap;
        ArmAttachSource.clip = ArmAttach;

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
    private void Update()
    {

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

            //update the game controller
            prevState = state;
        state = GamePad.GetState(PlayerIndex.One);
        player2PrevState = player2State;
        player2State = GamePad.GetState(PlayerIndex.Two);

        //////////////////////////////////////////////////////////////////////////
        //DropDown
        if (player2)
        {	//player 1 controls
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
				if (prevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha3))
                {
                    if (limbs[2].name.Contains("LeftLeg"))
                        DropDownLims("LeftLeg");
                }
				if (prevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha4))
                {
                    if (limbs[3].name.Contains("RightLeg"))
                        DropDownLims("RightLeg");
                }
            }
        }
        else
        {	//player 2 controls
			if (player2PrevState.Buttons.RightShoulder == ButtonState.Pressed || Input.GetKey(KeyCode.L))
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
				if (player2PrevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha8))
                {
                    if (limbs[2].name.Contains("LeftLeg"))
                        DropDownLims("LeftLeg");
                }
				if (player2PrevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha9))
                {
                    if (limbs[3].name.Contains("RightLeg"))
                        DropDownLims("RightLeg");
                }
            }
        }
        ////////////////////////////////////
        if (player2)
        {	//player 1 controls
			if (prevState.Buttons.LeftShoulder == ButtonState.Pressed || 
				Input.GetKey(KeyCode.LeftShift))
            {
                SpecificLimbExchange();
            }
        }
        else
        {
            //player 2 controls here
			if (player2PrevState.Buttons.LeftShoulder == ButtonState.Pressed || 
				Input.GetKey(KeyCode.RightShift))
            {
                SpecificLimbExchange();
            }
        }

        if(leftArmFly)
        {
            if(leftArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(leftArmFly);
                leftArmFly = null;
                Exchange("LeftArm", otherPlayerTag);
                ArmAttachSource.Play();

            }
        }

        if (rightArmFly)
        {
            if (rightArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightArmFly);
                rightArmFly = null;
                Exchange("RightArm", otherPlayerTag);

                ArmAttachSource.Play();
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

                ArmSwapSource.Play();
            }
        }

        //player2 left arm
		if (player2PrevState.ThumbSticks.Right.Y > 0.1f || Input.GetKey(KeyCode.Alpha6))
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                LimFly("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");

                ArmSwapSource.Play();
            }
        }


        //player1 right arm
		if (prevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha2))
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");

                ArmSwapSource.Play();
            }
        }

        //player2 right arm
		if (player2PrevState.ThumbSticks.Right.Y < -0.1f || Input.GetKey(KeyCode.Alpha7))
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");

                ArmSwapSource.Play();
            }
        }

		if (prevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha3))
        {
            if (limbs[2].name.Contains("LeftLeg"))
            {
                //find the other player
                LimFly("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }

        //player2 left leg
		if (player2PrevState.ThumbSticks.Right.X > 0.1f || Input.GetKey(KeyCode.Alpha8))
        {
            if (limbs[2].name.Contains("LeftLeg"))
            {
                LimFly("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }


        //player1 right arm
		if (prevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha4))
        {

            if (limbs[3].name.Contains("RightLeg"))
            {
                LimFly("RightLeg", otherPlayerTag);
                RemoveLimb("RightLeg");
            }
        }

        //player2 right arm
		if (player2PrevState.ThumbSticks.Right.X < -0.1f || Input.GetKey(KeyCode.Alpha9))
        {
            if (limbs[3].name.Contains("RightLeg"))
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


    protected void Exchange(string newLimbName, string playerTag)
    {

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        int limbNumber = LimbNumber(newLimbName);
        GameObject newLimb = Instantiate(Resources.Load("Prefabs/Player/" + newLimbName)) as GameObject;
	
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

        ArmSwapSource.Play();

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
