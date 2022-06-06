using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleUI : MonoBehaviour,IPointerClickHandler
{
    
    private void Awake()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.transform.parent != null)
        {
            this.transform.parent.DOScale(0f, 0.55f);
        }
    }
}
