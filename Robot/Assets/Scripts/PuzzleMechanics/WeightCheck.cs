using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightCheck : MonoBehaviour
{
    //There are four limbs in total, with a player potentially having all four at once, each limb represents one in weight. 
    private const int maximumWeight = 4;
    //Custom weight requirement for each unique pressure plate, defined in editor through the use of its public definition
    //allows different kinds of plates, that need a different amount of limbs to work. This system allows for dynamically
    //changing modular plates to exists ingame.
    [Range (0, maximumWeight)] 
    public int requiredWeight = 1;
    //Defined point for the pressure plate to go to during execution. Public var was used to allow for different depths for each
    //pressure plate. 
    public float bottomPoint = -0.62f;
    //Start and end points thatare used to control where the pressure plate go between during its execution 
    private Vector3 startPosition;
    private Vector3 endPosition;
    //Used to store the amount of limbs that were found on the player that hit the pressure plate.
    private int playersWeight = 0;

    //These checks control the execution of the pressure plates. 
    //checks to ensure that the plate has reached the floor/place of rest.
    private bool plateOnFloor = false;
    //checks to ensure that the plate has reached the start point, top point.
    private bool movePlateUp = false;
    //This check ensures that after triggering the plate, before it moves, that the player is still on the plate.
    private bool plateReadyToTrigger = false;

	public bool pressed = false;

    public AudioClip Plate;
    public AudioSource PlateSource;

    void Start()
    {
        startPosition = this.transform.position;
        endPosition = new Vector3(startPosition.x, bottomPoint, startPosition.z);

        PlateSource.clip = Plate;

    }

    //Upon a collison being detected with a player, the total limbs attached to that player is quantified.
    void OnCollisionEnter(Collision collider)
    {
        //y velocity check to ensure that the plate isnt moving, and therefore that the collison is a new unique one 
        if ((this.GetComponent<Rigidbody>().velocity.y == 0)&&(!plateOnFloor))
        {
            //checks to see if this player has the right limbs count to move the plate.
            if(PressurePlateWeightCheck(collider.gameObject))
            {
                //some time delay for dramatic effect
                Invoke("MovePlateDown", 1.0f);
                plateReadyToTrigger = true;
            }
        }
    }

    //Upon player leaving the pressure plate, this stop the plate when its reset back to its orignal place
    void OnCollisionExit(Collision collider)
    {    
        if (plateOnFloor) 
        {
            //checks to see if this player has the right limbs count to move the plate.
            if (PressurePlateWeightCheck(collider.gameObject))
            {
                movePlateUp = true;
            }
        }
        //if the player leaves before the plate has moved, prevent the plate from triggering.
        else if(plateReadyToTrigger)
        {
            plateReadyToTrigger = false;
        }
    } 

    private void MovePlateUp()
    {
      this.GetComponent<Rigidbody>().MovePosition(this.transform.position + this.transform.up * Time.deltaTime);

        PlateSource.Play();

    }

    //allows the plate to fall to the floor, using Unity physics components 
    private void MovePlateDown()
    {
        if (plateReadyToTrigger)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().isKinematic = false;

            PlateSource.Play();

        }
    }

    void Update()
    {
        if (this.transform.position.y > startPosition.y)
        {
            this.transform.position = startPosition;
            plateOnFloor = false;
            movePlateUp = false;
			pressed = false;
        }
        else if (this.transform.position.y < bottomPoint)
        {
            this.transform.position = endPosition;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            plateOnFloor = true;
			pressed = true;
			Debug.Log ("button has been pressed");
        }
        else if (movePlateUp) 
        {
            MovePlateUp();
        }
    }

    private bool PressurePlateWeightCheck(GameObject player)
    {
        CalculatePlayerWeight(player);
        return (playersWeight >= requiredWeight);
    }

    //Subject to change, the limbs are found based on a tag, as there are separate objects in the scene.
    //From there, the var that stores its owner is found, in order to discern the weight of the specific player
    private void CalculatePlayerWeight(GameObject player)
    {
        //resets weight value to ensure only new objects weight is factored. 
        playersWeight = 0;
        //finds all the limbs in the game

		//loop through all the child objects attached to player
		for (int i = 0; i < player.transform.childCount; i++)
		{
			if (player.transform.GetChild(i).name.Contains ("area"))
			{
				//loop through all of that objects children
				for (int u = 0; u < player.transform.GetChild (i).childCount; u++)
				{	//if one of those objects is not a hinge
					if(!player.transform.GetChild(i).GetChild(u).name.Contains("Hinge"))
					{
						//add weight i.e. if 1 limb is on weight is 1 if 4 limbs are there weight is 4
						playersWeight++;
					}
				}
			}
		}


        //GameObject[] limbs = GameObject.FindGameObjectsWithTag("Limb");
		/*
        //goes through all the limbs, in order to find which ones are on this player, in order to then find its current weight
        foreach (GameObject limb in limbs)
        {
           //string ownerName = limb.GetComponent<CharacterJoint>().connectedBody.name;

			string ownerName = limb.GetComponent<Transform> ().name;

            if (string.Compare(ownerName, player.name) == 0)
            {
                playersWeight++;
            }
        }
		*/
    }
}
