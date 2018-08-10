using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeControl : MonoBehaviour {

   

	// Use this for initialization
	void Start () {

       // GameManager.Instance.playerSetting.volume.onValueChanged.AddListener(ListenerMethod);

    }

    public void ListenerMethod(float value)
    {
    }

    // Update is called once per frame
    void Update ()
    {

      //  AkSoundEngine.SetRTPCValue ("Master_Volume", GameManager.Instance.playerSetting.volume); 

    }
}
