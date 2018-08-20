using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene10_1 : BaseCutScene {

    private GameObject insidePlayer = null;

    protected override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
        GoInside(other, "Player1", ref insidePlayer);
        GoInside(other, "Player2", ref insidePlayer);
    }

    private void GoInside(Collision other, string tag, ref GameObject insidePlr)
    {
        int btnIndex = tag == "Player1" ? 9 : 22;
        GameObject player = tag == "Player1" ? p1 : p2;
        if (other.gameObject.tag == tag && !insidePlr)
        {
            if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex]))
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
                // AkSoundEngine.SetState("Environemnt", "Sacrifice");
                AkSoundEngine.PostEvent("Machine_Charge", gameObject);
            }
        }
    }

    public GameObject GetInsidePlayer()
    {
        return insidePlayer;
    }

}
