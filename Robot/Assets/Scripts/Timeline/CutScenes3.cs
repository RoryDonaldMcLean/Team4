using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenes3 : BaseCutScene {

    protected override void OnCollisionEnter(Collision other)
    { 
        base.OnCollisionEnter(other);
        if(other.gameObject.name == "Wall 6 (5)")
        {
            p1.GetComponent<SCR_TradeLimb>().DropDownLims("LeftArm");
            p1.GetComponent<Animator>().SetBool("IsPushing", false);
        }
    }
}
