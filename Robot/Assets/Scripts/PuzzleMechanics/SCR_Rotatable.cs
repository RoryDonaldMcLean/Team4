using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject tutorialPrompt;

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
	void Update() 
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
            if (tutorialPrompt == null)
            {
                tutorialPrompt = Instantiate(Resources.Load("Prefabs/UI/RotatableObjectVisualAid")) as GameObject;
                tutorialPrompt.transform.SetParent(GameObject.FindGameObjectWithTag("TutorialUI").transform, false);
                StartCoroutine(RotateObjectUI());
            }
        }
	}

    private IEnumerator RotateObjectUI()
    {
        while (tutorialPrompt != null)
        {
            Vector3 objectPosition = this.transform.GetChild(1).position;
            objectPosition.y = 0;
            objectPosition.z += 1;
            Vector3 UIposition = Camera.main.WorldToScreenPoint(objectPosition);
            tutorialPrompt.transform.position = UIposition;

            yield return new WaitForFixedUpdate();
        }
    }

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.name.Contains ("Player"))
		{
			Entered = false;
            StopCoroutine(RotateObjectUI());
            Destroy(tutorialPrompt);
		}
	}
}
