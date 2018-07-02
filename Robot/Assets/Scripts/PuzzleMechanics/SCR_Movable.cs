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

    public Color beamColour = Color.white;
    public int beamLength = 5;
    public bool lightOn = false;

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

		maxDistance = Vector3.Dot(pole.GetComponent<Collider>().bounds.max, this.transform.right);
		minDistance = Vector3.Dot(pole.GetComponent<Collider>().bounds.min, this.transform.right);

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
			LimitBreachResponse(maxDistance - (this.gameObject.transform.GetChild(1).lossyScale.x / 2.0f));
        }
	}

	//the far left of the pole
	private void LeftLimit(ref float movableObjectPosition)
	{
		if (movableObjectPosition <= minDistance)
        {
			LimitBreachResponse(minDistance + (this.gameObject.transform.GetChild(1).lossyScale.x / 2.0f));
        }
	}

    private void AwayFromBox()
    {
		Transform playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;

		float boxPos = Vector3.Dot(this.gameObject.transform.GetChild(1).position, this.transform.forward);
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
		Transform boxTransform = this.gameObject.transform.GetChild(1);
	    //past its limit
		Vector3 boxPos = Vector3.Scale(this.transform.right, Vector3.one) * (limitPoint + -Vector3.Dot(boxTransform.position, this.transform.right));
		Vector3 newBoxPos = boxPos + boxTransform.position;;
		boxTransform.position = newBoxPos;

		Transform playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
		DropBox(ref playerTransform);
    }

    private void LimitCheck()
    {
		float movableObjectScrollingPosition = Vector3.Dot(this.transform.GetChild(1).position, this.transform.right);

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
