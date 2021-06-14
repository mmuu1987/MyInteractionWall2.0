using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class LogoItem : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    private float _moveSpeed = 1f;

    public RectTransform RectTransform;

    /// <summary>
    /// 填充的宽度的倍数
    /// </summary>
    private float _scalar = 2f;

    /// <summary>
    /// 长按触发拖动的时间
    /// </summary>
    private float _touchMoveTime = 1f;
    /// <summary>
    /// 长按触发拖动的时间缓存
    /// </summary>
    private float _touchMoveTemp = -1;

    /// <summary>
    /// 是否按下
    /// </summary>
    private bool _isPress = false;

    private bool _isEnableDrag = false;



    

    private Vector2 _dragDelta;

   

    /// <summary>
    /// 分配好的位置
    /// </summary>
    private Vector2 _orinigalVector2;

    private void Awake()
    {
        RectTransform = this.GetComponent<RectTransform>();


    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       // Debug.Log("click this name is" +this.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

       // Debug.Log("OnPointerDown this name is" + this.name);

        _touchMoveTemp = 0;
        _isPress = true;
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
       
        _isPress = false;
        _isEnableDrag = false;
       //f Debug.Log("OnPointerUp this name is" + this.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isEnableDrag)
        {
            _dragDelta = eventData.delta;
        }
    }
    // Update is called once per frame
    void Update()
    {

       
            Vector2 pos = RectTransform.anchoredPosition;

            _orinigalVector2 = new Vector2(_orinigalVector2.x + _moveSpeed, _orinigalVector2.y);

            if (!_isEnableDrag )
            {


                if (!_isPress)
                {
                    RectTransform.anchoredPosition = Vector2.Lerp(pos, _orinigalVector2, Time.deltaTime * 1f);

                    if (pos.x <= 0)
                    {
                        RectTransform.anchoredPosition = new Vector2(7680f * _scalar, pos.y);

                        _orinigalVector2 = new Vector2(7680f * _scalar, pos.y);

                    }
                }

               
            }
            else
            {

                RectTransform.anchoredPosition += new Vector2(_dragDelta.x, _dragDelta.y);
                _dragDelta = Vector2.zero;
            }



            if (_isPress)
            {
                _touchMoveTemp += Time.deltaTime;

                if (_touchMoveTime <= _touchMoveTemp)
                {
                    _isEnableDrag = true;
                    //触发拖动
                }
            }
        

      
    }

    public void SetInfo(float speed,float scalar)
    {
         _scalar = scalar;
         _moveSpeed = speed;
         
         _orinigalVector2 = RectTransform.anchoredPosition;
         this.gameObject.SetActive(false);
    }

    public void Active(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
   
}
