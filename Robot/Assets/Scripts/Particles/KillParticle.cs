using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticle : MonoBehaviour {

    public float lifetime;

    void Start()
    {
        //Destroys the particle if its lifetime Is overs
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
