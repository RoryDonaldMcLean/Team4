using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene10_4 : MonoBehaviour {
    
    private bool p1Back = false, p2Back = false;
    private GameObject BlackFade;
    private Animator anim;
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
			Debug.Log ("just walk away...");
            //CancelInvoke();
            Invoke("Fading", 1.0f);
            Invoke("End", 3.0f);
			EndingCheck.ending = Ending.NoOneDestroyed; 
        }
    }

    private void Start()
    {
        BlackFade = GameObject.Find("BlackFade");
        anim = BlackFade.GetComponent<Animator>();
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
