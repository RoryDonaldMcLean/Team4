using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene8 : BaseCutScene
{
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if(other.gameObject.tag == "Player1")
        {
            p1.GetComponent<SCR_TradeLimb>().DropDownLims("RightLeg");
        }
        if(other.gameObject.tag == "Player2")
        {
            p2.GetComponent<SCR_TradeLimb>().DropDownLims("LeftArm");
        }
    }
}
