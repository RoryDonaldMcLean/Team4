using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndDropdown : MonoBehaviour
{
    private bool holding;
    private GameObject pickedUpGameObject;
    private float alpha; //float For lerp

    private GameObject pickupLocation; //picking location

    public float pickingMaxDistance; //Max Distance

    private float offset;

    public AudioClip PickCrystal;
    public AudioSource PickCrystalSource;

    public AudioClip DropCrystal;
    public AudioSource DropCrystalSource;


    // Use this for initialization
    private void Start()
    {
        holding = false;
        alpha = 0;
        PickCrystalSource.clip = PickCrystal;
        DropCrystalSource.clip = DropCrystal;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!holding)
        {
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
				if (ObjectFound (out hit))//ray cast detection
				{
					//if the object the player is trying to pick up is the SlideBox (object attached to the pole)
					if (hit.transform.name.Contains ("SlideBox"))
					{
						pickedUpGameObject = hit.transform.gameObject;
						Vector3 temp = pickedUpGameObject.transform.position;
						temp.x = this.transform.position.x;
						pickedUpGameObject.transform.position = temp;
						pickedUpGameObject.transform.parent.GetComponent<SCR_Movable> ().pickedUp = true;
						pickedUpGameObject.transform.parent.GetComponent<SCR_Movable> ().playerTag = this.tag;
						holding = true;
					} 
					else
					{
						GenericPickUpCheck (ref hit);
					}

					//if the object the player is trying to pick up is the RotateBox
					if (hit.transform.name.Contains ("RotateBox"))
					{
						pickedUpGameObject = hit.transform.gameObject;

						//when you "pick up" the box it will rotate to face the same direction as the player
						float speed = 200.0f;
						float step = speed * Time.deltaTime;
						pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
							this.transform.rotation, step);

						pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable> ().pickedUp = true;
						pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable> ().playerTag = this.tag;
						holding = true;
					} 
					else
					{
						GenericPickUpCheck (ref hit);
					}

				}
            }
        }
        else
        {
			if (pickedUpGameObject.transform.name.Contains ("SlideBox"))
			{
				Vector3 temp = pickedUpGameObject.transform.position;
				temp.x = this.transform.position.x;
				pickedUpGameObject.transform.position = temp;

				if (Input.GetMouseButtonDown (0))
				{
					Debug.Log ("dropped on click");
					LimitDrop ();
				}
			} 
			else if (pickedUpGameObject.transform.name.Contains ("RotateBox"))
			{
				float speed = 200.0f;
				float step = speed * Time.deltaTime;
				pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
					this.transform.rotation, step);

				if (Input.GetMouseButtonDown (0))
				{
					RotateDrop ();
				}
			}
			else
			{
				if (alpha <= 1.0f)
					alpha += 0.001f;

				pickedUpGameObject.GetComponent<Transform>().position = pickupLocation.transform.position; // set the picking up object position
				pickedUpGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickedUpGameObject.GetComponent<Transform>().rotation, this.GetComponent<Transform>().rotation, alpha); //make the rotation of object same as camera
				pickedUpGameObject.GetComponent<Transform>().rotation = new Quaternion(0, pickedUpGameObject.GetComponent<Transform>().rotation.y, 0, pickedUpGameObject.GetComponent<Transform>().rotation.w);

				if (Input.GetMouseButtonDown(0))
				{
					PutDownObject();
				}
			}
           
        }
    }

	public void LimitDrop()
	{
		pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = false;
		offset = 0;
		PutDownObject();
	}

	public void RotateDrop()
	{
		pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable> ().pickedUp = false;
		offset = 0;
		PutDownObject ();
	}

	private void GenericPickUpCheck(ref RaycastHit hit)
	{
		if ((hit.collider.tag == "LightBox") && (this.GetArmQuantity () >= 1))
		{
			PickUpObject (hit.transform);
		} else if ((hit.collider.tag == "HeavyBox") && (this.GetArmQuantity () >= 2))
		{
			PickUpObject (hit.transform);
		}
	}
		
    private void PutDownObject()
    {
        holding = false; //set pick up bool
        pickedUpGameObject.GetComponent<Transform>().position = new Vector3(pickedUpGameObject.GetComponent<Transform>().position.x, pickedUpGameObject.GetComponent<Transform>().position.y - offset, pickedUpGameObject.GetComponent<Transform>().position.z);
        pickedUpGameObject = null; //empty the pick up object
        Destroy(pickupLocation);
        DropCrystalSource.Play();
    }

    private void PickUpObject(Transform objectBeingPickedUp)
    {
        holding = true; //set pick up boolean
        pickedUpGameObject = objectBeingPickedUp.gameObject; // set the pick up object
        alpha = 0;

        pickupLocation = Instantiate(Resources.Load("Prefabs/Light/PickLocation"), this.transform) as GameObject;
        offset = pickupLocation.transform.position.y - objectBeingPickedUp.position.y;

        float x = pickedUpGameObject.GetComponent<BoxCollider>().size.x / pickupLocation.transform.lossyScale.x;
        float y = pickedUpGameObject.GetComponent<BoxCollider>().size.y / pickupLocation.transform.lossyScale.y;
        float z = pickedUpGameObject.GetComponent<BoxCollider>().size.z / pickupLocation.transform.lossyScale.z;

        Vector3 colliderScale = new Vector3(x, y, z);
        pickupLocation.GetComponent<BoxCollider>().size = colliderScale;
        pickupLocation.GetComponent<BeamPoint>().pickedUpTransform = pickedUpGameObject.transform;

        PickCrystalSource.Play();
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

    private bool ObjectFound(out RaycastHit hit)
    {
        Vector3 tempPoss = this.GetComponent<Transform>().position;
        tempPoss -= this.GetComponent<Transform>().forward * 0.7f;
        return Physics.BoxCast(tempPoss, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, pickingMaxDistance);
    }
}
