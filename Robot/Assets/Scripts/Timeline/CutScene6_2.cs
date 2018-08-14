using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene6_2 : BaseCutScene {

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if(other.gameObject.name == "Wall 6 (1)")
        {
            p2.GetComponent<SCR_TradeLimb>().DropDownLims("RightArm");
        }
    }
}
