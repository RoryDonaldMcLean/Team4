using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene10_2 : BaseCutScene {
    private GameObject controlPlayer;
    private GameObject otherPlayer;
    public GameObject Tube;

	GameObject BlackFade;
	Animator anim;

	private void Start()
	{
		BlackFade = GameObject.Find ("BlackFade");
		anim = BlackFade.GetComponent<Animator> ();
	}

    protected override void OnCollisionStay(Collision other)
    {
        base.OnCollisionStay(other);
        otherPlayer = Tube.GetComponent<CutScene10_1>().GetInsidePlayer();
        
        if (otherPlayer)
        {
            controlPlayer = otherPlayer.tag == "Player1" ? p2 : p1;

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
                    CancelInvoke();
                    Invoke("StopAnim", 1.5f);
					//StartCoroutine ("Fading", 10.0f);
                    Invoke("End", 10.0f);
                    GameObject.FindObjectOfType<SCR_CameraFollow>().enabled = false;
                    //SceneManager.LoadScene("Transition");

                }
            }
        }
    }

    private void StopAnim()
    {
        if(controlPlayer.GetComponent<Animator>().GetBool("IsButtonPressed"))
            controlPlayer.GetComponent<Animator>().SetBool("IsButtonPressed", false);
    }
   
    private void End()
    {
		//BlackFade = GameObject.Find ("BlackFade");
		//anim = BlackFade.GetComponent<Animator> ();
		//anim.Play("FadeOut");
		//yield return new WaitUntil (() => BlackFade.GetComponent<Image>().color.a==1);
		//if (BlackFade.GetComponent<Image> ().color.a == 1)
		//{
			SceneManager.LoadScene("End");
		//}
    }

	IEnumerator Fading()
	{
		
		anim.Play("FadeOut");
		yield return new WaitUntil (() => BlackFade.GetComponent<Image>().color.a==1);
		SceneManager.LoadScene("End");
	}
}
