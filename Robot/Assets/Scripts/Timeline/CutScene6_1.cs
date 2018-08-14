using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene6_1 : BaseCutScene {

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if(other.gameObject.tag == "Player1")
        {
            p1.GetComponent<SCR_TradeLimb>().DropDownLims("LeftLeg");
        }
    }
}
