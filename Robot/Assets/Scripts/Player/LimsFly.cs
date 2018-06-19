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
        isFinish = false;
        alpha = 0;
        parabolaVariable = -25;
        if(this.GetComponent<CapsuleCollider>())
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha <= 1.0f)
            alpha += 0.01f;
        endPosition = targetPlayer.GetComponent<SCR_TradeLimb>().limbs[limsNumber].GetComponent<Transform>().position;
        currentPosition.x = Mathf.Lerp(startPosition.x, endPosition.x, alpha);
        currentPosition.z = Mathf.Lerp(startPosition.z, endPosition.z, alpha);
        currentPosition.y = startPosition.y + parabolaVariable * alpha * alpha + (endPosition.y - parabolaVariable) * alpha;
        this.GetComponent<Transform>().position = currentPosition;

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
