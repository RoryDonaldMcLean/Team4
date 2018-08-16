using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene10_2 : BaseCutScene {

    public GameObject Tube;

    protected override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
        GameObject otherPlayer = Tube.GetComponent<CutScene10_1>().GetInsidePlayer();
        
        if (otherPlayer)
        {
            GameObject controlPlayer = otherPlayer.tag == "Player1" ? p2 : p1;

            int btnIndex = otherPlayer.tag == "Player1" ? 22 : 9;
            if (otherPlayer != null && other.gameObject.tag == controlPlayer.tag)
            {
                if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex]))
                {

                    AkSoundEngine.PostEvent("Switch", gameObject);
                    AkSoundEngine.SetState("Environment", "P6_EndSacrifice");
                    AkSoundEngine.PostEvent("Die", Tube);
                    //AkSoundEngine.PostEvent("TheCore", Tube);


                    controlPlayer.GetComponent<Animator>().SetBool("IsButtonPressed", true);
                    Destroy(otherPlayer);

                    
                    


                    GameObject.FindObjectOfType<SCR_CameraFollow>().enabled = false;
                    //SceneManager.LoadScene("Transition");

                }
            }
        }
    }

   
}
