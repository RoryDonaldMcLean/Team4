using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndDropdown : MonoBehaviour
{
    private bool holding;
    private GameObject pickingGameObject;
    private float alpha; //float For lerp

    public Transform pickingLocation; //picking location

    public float pickingMaxDistance; //Max Distance

    private float offset;
    // Use this for initialization
    private void Start()
    {
        holding = false;
        alpha = 0;
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(this.ArmQuantity());
        if (!holding)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward * pickingMaxDistance, Color.red);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, out hit, pickingMaxDistance))//ray cast detection
                {
                    //Debug.Log("pick");
                    if (hit.collider.tag == "LightBox")
                    {
                        if (this.GetArmQuantity() >= 1)
                        {
                            
                            holding = true; //set pick up boolean
                            offset = pickingLocation.position.y - hit.collider.gameObject.GetComponent<Transform>().position.y;
                            pickingGameObject = hit.collider.gameObject; // set the pick up object
                            //Camera.main.GetComponent<Transform>().position += new Vector3(0, 1.0f, 0); //make the camera upper a little bit for brief 2
                            alpha = 0;
                        }
                    }

                    if (hit.collider.tag == "HeavyBox")
                    {
                        if (this.GetArmQuantity() >= 2)
                        {
                            holding = true; //set pick up boolean
                            offset = pickingLocation.position.y - hit.collider.gameObject.GetComponent<Transform>().position.y;
                            pickingGameObject = hit.collider.gameObject; // set the pick up object
                            //Camera.main.GetComponent<Transform>().position += new Vector3(0, 1.0f, 0); //make the camera upper a little bit for brief 2
                            alpha = 0;
                        }
                    }
                }
            }
        }

        else
        {
            if (alpha <= 1.0f)
                alpha += 0.001f;
            pickingGameObject.GetComponent<Transform>().position = pickingLocation.position; // set the picking up object position
            pickingGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickingGameObject.GetComponent<Transform>().rotation, this.GetComponent<Transform>().rotation, alpha); //make the rotation of object same as camera
            if (Input.GetMouseButtonDown(0))
            {
                holding = false; //set pick up bool
                pickingGameObject.GetComponent<Transform>().position = new Vector3(pickingGameObject.GetComponent<Transform>().position.x, pickingGameObject.GetComponent<Transform>().position.y - offset, pickingGameObject.GetComponent<Transform>().position.z);
                pickingGameObject = null; //empty the pick up object
                //Camera.main.GetComponent<Transform>().position += new Vector3(0, -1.0f, 0); //reset the camera location in brief 2
            }
        }
    }

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
                    if (this.transform.GetChild(i).transform.GetChild(u).name.Contains("Arm"))
                    {
                        quantity++;
                    }
                }
            }
        }
        return quantity;
    }
}
