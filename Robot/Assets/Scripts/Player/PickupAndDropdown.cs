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


	public GameObject pole;
	Vector3 MaxDistance;
	Vector3 MinDistance;
	Vector3 PoleSize;
	float PoleOffset = 1.0f;


    // Use this for initialization
    private void Start()
    {
        holding = false;
        alpha = 0;

        PickCrystalSource.clip = PickCrystal;
        DropCrystalSource.clip = DropCrystal;


		MaxDistance = pole.GetComponent<Collider>().bounds.max;
		MinDistance = pole.GetComponent<Collider>().bounds.min;

		PoleSize = pole.GetComponent<Collider>().bounds.size;
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(this.ArmQuantity());
        if (!holding)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward * pickingMaxDistance, Color.red);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0))
            {
				//GameObject movable = GameObject.FindGameObjectWithTag ("Movable");
				//if (movable.GetComponent<SCR_Test> ().Entered == true)
				//{
					if (ObjectFound (out hit))//ray cast detection
					{
						if (hit.transform.name.Contains ("SlideBox"))
						{
							pickedUpGameObject = hit.transform.gameObject;
							//GenericPickUpCheck(ref hit);
							Vector3 temp = pickedUpGameObject.transform.position;
							temp.x = this.transform.position.x;
							//pickedUpGameObject.transform.position.x = temp.x;
							pickedUpGameObject.transform.position = temp;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Test> ().pickedUp = true;
							pickedUpGameObject.transform.parent.GetComponent<SCR_Test> ().playerTag = this.tag;
							holding = true;
							Debug.Log ("test");
						} 
						else
						{
							//Debug.Log ("hit");
							GenericPickUpCheck (ref hit);
						}
					}



            }
        }
        else
        {
			if (pickedUpGameObject.transform.name.Contains ("SlideBox"))
			{
				//sdsd movement stuff
				Vector3 temp = pickedUpGameObject.transform.position;
				temp.x = this.transform.position.x;
				pickedUpGameObject.transform.position = temp;

				if (Input.GetMouseButtonDown(0))
				{
					Debug.Log ("dropped on click");
					LimitDrop();
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
		pickedUpGameObject.transform.parent.GetComponent<SCR_Test>().pickedUp = false;
		PutDownObject();
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


	private void PickUpSlideBox(ref RaycastHit hit)
	{
		Transform objectBeingPickedUp = hit.transform;
		pickedUpGameObject = objectBeingPickedUp.gameObject; // set the pick up object
		holding = true;

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
        //tempPoss.y -= this.GetComponent<CapsuleCollider>().height / 4.0f;
        tempPoss -= this.GetComponent<Transform>().forward * 0.7f;

        //Debug.DrawRay(tempPoss, this.GetComponent<Transform>().forward, Color.red,  pickingMaxDistance);
        return Physics.BoxCast(tempPoss, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, pickingMaxDistance);
    }
}
