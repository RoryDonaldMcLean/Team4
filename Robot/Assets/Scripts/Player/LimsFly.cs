using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimsFly : MonoBehaviour
{
    public bool isFinish;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private GameObject targetPlayer;
    private float alpha;
    private int limsNumber;

    private Vector3 currentPosition;
    private float parabolaVariable;

    // Use this for initialization
    void Start()
    {
		AkSoundEngine.PostEvent("Arm_Detatch", targetPlayer);

        isFinish = false;
        alpha = 0;
        parabolaVariable = -25;

        if (this.GetComponent<CapsuleCollider>())
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Sets new rotation to look at the camera (not needed)
        //var newRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position).eulerAngles;

        //Sets the new Rotation
        //newRotation.x = 5;
        //newRotation.z = 5;
        //newRotation.y = 20;

        if (alpha <= 1.0f)
            alpha += 0.01f;
        endPosition = targetPlayer.GetComponent<SCR_TradeLimb>().limbs[limsNumber].GetComponent<Transform>().position;
        currentPosition.x = Mathf.Lerp(startPosition.x, endPosition.x, alpha);
        currentPosition.z = Mathf.Lerp(startPosition.z, endPosition.z, alpha);
        currentPosition.y = startPosition.y + parabolaVariable * alpha * alpha + (endPosition.y - parabolaVariable) * alpha;
        this.GetComponent<Transform>().position = currentPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -900), Time.deltaTime * 1);

        //this.GetComponent<Transform>().position = Vector3.Lerp(startPosition, endPosition, alpha);
        if (alpha >= 1)
        {
            isFinish = true;
        }
    }

    public void SetStartPosition(Vector3 start)
    {
        startPosition = start;
    }

    public void SetLimsNunber(int lims)
    {
        limsNumber = lims;
    }

    public void SetTargetPlayer(GameObject player)
    {
        targetPlayer = player;
    }

    public bool GetFinish()
    {
        return isFinish;
    }
}
