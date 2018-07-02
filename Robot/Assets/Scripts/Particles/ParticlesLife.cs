using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesLife : MonoBehaviour {

    private float particleLifeTime, startTime;
    private Vector3 from, to;
    private Vector3 direction;
    // Use this for initialization
    void Awake () {
       
    }
	
    void Start()
    {
        startTime = Time.time;
        from = this.GetComponent<Transform>().position;

        float rand = Random.Range(0.15f, 0.3f);
        float randsphere = Random.Range(-0.3f, 0.3f);
        float rands = Random.Range(-0.3f, 0.3f);

        to = from + rand * direction + randsphere * Vector3.up + rands * Vector3.Cross(direction, Vector3.up);
           
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void SetLifeTime(float time)
    {
        particleLifeTime = time;
    }

    private void ScaleDown()
    {
        if (Time.time - startTime > particleLifeTime)
            Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }
}
