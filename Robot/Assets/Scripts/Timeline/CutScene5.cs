using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene5 : BaseCutScene {
    
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if(other.gameObject.tag == "Player2")
        {
            p2.GetComponent<SCR_TradeLimb>().DropDownLims("RightLeg");
        }
    }
}
