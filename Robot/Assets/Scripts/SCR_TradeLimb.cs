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

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    // Use this for initialization
    private void Start()
    {
        InitialisePlayerLimbs();
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


    }

    private void SpecificLimbExchange()
    {
        string otherPlayerTag = "";
        Movement_[] players = GameObject.FindObjectsOfType<Movement_>();
        foreach (Movement_ player in players)
        {
            if (player.tag != this.transform.tag)
            {
                otherPlayerTag = player.tag;
            }
        }

        //this is what would change which limb you exchange
        //player 1 left arm
        if (prevState.ThumbSticks.Right.Y > 0.1f)
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                //find the other player
                Exchange("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");
            }
        }

        //player2 left arm
        if (player2PrevState.ThumbSticks.Right.Y > 0.1f)
        {
            if (limbs[0].name.Contains("LeftArm"))
            {
                Exchange("LeftArm", otherPlayerTag);
                RemoveLimb("LeftArm");
            }
        }
        
        //player1 right arm
        if (prevState.ThumbSticks.Right.Y < -0.1f)
        {

            if (limbs[1].name.Contains("RightArm"))
            {
                Exchange("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");
            }
        }

        //player2 right arm
        if (player2PrevState.ThumbSticks.Right.Y < -0.1f)
        {
            if (limbs[1].name.Contains("RightArm"))
            {
                Exchange("RightArm", otherPlayerTag);
                RemoveLimb("RightArm");
            }
        }

        //Leg
        //player 1 left Leg
        if (prevState.ThumbSticks.Right.X > 0.1f)
        {
            if (limbs[0].name.Contains("LeftLeg"))
            {
                //find the other player
                Exchange("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }

        //player2 left arm
        if (player2PrevState.ThumbSticks.Right.X > 0.1f)
        {
            if (limbs[0].name.Contains("LeftLeg"))
            {
                Exchange("LeftLeg", otherPlayerTag);
                RemoveLimb("LeftLeg");
            }
        }


        //player1 right arm
        if (prevState.ThumbSticks.Right.X < -0.1f)
        {

            if (limbs[1].name.Contains("RightLeg"))
            {
                Exchange("RightLeg", otherPlayerTag);
                RemoveLimb("RightLeg");
            }
        }

        //player2 right arm
        if (player2PrevState.ThumbSticks.Right.X < -0.1f)
        {
            if (limbs[1].name.Contains("RightLeg"))
            {
                Exchange("RightLeg", otherPlayerTag);
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
        GameObject newLimb = Instantiate(Resources.Load("Prefabs/" + newLimbName)) as GameObject;
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
        GameObject hinge = Instantiate(Resources.Load("Prefabs/Hinge")) as GameObject;

        List<GameObject> tempList = this.GetComponent<SCR_TradeLimb>().limbs;
        hinge.transform.position = tempList[limbNumber].transform.position;
        hinge.transform.parent = tempList[limbNumber].transform.parent;

        Destroy(tempList[limbNumber]);
        tempList.RemoveAt(limbNumber);

        tempList.Insert(limbNumber, hinge);
    }
}
