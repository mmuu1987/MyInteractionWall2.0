using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private RectTransform _rectTransform;

    /// <summary>
    /// 右滑的最大距离
    /// </summary>
    public float MaxRight = 0f;

    /// <summary>
    /// 左滑最大距离
    /// </summary>
    public float MaxLeft = 0f;

    private void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x+eventData.delta.x,_rectTransform.anchoredPosition.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x + eventData.delta.x, _rectTransform.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float x = _rectTransform.anchoredPosition.x + eventData.delta.x;

        if (x > MaxRight)
        {
            _rectTransform.DOAnchorPosX(MaxRight, 0.5f);
        }
        else if(x<-MaxLeft)
        {
            _rectTransform.DOAnchorPosX(-MaxLeft, 0.5f);
        }
        else
        {
            _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
        }
        
    }
}
