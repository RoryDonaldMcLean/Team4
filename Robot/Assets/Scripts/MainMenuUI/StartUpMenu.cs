using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUpMenu : MonoBehaviour {

    public Button btn;
    public GameObject red, blue;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(red.GetComponent<Transform>().position.x < Screen.width / 2 && blue.GetComponent<Transform>().position.x > Screen.width / 2)
        {
            GameManager.Instance.whichAndroid.player1ControlBlue = true;
            btn.gameObject.SetActive(true);
            AkSoundEngine.SetSwitch("PlayerID", "Player_2", gameObject);
            AkSoundEngine.SetState("Chirp_Type", "Happy");
            AkSoundEngine.PostEvent("Chirp", gameObject);
        }
        else if(red.GetComponent<Transform>().position.x > Screen.width / 2 && blue.GetComponent<Transform>().position.x < Screen.width / 2)
        {
            GameManager.Instance.whichAndroid.player1ControlBlue = false;
            btn.gameObject.SetActive(true);
            AkSoundEngine.SetSwitch("PlayerID", "Player_1", gameObject);
            AkSoundEngine.SetState("Chirp_Type", "Happy");
            AkSoundEngine.PostEvent("Chirp", gameObject);
        }
        else
        {
            btn.gameObject.SetActive(false);
        }
	}
}
