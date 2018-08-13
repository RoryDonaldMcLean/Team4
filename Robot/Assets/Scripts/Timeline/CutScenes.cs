using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenes : MonoBehaviour {

    private string level = "";
	// Use this for initialization
	void Start () {
        level = GetLevel();
	}
	
    private string GetLevel()
    {
        string level = "";
        level = GameObject.FindObjectOfType<WhichLevel>().level.ToString();
        return level;
    }

    private void Drop(GameObject player, string tag, string limbName)
    {
        switch(tag)
        {
            case "Player1":
                player.GetComponent<SCR_TradeLimb>().DropDownLims(limbName);
                break;
            case "Player2":
                player.GetComponent<SCR_TradeLimb>().DropDownLims(limbName);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(level)
        {
            case "One":
                break;
            case "Two":
                break;
            case "Three":
                break;
            case "Four":
                break;
            case "Five":
                break;
        }
    }
}
