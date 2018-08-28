using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class CutScene10_2 : BaseCutScene
{
    private GameObject controlPlayer;
    private GameObject otherPlayer;
    public GameObject Tube;

	private GameObject BlackFade;
	private Animator anim;

	//GameObject DeadRobot;

	protected override void Start()
	{
        base.Start();
		BlackFade = GameObject.Find ("BlackFade");
		anim = BlackFade.GetComponent<Animator> ();
		//DeadRobot = Resources.Load<GameObject> ("Models/Dead/Red Robo Dead") as GameObject;
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

            int btnIndex = otherPlayer.tag == "Player1" ? 24 : 11;

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
        AkSoundEngine.PostEvent("Machine_Charge", Tube);

        //AkSoundEngine.PostEvent("TheCore", Tube);
        string whichPlayer = " ";
		Debug.Log("here");
		if (otherPlayer.tag == "Player1")
		{
			whichPlayer = "Red Robo Death new";
		} else
		{
			whichPlayer = "Blue Robo Death new";
		}
		GameObject instance = Instantiate (Resources.Load ("Models/Dead/" + whichPlayer, typeof(GameObject))) as GameObject;
		instance.transform.position = new Vector3 (10.4f, 0.6f, 111.64f);
		instance.transform.localScale = new Vector3 (30.0f, 30.0f, 30.0f);

        controlPlayer.GetComponent<Animator>().SetBool("IsButtonPressed", true);
        Destroy(otherPlayer);
        CancelInvoke();
        Invoke("StopAnim", 1.5f);
        Invoke("Fading", 7.0f);
        Invoke("End", 8.0f);
        GameObject.FindObjectOfType<SCR_CameraFollow>().enabled = false;
        //SceneManager.LoadScene("Transition");
		EndingCheck.ending = Ending.PlayerDestroyed;
    }

    private void StopAnim()
    {
        if (controlPlayer.GetComponent<Animator>().GetBool("IsButtonPressed"))
            controlPlayer.GetComponent<Animator>().SetBool("IsButtonPressed", false);
    }

    private void End()
    {
		SceneManager.LoadScene("End");
    }

	private void Fading()
	{
		anim.Play("FadeOut");
	}
}
