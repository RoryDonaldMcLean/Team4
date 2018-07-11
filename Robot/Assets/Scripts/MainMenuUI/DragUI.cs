using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventDate)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(1);
        GameObject pointerDrag = eventData.pointerDrag;
        pointerDrag.transform.position = Input.mousePosition;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

}
