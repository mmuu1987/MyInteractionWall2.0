using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvent : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{


    public event Action<bool> TouchMoveEvent;

    public event Action<float> DragMoveEvent;

    public event Action OnBeginDragEvent;


    public event Action OnEndDragEvent;


    private Vector2 _beginDragPos;   
    public void OnDrag(PointerEventData eventData)
    {
       //Debug.Log("OnDrag");

       if (DragMoveEvent != null)
           DragMoveEvent(eventData.delta.x);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        _beginDragPos = eventData.position;

        if (OnBeginDragEvent != null) OnBeginDragEvent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;

        if (pos.x > _beginDragPos.x)
        {
            //isLeft = false;
            if (TouchMoveEvent != null) TouchMoveEvent(false);
           // Debug.Log("向右滑");
        }
        else
        {
           // Debug.Log("向左滑");
            if (TouchMoveEvent != null) TouchMoveEvent(true);
            //isLeft = true;
        }

        if (OnEndDragEvent != null) OnEndDragEvent();
        // Debug.Log("OnEndDrag");
    }
}
