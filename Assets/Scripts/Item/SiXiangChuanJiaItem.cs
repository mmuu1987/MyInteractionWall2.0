using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiXiangChuanJiaItem : MonoBehaviour
{

    private YearsEvent _yearsEvent;
    public RectTransform RectTransform { get; private set; }
    private void Start()
    {
        RectTransform = this.GetComponent<RectTransform>();
    }
    internal bool Move(float delta, float width)
    {
        if (RectTransform == null) return false;
        //-412.5  -1287.5 -2162.5
        Vector2 pos = new Vector2(RectTransform.anchoredPosition.x + delta, RectTransform.anchoredPosition.y);

        Vector2 size = RectTransform.sizeDelta;
        if (delta < 0)
        {
            if (pos.x + size.x / 2 < 0)
            {
                 pos = new Vector2(pos.x+width  + 70f, pos.y);
                
            }
        }
        else
        {
            if (pos.x + size.x / 2 > width )
            {
                pos = new Vector2(pos.x-width  -70f, pos.y);
               
            }
        }



        RectTransform.anchoredPosition = pos;

        return false;
    }

    internal void SetInfo(YearsEvent yearsEvent)
    {
        _yearsEvent = yearsEvent;

    }
}
