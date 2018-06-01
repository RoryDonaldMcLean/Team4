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
    // Use this for initialization
    private void Start()
    {
        holding = false;
        alpha = 0;
        Debug.Log(this.GetComponent<Transform>().forward);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!holding)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(this.GetComponent<Transform>().position - new Vector3(0,1,0), this.GetComponent<Transform>().forward * pickingMaxDistance, Color.red);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(this.GetComponent<Transform>().position - new Vector3(0, 1, 0), this.GetComponent<Transform>().forward, out hit, pickingMaxDistance))//ray cast detection
                {
                    //Debug.Log("pick");
                    if (hit.collider.tag == "LightBox")
                    {
                        holding = true; //set pick up boolean
                        pickingGameObject = hit.collider.gameObject; // set the pick up object
                        //Camera.main.GetComponent<Transform>().position += new Vector3(0, 1.0f, 0); //make the camera upper a little bit for brief 2
                        alpha = 0;
                    }
                }
            }
        }

        else
        {
            if(alpha<=1.0f)
                alpha += 0.001f;
            pickingGameObject.GetComponent<Transform>().position = pickingLocation.position; // set the picking up object position
            pickingGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickingGameObject.GetComponent<Transform>().rotation, this.GetComponent<Transform>().rotation, alpha); //make the rotation of object same as camera
            if (Input.GetMouseButtonDown(0))
            {
                holding = false; //set pick up bool
                pickingGameObject = null; //empty the pick up object
                //Camera.main.GetComponent<Transform>().position += new Vector3(0, -1.0f, 0); //reset the camera location in brief 2
            }
        }
    }
}
