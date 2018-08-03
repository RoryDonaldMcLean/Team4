using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushbox : MonoBehaviour {
    
    private int GetArmQuantity()
    {
        int quantity = 0;
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.GetChild(i).name.Contains("area"))
            {
                //loop through all of that objects children, they should all be the hinges OR Limbs
                for (int u = 0; u < this.transform.GetChild(i).childCount; u++)
                {
                    //find the object that has the "Arm" in it's name
                    if (this.transform.GetChild(i).transform.GetChild(u).name.Contains("Arm") 
                        && this.transform.GetChild(i).transform.GetChild(u).gameObject.activeSelf)
                    {
                        quantity++;
                    }
                }
            }
        }
        return quantity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HugeBox")
        {
            if (this.GetArmQuantity() >= 2)
            {
                AkSoundEngine.PostEvent("Push_Box", gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HugeBox")
        {
            if (this.GetArmQuantity() >= 2)
            {
                Vector3 boxPosition = other.gameObject.GetComponent<Transform>().position;
                if (Mathf.Abs(boxPosition.z - this.GetComponent<Transform>().position.z) > Mathf.Abs(boxPosition.x - this.GetComponent<Transform>().position.x))
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
                else
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HugeBox")
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            AkSoundEngine.PostEvent("Push_Box_Stop", gameObject);
        }
    }
}
