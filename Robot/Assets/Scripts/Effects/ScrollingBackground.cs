using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [Range(0, 1)][SerializeField]float xSpeed = 1f;
    [Range(0, 1)][SerializeField]float ySpeed = 1f;

    Vector2 offset = Vector2.zero;
    Material mat;

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    public void Scroll(Vector2 pos)
    {
        //adds the Y and X possitions to the offset
        offset.x -= pos.x * xSpeed;
        offset.y += pos.y * ySpeed;

        //Checks the X offset and adjusts it 
        if (offset.x > 1f)
            offset.x -= 1f;
        else if (offset.x < -1)
            offset.x += 1f;

        //Checks the X offset and adjusts it 
        if (offset.y > 1f)
            offset.y -= 1f;
        else if (offset.y < -1)
            offset.y= 1f;

        //sets the new offset
        mat.mainTextureOffset = offset;
    }

}
