using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightCheck : MonoBehaviour
{
    private Vector3 startPosition;

    //There are four limbs in total, with a player potentially having all four at once, each limb represents one in weight. 
    protected int maximumWeight = 4;
    protected int playersWeight = 0;

    void Start()
    {
        startPosition = this.transform.position;
    }

    //Upon a collison being detected with a player, the total limbs attached to that player is quantified.
    void OnCollisionEnter(Collision collision)
    {
        //y velocity check to ensure that the plate isnt moving, and therefore that the collison is a new unique one 
        if (this.GetComponent<Rigidbody>().velocity.y == 0)
        {
            CalculatePlayerWeight(collision.gameObject);
            WeightResponse();
        }
    }

    //Upon player leaving the pressure plate, this stop the plate when its reset back to its orignal place 
    void OnCollisionExit(Collision collision)
    {
		Debug.Log ("pressurePlate velocity.y= " + this.GetComponent<Rigidbody>().velocity.y);
        if(this.GetComponent<Rigidbody>().velocity.y == 0)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.5f, 0);
			Debug.Log ("hell");
            InvokeRepeating("MovePlateUpCheck", 0.0f, 0.1666f);
        }


    }

    private void MovePlateUpCheck()
    {
       if(this.transform.position.y >= startPosition.y)
        {
            CancelInvoke("MovePlateUpCheck");
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }


    protected virtual void WeightResponse()
    {
        //to be overwritten by the inherted class, that has specfic instructions for its specfic pressure plate.
    }

    //Subject to change, the limbs are found based on a tag, as there are sepearte objects in the scene.
    //From there, the var that stores its owner is found, in order to discern the weight of the specific player
    private void CalculatePlayerWeight(GameObject player)
    {
        //resets weight value to ensure only new objects weight is factored. 
        playersWeight = 0;
        //finds all the limbs in the game
        GameObject[] limbs = GameObject.FindGameObjectsWithTag("Limb");

        //goes through all the limbs, in order to find which ones are on this player, in order to then find its current weight
        foreach (GameObject limb in limbs)
        {
            string ownerName = limb.GetComponent<CharacterJoint>().connectedBody.name;

            if (string.Compare(ownerName, player.name) == 0)
            {
                playersWeight++;
            }
        }
    }
}
