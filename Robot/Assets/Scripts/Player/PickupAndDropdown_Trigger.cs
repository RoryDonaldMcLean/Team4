using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using InControl;

public class PickupAndDropdown_Trigger : MonoBehaviour
{
    #region private variable
    private bool holding;
    private GameObject pickedUpGameObject;
    private Animator anim;
    //private float alpha; //float For lerp

    private GameObject pickupLocation; //picking location
    
    private float offset;
    #endregion

    #region public variable
    public List<GameObject> triggerList = new List<GameObject>();


    public int playerNum;

    public bool isBlue = false;
    #endregion

    // Use this for initialization
    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
        holding = false;
        if (this.transform.parent.tag == "Player1")
        {
            playerNum = 0;
            isBlue = false;
        }
        else
        {
            playerNum = 1;
            isBlue = true;
        }
    }

    private void PickUpDropInterface()
    {
        var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;

        //keyboard
        if (inputDevice == null)
        {
            bool pickUp = ((PlayerOnePickUp(9)) || (PlayerTwoPickUp(22)));
            bool interact = ((PlayerOnePickUp(11)) || (PlayerTwoPickUp(24)));

            PickUpDropControl(pickUp, interact, null);
        }
        else //controllers
        {
            PickUpDropControl(inputDevice.Action2.WasPressed, inputDevice.Action4.WasPressed, inputDevice);
        }
    }

    private void PickUpDropControl(bool PickUp, bool Interact, InputDevice device)
    {
        if (!holding)
        {
            GameObject hit;
            if (PickUp)
            {
                PickUpObject(out hit);
            }
            else if (Interact)
            {
                InteractWithObject(out hit);
            }
        }
        else
        {
            ObjectHeld(PickUp, device);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        PickUpDropInterface();
        OutlinePickUpObjects();
    }

    private void OutlinePickUpObjects()
    {   
        if ((!holding) && (triggerList.Count > 0))
        {
            foreach (GameObject go in triggerList)
            {
                if (go.tag != "Untagged")
                {
                    //outline post process enable or activate
                    go.GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 1);
                }
            }
        }
        else if(holding)
        {
            foreach (GameObject go in triggerList)
            {
                if (go.tag != "Untagged")
                {
                    //cancle outline when pick up something
                    go.GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 0);
                }
            }
        }
    }

    private void ObjectHeld(bool pickUpState, InputDevice device)
    {
        if (pickedUpGameObject.transform.name.Contains("SlideBox"))
        {
            Vector3 temp = pickedUpGameObject.transform.position;
            if ((int)pickedUpGameObject.transform.parent.right.x == 0)
            {
                temp.z = this.transform.parent.position.z;
            }
            else
            {
                temp.x = this.transform.parent.position.x;
            }
            pickedUpGameObject.transform.position = temp;

            if (pickUpState)
            {
                Debug.Log("dropped on click");
                LimitDrop();
            }
        }
        else if (pickedUpGameObject.transform.name.Contains("RotateBox"))
        {
            Vector3 eulerAng = pickedUpGameObject.GetComponent<Transform>().rotation.eulerAngles;

            if (device == null)
            {
                bool left = RotationControl(1, 14);
                bool right = RotationControl(3, 16);

                float leftrot = left ? -1.0f : 0.0f;
                float rightrot = right ? 1.0f : 0.0f;

                pickedUpGameObject.transform.rotation = Quaternion.Euler(eulerAng.x, eulerAng.y + leftrot + rightrot, eulerAng.z);
            }
            else
            {
                pickedUpGameObject.transform.rotation = Quaternion.Euler(eulerAng.x, eulerAng.y + device.LeftStickX, eulerAng.z);
            }
            if (pickUpState)
            {
                RotateDrop();
                this.transform.parent.GetComponentInChildren<InControlMovement>().enabled = true;
            }
        }
        else
        {
            pickedUpGameObject.GetComponent<Transform>().position = pickupLocation.transform.position; // set the picking up object position
            pickedUpGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickedUpGameObject.GetComponent<Transform>().rotation, this.transform.parent.GetComponent<Transform>().rotation, 1); //make the rotation of object same as camera
            pickedUpGameObject.GetComponent<Transform>().rotation = new Quaternion(0, pickedUpGameObject.GetComponent<Transform>().rotation.y, 0, pickedUpGameObject.GetComponent<Transform>().rotation.w);

            if (pickUpState)
            {
                PutDownObject();
            }
        }
    }

    private bool RotationControl(int player1BtnIndex, int player2BtnIndex)
    {
       return (Input.GetKey(GameManager.Instance.playerSetting.currentButton[player1BtnIndex]) && isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
                || (Input.GetKey(GameManager.Instance.playerSetting.currentButton[player2BtnIndex]) && isBlue != GameManager.Instance.whichAndroid.player1ControlBlue);
    }

    private bool PlayerOnePickUp(int btnIndex)
    {
        return Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex])
                && isBlue == GameManager.Instance.whichAndroid.player1ControlBlue;
    }

    private bool PlayerTwoPickUp(int btnIndex)
    {
        return Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex])
                    && isBlue != GameManager.Instance.whichAndroid.player1ControlBlue;
    }

    private void PickUpObject(out GameObject hit)
    {
        if (ObjectFound(out hit))//ray cast detection
        {
            //if the object the player is trying to pick up is the SlideBox (object attached to the pole)
            if (hit.transform.name.Contains("SlideBox"))
            {
                pickedUpGameObject = hit.transform.gameObject;
                Vector3 temp = pickedUpGameObject.transform.position;
                if ((int)pickedUpGameObject.transform.parent.right.x == 0)
                {
                    temp.z = this.transform.parent.position.z;
                }
                else
                {
                    temp.x = this.transform.parent.position.x;
                }

                pickedUpGameObject.transform.position = temp;
                pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = true;
                pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().playerTag = this.transform.parent.tag;
                holding = true;

                anim.SetBool("IsLifting", true);
            }
            //if the object the player is trying to pick up is the RotateBox
            else if (hit.transform.name.Contains("RotateBox"))
            {
                pickedUpGameObject = hit.transform.gameObject;

                AkSoundEngine.PostEvent("Arm_Attach", gameObject);

                this.transform.parent.GetComponentInChildren<InControlMovement>().enabled = false;

                pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().pickedUp = true;
                pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().playerTag = this.transform.parent.tag;
                holding = true;
                anim.SetBool("IsLifting", true);

            }
            else
            {
                GenericPickUpCheck(ref hit);
            }
        }
    }

    private void InteractWithObject(out GameObject hit)
    {
        if (ObjectFound(out hit))//ray cast detection
        {
			if (hit.transform.name.Contains ("LimbLight"))
			{
				LimbLight limbLightBox = hit.transform.GetComponent<LimbLight> ();
				if (limbLightBox.IsLimbAttached ())
				{
					limbLightBox.RemoveLimbFromLightBox (this.transform.parent.tag);
				} else
				{
					limbLightBox.AttachLimbToLightBox (this.transform.parent.tag);
				}
			} else if (hit.transform.name.Contains ("LightEmitter"))
			{
				hit.transform.GetComponent<LightEmitter> ().InteractWithEmitter ();
				hit.transform.GetComponent<LightEmitter> ().switchedOn = !hit.transform.GetComponent<LightEmitter> ().switchedOn;
				Debug.Log ("light emitter hit");
			} else if (hit.transform.name.Contains ("RotateBox"))
			{
				if (hit.transform.parent.GetComponent<SCR_Rotatable> ().rotatableObjectString.Contains ("LightEmitter"))
				{
					hit.transform.GetComponent<LightEmitter> ().InteractWithEmitter ();
				}
			} 
			else if (hit.transform.name.Contains ("Switch"))
			{
				//Debug.Log ("you have indeed hit the switch. please don't hate me anymore :'(");
				hit.transform.GetComponentInChildren<TimelinePlaybackManager> ().PlayTimeline ();
			}

        }
    }

    public void LimitDrop()
    {
        pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = false;
        offset = 0;
        PutDownObject();
        AkSoundEngine.PostEvent("Place_Crystal", gameObject);

        anim.SetBool("IsLifting", false);
    }

    public void RotateDrop()
    {
        pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().pickedUp = false;
        offset = 0;
        PutDownObject();

        anim.SetBool("IsLifting", false);
    }

    private void GenericPickUpCheck(ref GameObject hit)
    {
        if ((hit.tag == "LightBox") && (GetArmQuantity() >= 1))
        {
            anim.SetBool("IsLifting", true);
            PickUpObject(hit.transform);
        }
        else if ((hit.tag == "HeavyBox") && (GetArmQuantity() >= 2))
        {
            anim.SetBool("IsLifting", true);
            PickUpObject(hit.transform);
        }
    }

    private void PutDownObject()
    {
        holding = false; //set pick up bool
        pickedUpGameObject.GetComponent<Transform>().position = new Vector3(pickedUpGameObject.GetComponent<Transform>().position.x, pickedUpGameObject.GetComponent<Transform>().position.y - offset, pickedUpGameObject.GetComponent<Transform>().position.z);
        pickedUpGameObject = null; //empty the pick up object
        Destroy(pickupLocation);

        AkSoundEngine.PostEvent("Place_Crystal", gameObject);
        anim.SetBool("IsLifting", false);
    }

    private void PickUpObject(Transform objectBeingPickedUp)
    {
        AkSoundEngine.PostEvent("PickUp_Crystal", gameObject);

        Debug.Log("at the pickup function");
        holding = true; //set pick up boolean
        pickedUpGameObject = objectBeingPickedUp.gameObject; // set the pick up object
        //alpha = 0;

        pickupLocation = Instantiate(Resources.Load("Prefabs/Light/PickLocation")) as GameObject;
        pickupLocation.transform.parent = this.transform.parent;
        pickupLocation.transform.localPosition = new Vector3(0, 1.5f /30.0f, 2.0f / 30.0f);
        offset = pickupLocation.transform.position.y - objectBeingPickedUp.position.y;

        float x = pickedUpGameObject.GetComponent<BoxCollider>().size.x / pickupLocation.transform.lossyScale.x;
        float y = pickedUpGameObject.GetComponent<BoxCollider>().size.y / pickupLocation.transform.lossyScale.y;
        float z = pickedUpGameObject.GetComponent<BoxCollider>().size.z / pickupLocation.transform.lossyScale.z;

        Vector3 colliderScale = new Vector3(x, y, z);
        pickupLocation.GetComponent<BoxCollider>().size = colliderScale;

        //PickCrystalSource.Play();
    }

    private int GetArmQuantity()
    {
        int quantity = 0;
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.parent.GetChild(i).name.Contains("area"))
            {
                //loop through all of that objects children, they should all be the hinges OR Limbs
                for (int u = 0; u < this.transform.parent.GetChild(i).childCount; u++)
                {
                    //find the object that has the "Arm" in it's name
                    Transform limb = this.transform.parent.GetChild(i).transform.GetChild(u);
                    if (limb.name.Contains("Arm") && (limb.gameObject.activeSelf))
                    {
                        quantity++;
                    }
                }
            }
        }
        return quantity;
    }

    private bool ObjectFound(out GameObject hit)
    {
        hit = null;
        if (triggerList.Count > 0)
        {
            int index = 0;
            float distance = Vector3.Distance(this.transform.parent.position, triggerList[0].transform.position);
            for (int i = 0; i < triggerList.Count; i++)
            {
                if (Vector3.Distance(this.transform.parent.position, triggerList[i].transform.position) < distance)
                {
                    distance = Vector3.Distance(this.transform.parent.position, triggerList[i].transform.position);
                    index = i;
                }
            }

            hit = triggerList[index];
        }

        return hit != null ? true : false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player1" && other.tag != "Player2" && other.tag != "Line")
            triggerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player1" && other.tag != "Player2" && other.tag != "Line")
            triggerList.Remove(other.gameObject);
        if (other.tag != "Untagged")
        {
            //disable when out of range 
            other.GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 0);
        }
    }
}
