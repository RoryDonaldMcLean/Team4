using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using InControl;

public class PickupAndDropdown_Trigger : MonoBehaviour
{
    #region private variable
    private bool holding;
    private bool rotateGeneric = false;
    private GameObject pickedUpGameObject;
    private Animator anim;
    //private float alpha; //float For lerp

    private GameObject pickupLocation; //picking location
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
            bool rotateMode = ((PlayerOnePickUp(26)) || (PlayerTwoPickUp(27)));

            PickUpDropControl(pickUp, interact, rotateMode, null);
        }
        else //controllers
        {
            PickUpDropControl(inputDevice.Action2.WasPressed, inputDevice.Action4.WasPressed, inputDevice.Action3.WasPressed, inputDevice);
        }
    }

    public void DropObject()
    {
        if (rotateGeneric)
        {
            ToggleRotateState();
            CleanupRotateState();
        }
        else if (holding)
        {
            ObjectHeld(true, null);
           // AkSoundEngine.PostEvent("Turn_Stop", gameObject);

        }
    }

    private void PickUpDropControl(bool pickUp, bool interact, bool rotateMode, InputDevice device)
    {
        if (!holding)
        {
            GameObject hit;
            if (pickUp)
            {
                if (rotateGeneric)
                {
                    ToggleRotateState();
                    CleanupRotateState();
                }
                else
                {
                    PickUpObject(out hit);
                }
            }
            else if (rotateMode)
            {
                RotateImmediateState(out hit);
            }
            else if (interact)
            {
                InteractWithObject(out hit);
            }
            else if (rotateGeneric)
            {
                RotationExecution(ref device);
            }
        }
        else
        {
            ObjectHeld(pickUp, device);
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
            for (int i = 0; i < triggerList.Count; i++)
            {
                if (triggerList[i] == null)
                {
                    triggerList.RemoveAt(i);
                }
                else if (triggerList[i].tag != "Untagged")
                {
                    //outline post process enable or activate
                    triggerList[i].GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 1);
                }
            }
        }
        else if (holding)
        {
            for (int i = 0; i < triggerList.Count; i++)
            {
                if (triggerList[i] == null)
                {
                    triggerList.RemoveAt(i);
                }
                else if (triggerList[i].tag != "Untagged")
                {
                    //outline post process enable or activate
                    triggerList[i].GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 0);
                }
            }
        }
    }

    private void AnimStop()
    {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Carrying")) && (anim.speed == 1))
        {
            Rigidbody rb = this.transform.parent.GetComponent<Rigidbody>();
            if (Mathf.Abs(rb.velocity.x) <= 0.1 && Mathf.Abs(rb.velocity.z) <= 0.1)
            {
                anim.speed = 0;

                CancelInvoke("AnimPlay");
                InvokeRepeating("AnimPlay", 0.1f, 0.016666f);
            }
        }
    }

    private void AnimPlay()
    {
        Rigidbody rb = this.transform.parent.GetComponent<Rigidbody>();
        if (Mathf.Abs(rb.velocity.x) > 0.1 || Mathf.Abs(rb.velocity.z) > 0.1)
        {
            anim.speed = 1;
            CancelInvoke("AnimPlay");
        }
    }

    private void ObjectHeld(bool pickUpState, InputDevice device)
    {
        AnimStop();

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
            if (pickUpState)
            {
                RotateDrop();
                this.transform.parent.GetComponentInChildren<InControlMovement>().enabled = true;
            }
            else
            {
                RotationExecution(ref device);
            }
        }
        else
        {
            // set the picking up object position
            pickedUpGameObject.GetComponent<Transform>().position = pickupLocation.transform.position;
            pickedUpGameObject.GetComponent<Transform>().rotation = Quaternion.Lerp(pickedUpGameObject.GetComponent<Transform>().rotation, this.transform.parent.GetComponent<Transform>().rotation, 1); //make the rotation of object same as camera
            pickedUpGameObject.GetComponent<Transform>().rotation = new Quaternion(0, pickedUpGameObject.GetComponent<Transform>().rotation.y, 0, pickedUpGameObject.GetComponent<Transform>().rotation.w);


			//Rory added bullshit to try and prevent objects going through walls
			/*if (pickupLocation.transform.GetComponent<Rigidbody> () == null)
			{
				Rigidbody rb = pickupLocation.transform.gameObject.AddComponent (typeof(Rigidbody)) as Rigidbody;
				rb.useGravity = false;
				//rb.constraints = RigidbodyConstraints.FreezeAll;
				rb.constraints = RigidbodyConstraints.FreezeRotation;
				pickupLocation.layer = LayerMask.NameToLayer("WallDetect");

				rb.transform.position = pickupLocation.transform.position;
			}*/


            if (pickUpState)
            {
                PutDownObject();
                ToggleRotateState();
            }
        }
    }

    private void ToggleRotateState()
    {
        this.transform.parent.GetComponentInChildren<InControlMovement>().enabled = rotateGeneric;
        rotateGeneric = !rotateGeneric;
    }

    private void CleanupRotateState()
    {
        pickedUpGameObject = null; //empty the pick up object
        Destroy(pickupLocation);
        AkSoundEngine.PostEvent("Place_Crystal", gameObject);
        AkSoundEngine.PostEvent("Turn_Stop", gameObject);

        anim.SetBool("IsLifting", false);
    }

    private void RotationExecution(ref InputDevice device)
    {
        AnimStop();

        this.transform.parent.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        Vector3 eulerAng = pickedUpGameObject.GetComponent<Transform>().rotation.eulerAngles;

        if (device == null)
        {
            bool left = RotationControl(1, 14);
            bool right = RotationControl(3, 16);

            float leftrot = left ? -1.0f : 0.0f;
            float rightrot = right ? 1.0f : 0.0f;

            RotatingObjectAudio((left || right));

            pickedUpGameObject.transform.rotation = Quaternion.Euler(eulerAng.x, eulerAng.y + leftrot + rightrot, eulerAng.z);
        }
        else
        {
            RotatingObjectAudio(device.LeftStickX);

            pickedUpGameObject.transform.rotation = Quaternion.Euler(eulerAng.x, eulerAng.y + device.LeftStickX, eulerAng.z);
        }
    }

    private void RotatingObjectAudio(bool toggleControl)
    {
        if(toggleControl)
        {
            //play audio
            AkSoundEngine.PostEvent("Turn", gameObject);

        }
        else
        {
            //turn off audio
            AkSoundEngine.PostEvent("Turn_Stop", gameObject);

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

    private bool SamePickUpObject(GameObject hit)
    {
        PickupAndDropdown_Trigger[] p = new PickupAndDropdown_Trigger[2];
        p = FindObjectsOfType<PickupAndDropdown_Trigger>();
        PickupAndDropdown_Trigger other = p[0].playerNum == this.playerNum ? p[1] : p[0];
        return hit == other.GetPickObject();
    }

    private void RotateImmediateState(out GameObject hit)
    {
        if ((ObjectFound(out hit)) && (!rotateGeneric))
        {
            if ((!SamePickUpObject(hit)) && (!hit.transform.name.Contains("SlideBox")) && (!hit.transform.name.Contains("MelodyGate")) && (!hit.tag.Contains("Ground")))
            {
                if(GenericPickUpCheck(ref hit))
                {
                    holding = false;
                    ToggleRotateState();
                    CancelInvoke("AnimStop");
                    CancelInvoke("AnimPlay");
                    Invoke("AnimStop", 1.1f);
                }
            }
        }
        else if (rotateGeneric)
        {
            ToggleRotateState();
            CleanupRotateState();
        }
    }

    private void PickUpObject(out GameObject hit)
    {
        if (ObjectFound(out hit))//ray cast detection
        {
            if (!SamePickUpObject(hit))
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
    }

    private void InteractWithObject(out GameObject hit)
    {
        if (ObjectFound(out hit))//ray cast detection
        {
            if (hit.transform.name.Contains("LimbLight"))
            {
                LimbLight limbLightBox = hit.transform.GetComponent<LimbLight>();
                if (limbLightBox.IsLimbAttached())
                {
                    limbLightBox.RemoveLimbFromLightBox(this.transform.parent.tag);
                }
                else
                {
                    limbLightBox.AttachLimbToLightBox(this.transform.parent.tag);
                }
            }
            else if (hit.transform.name.Contains("LightEmitter"))
            {
                hit.transform.GetComponent<LightEmitter>().InteractWithEmitter();
                hit.transform.GetComponent<LightEmitter>().switchedOn = !hit.transform.GetComponent<LightEmitter>().switchedOn;
            }
            else if (hit.transform.name.Contains("RotateBox"))
            {
                if (hit.transform.parent.GetComponent<SCR_Rotatable>().rotatableObjectString.Contains("LightEmitter"))
                {
                    hit.transform.GetComponent<LightEmitter>().InteractWithEmitter();
                }
            }
            else if (hit.transform.name.Contains("Switch"))
            {
                Debug.Log("Switch");
                hit.transform.GetComponentInChildren<TimelinePlaybackManager>().PlayTimeline();
            }

        }
    }

    public void LimitDrop()
    {
        pickedUpGameObject.transform.parent.GetComponent<SCR_Movable>().pickedUp = false;
        PutDownObject();

        pickedUpGameObject = null; //empty the pick up object
        Destroy(pickupLocation);

        AkSoundEngine.PostEvent("Place_Crystal", gameObject);
        AkSoundEngine.PostEvent("Turn_Stop", gameObject);

        anim.SetBool("IsLifting", false);
    }

    public void RotateDrop()
    {
        pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().pickedUp = false;
        pickedUpGameObject.transform.parent.GetComponent<SCR_Rotatable>().playerTag = "Reset";
        PutDownObject();

        pickedUpGameObject = null; //empty the pick up object
        Destroy(pickupLocation);

        AkSoundEngine.PostEvent("Place_Crystal", gameObject);
        AkSoundEngine.PostEvent("Turn_Stop", gameObject);
        anim.SetBool("IsLifting", false);
    }

    private bool GenericPickUpCheck(ref GameObject hit)
    {
        if ((hit.tag == "LightBox") && (GetArmQuantity() >= 1))
        {
            anim.SetBool("IsLifting", true);
            PickUpObject(hit.transform);
            return true;
        }
        else if ((hit.tag == "HeavyBox") && (GetArmQuantity() >= 2))
        {
            anim.SetBool("IsLifting", true);
            PickUpObject(hit.transform);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PutDownObject()
    {
        holding = false; //set pick up bool
        RaycastHit hit;
        float y = int.MinValue;
        if (Physics.Raycast(new Vector3(pickedUpGameObject.GetComponent<Transform>().position.x,
            pickedUpGameObject.GetComponent<Transform>().position.y + 1, pickedUpGameObject.GetComponent<Transform>().position.z), -Vector3.up, out hit))
        {
            bool carry = false;
            foreach (Transform mc in pickedUpGameObject.GetComponentsInChildren<Transform>())
            {
                if (mc.name.Contains("CarryCrate"))
                {
                    carry = true;
                }
            }

            if (carry)
            {
                y = hit.point.y + 1;
            }
            else
            {
                y = hit.point.y;
            }
        }

        pickedUpGameObject.GetComponent<Transform>().position = new Vector3(pickedUpGameObject.GetComponent<Transform>().position.x,
            y, pickedUpGameObject.GetComponent<Transform>().position.z);
    }

    private void PickUpObject(Transform objectBeingPickedUp)
    {
        AkSoundEngine.PostEvent("PickUp_Crystal", gameObject);

        holding = true;
        pickedUpGameObject = objectBeingPickedUp.gameObject; // set the pick up object

        pickupLocation = Instantiate(Resources.Load("Prefabs/Light/PickLocation")) as GameObject;
        pickupLocation.transform.parent = this.transform.parent;
        pickupLocation.transform.localPosition = new Vector3(0, 1.5f / 30.0f, 2.0f / 30.0f);

        float x = pickedUpGameObject.GetComponent<BoxCollider>().size.x / pickupLocation.transform.lossyScale.x;
        float y = pickedUpGameObject.GetComponent<BoxCollider>().size.y / pickupLocation.transform.lossyScale.y;
        float z = pickedUpGameObject.GetComponent<BoxCollider>().size.z / pickupLocation.transform.lossyScale.z;

        Vector3 colliderScale = new Vector3(x, y, z);
        pickupLocation.GetComponent<BoxCollider>().size = colliderScale;

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

    public GameObject GetPickObject()
    {
        return pickedUpGameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player1" && other.tag != "Player2" && other.tag != "Line" && other.tag != "EndBeam")
            triggerList.Add(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player1" && other.tag != "Player2" && other.tag != "Line" && other.tag != "EndBeam")
        {   
            
            if(pickedUpGameObject != null)
            {
                if (pickedUpGameObject.name.Contains("RotateBox"))
                {
                    RotateDrop();
                    this.transform.parent.GetComponentInChildren<InControlMovement>().enabled = true;
                }
            }  
             
            triggerList.Remove(other.gameObject);
        }
        if (other.tag != "Untagged")
        {
            //disable when out of range 
            other.GetComponent<MeshRenderer>().material.SetFloat("_PickUpDetected", 0);
        }
    }
}
