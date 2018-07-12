using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using InControl;

public class PickupAndDropdown : MonoBehaviour
{
    private bool holding;
    private GameObject pickedUpGameObject;
    private float alpha; //float For lerp

    private GameObject pickupLocation; //picking location

    public float pickingMaxDistance; //Max Distance

    private float offset;

	public int playerNum;

    // Use this for initialization
    private void Start()
    {
        holding = false;
        alpha = 0;
    }

    // Update is called once per frame
    private void Update()
    {
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
			if (!holding)
			{
				RaycastHit hit;
				if(Input.GetKeyDown(KeyCode.E))
				{
					if (ObjectFound(out hit))//ray cast detection
					{             
						//if the object the player is trying to pick up is the SlideBox (object attached to the pole)
						if (hit.transform.name.Contains("SlideBox"))
						{
							pickedUpGameObject = hit.transform.gameObject;
							Vector3 temp = pickedUpGameObject.transform.position;
							if ((int)pickedUpGameObject.transform.right.x == 0) 
							{
								temp.z = this.transform.position.z;
							} 
							else 
							{
								temp.x = this.transform.position.x;
							}

							pickedUpGameObject.transform.position = temp;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = true;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().playerTag = this.tag;
							holding = true;
						} 
						//if the object the player is trying to pick up is the RotateBox
						else if (hit.transform.name.Contains("RotateBox"))
						{
							Debug.Log ("hit the rotate box");
							pickedUpGameObject = hit.transform.gameObject;

							//AkSoundEngine.PostEvent ("Arm_Attach", gameObject);

							//when you "pick up" the box it will rotate to face the same direction as the player
							float speed = 200.0f;
							float step = speed * Time.deltaTime;
							pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
								this.transform.rotation, step);

							pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().pickedUp = true;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().playerTag = this.tag;
							holding = true;
						} 
						else
						{
							GenericPickUpCheck (ref hit);
						}
					}
				}
				else if(Input.GetKeyDown(KeyCode.R))
				{
					if (ObjectFound (out hit))//ray cast detection
					{
						if (hit.transform.name.Contains("LimbLight")) 
						{
							LimbLight limbLightBox = hit.transform.GetComponent<LimbLight>();
							if (limbLightBox.IsLimbAttached()) 
							{
								limbLightBox.RemoveLimbFromLightBox (this.tag);
							} 
							else 
							{
								limbLightBox.AttachLimbToLightBox (this.tag);
							}
						} 
						else if (hit.transform.name.Contains("LightEmitter")) 
						{
							hit.transform.GetComponent<LightEmitter>().ToggleLight();
							hit.transform.GetComponent<LightEmitter>().switchedOn = !hit.transform.GetComponent<LightEmitter>().switchedOn;
						}
						else if (hit.transform.name.Contains("RotateBox")) 
						{
							if (hit.transform.parent.GetComponent<SCR_Rotatable>().rotatableObjectString.Contains("LightEmitter")) 
							{
								hit.transform.GetComponent<LightEmitter>().ToggleLight();
							}
						}
					}
				}
			}
			else
			{
				if (pickedUpGameObject.transform.name.Contains("SlideBox"))
				{
					Vector3 temp = pickedUpGameObject.transform.position;
					if ((int)pickedUpGameObject.transform.right.x == 0) 
					{
						temp.z = this.transform.position.z;
					} 
					else 
					{
						temp.x = this.transform.position.x;
					}
					pickedUpGameObject.transform.position = temp;

					if(Input.GetKeyDown(KeyCode.E))
					{
						Debug.Log("dropped on click");
						LimitDrop();
					}
				} 
				else if (pickedUpGameObject.transform.name.Contains("RotateBox"))
				{
					float speed = 200.0f;
					float step = speed * Time.deltaTime;
					pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
						this.transform.rotation, step);

					if(Input.GetKeyDown(KeyCode.E))
					{
						RotateDrop();
					}
				}
				else
				{
					if (alpha <= 1.0f)
						alpha += 0.001f;

					pickedUpGameObject.GetComponent<Transform>().position = pickupLocation.transform.position; // set the picking up object position
					pickedUpGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickedUpGameObject.GetComponent<Transform>().rotation, this.GetComponent<Transform>().rotation, alpha); //make the rotation of object same as camera
					pickedUpGameObject.GetComponent<Transform>().rotation = new Quaternion(0, pickedUpGameObject.GetComponent<Transform>().rotation.y, 0, pickedUpGameObject.GetComponent<Transform>().rotation.w);

					if (Input.GetKeyDown(KeyCode.E))
					{
						PutDownObject();
					}
				}

			}

		} 
		else
		{	//controllers
			if (!holding)
			{
				RaycastHit hit;
				if(inputDevice.Action2.WasPressed)
				{
					if (ObjectFound(out hit))//ray cast detection
					{             
						//if the object the player is trying to pick up is the SlideBox (object attached to the pole)
						if (hit.transform.name.Contains("SlideBox"))
						{
							pickedUpGameObject = hit.transform.gameObject;
							Vector3 temp = pickedUpGameObject.transform.position;
							if ((int)pickedUpGameObject.transform.right.x == 0) 
							{
								temp.z = this.transform.position.z;
							} 
							else 
							{
								temp.x = this.transform.position.x;
							}

							pickedUpGameObject.transform.position = temp;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = true;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().playerTag = this.tag;
							holding = true;
						} 
						//if the object the player is trying to pick up is the RotateBox
						else if (hit.transform.name.Contains("RotateBox"))
						{
							Debug.Log ("hit the rotate box");
							pickedUpGameObject = hit.transform.gameObject;

							//when you "pick up" the box it will rotate to face the same direction as the player
							float speed = 200.0f;
							float step = speed * Time.deltaTime;
							pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
								this.transform.rotation, step);

							pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().pickedUp = true;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().playerTag = this.tag;
							holding = true;
						} 
						else
						{
							GenericPickUpCheck (ref hit);
						}
					}
				}
				else if(inputDevice.Action4.WasPressed)
				{
					if (ObjectFound (out hit))//ray cast detection
					{
						if (hit.transform.name.Contains("LimbLight")) 
						{
							LimbLight limbLightBox = hit.transform.GetComponent<LimbLight>();
							if (limbLightBox.IsLimbAttached()) 
							{
								limbLightBox.RemoveLimbFromLightBox (this.tag);
							} 
							else 
							{
								limbLightBox.AttachLimbToLightBox (this.tag);
							}
						} 
						else if (hit.transform.name.Contains("LightEmitter")) 
						{
							hit.transform.GetComponent<LightEmitter>().ToggleLight();
							hit.transform.GetComponent<LightEmitter>().switchedOn = !hit.transform.GetComponent<LightEmitter>().switchedOn;
						}
						else if (hit.transform.name.Contains("RotateBox")) 
						{
							if (hit.transform.parent.GetComponent<SCR_Rotatable>().rotatableObjectString.Contains("LightEmitter")) 
							{
								hit.transform.GetComponent<LightEmitter>().ToggleLight();
							}
						}
					}
				}
			}
			else
			{
				if (pickedUpGameObject.transform.name.Contains("SlideBox"))
				{
					Vector3 temp = pickedUpGameObject.transform.position;
					if ((int)pickedUpGameObject.transform.right.x == 0) 
					{
						temp.z = this.transform.position.z;
					} 
					else 
					{
						temp.x = this.transform.position.x;
					}
					pickedUpGameObject.transform.position = temp;

					if(inputDevice.Action2.WasPressed)
					{
						Debug.Log("dropped on click");
						LimitDrop();
					}
				} 
				else if (pickedUpGameObject.transform.name.Contains("RotateBox"))
				{
					float speed = 200.0f;
					float step = speed * Time.deltaTime;
					pickedUpGameObject.transform.rotation = Quaternion.RotateTowards (pickedUpGameObject.transform.rotation,
						this.transform.rotation, step);

					if(inputDevice.Action2.WasPressed)
					{
						RotateDrop();
					}
				}
				else
				{
					if (alpha <= 1.0f)
						alpha += 0.001f;

					pickedUpGameObject.GetComponent<Transform>().position = pickupLocation.transform.position; // set the picking up object position
					pickedUpGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickedUpGameObject.GetComponent<Transform>().rotation, this.GetComponent<Transform>().rotation, alpha); //make the rotation of object same as camera
					pickedUpGameObject.GetComponent<Transform>().rotation = new Quaternion(0, pickedUpGameObject.GetComponent<Transform>().rotation.y, 0, pickedUpGameObject.GetComponent<Transform>().rotation.w);

					if (inputDevice.Action2.WasPressed)
					{
						PutDownObject();
					}
				}

			}
		}
			
    }

	public void LimitDrop()
	{
		pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = false;
		offset = 0;
		PutDownObject();
		//AkSoundEngine.PostEvent ("Place_Crystal", gameObject);

	}

	public void RotateDrop()
	{
		pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable> ().pickedUp = false;
		offset = 0;
		PutDownObject ();
		//AkSoundEngine.PostEvent ("Place_Crystal", gameObject);

	}

	private void GenericPickUpCheck(ref RaycastHit hit)
	{
		//Debug.Log ("generic pick up");
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

		//AkSoundEngine.PostEvent ("Place_Crystal", gameObject);

    }

    private void PickUpObject(Transform objectBeingPickedUp)
    {
		//AkSoundEngine.PostEvent ("PickUp_Crystal", gameObject);

		Debug.Log ("at the pickup function");
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


        //PickCrystalSource.Play();
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
		tempPoss -= this.GetComponent<Transform> ().forward * 0.3f;
        return Physics.BoxCast(tempPoss, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, pickingMaxDistance);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "HugeBox")
		{
			if (this.GetArmQuantity () >= 2)
			{
				//AkSoundEngine.PostEvent ("Push_Box", gameObject);
			}
		}
	}
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "HugeBox")
        {
            if(this.GetArmQuantity()>=2)
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
			//AkSoundEngine.PostEvent ("Push_Box_Stop", gameObject);

        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;

        float nearDistance = 1.0f;
      
        {
			Vector3 raycastStartLocation = this.GetComponent<Transform>().position;
			raycastStartLocation -= this.GetComponent<Transform> ().forward * 0.3f;

            //Check if there has been a hit yet
			if (Physics.BoxCast(raycastStartLocation, this.transform.lossyScale, this.GetComponent<Transform>().forward, out hit, Quaternion.identity, pickingMaxDistance))
            {
//                Debug.Log("?>LOP");
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                //Draw a cube that extends to where the hit exists
				Gizmos.DrawWireCube(raycastStartLocation + transform.forward * nearDistance, this.GetComponent<Transform>().lossyScale);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Debug.Log("sadasd");
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(transform.position, transform.forward * nearDistance);
                //Draw a cube at the maximum distance
				Gizmos.DrawWireCube(raycastStartLocation + transform.forward * nearDistance, this.GetComponent<Transform>().lossyScale);
            }
        }
    }
    
}
