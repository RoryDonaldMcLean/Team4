using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    //public PlayerSetting playerSetting = new PlayerSetting();
    public PlayerSetting playerSetting;
    public WhichAndroid whichAndroid;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
         
        playerSetting = new PlayerSetting();
        whichAndroid = new WhichAndroid();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
