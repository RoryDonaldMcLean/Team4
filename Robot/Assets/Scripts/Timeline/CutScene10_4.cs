using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene10_4 : MonoBehaviour {
    
    private bool p1Back = false, p2Back = false;
        
	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player1" && FindObjectOfType<CutScene10_3>().BothEnter())
        {
            p1Back = true;
            //FindObjectOfType<CutScene10_3>().DisablePlayerEnter("Player1");
        }
        if (other.gameObject.tag == "Player2" && FindObjectOfType<CutScene10_3>().BothEnter())
        {
            p2Back = true;
            //FindObjectOfType<CutScene10_3>().DisablePlayerEnter("Player2");
        }
    }

    public void DisablePlayerBack(string playerTag)
    {
        if (playerTag == "player1")
            p1Back = false;
        else
            p2Back = false;
    }

    private void Update()
    {
        if(p1Back && p2Back)
        {
            SceneManager.LoadScene("End");
        }
    }
}
