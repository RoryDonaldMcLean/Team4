﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Pushbox : MonoBehaviour
{

    private Animator anim;
    private bool isBlue;
    private int playerNum;

    private void Start()
    {
        isBlue = this.GetComponent<SCR_TradeLimb>().isBlue;
        anim = this.GetComponent<Animator>();
        playerNum = this.GetComponent<InControlMovement>().playerNum;
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
        var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
        if (other.gameObject.tag == "HugeBox")
        {
            if (this.GetArmQuantity() >= 2)
            {
                Vector3 boxPosition = other.gameObject.GetComponent<Transform>().position;
                if (Mathf.Abs(boxPosition.z - this.GetComponent<Transform>().position.z) > Mathf.Abs(boxPosition.x - this.GetComponent<Transform>().position.x))
                {
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
                    if (PushBoxZ(inputDevice, boxPosition, 0, 13))
                    {
                        anim.SetBool("IsPushing", true);
                    }
                    else
                    {
                        anim.SetBool("IsPushing", false);
                    }
                }
                else
                {
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    if (PushBoxX(inputDevice, boxPosition, 1, 14))
                    {
                        anim.SetBool("IsPushing", true);
                    }
                    else
                    {
                        anim.SetBool("IsPushing", false);
                    }
                }
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
            anim.SetBool("IsPushing", false);
        }
    }

    private bool Player1M(int btnIndex)
    {
        return Input.GetKey(GameManager.Instance.playerSetting.currentButton[btnIndex])
                && isBlue == GameManager.Instance.whichAndroid.player1ControlBlue;
    }

    private bool Player2M(int btnIndex)
    {
        return Input.GetKey(GameManager.Instance.playerSetting.currentButton[btnIndex])
                    && isBlue != GameManager.Instance.whichAndroid.player1ControlBlue;
    }

    private bool PushBoxX(InputDevice inputDevice, Vector3 boxPosition, int p1BtnIndex, int p2BtnIndex)
    {
        return ((Player1M(p1BtnIndex) || Player2M(p2BtnIndex) || (inputDevice != null && inputDevice.LeftStickX < 0)) && boxPosition.x < this.GetComponent<Transform>().position.x)
        || ((Player1M(p1BtnIndex + 2) || Player2M(p2BtnIndex + 2) || (inputDevice != null && inputDevice.LeftStickX > 0)) && boxPosition.x > this.GetComponent<Transform>().position.x);
    }

    private bool PushBoxZ(InputDevice inputDevice, Vector3 boxPosition, int p1BtnIndex, int p2BtnIndex)
    {
        return ((Player1M(p1BtnIndex) || Player2M(p2BtnIndex) || (inputDevice != null && inputDevice.LeftStickY > 0)) && boxPosition.z > this.GetComponent<Transform>().position.z)
        || ((Player1M(p1BtnIndex + 2) || Player2M(p2BtnIndex + 2) || (inputDevice != null && inputDevice.LeftStickY < 0)) && boxPosition.z < this.GetComponent<Transform>().position.z);
    }
}