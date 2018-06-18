﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour {

    public Material[] material;
    Renderer rend;
    private int index;
    private int maxMaterialNumber;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        maxMaterialNumber = material.Length;
        index = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rend.sharedMaterial = material[index];
            index ++;
            if (index == maxMaterialNumber)
            {
                index = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            rend.sharedMaterial = material[index];
            index--;
            if (index == 0)
            {
                index = 4;
            }
        }
    }


}
 