using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XHFrameWork;

public class CompanyIntroductionFSM : UIStateFSM
{


    /// <summary>
    /// 公司介绍
    /// </summary>
    public Button Introduce;
    /// <summary>
    /// 基本信息
    /// </summary>
    public Button Info;
    /// <summary>
    /// 股东概况
    /// </summary>
    public Button Shareholder;
    /// <summary>
    /// 荣誉奖项
    /// </summary>
    public Button Honor;
    /// <summary>
    /// 产品体系
    /// </summary>
    public Button Product;
    /// <summary>
    /// 服务体系
    /// </summary>
    public Button Service;

    public List<Texture2D> IntroduceTexs;

    public List<Texture2D> InfoTexs;

    public List<Texture2D> ShareholderTexs;

    public List<Texture2D> HonorTexs;

    public List<Texture2D> ProductTexs;

    public List<Texture2D> ServiceTexs;

    /// <summary>
    /// 当前选中的贴图集合  
    /// </summary>
    private List<Texture2D> _curTex;

    /// <summary>
    /// 显示内容的贴图  
    /// </summary>
    private RawImage ShowImage;

    private List<Image> _highlights;

    private int _curIndex;

    private Transform _previous;

    private Transform _next;

    private Transform _previousTouch;

    private Transform _nextTouch;

    private GameObject _gridGameObject;

    private Transform _gridTransform;

    private TouchEvent _touchEvent;

    private Scrollbar _scrollbar;

    private Tween _tween;

    private float _scrollBarValue;

    private Text _numberText;

    public CompanyIntroductionFSM(Transform go, params  object[] args)
        : base(go)
    {
      

        _highlights = new List<Image>();

        Introduce = Parent.transform.Find("1集团介绍").GetComponent<Button>();
        Info = Parent.transform.Find("2基本信息").GetComponent<Button>();
        Shareholder = Parent.transform.Find("3股东概况").GetComponent<Button>();
        Honor = Parent.transform.Find("4荣誉奖项").GetComponent<Button>();
        Product = Parent.transform.Find("5产品体系").GetComponent<Button>();
        Service = Parent.transform.Find("6服务体系").GetComponent<Button>();

        IntroduceTexs = PictureHandle.Instance.CompanyAllTexList[0].TexInfo;
        InfoTexs = PictureHandle.Instance.CompanyAllTexList[1].TexInfo;
        ShareholderTexs = PictureHandle.Instance.CompanyAllTexList[2].TexInfo;
        HonorTexs = PictureHandle.Instance.CompanyAllTexList[3].TexInfo;
        ProductTexs = PictureHandle.Instance.CompanyAllTexList[4].TexInfo;
        ServiceTexs = PictureHandle.Instance.CompanyAllTexList[5].TexInfo;


        ShowImage = Parent.parent.Find("ShowImage").GetComponent<RawImage>();

        Introduce.onClick.AddListener((() =>
        {
            SetBtn(IntroduceTexs);
            SetHighlight(Introduce.transform);
        }));
        Introduce.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcOne);

        Info.onClick.AddListener((() =>
        {
            SetBtn(InfoTexs);
            SetHighlight(Info.transform);

        }));
        Info.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcTwo);

        Shareholder.onClick.AddListener((() =>
        {
            SetBtn(ShareholderTexs);
            SetHighlight(Shareholder.transform);

        }));
        Shareholder.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcThree);


        Honor.onClick.AddListener((() =>
        {
            SetBtn(HonorTexs);
            SetHighlight(Honor.transform);

        }));
        Honor.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcFour);

        Product.onClick.AddListener((() =>
        {
            SetBtn(ProductTexs);
            SetHighlight(Product.transform);
        }));
        Product.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcFive);

        Service.onClick.AddListener((() =>
        {
            SetBtn(ServiceTexs);
            SetHighlight(Service.transform);
        }));
        Service.transform.Find("Text").GetComponent<Text>().text = SettingManager.Instance.GetDirectName(Direct.IcSix);

        _highlights.Add(Introduce.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(Info.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(Shareholder.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(Honor.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(Product.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(Service.transform.Find("Image").GetComponent<Image>());

        SetHighlight(Introduce.transform);

        AddVideoTex(IntroduceTexs, PictureHandle.Instance.CompanyAllTexList[0].VideoInfo);
        AddVideoTex(InfoTexs, PictureHandle.Instance.CompanyAllTexList[1].VideoInfo);
        AddVideoTex(ShareholderTexs, PictureHandle.Instance.CompanyAllTexList[2].VideoInfo);
        AddVideoTex(HonorTexs, PictureHandle.Instance.CompanyAllTexList[3].VideoInfo);
        AddVideoTex(ProductTexs, PictureHandle.Instance.CompanyAllTexList[4].VideoInfo);
        AddVideoTex(ServiceTexs, PictureHandle.Instance.CompanyAllTexList[5].VideoInfo);


        _gridGameObject = UIControl.Instance.GridPrefab;
        _gridTransform = UIControl.Instance.GridParent;

        CreatItem();

        _touchEvent = ShowImage.GetComponent<TouchEvent>();

        go.transform.parent.Find("CloseBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        go.transform.parent.Find("CloseBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            UIControl.Instance.ChangeState(UIState.Close);
        }));



        _scrollbar = go.transform.Find("ZhongXinBaoCheng/Scroll View/Scrollbar Horizontal").GetComponent<Scrollbar>();
        go.transform.Find("ZhongXinBaoCheng/dir/button").GetComponent<Button>().onClick.AddListener((() =>
        { 
            _scrollBarValue = _scrollbar.value;
          _tween=   DOTween.To(() => _scrollBarValue, x => _scrollBarValue = x, 0, 0.55f).OnComplete((() =>
          {
              _tween = null;
          }));
        }));


    }

    private void TouchMoveEvent(bool isleft)
    {
        if (!isleft)
        {
            if(_previous.gameObject.activeInHierarchy)
             Previous(null,null);
        }
        else
        {
            if (_next.gameObject.activeInHierarchy)
                Next(null,null);
        }
    }


    private void CreatItem()
    {

        float xTempUp = 560f;
        float xtempDown = 1000f;
        float endXUp = -1f;//上轴实例化出来后所摆放的右下角x位置
        float endXDown = -1f;//下周实例化出来后所摆放右下角x位置

        Vector2 itemSize = Vector2.zero;
        for (int i = 0; i < 100; i++)
        {

            GridItem gi = Object.Instantiate(_gridGameObject, _gridTransform).GetComponent<GridItem>();



            RectTransform rt = null;


            if (i == 0 || i % 2 == 0)//在上面的
            {
                rt = gi.SetInfo(true);

                if (rt == null)
                {
                    float maxVal = endXUp > endXDown ? endXUp : endXDown;

                    Vector2 size = _gridTransform.GetComponent<RectTransform>().sizeDelta;
                    _gridTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVal + itemSize.x, size.y);
                    Object.Destroy(gi.gameObject);
                    break;
                }

                itemSize = rt.sizeDelta;
                if (endXUp < 0f)
                    endXUp = xTempUp + itemSize.x * (i / 2f);
                else
                {
                    endXUp += itemSize.x;
                }
                rt.anchoredPosition = new Vector2(endXUp, 0f);

            }
            else if (i % 2 != 0)//在下面的
            {
                rt = gi.SetInfo(false);

                if (rt == null)
                {
                    float maxVal = endXUp > endXDown ? endXUp : endXDown;

                    Vector2 size = _gridTransform.GetComponent<RectTransform>().sizeDelta;
                    _gridTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(maxVal + itemSize.x, size.y);
                    Object.Destroy(gi.gameObject);
                    break;
                }
                itemSize = rt.sizeDelta;
                if (endXDown < 0f)
                    endXDown = xtempDown + itemSize.x * (i - 1) / 2;
                else
                {
                    endXDown += itemSize.x;
                }
                rt.anchoredPosition = new Vector2(endXDown, -1164f);
            }


        }
    }
    private void SetBtn(List<Texture2D> texs)
    {
        _curTex = texs;
        _curIndex = 0;
        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(null, ShowImage.gameObject);

        if (_curTex.Count == 1)
        {
            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(false);
            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/1";
        }
        else
        {

            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/" + _curTex.Count;

        }

    }

    /// <summary>
    /// 设置高亮
    /// </summary>
    public void SetHighlight(Transform parent)
    {
        foreach (Image image in _highlights)
        {
            if (image.transform.parent == parent)
            {
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
    public override void Enter()
    {
        base.Enter();
        _touchEvent.TouchMoveEvent += TouchMoveEvent;
        _previous = Target.transform.Find("ZhongXinBaoChengFSM/Previous");

        _next = Target.transform.Find("ZhongXinBaoChengFSM/Next");

        _previousTouch = Target.transform.Find("ZhongXinBaoChengFSM/TouchPrevious"); 

        _nextTouch = Target.transform.Find("ZhongXinBaoChengFSM/TouchNext");

        _numberText = Target.transform.Find("ZhongXinBaoChengFSM/NumberTip").GetComponent<Text>();

        EventTriggerListener.Get(_previous.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Previous);

        EventTriggerListener.Get(_next.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Next);


        _curTex = IntroduceTexs;
        Introduce.onClick.Invoke();
        Parent.parent.gameObject.SetActive(true);//父级别也要显示

     
    }

    public override void Excute()
    {
        base.Excute();

        if (_tween != null)
        {
            _scrollbar.value = _scrollBarValue;
        }
    }

    private void Next(GameObject _listener, object _args, params object[] _params)
    {
        _curIndex++;
        if (_curIndex >= _curTex.Count)
        {
            _curIndex--;
        }
      
        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        if (_curIndex == _curTex.Count-1)
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(false);
            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex+1) +"/"+ _curTex.Count;
        }
        else
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
        }
      
    }

    private void Previous(GameObject _listener, object _args, params object[] _params)
    {
        //Debug.Log("previous");
        _curIndex--;
        if (_curIndex < 0)
        {
            _curIndex = 0;
        }

        ShowImage.texture = _curTex[_curIndex];
        CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        if (_curIndex == 0)
        {
            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/"  + _curTex.Count; 
        }
        else
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
        }

     
    }

    public override void Exit()
    {
        base.Exit();
        _touchEvent.TouchMoveEvent -= TouchMoveEvent;

    }


}
