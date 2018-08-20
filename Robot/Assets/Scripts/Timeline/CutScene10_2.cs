using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.UI;

public class CutScene10_2 : BaseCutScene
{
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

        var inputDevice1 = (InputManager.Devices.Count > 0) ? InputManager.Devices[0] : null;
        var inputDevice2 = (InputManager.Devices.Count > 1) ? InputManager.Devices[1] : null;
        if (otherPlayer)
        {
            controlPlayer = otherPlayer.tag == "Player1" ? p2 : p1;

            int btnIndex = otherPlayer.tag == "Player1" ? 22 : 9;

            var device = otherPlayer.tag == "Player1" ? inputDevice2 : inputDevice1;
            PressButton(other, btnIndex, device);
        }
    }
    
    private void PressButton(Collision other, int btnIndex, InputDevice device)
    {
        if (otherPlayer != null && other.gameObject.tag == controlPlayer.tag)
        {
            if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[btnIndex]) && device == null)
            {
                PressButton();
                
            }
            else if(device != null && device.Action2)
            {
                PressButton();
            }

        }
    }

    private void PressButton()
    {
        AkSoundEngine.PostEvent("Switch", gameObject);
        AkSoundEngine.SetState("Environment", "P6_EndSacrifice");
        AkSoundEngine.PostEvent("Die", Tube);
        //AkSoundEngine.PostEvent("TheCore", Tube);


        controlPlayer.GetComponent<Animator>().SetBool("IsButtonPressed", true);
        Destroy(otherPlayer);
        CancelInvoke();
        Invoke("StopAnim", 1.5f);
        Invoke("End", 5.0f);
        GameObject.FindObjectOfType<SCR_CameraFollow>().enabled = false;
        //SceneManager.LoadScene("Transition");
    }

    private void StopAnim()
    {
        if (controlPlayer.GetComponent<Animator>().GetBool("IsButtonPressed"))
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
