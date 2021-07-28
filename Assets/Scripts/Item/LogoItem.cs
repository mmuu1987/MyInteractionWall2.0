using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class LogoItem : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    private float _moveSpeed = 0f;

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
    private bool _isLeft = false;
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


    public RawImage ContentRawImage;

    public Image BgImage;

    private int _siblingIndex;

    private List<MaskableGraphic> _graphics = new List<MaskableGraphic>();
    private void Awake()
    {
        RectTransform = this.GetComponent<RectTransform>();

        MaskableGraphic[] temps = this.GetComponentsInChildren<MaskableGraphic>(true);

        _graphics.AddRange(temps);


    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private Coroutine _coroutine;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click this name is" +this.name);

        //if (!_isLeft) return;
        //if (eventData.pointerCurrentRaycast.gameObject != this.gameObject) return;

        if (BgImage.gameObject.activeInHierarchy)
        {
            if (DefaultSprite != null)
                BgImage.sprite = DefaultSprite;
            else
            {
                BgImage.gameObject.SetActive(false);
            }
            _isEnableDrag = false;
            ShowDescription(false);
            RectTransform.SetSiblingIndex(_siblingIndex);
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
        else
        {
            BgImage.gameObject.SetActive(true);
            BgImage.sprite = ActiveSprite;
            _isEnableDrag = true;
            ShowDescription(true);
           
            _siblingIndex = RectTransform.GetSiblingIndex();
            RectTransform.SetAsLastSibling();

            if(_coroutine!=null)StopCoroutine(_coroutine);
            _coroutine= StartCoroutine(Revert());
            //显示信息
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!_isLeft) return;
        // Debug.Log("OnPointerDown this name is" + this.name);
       // if (eventData.pointerCurrentRaycast.gameObject != this.gameObject) return;
        _touchMoveTemp = 0;
        _isPress = true;
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
      //  if (!_isLeft) return;
        //if (eventData.pointerCurrentRaycast.gameObject != this.gameObject) return;
        _isPress = false;
        _isEnableDrag = false;
       //f Debug.Log("OnPointerUp this name is" + this.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
      //  if (!_isLeft) return;
       // if (eventData.pointerCurrentRaycast.gameObject != this.gameObject) return;
        if (_isEnableDrag)
        {
            _dragDelta = eventData.delta;
        }
    }

    private void DoFade(float time,float target)
    {
        foreach (MaskableGraphic graphic in _graphics)
        {
            graphic.DOKill();
            graphic.DOFade(target, time);
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


                if (!_isPress) 
                {
                    RectTransform.anchoredPosition = Vector2.Lerp(pos, _orinigalVector2, Time.deltaTime * 1f);

                    if (pos.x + size .x/2<= 0 && _tween == null)
                    {
                       // RectTransform.anchoredPosition = RectTransform.anchoredPosition+new Vector2(18633f + size.x / 2, 0);


                      //  _orinigalVector2 = RectTransform.anchoredPosition;

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

            //if (_tween != null)
            //{
            //    _material.SetFloat("_Alpha", CurTarget);
            //}

      
    }

    private IEnumerator Revert()
    {
        yield return new WaitForSeconds(10f);

        if (DefaultSprite != null)
            BgImage.sprite = DefaultSprite;
        else
        {
            BgImage.gameObject.SetActive(false);
        }
        _isEnableDrag = false;
        ShowDescription(false);
        RectTransform.SetSiblingIndex(_siblingIndex);
    }

    private void ShowDescription(bool isShow)
    {
        if (isShow)
        {
            Vector2 pos = DescriptionImage.rectTransform.anchoredPosition;
            if (RectTransform.anchoredPosition.x + 1600 > 3840f)
            {
                DescriptionImage.rectTransform.anchoredPosition = new Vector2(-880f, pos.y);
            }
            else
            {
                DescriptionImage.rectTransform.anchoredPosition = new Vector2(880f, pos.y);
            }
            
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
            Vector2 dir = new Vector2(-962, -100f);

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
            DoFade(_animationTime, 1f);
            _isLeft = true;

            RectTransform.SetAsLastSibling();
        }
        else
        {
            Vector2 dir = new Vector3(962, 100f);

            _moveTarget = this.RectTransform.anchoredPosition + dir;

            _material.DOColor(new Color(0.65f, 0.65f,0.65f,0.35f), _animationTime);

            DOTween.To(() => CurTarget, x => CurTarget = x, 1, _animationTime);

            DoFade(_animationTime, 0.35f);
            RectTransform.SetAsFirstSibling();

            _scaleBack = 0.65f;
            _tween=  this.RectTransform.DOSizeDelta(_orinigalSize * _scale*_scaleBack, _animationTime).SetEase(Ease.InOutQuart).OnComplete((() =>
            {
                _moveSpeed = -2f*0.65f;
                _orinigalVector2 = _moveTarget;
                _tween = null;
            })); 
            _isLeft = false;
        }





        this.RectTransform.DOAnchorPos(_moveTarget, _animationTime).SetEase(Ease.InOutQuart);


      

    }

    public void Scale()
    {
        if (_tween == null)
        {
            RectTransform.SetAsLastSibling();
            RectTransform.DOScale(Vector3.one * 1.35f, 0.35f).SetEase(Ease.InOutQuad).SetDelay(Random.Range(0f, 1f)).OnComplete((() =>
            {
                RectTransform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutQuad).SetDelay(Random.Range(1f, 2.5f)).OnComplete((() =>
                {
                    RectTransform.SetSiblingIndex(_siblingIndex);
                    _tween = null;
                }));
            }));
        }
    }

    public void SetInfo(float speed,bool isLeft, Vector2 pos,YearsEvent yearsEvent,Material material)
    {
         
        
         _isLeft = isLeft;
        // Image.sprite = DefaultSprite;
        //算出新位置，y轴上下边距不需要有运动
        _orinigalVector2 = pos;//(pos.y- (3640 - Common.ContainerHeight)/2));

        RectTransform.anchoredPosition = _orinigalVector2;
        
       

         _orinigalSize = new Vector2(800f,800f);
         _material = material;

         Image.material = _material;

         Texture2D content = null;
         Description.text = yearsEvent.Describe;
        
        if (yearsEvent.TexList.Count > 1)
         {
             foreach (Texture2D texture2D in yearsEvent.TexList)
             {
                 if (texture2D.name.Contains("Logo"))//含有logo的图片规定必须这样命名
                 {
                   // Vector2 size = Common.ScaleImageSize(new Vector2(texture2D.width, texture2D.height),new Vector2(800f, 800f),true);
                    // _material.SetTexture("_ShowTex", texture2D);
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

                    Image.sprite = sprite;
                   // Image.rectTransform.sizeDelta = size;
                 }
                 else
                 {
                     content = texture2D;
                 }
             }
         }
         else
         {
            Texture2D tex = yearsEvent.TexList[0];
           // Vector2 size = Common.ScaleImageSize(new Vector2(tex.width, tex.height), new Vector2(800f, 800f), true);
            Sprite sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            // _material.SetTexture("_ShowTex", yearsEvent.TexList[0]);
            Image.sprite = sprite;
           // Image.rectTransform.sizeDelta = size;
         }

         float height = 0f;
         if (content != null)
         {
             ContentRawImage.texture = content;
             Vector2 oldSize = new Vector2(content.width,content.height);
             Vector2 newSize = Common.ScaleImageSize(oldSize, new Vector2(900f, 500f));
             ContentRawImage.rectTransform.sizeDelta = newSize;
             //设置图片的位置，在文字的框的下方10个单位
             ContentRawImage.rectTransform.anchoredPosition = new Vector2(ContentRawImage.rectTransform.anchoredPosition.x, -Description.preferredHeight-newSize.y/2-10);
             height = Description.preferredHeight + newSize.y + 20;

         }
         else
         {
             Destroy(ContentRawImage.gameObject);

             height = Description.preferredHeight + 20;

             
         }


         Description.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, height);
        _scale = 1f;

         if (_isLeft)
         {
             _moveSpeed = speed;
            _scaleBack = 1f;
             _material.color = new Color(1f,1f,1f,1f);
         }
         else
         {
             _moveSpeed = speed*0.65f;
            _scaleBack = 0.65f;
             _material.color = new Color(0.65f, 0.65f, 0.65f, 0.35f);
             DoFade(1f,0.35f);
         }



         _moveSpeed = 0f;
        Vector2 sizeTemp = _orinigalSize * _scale * _scaleBack;

        //RectTransform.sizeDelta = sizeTemp;

         _yearsEvent = yearsEvent;
        
        this.gameObject.SetActive(false);
    }
    public void Active(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
   
}
