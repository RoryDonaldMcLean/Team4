using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Level { None, One, Two, Three, Four, Five}

//[Serializable]
public class WhichLevel : MonoBehaviour {

    public Level level;
    
    private void Start()
    {
        GameManager.Instance.playerSetting.lastLevel = level;
        SaveNLoad.Save(GameManager.Instance.playerSetting);
        return;
    }
}
