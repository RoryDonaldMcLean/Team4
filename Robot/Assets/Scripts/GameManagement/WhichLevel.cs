﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Level { None, One, Two, Three, Four, Five}

//[Serializable]
public class WhichLevel : MonoBehaviour {

    public Level level;
    public GameObject pauseCanvas;

    private void Start()
    {
        GameManager.Instance.playerSetting.lastLevel = level;
        SaveNLoad.Save(GameManager.Instance.playerSetting);
        return;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !pauseCanvas.activeSelf)
        {
            pauseCanvas.SetActive(true);
        }
    }
}