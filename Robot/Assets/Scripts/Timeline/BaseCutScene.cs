using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCutScene : MonoBehaviour {
    protected GameObject p1, p2;

    // Use this for initialization
    private void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
    }

    protected virtual void OnCollisionEnter(Collision other)
    {

    }
}
