using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Movable : MonoBehaviour 
{
	public bool pickedUp = false;
	public string playerTag;
	public string movableObjectString;

	private GameObject movableObject;
	float maxDistance;
	float minDistance;

    public Color beamColour = Color.white;
    public int beamLength = 5;
    public bool lightOn = false;

    // Use this for initialization
    void Start() 
	{
        //on start, create a new movable object determined by the string in the editor
        movableObject = Instantiate(Resources.Load ("Prefabs/Light/" + movableObjectString)) as GameObject;

        //force it's name to be "SlideBox"
        movableObject.name = "SlideBox";

        //set it's position to be the same as the placeholder slideBox
        movableObject.transform.position = this.transform.GetChild(1).position;

        //become a child of the "Movable" object
        movableObject.transform.SetParent(this.transform);

        float newYRot = this.transform.rotation.eulerAngles.y + this.transform.GetChild(1).transform.localEulerAngles.y;
        movableObject.transform.Rotate(0, newYRot, 0, Space.Self);

        if (movableObjectString.Contains("Emitter"))
        {
            movableObject.GetComponent<LightEmitter>().colouredBeam = beamColour;
            movableObject.GetComponent<LightEmitter>().beamLength = beamLength;
            movableObject.GetComponent<LightEmitter>().switchedOn = lightOn;
        }
        else if(movableObjectString.Contains("LightRedirect"))
        {
            movableObject.GetComponent<LightRedirect>().beamLength = beamLength;
        }

        //delete the placeholderbox
        Destroy(this.transform.GetChild(1).gameObject);

        LimitFind();
	}

    private void LimitFind()
    {
        float width = this.transform.GetChild(0).lossyScale.x /2.0f;
        float centrePoint = Vector3.Dot(this.transform.position, this.transform.right);

        maxDistance = width + centrePoint;
		minDistance = -width + centrePoint;

        if (maxDistance < minDistance) 
		{
			float temp = minDistance;
			minDistance = maxDistance;
			maxDistance = temp;
		}
    }

	//the far right of the pole
	private void RightLimit(ref float movableObjectPosition)
	{
        if (movableObjectPosition >= maxDistance)
        {
			LimitBreachResponse(maxDistance - (movableObject.transform.lossyScale.x / 2.0f));
        }
	}

	//the far left of the pole
	private void LeftLimit(ref float movableObjectPosition)
	{
		if (movableObjectPosition <= minDistance)
        {
			LimitBreachResponse(minDistance + (movableObject.transform.lossyScale.x / 2.0f));
        }
	}

    private void AwayFromBox()
    {
		Transform playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;

		float boxPos = Vector3.Dot(movableObject.transform.position, this.transform.forward);
		float playerPos = Vector3.Dot(playerTransform.position, this.transform.forward);
		float difference = Mathf.Abs(playerPos - boxPos);
        float maxDistFromBox = 2.5f;

        if (difference > maxDistFromBox)
        {
			DropBox(ref playerTransform);
        }
    }

    private void LimitBreachResponse(float limitPoint)
    {
	    //past its limit
		Vector3 boxPos = Vector3.Scale(this.transform.right, Vector3.one) * (limitPoint + -Vector3.Dot(movableObject.transform.position, this.transform.right));
		Vector3 newBoxPos = boxPos + movableObject.transform.position;
		movableObject.transform.position = newBoxPos;

		Transform playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
		DropBox(ref playerTransform);
    }

    private void LimitCheck()
    {
		float movableObjectScrollingPosition = Vector3.Dot(movableObject.transform.position, this.transform.right);

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

	private void DropBox(ref Transform playerTransform)
    {
        //if the player leaves the movable trigger box then drop the box
		playerTransform.GetComponent<PickupAndDropdown>().LimitDrop();
    }
}
