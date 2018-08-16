using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene10_3 : MonoBehaviour {
    
    private bool p1Enter = false, p2Enter = false;
	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player1")
        {
            p1Enter = true;
            FindObjectOfType<CutScene10_4>().DisablePlayerBack("Player1");
        }
        if(other.gameObject.tag == "Player2")
        {
            p2Enter = true;
            FindObjectOfType<CutScene10_4>().DisablePlayerBack("Player2");
        }
    }

    public bool BothEnter()
    {
        return p1Enter && p2Enter;
    }

    public void DisablePlayerEnter(string playerTag)
    {
        if (playerTag == "player1")
            p1Enter = false;
        else
            p2Enter = false;
    }
}
