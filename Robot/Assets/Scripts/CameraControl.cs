using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour 
{
	public float m_DampTime = 0.2f; //approximate time for the camera to refocus
	public float m_ScreenEdgeBuffer = 4.0f;	//space between the top/bottom most target and the screen edge
	public float m_MinSize = 6.5f;	//the smallest orthographic size the camera can be.
	public Transform[] m_Targets;	//all the targets the camera needs to encompass.


	private Camera m_Camera; 		//used for referencing the camera
	private float m_ZoomSpeed;		//reference speed for the smooth damping of orthographic size
	private Vector3 m_MoveVelocity;	//reference velocity for the smooth damping of the position
	private Vector3 m_DesiredPosition;	//the position the camera is moving towards


	private void Awake()
	{
		m_Camera = GetComponentInChildren<Camera> ();
	}

	private void FixedUpdate()
	{
		//Move the camera towards a desired position
		Move();

		//change the size of the camera based
		Zoom();
	}

	private void Move()
	{
		//find the average position of the targets
		FindAveragePosition();

		//smoothly transition to the position
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		//go through all the targets and add their positions together
		for (int i = 0; i < m_Targets.Length; i++)
		{
			//if the target isn't active, go on to the next one
			if (!m_Targets [i].gameObject.activeSelf)
				continue;

			//add to the average and increment the number of targets in the average
			averagePos += m_Targets[i].position;
			numTargets++;
		}

		//if there are targets divide the sum of the positions by the number of them to find the average
		if (numTargets > 0)
			averagePos /= numTargets;

		//keep the same y value
		averagePos.y = transform.position.y;

		//the desired position is the average position
		m_DesiredPosition = averagePos;
	
	}

	private void Zoom()
	{
		//find the required size based on the desired position and smoothly
		//transition to that size
		float requiredSize = FindRequiredSize();
		m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}

	private float FindRequiredSize()
	{
		//find the position the camera rig is moving towards in its local space
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		//start the camera's size calculation at zero
		float size = 0.0f;

		//go through all the targets
		for (int i = 0; i < m_Targets.Length; i++)
		{
			//.. and if they aren't active continue on to the next target
			if (!m_Targets [i].gameObject.activeSelf)
				continue;

			//otherwise, find the position of the target in the cameras local space
			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

			//find the position of the target from the desired position of the cameras local space
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			//choose the largest out of the current size and the distance of the player 'up' or 'down' from the camera
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

			//choose the largest out of the current size and the calculated size based on the player being to the left or right of the camera
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
		}

		//add the edge buffer to the size
		size += m_ScreenEdgeBuffer;

		//make sure the camera's size isn't below the minimum
		size = Mathf.Max(size, m_MinSize);

		return size;

	}

	public void SetStartPositionAndSize()
	{
		//find the desired position
		FindAveragePosition();

		//set the cameras position to the desired position without damping
		transform.position = m_DesiredPosition;

		//find and set the required size of the camera
		m_Camera.orthographicSize = FindRequiredSize();
	}


}
