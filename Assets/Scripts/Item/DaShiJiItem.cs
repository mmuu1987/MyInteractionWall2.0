using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DaShiJiItem : MonoBehaviour
{

    public YearsEvent CurYearsEvent;

    public RawImage RawImage;

    public RawImage BackImage;

    public Button Click;

    public Button BackClick;

    public RectTransform RectTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        RectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(YearsEvent yearsEvent,Texture2D texture2D)
    {
        RawImage.texture = texture2D;
        BackImage.texture = texture2D;
        CurYearsEvent = yearsEvent;
    }

    public void Rotation()
    {
        if (Math.Abs(BackImage.rectTransform.eulerAngles.y - 180f) < Mathf.Epsilon)
        {
            BackImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 0.55f);
            RawImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 180f, 0f)), 0.55f);
        }
        else
        {
            BackImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 180f, 0f)), 0.55f);
            RawImage.rectTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 0.55f);
        }
       
    }
    public bool Move(float delta,float width)
    {
        if (RectTransform == null) return false;
        //-412.5  -1287.5 -2162.5
        Vector2 pos = new Vector2(RectTransform.anchoredPosition.x+delta, RectTransform.anchoredPosition.y);

        Vector2 size = RectTransform.sizeDelta;
        if (delta < 0)
        {
            if (pos.x + size.x / 2 < 0)
            {
               // pos = new Vector2(pos.x + width , pos.y);
                return true;
            }
        }
        else
        {
            if (pos.x - size.x / 2  > width - size.x)
            {
                return true;
            }
        }



        RectTransform.anchoredPosition = pos;

        return false;
    }
}
