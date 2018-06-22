using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnim : MonoBehaviour
{

    //Sets the speed the material rotates
    //public float scrollSpeed = 0.5F;
  
    private Renderer rend;

    public int columns = 2;
    public int rows = 2;
    public float framesPerSecond = 10f;

    //the current frame to display
    private int index = 0;

    void Start()
    {
        //Sets the renderer on the component the script is set on
        rend = GetComponent<Renderer>();
        StartCoroutine(updateTiling());

        //set the tile size of the texture (in UV units), based on the rows and columns
        Vector2 size = new Vector2(1f / columns, 1f / rows);
        rend.sharedMaterial.SetTextureScale("_MainTex", size);
    }

    //void Update()
    //{
    //      //Rotates the object instead of seting the animation to play
    //    float offset = Time.time * scrollSpeed;
    //    rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    //}

    private IEnumerator updateTiling()
    {
        while (true)
        {
            //move to the next index of the sheet
            index++;
            if (index >= rows * columns)
                index = 0;

            //split into x and y indexes
            Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
                                          (index / columns) / (float)rows);          //y index

            rend.sharedMaterial.SetTextureOffset("_MainTex", offset);

            //waits between frames
            yield return new WaitForSeconds(1f / framesPerSecond);
        }

    }
}
