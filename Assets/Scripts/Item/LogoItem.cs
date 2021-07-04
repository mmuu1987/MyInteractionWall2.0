using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class LogoItem : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    private float _moveSpeed = 1f;

    /// <summary>
    /// 速度缓存
    /// </summary>
    private float _moveSpeedTemp = 1F;

    public RectTransform RectTransform;

    private Material _material;

    


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


    private YearsEvent _yearsEvent;

    /// <summary>
    /// 整体的缩放倍数
    /// </summary>
    private float _scale = 1.1f;

    /// <summary>
    /// 后排需要强制整体缩放的倍数
    /// </summary>
    private float _scaleBack = 0.35f;

    /// <summary>
    /// 原本的尺寸
    /// </summary>
    private Vector2 _orinigalSize;

    private Vector2 _dragDelta;

    /// <summary>
    /// 是否是前面的logo
    /// </summary>
    private bool _isFront = false;
    /// <summary>
    /// 分配好的位置
    /// </summary>
    private Vector2 _orinigalVector2;

    private float _animationTime = 0.55f;

    /// <summary>
    ///
    /// </summary>
    private Vector2 _moveTarget;


    private Tween _tween;

    public Image Image;


    public float CurTarget = 0f;

    public Sprite DefaultSprite;

    public Sprite ActiveSprite;

    /// <summary>
    /// 介绍文字的背景
    /// </summary>
    public Image DescriptionImage;

    public Text Description;


    private int _siblingIndex;
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
        Debug.Log("click this name is" +this.name);

        if (!_isFront) return;

        if (Image.sprite == ActiveSprite)
        {
            Image.sprite = DefaultSprite;
            _isEnableDrag = false;
            ShowDescription(false);
            RectTransform.SetSiblingIndex(_siblingIndex);
        }
        else
        {
            Image.sprite = ActiveSprite;
            _isEnableDrag = true;
            ShowDescription(true);
            Description.text = _yearsEvent.Describe;
            _siblingIndex = RectTransform.GetSiblingIndex();
            RectTransform.SetAsLastSibling();
            
            //显示信息
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isFront) return;
        // Debug.Log("OnPointerDown this name is" + this.name);

        _touchMoveTemp = 0;
        _isPress = true;
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isFront) return;
        _isPress = false;
        _isEnableDrag = false;
       //f Debug.Log("OnPointerUp this name is" + this.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isFront) return;
        if (_isEnableDrag)
        {
            _dragDelta = eventData.delta;
        }
    }
    // Update is called once per frame
    void Update()
    {

       
            Vector2 pos = RectTransform.anchoredPosition;

          
             _orinigalVector2 = new Vector2(_orinigalVector2.x + _moveSpeed, _orinigalVector2.y) ;

            

            Vector2 size = RectTransform.sizeDelta;

            if (!_isEnableDrag )
            {


                if (!_isPress && Math.Abs(_moveSpeed) > Mathf.Epsilon) 
                {
                    RectTransform.anchoredPosition = Vector2.Lerp(pos, _orinigalVector2, Time.deltaTime * 1f);

                    if (pos.x + size .x/2<= 0 && _tween == null)
                    {
                        RectTransform.anchoredPosition = RectTransform.anchoredPosition+new Vector2(Common.ContainerWidth*Common.Scale - size.x / 2, 0);


                        _orinigalVector2 = RectTransform.anchoredPosition;

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

            if (_tween != null)
            {
                _material.SetFloat("_Alpha", CurTarget);
            }

      
    }

    private void ShowDescription(bool isShow)
    {
        if (isShow)
        {
            DescriptionImage.color = new Color(1,1,1,0);
            DescriptionImage.gameObject.SetActive(true);
            DescriptionImage.DOFade(1f, 0.35f);
            Description.DOFade(1f, 0.35f);
        }
        else
        {
            DescriptionImage.DOFade(0f, 0.35f).OnComplete((() => {DescriptionImage.gameObject.SetActive(false);}));
            Description.DOFade(0f, 0.35f);
        }
        
    }
    public void Move(bool isFront)
    {


        //delay = Random.Range(0, 2.5f);

        _moveSpeed = 0f;

        ShowDescription(false);

        if (isFront)//如果向屏幕移动
        {
            Vector2 dir = new Vector2(-512, -100f);

            _moveTarget = this.RectTransform.anchoredPosition + dir;


            _material.DOColor(Color.white, _animationTime);

            _scaleBack = 1f;

            _tween= this.RectTransform.DOSizeDelta(_orinigalSize * _scale, _animationTime).SetEase(Ease.InOutQuart).OnComplete((() =>
            {
                _moveSpeed = -2f;

                _orinigalVector2 = _moveTarget;

                _tween = null;
            }));

            DOTween.To(() => CurTarget, x => CurTarget = x, 0, _animationTime);

            _isFront = true;

            RectTransform.SetAsLastSibling();
        }
        else
        {
            Vector2 dir = new Vector3(512, 100f);

            _moveTarget = this.RectTransform.anchoredPosition + dir;

            _material.DOColor(new Color(0.65f, 0.65f,0.65f,0.65f), _animationTime);

            DOTween.To(() => CurTarget, x => CurTarget = x, 1, _animationTime);

            RectTransform.SetAsFirstSibling();

            _scaleBack = 0.75f;
            _tween=  this.RectTransform.DOSizeDelta(_orinigalSize * _scale*_scaleBack, _animationTime).SetEase(Ease.InOutQuart).OnComplete((() =>
            {
                _moveSpeed = -2f;
                _orinigalVector2 = _moveTarget;
                _tween = null;
            })); 
            _isFront = false;
        }





        this.RectTransform.DOAnchorPos(_moveTarget, _animationTime).SetEase(Ease.InOutQuart);


      

    }

    public void SetInfo(float speed,bool isFront, Vector2 pos,YearsEvent yearsEvent,Material material)
    {
         
         _moveSpeed = speed;
         _isFront = isFront;

         //算出新位置，y轴上下边距不需要有运动
         _orinigalVector2 = new Vector2(pos.x, (3640 - Common.ContainerHeight) / 2.5f+pos.y);//(pos.y- (3640 - Common.ContainerHeight)/2));

        RectTransform.anchoredPosition = _orinigalVector2;

         _orinigalSize = ShowImage(new Vector2(yearsEvent.TexList[0].width, yearsEvent.TexList[0].height));

         _material = material;

         Image.material = _material;

        _material.SetTexture("_ShowTex", yearsEvent.TexList[0]); 


         _scale = 1.5f;

         if (_isFront)
         {
             _scaleBack = 1f;
             _material.color = new Color(1f,1f,1f,1f);
         }
         else
         {
             _scaleBack = 0.75f;
             _material.color = new Color(0.65f, 0.65f, 0.65f, 0.5f);
         }

         float widthScale = 0.15f * _orinigalSize.x;

         float heightScale = 0.15f * _orinigalSize.y;


         Vector2 sizeTemp = _orinigalSize * _scale * _scaleBack;

        RectTransform.sizeDelta = sizeTemp;

         _yearsEvent = yearsEvent;

         this.gameObject.SetActive(false);
    }
    private Vector2 ShowImage(Vector2 texSize)
    {
        Vector2 temp = texSize;

        // temp.y -= PictureHandle.Instance.LableHeight;
        //图片的容器的宽高
        Vector2 size = new Vector2(512f, 512f);
        float v2 = temp.x / temp.y;//图片的比率


        if (temp.x > temp.y)//如果图片宽大于高
        {
            if (temp.x > size.x)//如果图片宽大于容器的宽
            {
                temp.x = size.x;//以容器宽为准

                temp.y = size.x / v2;//把图片高按比例缩小

                if (temp.y > size.y)//如果图片的高还是大于容器的高
                {
                    temp.y = size.y;//则以容器的高为标准

                    temp.x = size.y * v2;//容器的高再度计算赋值

                    //一下逻辑同理
                }
            }
            else //如果图片宽小于容器的宽
            {

                if (temp.y > size.y)//如果图片的高还是大于容器的高
                {
                    temp.y = size.y;//则以容器的高为标准

                    temp.x = size.y * v2;//容器的高再度计算赋值


                }
            }
        }
        else if (temp.x <= temp.y)//如果图片的高大于宽 
        {
            if (temp.y > size.y)//如果图片高大于容器的高
            {
                temp.y = size.y;//以容器的高为准

                temp.x = size.y * v2;//重新计算图片的宽

                if (temp.x > size.x)//如果图片的宽还是大于容器的高
                {

                    temp.x = size.x;//则再次以容器的宽为标准

                    temp.y = size.x / v2;//再以容器的宽计算得到容器的高
                }
            }
            else //如果图片的高小于容器的高
            {
                //但是图片的宽大于容器的宽
                if (temp.x > size.x)
                {
                    temp.x = size.x;//以容器的宽为准
                    temp.y = size.x / v2;//再以容器的宽计算得到容器的高
                }

            }
        }


        //   Vector2 realSize =_yearsEvent.PictureIndes

        Vector2 newSize = new Vector2(temp.x, temp.y);

        return newSize;


    }
    public void Active(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
   
}
