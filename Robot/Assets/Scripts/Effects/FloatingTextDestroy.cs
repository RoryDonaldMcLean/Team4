using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextDestroy : MonoBehaviour {

    private Vector3 Offset = new Vector3(0, 2, 0);
    private Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);
    private float DestroyTime = 0.5f;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

    }


}
