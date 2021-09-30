using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XHFrameWork;

public class ZhongXinXieTongFSM : UIStateFSM
{
    private List<LogoItem> _frontItems = new List<LogoItem>();

    private List<LogoItem> _backItems = new List<LogoItem>();

    private int count = 100;

    private float Width = 500f;

    private float height = 309f;

    /// <summary>
    /// 所示的UI，布局在屏幕上，上下不能触及的范围
    /// </summary>
    private float heightReduce = 600;


    private bool _isFront = true;

    /// <summary>
    /// 外发光材质
    /// </summary>
    private Material _material;
    /// <summary>
    /// 填充的宽度的倍数
    /// </summary>
    private float scalar = 2.7f;

   // private XieTongWeiHuoDong _xieTongWeiHuoDong;

    private List<RectTransform> posLeftList = new List<RectTransform>();
    private List<RectTransform> posRightList = new List<RectTransform>();

    private RectTransform centeRectTransform;

    private List<LogoItem> logos = new List<LogoItem>();

    private TouchMove _touchMove;

    private Text _numberText;

    /// <summary>
    /// 当前选中的贴图集合  
    /// </summary>
    private List<Texture2D> _curTex;

    private int _curIndex;

    /// <summary>
    /// 显示内容的贴图  
    /// </summary>
    private RawImage ShowImage;
    private Transform _previous;

    private Transform _next;

    private Transform _previousTouch;

    private Transform _nextTouch;



    private List<Image> _highlights;

    /// <summary>
    /// 公司介绍
    /// </summary>
    public Button FenHuiJieShao;
    /// <summary>
    /// 基本信息
    /// </summary>
    public Button XieTongFengCai;


    public List<Texture2D> FenHuiJieShaoTexs;

    public List<Texture2D> XieTongFengCaiTexs;

    private TouchEvent _touchEvent;

    private SiXiangChuanJiaHuoDong zhongXinXieTongHuoDong;

    private SiXiangChuanJiaShowPicture _zhongXinXieTongShowPicture;
    public ZhongXinXieTongFSM(Transform go,GameObject logoPrefab,Material material) : base(go)
    {
        _material = material;



        zhongXinXieTongHuoDong = Parent.transform.Find("XieTongWeiHuoDong/Scroll View1").GetComponent<SiXiangChuanJiaHuoDong>();

        //_zhongXinXieTongShowPicture = Parent.transform.Find("XieTongFengCaiShow").GetComponent<SiXiangChuanJiaShowPicture>();

        zhongXinXieTongHuoDong.Init(PictureHandle.Instance.XieTongHuoDongList);

        FenHuiJieShao = Parent.transform.Find("XieTongWeiHuoDong/FenHui/1协同分会").GetComponent<Button>();
        XieTongFengCai = Parent.transform.Find("XieTongWeiHuoDong/FenHui/2协同风采").GetComponent<Button>();

        RectTransform poss = this.Parent.Find("Positions").GetComponent<RectTransform>();

        _touchMove = this.Parent.Find("LogoParent").GetComponent<TouchMove>();

        RectTransform[] temps = poss.GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform rectTransform in temps)
        {
            if (rectTransform.parent == poss)
            {
                if(rectTransform.name.Contains("left"))
                    posLeftList.Add(rectTransform);
                else if(rectTransform.name.Contains("right"))
                    posRightList.Add(rectTransform);
                else
                    centeRectTransform = poss;
            }
        }
      
        Button switchBtn = go.transform.Find("Button").GetComponent<Button>();

        ShowImage = Parent.Find("XieTongWeiHuoDong/ShowImage").GetComponent<RawImage>();

        switchBtn.onClick.AddListener((() =>
        {
            _isFront = !_isFront;

            if (_isFront)
            {
                switchBtn.transform.Find("Image").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(-259, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(101, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<Text>().text = "非金融板块";
            }
            else
            {
               

                switchBtn.transform.Find("Image").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(245f, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(-70f, 0f), 0.55f);
                switchBtn.transform.Find("Text").GetComponent<Text>().text = "金融板块";
            }


            int index = 0;
            foreach (LogoItem item in _frontItems)
            {
                float delay = 1f;

               // item.Move(_isFront);

                LogoItem oldItem = _backItems[index];
              //  oldItem.Move(!_isFront);
                index++;
            }
        }));
      

        List<Vector2> randPosLeft = GetVector2(true);

        List<Vector2> randPos2Right = GetVector2(false);


        _highlights = new List<Image>();


        FenHuiJieShaoTexs = PictureHandle.Instance.XieTongJieShao[0].TexInfo;
        XieTongFengCaiTexs = PictureHandle.Instance.CompanyAllTexList[1].TexInfo;

        _highlights.Add(FenHuiJieShao.transform.Find("Image").GetComponent<Image>());
        _highlights.Add(XieTongFengCai.transform.Find("Image").GetComponent<Image>());

        SetHighlight(FenHuiJieShao.transform);

        //AddVideoTex(FenHuiJieShaoTexs, PictureHandle.Instance.CompanyAllTexList[0].VideoInfo);
        //AddVideoTex(XieTongFengCaiTexs, PictureHandle.Instance.CompanyAllTexList[1].VideoInfo);
       

        FenHuiJieShao.onClick.AddListener((() =>
        {
            SetBtn(FenHuiJieShaoTexs);
            SetHighlight(FenHuiJieShao.transform);

          
            _numberText.gameObject.SetActive(true);

            ShowImage.gameObject.SetActive(true);

            zhongXinXieTongHuoDong.gameObject.SetActive(false);

        }));

        XieTongFengCai.onClick.AddListener((() =>
        {
            SetBtn(FenHuiJieShaoTexs);
            SetHighlight(XieTongFengCai.transform);

            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(false);

            _numberText.gameObject.SetActive(false);
            ShowImage.gameObject.SetActive(false);
            zhongXinXieTongHuoDong.gameObject.SetActive(true);
            
        }));

        //while (true)
        //{

        //    else break;
        //}
        Transform logoParent = go.transform.Find("LogoParent");
        int n = 0;

        foreach (Vector2 vector2 in randPosLeft)
        {

            YearsEvent ye = PictureHandle.Instance.FrontLogos[n];
            Vector2 pos = vector2;
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _frontItems.Add(image);

            Material mat = Object.Instantiate(_material);
            image.name = "LogoLeft" + n;
            image.SetInfo(-2f, true, pos, ye, mat);

            image.ClickEvent += ItemClick;

            n++;
            if (n >= PictureHandle.Instance.FrontLogos.Count)
            {
                if (image.RectTransform.anchoredPosition.x < 7680f / -2)
                {
                    _touchMove.MaxRight = 7680f / -2 - image.RectTransform.anchoredPosition.x;
                }
                break;
            }
        }
       

        n = 0;

        foreach (Vector2 vector2 in randPos2Right)
        {

            YearsEvent ye = PictureHandle.Instance.BackLogos[n];
            Vector2 pos = vector2;
            LogoItem image = Object.Instantiate(logoPrefab, logoParent).GetComponent<LogoItem>();
            _backItems.Add(image);
            Material mat = Object.Instantiate(_material);
            image.name = "LogoRight" + n;
            image.SetInfo(-2f, false, pos, ye, mat);

            image.ClickEvent += ItemClick;
            n++;
            if (n >= PictureHandle.Instance.BackLogos.Count)
            {
                if (image.RectTransform.anchoredPosition.x > 7680f / 2)
                {
                    _touchMove.MaxLeft =  image.RectTransform.anchoredPosition.x - 7680f /2 + image.RectTransform.sizeDelta.x/2 ;
                }
                break;
            }
        }

        logos.AddRange(_backItems);
        logos.AddRange(_frontItems);



        //_xieTongWeiHuoDong = go.transform.Find("XieTongWeiHuoDong").GetComponent<XieTongWeiHuoDong>();
        //_xieTongWeiHuoDong.Init();

        go.transform.Find("XieTongWeiHuoDong/goBack").GetComponent<Button>().onClick.AddListener((() =>
        {
            RectTransform rt = zhongXinXieTongHuoDong.transform.parent.GetComponent<RectTransform>();

            
             rt.DOAnchorPos(new Vector2(-2004f, -300f), 0.35f);
           

        }));

        go.transform.Find("leftBtn").GetComponent<Button>().onClick.AddListener((() =>
        {
            RectTransform rt = zhongXinXieTongHuoDong.transform.parent.GetComponent<RectTransform>();

            if (rt.anchoredPosition.x <= -4840f)
            {
                rt.DOAnchorPos(new Vector2(-2004f, -300f), 0.35f);
            }
            else
            {
                rt.DOAnchorPos(new Vector2(-5270f, -300f), 0.35f);
            }
           
        }));

        go.transform.Find("Back").GetComponent<Button>().onClick.AddListener((() =>
        {
           Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("XieTongWeiHuoDong/BackLeft").GetComponent<Button>().onClick.AddListener((() =>
        {
            Target.ChangeState(UIState.Close);

        }));

        go.transform.Find("XieTongWeiHuoDong/goBack").GetComponent<Button>().onClick.AddListener((() =>
        {
            RectTransform rt = zhongXinXieTongHuoDong.transform.parent.GetComponent<RectTransform>();
            rt.DOAnchorPos(new Vector2(-5270f, -300f), 0.35f);

        }));

        _touchEvent = ShowImage.GetComponent<TouchEvent>();

    }

    /// <summary>
    /// Item点击的事件
    /// </summary>
    private void ItemClick()
    {
        if (_scaleCoroutine != null) Target.StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = Target.StartCoroutine(Scale(10f));
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

    private void TouchMoveEvent(bool isleft)
    {
        if (!isleft)
        {
            if (_previous.gameObject.activeInHierarchy)
                Previous(null, null);
        }
        else
        {
            if (_next.gameObject.activeInHierarchy)
                Next(null, null);
        }
    }

    private void SetBtn(List<Texture2D> texs)
    {
        _curTex = texs;
        _curIndex = 0;
        ShowImage.texture = _curTex[_curIndex];
       // CheckVideoTex(null, ShowImage.gameObject);

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
    private void Next(GameObject _listener, object _args, params object[] _params)
    {
        _curIndex++;
        if (_curIndex >= _curTex.Count)
        {
            _curIndex--;
        }

        ShowImage.texture = _curTex[_curIndex];
       // CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        if (_curIndex == _curTex.Count - 1)
        {
            _previous.gameObject.SetActive(true);
            _next.gameObject.SetActive(false);
            _nextTouch.gameObject.SetActive(false);
            _previousTouch.gameObject.SetActive(true);
            _numberText.text = (_curIndex + 1) + "/" + _curTex.Count;
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
       // CheckVideoTex(_curTex[_curIndex], ShowImage.gameObject);
        if (_curIndex == 0)
        {
            _previous.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
            _nextTouch.gameObject.SetActive(true);
            _previousTouch.gameObject.SetActive(false);
            _numberText.text = "1/" + _curTex.Count;
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

    private float _durDis = 0f;
    private Coroutine _scaleCoroutine;
    private Tween _figure;
    private IEnumerator Scale(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            _durDis = 0f;
            _figure= DOTween.To(() => _durDis, x => _durDis = x, 3840f*2, 1.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                _figure = null;
            });

        }
    }
    private List<Vector2> GetVector2(bool isLeft)
    {

        List<Vector2> temps = new List<Vector2>();

        if (isLeft)
        {
            foreach (RectTransform transform in posLeftList)
            {
                temps.Add(transform.anchoredPosition);
            }
        }
        else
        {
            foreach (RectTransform transform in posRightList)
            {
                temps.Add(transform.anchoredPosition);
            }
        }
       
        //List<Vector2> temp = new List<Vector2>();

        //temp = Common.Sample2D(seed, Common.ContainerWidth * Common.Scale, Common.ContainerHeight, 750f, 50);

        //Debug.Log("个数是 " + temp.Count);
        return temps;
    }

    public override void Enter()
    {
        base.Enter();
        if(_scaleCoroutine!=null) Target.StopCoroutine(_scaleCoroutine);
       _scaleCoroutine = Target.StartCoroutine(Scale(6f));
        foreach (LogoItem logo in _frontItems)
        {
            logo.gameObject.SetActive(true);
        }
        foreach (LogoItem logo in _backItems)
        {
            logo.gameObject.SetActive(true);
        }

        _touchEvent.TouchMoveEvent += TouchMoveEvent;
        _previous = Target.transform.Find("ZhongXinXieTongFSM/XieTongWeiHuoDong/Previous");

        _next = Target.transform.Find("ZhongXinXieTongFSM/XieTongWeiHuoDong/Next");

        _previousTouch = Target.transform.Find("ZhongXinXieTongFSM/XieTongWeiHuoDong/TouchPrevious");

        _nextTouch = Target.transform.Find("ZhongXinXieTongFSM/XieTongWeiHuoDong/TouchNext");

        _numberText = Target.transform.Find("ZhongXinXieTongFSM/XieTongWeiHuoDong/NumberTip").GetComponent<Text>();

        EventTriggerListener.Get(_previous.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Previous);

        EventTriggerListener.Get(_next.gameObject).SetEventHandle(EnumTouchEventType.OnClick, Next);


        _curTex = FenHuiJieShaoTexs;
        FenHuiJieShao.onClick.Invoke();
        Parent.parent.gameObject.SetActive(true);//父级别也要显示
        SetBtn(FenHuiJieShaoTexs);

    }

    public override void Excute()
    {
        base.Excute();

        if (_figure != null)
        {
            foreach (LogoItem logoItem in logos)
            {
                logoItem.Scale(_durDis);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        if (_scaleCoroutine != null) Target.StopCoroutine(_scaleCoroutine);
        zhongXinXieTongHuoDong.transform.parent.GetComponent<RectTransform>();
        _touchEvent.TouchMoveEvent -= TouchMoveEvent;

    }
}
