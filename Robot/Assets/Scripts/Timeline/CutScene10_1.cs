using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CutScene10_1 : BaseCutScene {

    private GameObject insidePlayer = null;

    protected override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
        var inputDevice1 = (InputManager.Devices.Count > 0) ? InputManager.Devices[0] : null;
        var inputDevice2 = (InputManager.Devices.Count > 1) ? InputManager.Devices[1] : null;
        GoInside(other, "Player1", ref insidePlayer, inputDevice1);
        GoInside(other, "Player2", ref insidePlayer, inputDevice2);
    }

    private void GoInside(Collision other, string tag, ref GameObject insidePlr, InputDevice device)
    {
        int btnIndex = tag == "Player1" ? 11 : 24;
        GameObject player = tag == "Player1" ? p1 : p2;
        if (other.gameObject.tag == tag && !insidePlr)
        {
            if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex]) && device == null)
            {
                GoInside(player);
            }
            else if (device != null && device.Action2)
            {
                GoInside(player);
            }
        }
    }

    private void GoInside(GameObject player)
    {
        player.transform.position = this.transform.position - new Vector3(0, 0.5f, 0);

        player.GetComponent<InControlMovement>().enabled = false;
        player.GetComponent<Chirps>().enabled = false;
        p1.GetComponent<SCR_player1Initalise>().enabled = false;
        p2.GetComponent<SCR_player2Initalise>().enabled = false;
        player.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = false;
        player.GetComponent<Animator>().SetBool("IsMoving", false);
        foreach (BoxCollider bc in player.GetComponents<BoxCollider>())
        {
            bc.enabled = false;
        }
        Destroy(player.GetComponent<Rigidbody>());
        insidePlayer = player;
        AkSoundEngine.SetState("Environment", "P6_EndSacrifice");
       // AkSoundEngine.PostEvent("Machine_Charge", gameObject);
       // AkSoundEngine.PostEvent("entermachine", gameObject);
    }

    public GameObject GetInsidePlayer()
    {
        return insidePlayer;
    }

}
