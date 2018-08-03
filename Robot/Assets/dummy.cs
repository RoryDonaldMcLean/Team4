using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        Animator anim = GameObject.Find("Player").GetComponent<Animator>();
        anim.SetBool("IsMoving", true);
	}
	
}
