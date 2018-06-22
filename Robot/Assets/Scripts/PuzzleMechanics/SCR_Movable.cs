using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Movable : MonoBehaviour 
{
	public bool pickedUp = false;
	public string playerTag;
	public string movableObjectString;

	GameObject pole;
	float maxDistance;
	float minDistance;
    public bool xAxisForward = true;

	public bool entered = false;

    public Color beamColour = Color.white;
    public int beamLength = 5;
    public bool lightOn = false;
    public float playerDistanceCheck;

    private float playerOriginalDistFromBox;

    // Use this for initialization
    void Start () 
	{
        LimitFind();

        //on start, create a new movable object determined by the string in the editor
        GameObject newMovable = Instantiate(Resources.Load ("Prefabs/Light/" + movableObjectString)) as GameObject;

		//force it's name to be "SlideBox"
		newMovable.name = "SlideBox";

		//set it's position to be the same as the placeholder slideBox
		newMovable.transform.position = this.transform.GetChild(1).position;

		//become a child of the "Movable" object
		newMovable.transform.SetParent(this.transform);

        float newYRot = this.transform.rotation.eulerAngles.y + this.transform.GetChild(1).transform.localEulerAngles.y;
        newMovable.transform.Rotate(0, newYRot, 0, Space.Self);

        if (movableObjectString.Contains("Emitter"))
        {
            newMovable.GetComponent<LightEmitter>().colouredBeam = beamColour;
            newMovable.GetComponent<LightEmitter>().beamLength = beamLength;
            newMovable.GetComponent<LightEmitter>().switchedOn = lightOn;
        }

        //delete the placeholderbox
        Destroy(this.transform.GetChild(1).gameObject);
	}

    private void LimitFind()
    {
        pole = this.gameObject.transform.GetChild(0).gameObject;
        //get the far right and far left bounds of the pole object
        Vector3 MaxDist = pole.GetComponent<Collider>().bounds.max;
        Vector3 MinDist = pole.GetComponent<Collider>().bounds.min;
        Vector3 difference = MaxDist - MinDist;

        if(difference.z > difference.x)
        {
            maxDistance = MaxDist.z;
            minDistance = MinDist.z;
            playerOriginalDistFromBox = this.gameObject.transform.GetChild(1).position.x;
            xAxisForward = false;
        }
        else
        {
            maxDistance = MaxDist.x;
            minDistance = MinDist.x;
            playerOriginalDistFromBox = this.gameObject.transform.GetChild(1).position.z;
            xAxisForward = true;
        }
    }

	//the far right of the pole
	private void RightLimit(ref float movableObjectPosition)
	{
        if (movableObjectPosition >= maxDistance)
        {
            LimitBreachResponse(maxDistance - 1);
        }
	}

	//the far left of the pole
	private void LeftLimit(ref float movableObjectPosition)
	{
		if (movableObjectPosition <= minDistance)
        {
            LimitBreachResponse(minDistance + 1);
        }
	}

    private void AwayFromBox()
    {
        float difference = Mathf.Abs(Mathf.Abs(playerDistanceCheck) - Mathf.Abs(playerOriginalDistFromBox));
        float maxDistFromBox = 2.5f;

        if (difference > maxDistFromBox)
        {
            DropBox();
        }
    }

    private void LimitBreachResponse(float limitPoint)
    {
        //past its limit
        Vector3 position = this.transform.GetChild(1).position;
        if (xAxisForward)
        {
            position.x = limitPoint;
        }
        else
        {
            position.z = limitPoint;
        }
        DropBox();
    }

    private void LimitCheck()
    {
        Vector3 objectPos = this.transform.GetChild(1).position;

        float movableObjectScrollingPosition;

        if (xAxisForward)
        {
            movableObjectScrollingPosition = objectPos.x;
        }
        else
        {
            movableObjectScrollingPosition = objectPos.z;
        }

        RightLimit(ref movableObjectScrollingPosition);
        LeftLimit(ref movableObjectScrollingPosition);
        AwayFromBox();
    }
	
	// Update is called once per frame
	void Update() 
	{
        if (pickedUp)
		{
            //if the movable box thing reaches the left side or right side of the pole then drop it
            LimitCheck();
        }
	}

    private void DropBox()
    {
        //if the player leaves the movable trigger box then drop the box 
        GameObject.FindGameObjectWithTag(playerTag).GetComponent<PickupAndDropdown>().LimitDrop();
    }
}
