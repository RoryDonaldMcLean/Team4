using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Rotatable : MonoBehaviour 
{
    public bool switchOn = false;
	public bool pickedUp = false;
    public bool interactable = false;
    public string playerTag;
	public string rotatableObjectString;
	public bool Entered = false;
    public Color beamColour = Color.white;
    public int beamLength = 5;

	// Use this for initialization
	void Start () 
	{
		GameObject newRotatable = Instantiate(Resources.Load("Prefabs/Light/" + rotatableObjectString)) as GameObject;
		newRotatable.name = "RotateBox";
		newRotatable.transform.position = this.transform.GetChild(1).position;
		newRotatable.transform.SetParent(this.transform);
        newRotatable.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (rotatableObjectString.Contains("Emitter"))
        {
            newRotatable.GetComponent<LightEmitter>().colouredBeam = beamColour;
            newRotatable.GetComponent<LightEmitter>().beamLength = beamLength;
            if (switchOn)
            {
                Debug.Log("?");
                newRotatable.GetComponent<LightEmitter>().switchedOn = true;
            }
            if (interactable) newRotatable.GetComponent<LightEmitter>().canBeTurnedOff = true;
        }
        Destroy(this.transform.GetChild(1).gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pickedUp)
		{
			//if the player leaves the movable trigger box then drop the box thing
			if (Entered == false)
			{
				GameObject.FindGameObjectWithTag(playerTag).GetComponent<PickupAndDropdown_Trigger>().RotateDrop();
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name.Contains("Player"))
		{
			Entered = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.name.Contains ("Player"))
		{
			Entered = false;
		}
	}
}
