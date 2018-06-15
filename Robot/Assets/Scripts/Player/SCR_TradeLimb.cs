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
    //public AudioClip ArmSwap;
    //public AudioSource ArmSwapSource;

    //public AudioClip ArmAttach;
    //public AudioSource ArmAttachSource;

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    private GameObject leftArmFly, rightArmFly, leftLegFly, rightLegFly;
    // Use this for initialization
    private void Start()
    {
        InitialisePlayerLimbs();
        SetPlayerTag();

        //ArmSwapSource.clip = ArmSwap;
        //ArmAttachSource.clip = ArmAttach;
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
        //update the game controller
        prevState = state;
        state = GamePad.GetState(PlayerIndex.One);
        player2PrevState = player2State;
        player2State = GamePad.GetState(PlayerIndex.Two);

        //////////////////////////////////////////////////////////////////////////
        //DropDown
        if (player2)
        {
            if (prevState.Buttons.X == ButtonState.Pressed)
            {
                if (prevState.ThumbSticks.Right.Y > 0.1f)
                {
                    if (limbs[0].name.Contains("LeftArm"))
                        DropDownLims("LeftArm");
                }
                if (prevState.ThumbSticks.Right.Y < -0.1f)
                {
                    if (limbs[1].name.Contains("RightArm"))
                        DropDownLims("RightArm");
                }
                if (prevState.ThumbSticks.Right.X > 0.1f)
                {
                    if (limbs[2].name.Contains("LeftLeg"))
                        DropDownLims("LeftLeg");
                }
                if (prevState.ThumbSticks.Right.X < -0.1f)
                {
                    if (limbs[3].name.Contains("RightLeg"))
                        DropDownLims("RightLeg");
                }
            }
        }
        else
        {
            if (player2PrevState.Buttons.X == ButtonState.Pressed)
            {
                if (player2PrevState.ThumbSticks.Right.Y > 0.1f)
                {
                    if (limbs[0].name.Contains("LeftArm"))
                        DropDownLims("LeftArm");
                }
                if (player2PrevState.ThumbSticks.Right.Y < -0.1f)
                {
                    if (limbs[1].name.Contains("RightArm"))
                        DropDownLims("RightArm");
                }
                if (player2PrevState.ThumbSticks.Right.X > 0.1f)
                {
                    if (limbs[2].name.Contains("LeftLeg"))
                        DropDownLims("LeftLeg");
                }
                if (player2PrevState.ThumbSticks.Right.X < -0.1f)
                {
                    if (limbs[3].name.Contains("RightLeg"))
                        DropDownLims("RightLeg");
                }
            }
        }
        ////////////////////////////////////
        if (player2)
        {
            //when the trade limb button is pressed
            /*if (prevState.Buttons.Y == ButtonState.Released &&
			    state.Buttons.Y == ButtonState.Pressed || Input.GetKey (KeyCode.K))
			{
				//Debug.Log ("123123");
				SpecificLimbExchange ();
			
			}*/

            if (prevState.Buttons.Y == ButtonState.Pressed)
            {
                SpecificLimbExchange();
            }
        }
        else
        {
            //player 2 controls here
            if (player2PrevState.Buttons.Y == ButtonState.Pressed)
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

                //ArmAttachSource.Play();
            }
        }

        if (rightArmFly)
        {
            if (rightArmFly.GetComponent<LimsFly>().GetFinish())
            {
                Destroy(rightArmFly);
                rightArmFly = null;
                Exchange("RightArm", otherPlayerTag);

                //ArmAttachSource.Play();
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
        if (prevState.ThumbSticks.Right.Y > 0.1f)
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                //find the other player
                //Exchange("LeftArm", otherPlayerTag);
                LimFly("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");

                //ArmSwapSource.Play();
            }
        }

        //player2 left arm
        if (player2PrevState.ThumbSticks.Right.Y > 0.1f)
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                //Exchange("LeftArm", otherPlayerTag);
                LimFly("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");

                //ArmSwapSource.Play();
            }
        }


        //player1 right arm
        if (prevState.ThumbSticks.Right.Y < -0.1f)
        {

            if (limbs[1].name.Contains("RightArm"))
            {
                //Exchange("RightArm", otherPlayerTag);
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");

                //ArmSwapSource.Play();
            }
        }

        //player2 right arm
        if (player2PrevState.ThumbSticks.Right.Y < -0.1f)
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                //Exchange("RightArm", otherPlayerTag);
                LimFly("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");

                //ArmSwapSource.Play();
            }
        }

        if (prevState.ThumbSticks.Right.X > 0.1f)
        {
            if (limbs[2].name.Contains("LeftLeg"))
            {
                //find the other player
                //Exchange("LeftArm", otherPlayerTag);
                LimFly("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }

        //player2 left arm
        if (player2PrevState.ThumbSticks.Right.X > 0.1f)
        {
            if (limbs[2].name.Contains("LeftLeg"))
            {
                //Exchange("LeftLeg", otherPlayerTag);
                LimFly("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }


        //player1 right arm
        if (prevState.ThumbSticks.Right.X < -0.1f)
        {

            if (limbs[3].name.Contains("RightLeg"))
            {
                //Exchange("RightLeg", otherPlayerTag);
                LimFly("RightLeg", otherPlayerTag);
                RemoveLimb("RightLeg");
            }
        }

        //player2 right arm
        if (player2PrevState.ThumbSticks.Right.X < -0.1f)
        {
            if (limbs[3].name.Contains("RightLeg"))
            {
                //Exchange("RightLeg", otherPlayerTag);
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

        //ArmSwapSource.Play();
    }

    private void LimFly(string limName, string targetPlayerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetPlayerTag);
        int limbNumber = LimbNumber(limName);
        Vector3 p = this.limbs[limbNumber].GetComponent<Transform>().position;
        Quaternion q = this.limbs[limbNumber].GetComponent<Transform>().rotation;
        GameObject limToFly = Instantiate(Resources.Load("Prefabs/Player/" + limName), p, q) as GameObject;

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
    {
        if (player2)
        {
            if (other.name == "LeftArm" && prevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "RightArm" && prevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "LeftLeg" && prevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "RIghtLeg" && prevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
        }
        else
        {
            if (other.name == "LeftArm" && player2PrevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "RightArm" && player2PrevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "LeftLeg" && player2PrevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
            if (other.name == "RIghtLeg" && player2PrevState.Buttons.B == ButtonState.Pressed)
                PickUpLims(other.gameObject);
        }
    }
}
