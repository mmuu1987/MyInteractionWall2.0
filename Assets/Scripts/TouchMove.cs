using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private RectTransform _rectTransform;

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
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x + eventData.delta.x, _rectTransform.anchoredPosition.y);
    }
}
